using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    //Makes the character spin normally for the enemy, when it has no possible action.


    private Action onSpinComplete;

    private float totalSpinAmount;
 
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }
    //Executes  the action
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
     
        totalSpinAmount = 0f;
        ActionStarts(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }
    //Gets the valid grid position.
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition

        };
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }
    //Gets the position of the posible action and the priority.
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 1,
        };
    }
}
