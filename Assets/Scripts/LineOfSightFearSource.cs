using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightFearSource : FearSource
{
    public override void TriggerEffect(Interactable source)
    {
        foreach(VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            RaycastHit hit;
            Vector3 rayDirection = visitor.transform.position - transform.position;
            int layerMask = ~(1 << 9); // Ignore player in the way
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.GetComponentInParent<VisitorController>() == visitor && Vector3.Dot(visitor.transform.forward, rayDirection) < 0)
                {
                    visitor.ApplyFear(fearWeight, source, isJumpScare);
                }
            }
            else
            {
                Debug.Log("Hit nothing");
            }
        }
    }
}
