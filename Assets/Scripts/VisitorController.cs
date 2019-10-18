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

    //TODO: this code should also be called whenever the visitor notices a prop change, not just in ApplyFear
    void ObserveState(Interactable interactable)
    {
        if (!states.ContainsKey(interactable))
        {
            states.Add(interactable, interactable.GetState());
        }
        else
        {
            states[interactable] = interactable.GetState();
        }
    }

    public void ApplyFear(float fearAmount, Interactable source, bool isJumpScare)
    {
        totalFear += fearAmount;
        if (source != null) // Can be player, which is not an Interactable
        {
            ObserveState(source);
        }
        if (isJumpScare)
        {
            RunAway();
        }
    }
}
