using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorController : MonoBehaviour
{
    public Transform exit;

    private Animator animator;
    private VisitorState state;

    private GameObject player;

    private Rigidbody rb;

    private Dictionary<Interactable, HashSet<string>> states = new Dictionary<Interactable, HashSet<string>>();
    private float totalFear = 0;
    private float fearThreshold = 4.9f; // How much fear before a jump scare makes them leave

    private NavMeshAgent agent;
    private InterestItem currentInterestItem;
    private float startedObservingTimestamp = -1;
    private float interestDuration = 5f; // How long a visitor stays interested in an object

    private float baseSpeed;

    public float runSpeed = 5f;

	private VisitorScript visitorScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        state = VisitorState.Idle;
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
		visitorScript = GetComponent<VisitorScript>();
        baseSpeed = agent.speed;
        StartCoroutine(CheckPropStates());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == VisitorState.Idle)
        {
            GetNewInterestItem();
			if( currentInterestItem != null ) {
				agent.destination = currentInterestItem.GetStandPosition();
				state = VisitorState.MovingToObserve;
				agent.speed = baseSpeed;
			}
        }
        else if (state == VisitorState.MovingToObserve)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.ResetPath();
                state = VisitorState.Observing;
                startedObservingTimestamp = Time.time;
            }
            agent.speed = baseSpeed;
        }
        else if (state == VisitorState.Observing)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentInterestItem.GetLookDirection(), Vector3.up), 0.2f);
            if (Time.time - startedObservingTimestamp > interestDuration)
            {
                GetNewInterestItem();
                agent.destination = currentInterestItem.GetStandPosition();
                state = VisitorState.MovingToObserve;
            }
            agent.speed = baseSpeed;
        }
        else if (state == VisitorState.FleeingRoom)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.ResetPath();
                state = VisitorState.Idle;
            }
            agent.speed = baseSpeed * 1.5f;
        }
        else if (state == VisitorState.FleeingBuilding)
        {
            agent.ResetPath();
            rb.isKinematic = false;
            Vector3 force = -visitorScript.LocalGradient().normalized * runSpeed;
            if (!player.GetComponent<PlayerInvisibility>().isInvisible)
            {
                Vector3 playerOffset = transform.position - player.transform.position;
                force += playerOffset.normalized * 10/playerOffset.magnitude * runSpeed;
            }
            Vector3 exitOffset = transform.position - exit.position;
            force += exitOffset.normalized * 2/exitOffset.magnitude * runSpeed;
            rb.AddForce(force);

            if ((transform.position - exit.position).magnitude < 5f)
            {
                Destroy(gameObject);
            }

            //if (!agent.pathPending && agent.remainingDistance < 0.5f)
            //{
            //    agent.ResetPath();
            //    Destroy(gameObject);
            //}
            //agent.speed = baseSpeed * 1.5f;
        }

    }

    void RunAway()
    {
        if (totalFear >= fearThreshold)
        {
            //agent.destination = exit.position;
            state = VisitorState.FleeingBuilding;
        }
        else
        {
            //TODO: Run out of current room
            state = VisitorState.FleeingRoom;
        }
    }

    void GetNewInterestItem()
    {
        InterestItem[] items = FindObjectsOfType<InterestItem>();
		if( items.Length > 0 ) {
			currentInterestItem = items[(int) (Random.value * items.Length)];
			startedObservingTimestamp = Time.time;
		}
    }

    IEnumerator CheckPropStates()
    {
        while (true)
        {
            foreach (Interactable interactable in FindObjectsOfType<Interactable>())
            {
                RaycastHit hit;
                int layerMask = 1 << 12 + 1 << 13; // Only check if walls or other props are in the way
                Vector3 rayDirection = interactable.transform.position - transform.position;
                if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, layerMask))
                {
                    Interactable hitInteractable = hit.collider.GetComponentInParent<Interactable>();
                    if (hitInteractable != null && hitInteractable.gameObject == interactable.gameObject && Vector3.Dot(transform.forward, rayDirection) > 0)
                    {
                        ObserveState(interactable, false);
                    }
                }
            }
            yield return new WaitForSeconds(2 + Random.value); // Don't need to do this every frame
        }
    }

    void ObserveState(Interactable interactable, bool causedByInteraction)
    {
        if (!states.ContainsKey(interactable))
        {
            states.Add(interactable, interactable.GetState());
        }
        else
        {
            if (!causedByInteraction)
            {
                // TODO: this is the case where the player changes something without the visitor noticing, and the visitor notices later that something is off
            }
            states[interactable] = interactable.GetState();
        }
    }

    public void ApplyFear(float fearAmount, Interactable source, bool isJumpScare)
    {
        totalFear += fearAmount;
        //Debug.Log("Fear is " + totalFear);
        if (source != null) // Source can be player, which is not an Interactable
        {
            UpdateSpookMap(source.transform.position, fearAmount);
            ObserveState(source, true);
        }
        if (isJumpScare)
        {
            RunAway();
        }
    }

    // Updates the spook map to show the current gradient
    public void UpdateSpookMap(Vector3 sourcePos, float fearAmount)
    {
        const float fearGain = 1.0e0f;
        foreach (Vector2Int cell in visitorScript.SpookMap.CellEnumerable())
        {
            Vector3 p = visitorScript.SpookMap.CellCenter3(cell);
            Vector3 deltaR = p - sourcePos;
            //Debug.Log(deltaR);
            float rMag = deltaR.magnitude;
            float v = visitorScript.SpookMap.ValueAt(cell);
            v += fearAmount * fearGain / rMag;
            visitorScript.SpookMap.SetAt(cell, v);
        }
    }

    enum VisitorState
    {
        Idle,
        Observing,
        MovingToObserve,
        FleeingRoom,
        FleeingBuilding
    }

    /**
     * Indicates whether or not a jumpscare can successfully cause the player to run away.
     */
    public bool CanJumpscare()
    {
        return totalFear >= fearThreshold;
    }
}
