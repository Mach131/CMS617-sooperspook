﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorController : MonoBehaviour
{
    public Transform exit;

    private VisitorState state;

    private Dictionary<Interactable, HashSet<string>> states = new Dictionary<Interactable, HashSet<string>>();
    private float totalFear = 0;
    private float fearThreshold = 4.9f; // How much fear before a jump scare makes them leave

    private NavMeshAgent agent;
    private InterestItem currentInterestItem;
    private float startedObservingTimestamp = -1;
    private float interestDuration = 5f; // How long a visitor stays interested in an object

    private float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        state = VisitorState.Idle;
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
        StartCoroutine(CheckPropStates());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == VisitorState.Idle)
        {
            GetNewInterestItem();
            agent.destination = currentInterestItem.GetStandPosition();
            state = VisitorState.MovingToObserve;
            agent.speed = baseSpeed;
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
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.ResetPath();
                Destroy(gameObject);
            }
            agent.speed = baseSpeed * 1.5f;
        }

    }

    void RunAway()
    {
        if (totalFear >= fearThreshold)
        {
            agent.destination = exit.position;
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
        currentInterestItem = items[(int)(Random.value * items.Length)];
        startedObservingTimestamp = Time.time;
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
        Debug.Log("Fear is " + totalFear);
        if (source != null) // Can be player, which is not an Interactable
        {
            ObserveState(source, true);
        }
        if (isJumpScare)
        {
            RunAway();
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
