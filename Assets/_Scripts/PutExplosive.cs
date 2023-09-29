using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PutExplosive : MonoBehaviour, Iinteractable
{
    [SerializeField] private GameObject putBomb; // Reference to the bomb object to be activated.
    public GridPosition gridPosition; // Grid position of the object.
    private Action onInteractionComplete; // Callback action for interaction completion.
    private bool isActive; // Flag to track if the interaction is active.
    private float timer; // Timer for interaction duration.
    public ExpCount exp; // Reference to an explosive count.

    public event EventHandler OnBombInteraction; // Event triggered on bomb interaction.

    private void Start()
    {
        // Get the grid position of the object and set it as an interactable in the level grid.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        // Check if the interaction is active.
        if (!isActive)
        {
            return;
        }

        // Update the timer and check if the interaction duration has passed.
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete(); // Complete the interaction when the timer expires.
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete; // Set the callback action.
        isActive = true; // Activate the interaction.
        timer = 0.5f; // Set the interaction duration timer.

        StartCoroutine(_PutExplosive()); // Start the coroutine for putting the explosive.
    }

    IEnumerator _PutExplosive()
    {
        yield return new WaitForSeconds(2f); // Wait for a brief delay before activating the bomb.

        putBomb.SetActive(true); // Activate the bomb object.
        exp.expCount++; // Increment the explosive count.
    }
}
