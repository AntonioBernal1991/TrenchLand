using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton; // Reference to the "End Turn" button
    [SerializeField] private TextMeshProUGUI turnNumberText; // Reference to a TextMeshProUGUI component for displaying the current turn number
    [SerializeField] private GameObject enemyTurnVisualGameObject; // Reference to a GameObject for visualizing the enemy's turn

    // This method is called when the GameObject is started (e.g., when the scene is loaded)
    private void Start()
    {
        // Add a click listener to the "End Turn" button to advance the turn in the TurnSystem
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        // Register a listener for the OnTurnChanged event of the TurnSystem
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        // Update the initial state of UI elements
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    // This method is called when the turn changes in the TurnSystem
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Update the displayed turn number, enemy turn visual, and the visibility of the "End Turn" button
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    // Update the displayed turn number
    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    // Update the visibility of the enemy turn visual GameObject
    private void UpdateEnemyTurnVisual()
    {
        // Show the enemy turn visual if it's not the player's turn, hide it otherwise
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    // Update the visibility of the "End Turn" button
    private void UpdateEndTurnButtonVisibility()
    {
        // Show the "End Turn" button if it's the player's turn, hide it otherwise
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}