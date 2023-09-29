using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// DestructibleCrate: Represents a destructible crate in the game world. 
// When damaged or destroyed, it triggers an explosion effect and notifies 
// other parts of the game using the OnAnyDestroyed event.
public class DestructibleCrate : MonoBehaviour
{
    // Event to notify when any instance of this class is destroyed.
    public static event EventHandler OnAnyDestroyed;

    // Serialized field to assign a prefab for the destroyed crate.
    [SerializeField] private Transform crateDestroyedPrefab;

    // Stores the grid position of the crate.
    private GridPosition gridPosition;

    private void Start()
    {
        // Initialize gridPosition by getting the grid position from the LevelGrid singleton.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    // Public method to retrieve the grid position of the crate.
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // Method to handle damage or destruction of the crate.
    public void Damage()
    {
        // Instantiate the crateDestroyedPrefab to create an explosion effect.
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        // Apply an explosion force to the children of the destroyed crate.
        ApplyExplosionToChildren(crateDestroyedTransform, 400f, transform.position, 30f);

        // Destroy the original crate game object.
        Destroy(gameObject);

        // Invoke the OnAnyDestroyed event to notify other parts of the game.
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    // Recursive method to apply explosion force to all child objects.
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            // Check if the child has a Rigidbody component.
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                // Apply explosion force to the child's Rigidbody.
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            // Recursively call this method for child's children.
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
