using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibility : MonoBehaviour
{
    private MeshRenderer[] mrs;

    private bool isInvisible = true;

    // Start is called before the first frame update
    void Start()
    {
        mrs = GetComponentsInChildren<MeshRenderer>();

        if (isInvisible)
        {
            foreach (MeshRenderer mr in mrs)
            {
                Color baseColor = mr.material.GetColor("_BaseColor");
                mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle Invisibility"))
        {
            ToggleInvisibility();
            Debug.Log("Invisibility: " + isInvisible);
        }
    }

    void ToggleInvisibility()
    {
        if (!isInvisible)
        {
            isInvisible = true;
        } else if (CauseSpook()) //returns true iff a visitor was spooked
        {
            isInvisible = false;
        } else
        {
            Debug.Log("Toggle failed");
        }

        foreach (MeshRenderer mr in mrs)
        {
            Color baseColor = mr.material.GetColor("_BaseColor");
            if (isInvisible)
            {
                mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
            }
            else
            {
                mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a / 0.3f));
            }
        }
    }

    //Returns whether or not a spook successfully happened
    bool CauseSpook()
    {
        bool spookSuccess = false;

        foreach (VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            if (!visitor.CanJumpscare())
            {
                continue;
            }

            RaycastHit hit;
            Vector3 rayDirection = visitor.transform.position - transform.position;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.GetComponentInParent<VisitorController>() == visitor &&
                    Vector3.Dot(visitor.transform.forward, rayDirection) < 0)
                {
                    spookSuccess = true;
                    float fearWeight = 1f;
                    visitor.ApplyFear(fearWeight, null, true);
                }
            }
        }

        return spookSuccess;
    }
}
