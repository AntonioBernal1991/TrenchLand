using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// GrenadeProjectile: Represents a grenade projectile that explodes upon reaching a target.
public class GrenadeProjectile : MonoBehaviour
{
    // Event triggered when any grenade explodes.
    public static event EventHandler OnAnyGrenadeExploded;

    // Serialized field for the grenade explosion visual effect prefab.
    [SerializeField] private Transform grenadeExplosionVFXPrefab;

    // Serialized field for the trail renderer of the grenade's trajectory.
    [SerializeField] private TrailRenderer trailRenderer;

    // Serialized field for the animation curve defining the grenade's arc.
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition; // The target position where the grenade will explode.
    private Action onGrenadeBehaviourComplete; // Callback to execute when the grenade behavior is complete.
    private float totalDistance; // The total distance from the starting position to the target.
    private Vector3 positionXZ; // The position of the grenade in the XZ plane.

    private void Update()
    {
        // Calculate the direction to move the grenade.
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        // Define the movement speed of the grenade.
        float moveSpeed = 20f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        // Calculate the current distance between the grenade and the target.
        float distance = Vector3.Distance(positionXZ, targetPosition);

        // Normalize the distance and evaluate the animation curve to determine the Y position (arc) of the grenade.
        float distanceNormalized = 1 - distance / totalDistance;
        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

        // Update the grenade's position in the world.
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        // Define a threshold for when the grenade has reached the target.
        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            // Determine the damage radius of the grenade explosion.
            float damageRadius = 7f;

            // Find all colliders within the damage radius.
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            // Iterate through colliders to apply damage to units and destroy crates.
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    // Apply random damage to units within the explosion radius.
                    targetUnit.Damage(Random.Range(50, 61));
                }
                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    // Damage destructible crates.
                    destructibleCrate.Damage();
                }
            }

            // Trigger the OnAnyGrenadeExploded event to notify other parts of the game.
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            // Detach the trail renderer from the grenade.
            trailRenderer.transform.parent = null;

            // Instantiate the grenade explosion visual effect at the target position.
            Instantiate(grenadeExplosionVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);

            // Instantiate a grenade audio variant and destroy it after 6 seconds.
            GameObject grenade = Instantiate(Resources.Load("GrenadeAudioVariant", typeof(GameObject))) as GameObject;
            Destroy(grenade, 6);

            // Destroy the grenade game object.
            Destroy(gameObject);

            // Execute the grenade behavior completion callback.
            onGrenadeBehaviourComplete();
        }
    }

    // Setup the grenade's behavior with a target grid position and a completion callback.
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        // Set the completion callback and calculate the target position.
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
