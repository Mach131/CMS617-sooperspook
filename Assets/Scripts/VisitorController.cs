using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorController : MonoBehaviour
{
    public Transform exit;

    private Dictionary<Interactable, HashSet<string>> states = new Dictionary<Interactable, HashSet<string>>();
    private float totalFear = 0;
    private float fearThreshold = 4.9f; // How much fear before a jump scare makes them leave

    private NavMeshAgent agent;
    private InterestItem currentInterestItem;
    private float lastSwitchedInterestTimestamp = -1;
    private float interestDuration = 15f; // How long a visitor stays interested in an object

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CheckPropStates());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentInterestItem == null || Time.time - lastSwitchedInterestTimestamp > interestDuration)
        {
            GetNewInterestItem();
            agent.destination = currentInterestItem.GetStandPosition();
        }
        if (agent.remainingDistance < 0.5f)
        {
            agent.ResetPath();
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentInterestItem.GetLookDirection(), Vector3.up), 0.1f);
        }
    }

    void RunAway()
    {
        if (totalFear >= fearThreshold)
        {
            agent.destination = exit.position;
        }
        else
        {
            // Run out of current room
        }
    }

    void GetNewInterestItem()
    {
        InterestItem[] items = FindObjectsOfType<InterestItem>();
        currentInterestItem = items[(int)(Random.value * items.Length)];
        lastSwitchedInterestTimestamp = Time.time;
    }

    IEnumerator CheckPropStates()
    {
        while (true)
        {
            foreach (Interactable interactable in FindObjectsOfType<Interactable>())
            {
                RaycastHit hit;
                Vector3 rayDirection = interactable.transform.position - transform.position;
                if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity))
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
        if (source != null) // Can be player, which is not an Interactable
        {
            ObserveState(source, true);
        }
        if (isJumpScare)
        {
            RunAway();
        }
    }

    /**
     * Indicates whether or not a jumpscare can successfully cause the player to run away.
     */
    public bool CanJumpscare()
    {
        return totalFear >= fearThreshold;
    }
}
