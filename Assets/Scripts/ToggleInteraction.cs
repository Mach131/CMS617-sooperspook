using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteraction : Interaction
{
    public string reverseAnimatorTriggerParameter = "Reverse Trigger";
    public string reverseMenuName;
    public FearSource reverseFearSource;

    private bool inActiveState = false;

    protected override void Start()
    {
        base.Start();
    }

    public override string GetMenuName()
    {
        if (inActiveState)
        {
            return reverseMenuName;
        }
        else
        {
            return menuName;
        }
    }

    public override string GetState()
    {
        if (inActiveState)
        {
            return activeState;
        }
        else
        {
            return baseState;
        }
    }

    public override void DoInteraction()
    {
        if (!inActiveState)
        {
            fearSource.TriggerEffect(interactable);
            animator.SetTrigger(animatorTriggerParameter);
        }
        else
        {
            reverseFearSource.TriggerEffect(interactable);
            animator.SetTrigger(reverseAnimatorTriggerParameter);
        }
        inActiveState = !inActiveState;
        gameObject.GetComponentInChildren<InteractionMenu>().OnInteractionChange();
    }
}
