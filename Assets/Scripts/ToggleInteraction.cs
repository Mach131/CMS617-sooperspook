using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteraction : Interaction
{
    public string reverseMenuName;
    private bool baseState = true;

    protected override void Start()
    {
        base.Start();
    }

    public override string GetMenuName()
    {
        if (baseState)
        {
            return menuName;
        }
        else
        {
            return reverseMenuName;
        }
    }

    public override void DoInteraction()
    {
        baseState = !baseState;
        gameObject.GetComponentInChildren<InteractionMenu>().OnInteractionChange();
    }
}
