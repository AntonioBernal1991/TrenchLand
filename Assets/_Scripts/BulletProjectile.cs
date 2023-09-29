using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer; // Reference to the TrailRenderer component

    private Vector3 targetPosition; // Stores the target position for the projectile

    // Method to set up the position when the projectile is created
    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        // Calculate the normalized direction vector from current position to the target position
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        // Calculate the initial distance between current position and target position
        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 1000f; // Constant speed at which the projectile moves

        // Update the position of the projectile based on direction, speed, and time
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // Calculate the distance between current position after moving and target position
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        // Check if the projectile has passed the target position
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition; // Set position to target position to avoid overshooting

            trailRenderer.transform.parent = null; // Detach TrailRenderer from its parent (if any)
            Destroy(gameObject); // Destroy the projectile GameObject
        }
    }
}