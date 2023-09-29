using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 3;

    // Events to handle various unit-related actions
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public GameObject fail;

    public TextMeshProUGUI textMeshPro;
    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;

    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;
    public static int numGrenades = 3;

    private void Awake()
    {
        // Initialize health system and actions array
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();

        // Ensure the unit persists between scenes
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Register the unit with the grid system upon spawning
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        // Subscribe to unit and health-related events
        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        // Update the grid position of the unit
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }

        // Update action points and display grenade count for non-enemy units
        if (!isEnemy)
        {
            textMeshPro.text = "" + numGrenades;
        }
    }

    // Method to get a specific type of action from the unit's actions
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    // Getter for the unit's grid position
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // Getter for the unit's world position
    public Vector3 GetWorldposition()
    {
        return transform.position;
    }

    // Getter for the unit's actions
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    // Attempt to spend action points to perform an action
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    // Check if there are enough action points to perform an action
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointsCost();
    }

    // Reduce action points by a specified amount
    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    // Getter for the unit's remaining action points
    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Reset action points at the start of the unit's turn
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Check if the unit is an enemy
    public bool IsEnemy()
    {
        return isEnemy;
    }

    // Inflict damage on the unit
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    // Heal the unit
    public void Heal(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        // Handle unit death by removing it from the grid and deactivating it
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        StartCoroutine(Dissapear());
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    // Get the unit's health as a normalized value (between 0 and 1)
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    // Coroutine to deactivate the unit after a delay
    IEnumerator Dissapear()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
}