using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    //Shows a Text of Busy state during a unit action
    private void Start()
    {
        // Register a listener for the OnBusyChanged event of the UnitActionSystem
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        // Hide the UI element initially
        Hide();
    }

    // Show the UI element
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Hide the UI element
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // This method is called when the busy state in the UnitActionSystem changes
    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        // If the system is busy, show the UI element; otherwise, hide it
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}