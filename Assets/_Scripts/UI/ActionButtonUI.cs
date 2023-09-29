using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    //Displays different Action buttons of each unit.

    [SerializeField] private TextMeshProUGUI textMeshPro; // Reference to a TextMeshProUGUI component for displaying text
    [SerializeField] private Button button; // Reference to a Button component
    [SerializeField] private GameObject selectedGameObject; // Reference to a GameObject to indicate the selected state

    private BaseAction BaseAction; // Reference to the associated BaseAction

    // Method for setting the associated BaseAction and configuring the UI elements
    public void SetBaseAction(BaseAction baseAction)
    {
        this.BaseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper(); // Display the action name in uppercase

        // Add a listener to the button's click event to set the selected action in the UnitActionSystem
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    // Update the visual state of the button to indicate if it's selected
    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

        // Activate or deactivate the selectedGameObject based on whether this action button is selected
        selectedGameObject.SetActive(selectedBaseAction == BaseAction);
    }
}
