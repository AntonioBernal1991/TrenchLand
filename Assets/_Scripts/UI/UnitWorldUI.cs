
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
// This class manages the user interface elements related to a unit's action points and health bar within the game world.
// It listens for events related to changes in action points and health and updates the corresponding UI elements accordingly.
// This allows the player to see the unit's action points and health status while playing the game.
public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText; // Reference to TextMeshProUGUI for displaying action points
    [SerializeField] private Unit unit; // Reference to the unit associated with this UI
    [SerializeField] private Image healthBarImage; // Reference to the health bar's Image component
    [SerializeField] private HealthSystem healthSystem; // Reference to the health system of the unit

    private void Start()
    {
        // Attach event handlers and update UI elements
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        // Update the displayed action points based on the unit's action points
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        // Update action points text when the unit's action points change
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        // Update the fill amount of the health bar based on the unit's health normalized value
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        // Update the health bar when the unit takes damage
        UpdateHealthBar();
    }
}