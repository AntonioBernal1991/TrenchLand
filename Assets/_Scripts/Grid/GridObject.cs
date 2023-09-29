using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem; // Reference to the grid system to which this object belongs
    private GridPosition gridPosition; // The position of this object within the grid
    private Unit unit; // Reference to a unit associated with this grid object (if any)
    private List<Unit> unitList; // List of units occupying the same grid position
    private Iinteractable interactable; // Reference to an interactable object associated with this grid object (if any)

    // Constructor for creating a new GridObject
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    // Override the ToString method to provide a custom string representation of the GridObject
    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList)
        {
            unitString += unit + "\n";
        }
        return gridPosition.ToString() + "\n" + unitString;
    }

    // Add a unit to this grid object
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    // Remove a unit from this grid object
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    // Get a list of units occupying this grid object
    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    // Check if there are any units occupying this grid object
    public bool HasAnyunit()
    {
        return unitList.Count > 0;
    }

    // Get the primary unit occupying this grid object (if any)
    public Unit GetUnit()
    {
        if (HasAnyunit())
        {
            return unitList[0];
        }
        else
        {
            return null;
        }
    }

    // Get the interactable object associated with this grid object (if any)
    public Iinteractable GetInteractable()
    {
        return interactable;
    }

    // Set the interactable object associated with this grid object
    public void SetInteractable(Iinteractable interactable)
    {
        this.interactable = interactable;
    }
}