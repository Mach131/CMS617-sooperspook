using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Units per second")]
    public float speed = 1.0f;

    [Header("Testing variables")]

    [SerializeField]
    private bool smoothInput = false;
    [SerializeField]
    private bool canMoveThroughWalls = false;

    private static string HORI_INPUT = "Horizontal";
    private static string VERT_INPUT = "Vertical";

    //TODO: movement using rigidbody instead of transform?

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: double check what coordinates we're working with; this assumes x-z
        Vector3 displacement = this.getInputVector() * this.speed * Time.deltaTime;
        transform.position += displacement;
    }

    /**
     * Collects the player's movement inputs into a vector. Assumes that movement is along the X-Z plane.
     **/
    private Vector3 getInputVector()
    {
        return smoothInput ?
            new Vector3(Input.GetAxis(HORI_INPUT), 0, Input.GetAxis(VERT_INPUT)):
            new Vector3(Input.GetAxisRaw(HORI_INPUT), 0, Input.GetAxisRaw(VERT_INPUT));
    }
}
