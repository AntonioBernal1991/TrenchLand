using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*This class handles debugging visualization within the Unity scene.
Inherits from GridDebugObject and displays useful debugging information about grid nodes, such as g, h, and f costs.
Aids developers in inspecting grid nodes and understanding how routes are being calculated.*/

public class PathFindingDebugObject : GridDebugObject
{
    // Text objects for displaying G, H, and F costs.
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro fCostText;

    private PathNode pathNode;

    // Override the base class method to set the grid object.
    public override void SetGridObject(object gridObject)
    {
        // Call the base class method to set the grid object.
        base.SetGridObject(gridObject);

        // Cast the grid object to a PathNode.
        pathNode = (PathNode)gridObject;
    }

    // Override the Update method to update text values.
    protected override void Update()
    {
        // Call the base class Update method.
        base.Update();

        // Update the text objects with G, H, and F costs from the PathNode.
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
    }
}