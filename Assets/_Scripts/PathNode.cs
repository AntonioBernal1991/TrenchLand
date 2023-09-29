using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This class represents individual nodes in a grid used for pathfinding.
Each node stores its grid position, movement costs (gCost, hCost, fCost), and other relevant information.
It allows setting and retrieving costs, calculates the total cost (fCost), and keeps track of the previous node in the path.*/

public class PathNode
{
    private GridPosition gridPosition; // The position of this node in the grid.
    private int gCost; // The cost of getting from the start node to this node.
    private int hCost; // The estimated cost of getting from this node to the end node.
    private int fCost; // The total cost (gCost + hCost) used to determine the best path.
    private PathNode cameFromPathNode; // The node that this node came from to reach it.
    private bool isWalkable = true; // Indicates whether this node is walkable or blocked.

    // Constructor that takes the grid position as a parameter.
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    // Override of the ToString method for debugging purposes.
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    // Getters and setters for gCost, hCost, and fCost.
    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }

    // Setters for gCost and hCost.
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    // Method to calculate fCost as the sum of gCost and hCost.
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    // Method to reset the cameFromPathNode to null.
    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    // Method to set the cameFromPathNode.
    public void SetCameFromPathNode(PathNode pathNode)
    {
        cameFromPathNode = pathNode;
    }

    // Method to get the cameFromPathNode.
    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    // Getter for gridPosition.
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    // Getter for isWalkable.
    public bool IsWalkable()
    {
        return isWalkable;
    }

    // Setter for isWalkable.
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}