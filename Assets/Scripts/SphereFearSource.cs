using System.Collections;
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

    public override void TriggerEffect()
    {
        foreach (VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            if ((visitor.transform.position - transform.position).magnitude < sphereRadius)
            {
                Debug.Log("Visitor in range!");
                visitor.ApplyFear(fearWeight);
            }
        }
    }
}
