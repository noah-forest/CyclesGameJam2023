using System;
using UnityEngine;

public class MopMover : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves towards the mouse position
    public bool trackVelocity = true; // Whether to track velocity or not

    private Vector3 targetPosition;
    private Animator animator;
    private Vector3 currentVelocity;

    private Vector3 lastPosition = Vector3.zero;

    public WipingController wiping;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Cast a ray from the camera to the mouse cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a LayerMask that includes only the layer of the object
        int layerMask = 1 << gameObject.layer;

        // Ignore raycast collision with the object itself
        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            // Get the intersection point with the world plane
            targetPosition = hit.point;
        }

        // Calculate the direction towards the target position
        Vector3 direction = targetPosition - transform.position;
        // Normalize the direction to get the unit vector
        direction.Normalize();

        // Move the object towards the target position
        transform.position = targetPosition;
        
        

        // If tracking velocity is enabled, calculate and update the current velocity
        if (trackVelocity)
        {
            Vector3 newVelocity = transform.position - lastPosition;
            currentVelocity = Vector3.Lerp(currentVelocity, newVelocity, 15f*Time.deltaTime);
            animator.SetFloat("Up", -currentVelocity.x*15);
            animator.SetFloat("Right", -currentVelocity.z*15);
            lastPosition = transform.position;
        }
    }

    // You can use this public method to get the current velocity from other scripts
    public Vector3 GetCurrentVelocity()
    {
        return currentVelocity;
    }
}