using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// The TurnSystem class manages the game's turn-based system, including
// tracking the current turn number and determining whether it's the
// player's turn or the enemy's turn. It provides methods to advance
// to the next turn and query the current turn status.
public class TurnSystem : MonoBehaviour
{
    // Singleton instance for the TurnSystem
    public static TurnSystem Instance { get; private set; }

    // Event triggered when the turn changes
    public event EventHandler OnTurnChanged;

    private int turnNumber = 1; // Current turn number
    private bool isPlayerTurn = true; // Flag to track if it's the player's turn

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of TurnSystem exists
        if (Instance != null)
        {
            Debug.LogError("There's already an instance of TurnSystem.");
            Destroy(gameObject); // Destroy the duplicate
            return;
        }
        Instance = this;
    }

    // Method to advance to the next turn
    public void NextTurn()
    {
        turnNumber++; // Increment the turn number
        isPlayerTurn = !isPlayerTurn; // Toggle between player and enemy turns
        OnTurnChanged?.Invoke(this, EventArgs.Empty); // Trigger the turn changed event
    }

    // Getter for the current turn number
    public int GetTurnNumber()
    {
        return turnNumber;
    }

    // Getter to check if it's the player's turn
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}