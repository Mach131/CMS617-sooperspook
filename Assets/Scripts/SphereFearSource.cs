﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFearSource : FearSource
{
    public float sphereRadius = 5;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

    public override void TriggerEffect(Interactable source)
    {
        foreach (VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            if ((visitor.transform.position - transform.position).magnitude < sphereRadius)
            {
                visitor.ApplyFear(fearWeight, source, isJumpScare);
            }
        }
    }
}
