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
    private bool manualInput = false; 
    private bool smoothInput = false;

    private Rigidbody rBody; //it complains if you try to name this rigidbody
    private bool movingToTarget;
    private Vector3 startPosition;
    private Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        this.rBody = transform.GetComponent<Rigidbody>();
        this.movingToTarget = false;
    }

    // FixedUpdate is usually better for physics things
    void FixedUpdate()
    {
        Vector3 movement = this.getInputVector() * this.speed;
        this.rBody.velocity = movement;
        //if (movement.magnitude > 0.1f)
        //{
        //    transform.rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z), Vector3.up);
        //}

        if (movingToTarget && (this.targetPosition - this.rBody.position).magnitude <= (this.speed * Time.deltaTime * 1.5))
        {
            // Stop automatic movement if close enough to target
            this.rBody.position = targetPosition;
            this.rBody.velocity = Vector3.zero;
            movingToTarget = false;
        }
    }

    /**
     * Collects the player's movement inputs into a vector, accounting for whether or not
     * smooth input is requested. Assumes that movement is along the X-Z plane.
     **/
    private Vector3 getInputVector()
    {
        if (manualInput)
        {
            return smoothInput ?
                new Vector3(Input.GetAxis(HORI_INPUT), 0, Input.GetAxis(VERT_INPUT)) :
                new Vector3(Input.GetAxisRaw(HORI_INPUT), 0, Input.GetAxisRaw(VERT_INPUT));
        } else
        {
            // allow a location to be selected, then move towards that
            return movingToTarget ? (this.targetPosition - this.rBody.position).normalized : Vector3.zero;
        }
    }

    /**
     * Has the player start automatically move towards a given point, given that they're
     * currently set not to accept automatic input.
     **/ 
    public void moveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        //this.movingToTarget = true;
        StartCoroutine(MoveBackToStart());
    }

    IEnumerator MoveBackToStart()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startPosition;
        //this.movingToTarget = true;
    }
}
