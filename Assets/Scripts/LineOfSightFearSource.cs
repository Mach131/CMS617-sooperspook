using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightFearSource : FearSource
{
    public override void TriggerEffect()
    {
        foreach(VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            RaycastHit hit;
            Vector3 rayDirection = visitor.transform.position - transform.position;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == visitor.gameObject && Vector3.Dot(visitor.transform.forward, rayDirection) < 0)
                {
                    Debug.Log("Saw visitor!");
                    visitor.ApplyFear(fearWeight);
                }
            }
        }
    }
}
