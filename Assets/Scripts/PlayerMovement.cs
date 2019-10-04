using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static string HORI_INPUT = "Horizontal";
    private static string VERT_INPUT = "Vertical";

    [Tooltip("Units per second")]
    public float speed = 1.0f;

    [Header("Testing variables")]
    [SerializeField]
    private bool smoothInput = false;

    private Rigidbody rBody; //it complains if you try to name this rigidbody
    private List<Bounds> thresholds;
    private Vector3 lastValidPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.rBody = transform.GetComponent<Rigidbody>();

        /* With the thresholds as triggers, it seems like the only ways to keep the player
         * inside them is to either try to reset the position when OnTriggerExit is called
         * (which has more ambiguous timing) or to do this; neither is particularly clean,
         * and it may be better to consider just surrounding the play area with normal
         * invisible colliders instead
         */
        this.thresholds = new List<Bounds>();
        GameObject[] thresholdObjects = GameObject.FindGameObjectsWithTag("Threshold");
        foreach(GameObject thresholdObject in thresholdObjects)
        {
            this.thresholds.Add(thresholdObject.GetComponent<BoxCollider>().bounds);
        }
        this.lastValidPosition = transform.position;
    }

    // FixedUpdate is usually better for physics things
    void FixedUpdate()
    {
        foreach (Bounds thresholdBound in this.thresholds)
        {
            if (!thresholdBound.Contains(transform.position))
            {
                //return the player in-bounds if they escape
                transform.position = lastValidPosition;
            }
        }
        lastValidPosition = transform.position;

        Vector3 movement = this.getInputVector() * this.speed;
        this.rBody.velocity = movement;
    }

    /**
     * Collects the player's movement inputs into a vector, accounting for whether or not
     * smooth input is requested. Assumes that movement is along the X-Z plane.
     **/
        private Vector3 getInputVector()
    {
        return smoothInput ?
            new Vector3(Input.GetAxis(HORI_INPUT), 0, Input.GetAxis(VERT_INPUT)):
            new Vector3(Input.GetAxisRaw(HORI_INPUT), 0, Input.GetAxisRaw(VERT_INPUT));
    }
}
