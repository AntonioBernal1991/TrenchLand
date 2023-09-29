using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;



    [SerializeField] private int maxMoveDistance = 4;
   
    private List<Vector3> positionList;
    private int currentPositionIndex;
   
    //Calculates the direccion to the destiny , sets the speed, and the moves the unit to its objective in a somooth way 
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
  
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete();

            }
           
        }

      
    }
    //Executes the acction , gets the Path wich the unit has to go through.
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition,out int pathlength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();
        foreach(GridPosition pathGridPosition in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
     
         

        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStarts(onActionComplete);
    }
    //Gets the grid  where the unit  can walk through.
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance;x++)
        { 
         
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                if(!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!PathFinding.Instance.HasPath(unitGridPosition,testGridPosition))
                {
                    continue;
                }
                int pathfindingDistanceMultiplier = 10;
                if (PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition)
                    > maxMoveDistance * pathfindingDistanceMultiplier)
                {
                    continue;
                }
                    validGridPositionList.Add(testGridPosition);
            }
        
        }

        return validGridPositionList;
    }


    public override string GetActionName()
    {
        return "Move";
    }

    //Gets the position of the AIUnit and the distance to its  target and sets the  priority of action.
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
