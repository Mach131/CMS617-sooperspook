using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibility : MonoBehaviour
{
    private MeshRenderer mr;

    private bool isInvisible = true;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        baseColor = mr.material.GetColor("_BaseColor");
        if (isInvisible)
        {
            mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle Invisibility"))
        {
            ToggleInvisibility();
        }
    }

    void ToggleInvisibility()
    {
        isInvisible = !isInvisible;
        if (isInvisible)
        {
            mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
        }
        else
        {
            CauseSpook();
            mr.material.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a));
        }
    }

    void CauseSpook()
    {
        foreach (VisitorController visitor in FindObjectsOfType<VisitorController>())
        {
            RaycastHit hit;
            Vector3 rayDirection = visitor.transform.position - transform.position;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == visitor.gameObject && Vector3.Dot(visitor.transform.forward, rayDirection) < 0)
                {
                    float fearWeight = 1f;
                    visitor.ApplyFear(fearWeight, null, true);
                }
            }
        }
    }
}
