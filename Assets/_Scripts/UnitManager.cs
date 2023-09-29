using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// The "UnitManager" class is responsible for managing units in the game. 
// It maintains lists of all units, friendly units, and enemy units.
// It also listens for unit spawn and death events and updates these lists accordingly.
// Other parts of the game can access these lists through the provided public methods.
// This class follows the singleton pattern to ensure there's only one instance throughout the game.

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; } // Singleton instance.

    private List<Unit> unitList; // List to store all units.
    private List<Unit> friendlyUnitList; // List to store friendly units.
    private List<Unit> enemyUnitList; // List to store enemy units.

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's a duplicate UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this; // Set the singleton instance.

        unitList = new List<Unit>(); // Initialize the unit list.
        friendlyUnitList = new List<Unit>(); // Initialize the friendly unit list.
        enemyUnitList = new List<Unit>(); // Initialize the enemy unit list.
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit); // Add the unit to the unit list.

        if (unit.IsEnemy())
        {
            enemyUnitList.Add(unit); // If the unit is an enemy, add it to the enemy unit list.
        }
        else
        {
            friendlyUnitList.Add(unit); // If the unit is friendly, add it to the friendly unit list.
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit); // Remove the unit from the unit list.

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit); // If the unit is an enemy, remove it from the enemy unit list.
        }
        else
        {
            friendlyUnitList.Remove(unit); // If the unit is friendly, remove it from the friendly unit list.
        }
    }

    public List<Unit> GetUnitList()
    {
        return unitList; // Get the list of all units.
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList; // Get the list of friendly units.
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList; // Get the list of enemy units.
    }
}
