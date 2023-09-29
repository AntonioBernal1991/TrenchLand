using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// EnemyAI: Controls the behavior of enemy units during their turns 
public class EnemyAI : MonoBehaviour
{
    // Enum representing the different states of the enemy AI.
    private enum State
    {
        WaitingForEnemyTurn, // Waiting for the enemy's turn.
        TakingTurn, // Performing actions during the enemy's turn.
        Busy, // Busy state, e.g., when performing actions.
    }

    private State state; // Current state of the enemy AI.
    private float timer; // Timer for controlling AI actions.

    private void Awake()
    {
        state = State.WaitingForEnemyTurn; // Initialize the state as waiting for the enemy's turn.
    }

    void Start()
    {
        // Subscribe to the turn change event from the TurnSystem.
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return; // Exit the update if it's not the enemy's turn.
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn(); // End the enemy's turn if no actions can be taken.
                    }
                }
                break;

            case State.Busy:
                break;
        }
    }

    // Set the state to "TakingTurn" and initialize the timer for AI actions.
    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    // Event handler for the turn change event from TurnSystem.
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn; // Set the state to "TakingTurn" when it's the enemy's turn.
            timer = 2f; // Initialize the timer for enemy actions.
        }
    }

    // Try to take AI actions for all enemy units.
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Actionn ");

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    // Try to take AI actions for a specific enemy unit.
    private bool TryTakeEnemyAIAction(Unit enemyunit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyunit.GetBaseActionArray())
        {
            if (!enemyunit.CanSpendActionPointsToTakeAction(baseAction))
            {
                continue; // Skip actions that can't be performed.
            }

            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if (bestEnemyAIAction != null && enemyunit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false; // No suitable action can be taken.
        }
    }
}
