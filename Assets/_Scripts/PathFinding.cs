using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script serves as the core pathfinding engine.
It manages a grid of nodes (represented by the PathNode class).
Provides functions to search for paths from a starting position to a destination position in the grid.
Utilizes search algorithms like A* to compute routes and determine node accessibility.
Handles obstacle detection and adjusts costs based on grid conditions.*/
public class PathFinding : MonoBehaviour
{
    // Singleton instance of the PathFinding class.
    public static PathFinding Instance { get; private set; }

    // Movement costs for pathfinding.
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private int width;
    private int height;
    private float cellSize;

    // GridSystem for managing PathNode objects.
    private GridSystem<PathNode> gridSystem;

    [SerializeField] private Transform gridDebugObjectPrefab; // Debug object for the grid.
    [SerializeField] private LayerMask obstaclesLayerMask;   // Layer mask for obstacles.

    // Awake is called when the script instance is loaded.
    public void Awake()
    {
        // Ensure there is only one instance of PathFinding.
        if (Instance != null)
        {
            Debug.LogError("Duplicate PathFinding instance. Destroying the duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Set up the grid for pathfinding with the specified width, height, and cell size.
    public void SetUp(int width, int height, float cellsize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellsize;

        // Create a new GridSystem for managing PathNode objects.
        gridSystem = new GridSystem<PathNode>(width, height, cellsize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        // Loop through the grid and check for obstacles to mark nodes as unwalkable.
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;

                // Perform a raycast to check for obstacles above the grid position.
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    // If an obstacle is found, mark the node as unwalkable.
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }


    // Find a path from the startGridPosition to the endGridPosition.
    // Returns a list of grid positions representing the path and updates the pathLength.
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();   // List of nodes to be evaluated.
        List<PathNode> closedList = new List<PathNode>(); // List of nodes already evaluated.

        PathNode startNode = gridSystem.GetGridObject(startGridPosition); // Starting node.
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);     // Ending node.
        openList.Add(startNode); // Add the starting node to the open list.

        // Reset various properties of all nodes in the grid.
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue); // Set initial G cost to a high value.
                pathNode.SetHCost(0);            // Initialize H cost to 0.
                pathNode.CalculateFCost();       // Calculate F cost (G cost + H cost).
                pathNode.ResetCameFromPathNode(); // Reset the reference to the previous node.
            }
        }

        startNode.SetGCost(0); // Set G cost of the starting node to 0.
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition)); // Calculate H cost.
        startNode.CalculateFCost(); // Calculate F cost of the starting node.

        // Continue searching while there are nodes in the open list.
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList); // Get the node with the lowest F cost.

            // If the current node is the end node, the path is found.
            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost(); // Set the path length to the F cost of the end node.
                return CalculatePath(endNode);   // Calculate and return the path.
            }

            openList.Remove(currentNode);  // Remove the current node from the open list.
            closedList.Add(currentNode);   // Add the current node to the closed list.

            // Iterate through the neighboring nodes of the current node.
            foreach (PathNode neighbourNode in GetNeighBourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue; // Skip nodes that have already been evaluated.
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue; // Skip nodes that are not walkable (obstacles).
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode); // Add the neighbor to the open list if not already present.
                    }
                }
            }
        }

        // No path found.
        pathLength = 0;
        return null;
    }

    // Calculate the heuristic distance (H cost) between two grid positions.
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        // Calculate the estimated cost to move from one position to another.
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    // Find and return the node with the lowest F cost from a list of nodes.
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFcostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFcostPathNode.GetFCost())
            {
                lowestFcostPathNode = pathNodeList[i];
            }
        }
        return lowestFcostPathNode;
    }

    // Get the PathNode at the specified grid position (x, z).
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    // Get a list of neighboring PathNodes for the given currentNode.
    private List<PathNode> GetNeighBourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Left Down
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Left Up
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));
            if (gridPosition.z - 1 >= 0)
            {
                // Right Down
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Right Up
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0)
        {
            // Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            // Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourList;
    }

    // Calculate the path by tracing back from the endNode to the startNode.
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        // Trace back the path by following the CameFromPathNode references.
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse(); // Reverse the list to get the correct order.

        List<GridPosition> gridPositionList = new List<GridPosition>();

        // Convert the list of PathNodes to a list of GridPositions.
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }

    // Set the walkable status of a grid position.
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    // Check if a grid position is walkable.
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    // Check if a path exists between two grid positions.
    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    // Get the length of the path between two grid positions.
    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
