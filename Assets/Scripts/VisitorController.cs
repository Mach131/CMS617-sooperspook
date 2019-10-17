using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorController : MonoBehaviour
{
    private Dictionary<Interactable, HashSet<string>> states = new Dictionary<Interactable, HashSet<string>>();
    private float totalFear = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyFear(float fearAmount)
    {
        totalFear += fearAmount;

        //TODO: this code should be called whenever the visitor notices a prop, not in ApplyFear
        //if (!states.ContainsKey(source))
        //{
        //    states.Add(source, source.GetState());
        //}
        //else
        //{
        //    states[source] = source.GetState();
        //}
    }
}
