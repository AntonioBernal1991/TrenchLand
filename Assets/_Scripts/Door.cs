using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Door: Represents an interactive door in the game.
public class Door : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOpen; // Flag indicating if the door is open.
    [SerializeField] private GameObject blackPlane; // The black plane used to block vision when the door is closed.
    [SerializeField] private List<GameObject> enemyList; // List of enemies affected by the door's state.
    public GridPosition gridPosition; // Grid position of the door.
    public Animator animator; // Reference to the door's animator component for animation control.
    private Action onInteractionComplete; // Callback to execute after the door interaction.
    private bool isActive; // Flag indicating if the door is actively interacting.
    private float timer; // Timer for controlling the interaction duration.

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Initialize the animator reference.
    }

    public static event EventHandler OnDoorInteraction; // Event triggered when a door is interacted with.

    private void Start()
    {
        // Initialize gridPosition and register the door as an interactable object in the level grid.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        blackPlane.SetActive(true); // Activate the black plane.

        // Check if the door is initially open or closed and start the appropriate coroutine.
        if (isOpen)
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            StartCoroutine(CloseDoor());
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return; // Exit the update loop if the door is not actively interacting.
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isActive = false; // Deactivate the interaction after the specified time.
            onInteractionComplete(); // Execute the interaction completion callback.
        }
    }

    // Implementation of the Interact method from the IInteractable interface.
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete; // Set the interaction completion callback.
        isActive = true; // Activate the interaction.
        timer = 0.5f; // Set the interaction duration timer.

        // Check if the door is open or closed and start the appropriate coroutine.
        if (isOpen)
        {
            StartCoroutine(CloseDoor());
        }
        else
        {
            StartCoroutine(OpenDoor());
        }
    }

    // Coroutine to open the door.
    IEnumerator OpenDoor()
    {
        OnDoorInteraction?.Invoke(this, EventArgs.Empty); // Trigger the door interaction event.
        yield return new WaitForSeconds(1f);
        isOpen = true;
        animator.SetBool("IsOpen", isOpen); // Set the animator parameter to control the door's animation.
        SetActiveGameObjectList(enemyList, true); // Activate the enemy objects affected by the open door.
        blackPlane.SetActive(false); // Deactivate the black plane to allow visibility.
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, true); // Update walkability on the grid.
    }

    // Coroutine to close the door.
    IEnumerator CloseDoor()
    {
        OnDoorInteraction?.Invoke(this, EventArgs.Empty); // Trigger the door interaction event.
        yield return new WaitForSeconds(1f);
        isOpen = false;
        animator.SetBool("IsOpen", isOpen); // Set the animator parameter to control the door's animation.
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false); // Update walkability on the grid.
    }

    // Helper method to set the active state of a list of GameObjects.
    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }
}
