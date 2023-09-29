using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// HealthBag: Represents a health bag that can be interacted with to heal units.
public class HealthBag : MonoBehaviour, Iinteractable
{
    // Serialized field indicating if the health bag is open.
    [SerializeField] private bool canBeOpen;

    // Grid position of the health bag.
    public GridPosition gridPosition;

    private Action onInteractionComplete; // Callback to execute after the interaction.
    private bool isActive; // Flag indicating if the health bag interaction is active.
    private float timer; // Timer for controlling the interaction duration.
    public List<Unit> units; // List of units to heal.
    private bool IsAlreadyOpen = false; // Flag to track if the health bag is already open.

    private void Awake()
    {
        // Find and add references to soldier units if they exist.
        GameObject soldier1 = GameObject.Find("Soldier1");
        GameObject soldier2 = GameObject.Find("Soldier2");
        GameObject soldier3 = GameObject.Find("Soldier3");
        GameObject soldier4 = GameObject.Find("Soldier4");

        if (soldier1 != null)
        {
            units.Add(soldier1.GetComponent<Unit>());
        }
        if (soldier2 != null)
        {
            units.Add(soldier2.GetComponent<Unit>());
        }
        if (soldier3 != null)
        {
            units.Add(soldier3.GetComponent<Unit>());
        }
        if (soldier4 != null)
        {
            units.Add(soldier4.GetComponent<Unit>());
        }
    }

    private void Start()
    {
        // Initialize grid position, set it as unwalkable, and register as an interactable object.
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        PathFinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        // If the health bag can be open, open it.
        if (canBeOpen)
        {
            OpenBox();
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
            onInteractionComplete(); // Execute the interaction completion callback.
        }
    }

    // Implementation of the Interact method from the IInteractable interface.
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        // Start the coroutine to open the health bag.
        StartCoroutine(OpenBox());

        // Destroy the health bag object after 2 seconds.
        Destroy(this.gameObject, 2);
    }

    // Coroutine to open the health bag and heal units.
    IEnumerator OpenBox()
    {
        yield return new WaitForSeconds(1f);

        // Check if the health bag is already open.
        if (IsAlreadyOpen == false)
        {
            canBeOpen = true;

            // Heal each unit in the list.
            foreach (Unit unit in units)
            {
                if (unit != null)
                {
                    unit.Heal(200);
                }
            }

            // Set the health bag as already open.
            IsAlreadyOpen = true;
        }
        else if (IsAlreadyOpen == true)
        {
            canBeOpen = true;
        }
    }
}