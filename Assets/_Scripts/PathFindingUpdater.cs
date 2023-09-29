using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 This script manages dynamic updates to the pathfinding grid.
Subscribes to the OnAnyDestroyed event of the DestructibleCrate class.
When any destructible crate is destroyed in the scene, 
this script is triggered and marks the crate's position as passable in the pathfinding grid.
 */

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to the OnAnyDestroyed event of DestructibleCrate when this script starts.
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    // This method is called when any DestructibleCrate object is destroyed.
    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        // Cast the sender object to a DestructibleCrate instance.
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;

        // Set the grid position of the destroyed crate as walkable in the pathfinding grid.
        PathFinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
