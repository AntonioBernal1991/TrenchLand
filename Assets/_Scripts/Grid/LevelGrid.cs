using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{
    // Singleton pattern instance
    public static LevelGrid Instance { get; private set; }

    // Event to notify when any unit is moved to a different grid position
    public event EventHandler OnAnyUnitMovedGridPosition;

    // Serialized prefab for visualizing grid cells in the Unity Inspector
    [SerializeField] private Transform gridDebugObjectPrefab;

    // Serialized grid dimensions and cell size
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null)
        {
            Debug.LogError("There's already an instance of LevelGrid. Destroying the duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize the grid system
        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));

        // Create visual debug objects for the grid
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Set up pathfinding with the grid dimensions and cell size
        PathFinding.Instance.SetUp(width, height, cellSize);
    }

    // Add a unit to a specific grid position
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    // Get a list of units at a specific grid position
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    // Remove a unit from a specific grid position
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    // Move a unit from one grid position to another and trigger an event
    public void UnitMoveGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    // Get the grid position of a world position
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }

    // Get the world position of a grid position
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    // Check if a grid position is valid within the grid bounds
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    // Get the width of the grid
    public int GetWidth() => gridSystem.GetWidth();

    // Get the height of the grid
    public int GetHeight() => gridSystem.GetHeight();

    // Check if there is any unit on a specific grid position
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyunit();
    }

    // Get the unit at a specific grid position
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    // Get the interactable object at a specific grid position
    public Iinteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    // Set an interactable object at a specific grid position
    public void SetInteractableAtGridPosition(GridPosition gridPosition, Iinteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}