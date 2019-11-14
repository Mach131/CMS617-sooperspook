using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPhysics : MonoBehaviour
{
    public GameObject[] objs;
    private Dictionary<GameObject, List<PosAndRot>> history = new Dictionary<GameObject, List<PosAndRot>>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in objs)
        {
            history.Add(obj, new List<PosAndRot>());
            obj.GetComponent<Rigidbody>().AddExplosionForce(20, transform.position + Vector3.right * 5, 20, 0, ForceMode.Impulse);
        }
        StartCoroutine(WaitAndPrintHistory());
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject obj in history.Keys)
        {
            history[obj].Add(new PosAndRot(obj.transform, Time.time));
        }
    }

    IEnumerator WaitAndPrintHistory()
    {
        yield return new WaitForSeconds(5);
        PrintHistory();
    }

    void PrintHistory()
    {
        foreach(GameObject obj in history.Keys)
        {
            string output = obj.name + "\n";
            foreach (PosAndRot par in history[obj])
            {
                output += par + "\n";
            }
            Debug.Log(output);
        }
    }

    class PosAndRot
    {
        public Vector3 pos;
        public Vector3 rot;
        public float time;

        public PosAndRot(Transform transform, float t)
        {
            pos = transform.localPosition;
            rot = transform.localEulerAngles;
            time = t;
        }

        public override string ToString()
        {
            return "t: " + time + ", pos: " + pos + ", rot: " + rot;
        }
    }
}
