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

    // Start is called before the first frame update
    void Start()
    {
        this.rBody = transform.GetComponent<Rigidbody>();
    }

    // FixedUpdate is usually better for physics things
    void FixedUpdate()
    {
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
