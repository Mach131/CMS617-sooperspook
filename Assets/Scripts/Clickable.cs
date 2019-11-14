using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    private PlayerMovement player;
    private Collider collider;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        collider = gameObject.GetComponent<Collider>();
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
                    // TODO: adjust exact position that the player moves to
                    Vector3 targetPosition = collider.ClosestPoint(player.transform.position);
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
