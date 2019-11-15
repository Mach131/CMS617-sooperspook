using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadAnimations : MonoBehaviour
{
    public Transform head;
    public Transform testthis;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LookAt(testthis));
    }

    // Update is called once per frame
    void Update()
    {
        
    }    

    public IEnumerator LookAt(Transform thisthing)
    {   
        while(true)
        {
            if (thisthing == null) {
                head.rotation = Quaternion.Euler(-10.6, 110.414, 21.2);
            }
            head.LookAt(thisthing);
            yield return null;
        }   
    }
}
