using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DiegeticLabel : MonoBehaviour
{
    public enum NameSource {Self, Other, Manual}
    public NameSource nameSource = NameSource.Self;
    
    public TMPro.TextMeshPro[] textMeshes;
    public GameObject otherSource;
    public string manualString;

    // Start is called before the first frame update
    void Start()
    {
        manualString = otherSource.name;
    }

    // Update is called once per frame
    void Update()
    {
        string label = "";
        
        switch (nameSource) {
        case NameSource.Manual:
            label = manualString;
            break;
        case NameSource.Other:
            label = otherSource.name;
            break;
        case NameSource.Self:
            label = gameObject.name;
            break;
        default:
            throw new System.Exception("Unhandled");
        }

        if (textMeshes.Length > 0) {
            foreach (TMPro.TextMeshPro tmp in textMeshes)
            {
                if (tmp != null)
                {
                    tmp.text = label;
                }
            }
        }

    }
}
