using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
