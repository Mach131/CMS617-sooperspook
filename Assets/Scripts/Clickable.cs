using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
                    player.moveToPosition(new Vector3(transform.position.x, 0, transform.position.z));
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
