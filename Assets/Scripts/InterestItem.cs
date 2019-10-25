using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestItem : MonoBehaviour
{
    public Vector3 visitorStandLocation;
    public Vector3 visitorLookDirection = Vector3.forward;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 gizmoCenter = transform.position + transform.rotation * visitorStandLocation + Vector3.up * 5;
        Gizmos.DrawWireCube(gizmoCenter, new Vector3(1f, 10f, 1f));
        Gizmos.DrawLine(gizmoCenter,gizmoCenter + transform.rotation * visitorLookDirection.normalized * 2);
    }

    public Vector3 GetStandPosition()
    {
        return transform.position + transform.rotation * visitorStandLocation;
    }

    public Vector3 GetLookDirection()
    {
        return transform.rotation * visitorLookDirection.normalized;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}