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

    private void Awake() {
        otherSource = gameObject;
        manualString = otherSource.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

        foreach( TMPro.TextMeshPro tmp in textMeshes ) {
            tmp.text = label;
        }

    }
}
