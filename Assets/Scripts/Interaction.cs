using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IComparable<Interaction>
{
    public string menuName;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
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
        //Debug.Log("Doing " + GetMenuName());
    }

    public int CompareTo(Interaction other)
    {
        return GetMenuName().CompareTo(other.GetMenuName());
    }
}
