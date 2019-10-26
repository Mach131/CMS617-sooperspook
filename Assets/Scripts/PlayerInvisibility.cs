using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvisibility : MonoBehaviour
{
    public GameObject invisibilityCloak;

    private SkinnedMeshRenderer[] mrs;

    private bool isInvisible = true;

    // Start is called before the first frame update
    void Start()
    {
        mrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        if (isInvisible)
        {
            invisibilityCloak.SetActive(true);
            //foreach (SkinnedMeshRenderer mr in mrs)
            //{
            //    Debug.Log("Materials:" + mr.materials.Length);
            //    foreach (Material mat in mr.materials)
            //    {
            //        Color baseColor = mat.GetColor("_BaseColor");
            //        mat.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
            //    }
            //}
        }
        else
        {
            invisibilityCloak.SetActive(false);
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
        if (!isInvisible)
        {
            isInvisible = true;
            invisibilityCloak.SetActive(true);
            //foreach (SkinnedMeshRenderer mr in mrs)
            //{
            //    foreach (Material mat in mr.materials)
            //    {
            //        Color baseColor = mat.GetColor("_BaseColor");
            //        mat.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a * 0.3f));
            //    }
            //}
        }
        else if (CauseSpook()) //returns true iff a visitor was spooked
        {
            isInvisible = false;
            invisibilityCloak.SetActive(false);
            //foreach (SkinnedMeshRenderer mr in mrs)
            //{
            //    foreach (Material mat in mr.materials)
            //    {
            //        Color baseColor = mat.GetColor("_BaseColor");
            //        mat.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a / 0.3f));
            //    }
            //}
        }
        else
        {
            // Toggle failed
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
