using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro; // Reference to a TextMeshPro component for displaying the coordenates x y of the grid.

    private object gridObject; // Reference to the grid object associated with this debug object

    // Method for setting the grid object associated with this debug object
    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Update the TextMeshPro text with the string representation of the associated grid object
        textMeshPro.text = gridObject.ToString();
    }
}