using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractableBox : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool canBeOpened;// Flag to track if the box is open or closed
    public GridPosition gridPosition;// Position of the box on the game grid
    public Animator animator;// Reference to the Animator component
    private Action onInteractionComplete;// Callback function for interaction completion
    private bool isActive;// Flag to track if the box's interaction is active
    private float timer;// Timer for controlling the interaction duration

    private bool IsAlreadyOpen = false;// Flag to track if the box was already opened

    private void Awake()
    {
        animator = GetComponent<Animator>();// Initialize the Animator reference
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);// Get the grid position of the box
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);// Mark the grid position as unwalkable
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);// Register the box as interactable in the grid

        if (canBeOpened)
        {
            OpenBox();// If the box can be opened , perform the opening animation
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isActive = false;
            onInteractionComplete();// Invoke the interaction completion callback
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;// Set the interaction completion callback
        isActive = true;
        timer = 0.5f;// Set the interaction timer

        StartCoroutine(OpenBox());// Start the box opening coroutine
    }

    IEnumerator OpenBox()
    {
        yield return new WaitForSeconds(1f);// Wait for 1 second before opening the box

        if (!IsAlreadyOpen)
        {
            canBeOpened = true;
            animator.SetBool("IsAmmoOpen", canBeOpened);// Open the box and trigger an animation
            Unit.numGrenades++;// Increase the player's grenade count
            IsAlreadyOpen = true;// Mark the box as already opened
        }
        else
        {
            canBeOpened = true;
            animator.SetBool("IsAmmoOpen", canBeOpened);// If already open, just play the animation
        }
    }
}