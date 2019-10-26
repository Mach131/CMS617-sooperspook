using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFearSource : FearSource
{
    public float sphereRadius = 20;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

    public override void TriggerEffect(Interactable source)
    {
        foreach (VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            Vector3 visitorPosition = new Vector3(visitor.transform.position.x, 0, visitor.transform.position.z);
            Vector3 thisPosition = new Vector3(transform.position.x, 0, transform.position.z);
            if ((visitorPosition - thisPosition).magnitude < sphereRadius)
            {
                visitor.ApplyFear(fearWeight, source, isJumpScare);
            }
        }
    }
}
