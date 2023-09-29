using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The "UnitSelectedVisual" class is responsible for visually indicating whether a unit is selected.
// It attaches to a unit and toggles the visibility of its mesh renderer based on whether the unit is selected.
// It listens for the "OnSelectedUnitChanged" event from the "UnitActionSystem" and updates the visual accordingly.
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit; // The unit this visual component is associated with.
    private MeshRenderer meshRenderer; // Reference to the mesh renderer for visibility control.

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateVisual();
    }

    private void Update()
    {
        // Subscribe to the "OnSelectedUnitChanged" event to detect changes in the selected unit.
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        // When the selected unit changes, update the visual to show or hide based on selection.
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Check if the associated unit is the currently selected unit.
        // If it is, enable the mesh renderer to show the visual indication; otherwise, disable it.
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the "OnSelectedUnitChanged" event when this component is destroyed.
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}

