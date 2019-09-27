using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 cameraOffsetDirection = new Vector3(0, 0.85f, -0.5f);
    private float cameraOffsetDistance = 8;
    private float cameraOffsetDistanceMultiplier = 1;
    private float leadingFactor = 0.4f;
    private float followDelay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetTargetPosition(); // Initializes camera position
        transform.rotation = GetTargetRotation(); // Initializes camera rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Zoom") > 0.5f)
        {
            cameraOffsetDistanceMultiplier = 2f;
        }
        else
        {
            cameraOffsetDistanceMultiplier = 1f;
        }
        UpdateCameraTransform();
    }

    /*
     * Returns the position the camera is trying to reach. The camera tries to move
     * such that it follows its targets based on their weights, and takes into
     * account their velocities, allowing it to lead moving objects.
     */
    Vector3 GetTargetPosition()
    {
        Vector3 targetPos = Vector3.zero;
        float totalWeight = 0;
        foreach(CameraTarget target in CameraTarget.targets)
        {
            totalWeight += target.weight;

            Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
            Vector3 targetLeadingOffset = Vector3.zero;
            if (targetRigidbody != null)
            {
                targetLeadingOffset = targetRigidbody.velocity * leadingFactor;
            }
            targetPos += (target.transform.position + targetLeadingOffset) * target.weight;
        }
        if (totalWeight < 0.001f)
        {
            return Vector3.zero;
        }
        return targetPos / totalWeight + cameraOffsetDirection * cameraOffsetDistance * cameraOffsetDistanceMultiplier;
    }

    /*
     * Returns the rotation needed for following objects around the scene. This
     * should be a fixed rotation (animal crossing style).    
     */   
    Quaternion GetTargetRotation()
    {
        cameraOffsetDirection.Normalize();
        return Quaternion.LookRotation(-cameraOffsetDirection, Vector3.up);
    }

    void UpdateCameraTransform()
    {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, GetTargetPosition(), ref velocity, followDelay); // Moves camera to target position over time
        transform.rotation = GetTargetRotation();
    }
}
