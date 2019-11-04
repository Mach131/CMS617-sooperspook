using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IComparable<Interaction>
{
    public string menuName;
    public string baseState;
    public string activeState;
    public FearSource fearSource;
    public Animator animator;
    public string animatorTriggerParameter = "Trigger";

    protected Interactable interactable;

    private bool activated = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual string GetMenuName()
    {
        return menuName;
    }

    public virtual void DoInteraction()
    {
        activated = true;
        fearSource.TriggerEffect(interactable);
        animator.SetTrigger(animatorTriggerParameter);
    }

    public virtual string GetState()
    {
        if (activated)
        {
            return activeState;
        }
        else
        {
            return baseState;
        }
    }

    public int CompareTo(Interaction other)
    {
        return GetMenuName().CompareTo(other.GetMenuName());
    }
}
