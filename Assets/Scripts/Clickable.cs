using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Vector3 playerPosition;

    private PlayerMovement player;
    private Vector3 finalPlayerPosition;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + playerPosition, 0.5f);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        finalPlayerPosition = transform.position + playerPosition;
    }

    // MUST CALL base.Update()!!!
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Clickable>() != null && hit.transform.GetComponent<Clickable>().gameObject == gameObject ||
                    hit.transform.GetComponentInParent<Clickable>() != null && hit.transform.GetComponentInParent<Clickable>().gameObject == gameObject)
                {
                    Vector3 targetPosition = finalPlayerPosition;
                    player.moveToPosition(new Vector3(targetPosition.x, 0, targetPosition.z));
                    OnClick();
                }
            }
        }
    }

    // Override this method to implement click events
    protected virtual void OnClick()
    {

    }
}
