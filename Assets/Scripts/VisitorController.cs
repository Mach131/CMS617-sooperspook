using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorController : MonoBehaviour
{
    private Dictionary<Interactable, HashSet<string>> states = new Dictionary<Interactable, HashSet<string>>();
    private float totalFear = 0;
    private float fearThreshold = 4.9f; //How much fear before a jump scare makes them leave

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckPropStates());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RunAway()
    {
        //TODO: Set visitor to running away state
        if (totalFear >= fearThreshold)
        {
            // Run out of building
        }
        else
        {
            // Run out of current room
        }
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
}
