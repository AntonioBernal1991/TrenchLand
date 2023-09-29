using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ShootAction : BaseAction
{

    public  event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    private State state;
    private int maxShootDistance = 8;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    [SerializeField] private LayerMask obstaclesLayerMask;

    //Starts the StatesTimer countdown, sets what each state do.
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

       stateTimer -= Time.deltaTime;
       switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldposition() - unit.GetWorldposition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.Shooting:  
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
             
                break;
        }
        if (stateTimer <= 0f)
        {
            NextState();
        }

    }
    //Changes to next state , and sets timer again.
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                    break;

            case State.Shooting:
                    state = State.Cooloff;
                    float CooloffStateTime = 0.5f;
                    stateTimer = CooloffStateTime;
                    break;

            case State.Cooloff:
                ActionComplete();
                break;
        }

        Debug.Log(state);
    }

    //Sends event of shooting occured and sets the probabilty hitting target and the damgae.
    private void Shoot()
    {
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        int rate = Random.Range(0, 11);
        if(rate < 4)
        {
            targetUnit.Damage(0);
            StartCoroutine(FailText());
        }
        if (rate >= 4)
        {
            targetUnit.Damage(40);
        }



    }


    public override string GetActionName()
    {
        return "Shoot";
    }
    //Gets the valid Grid of the unit that is going to shoot
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        print("coozi");
        return GetValidActionGridPositionList(unitGridPosition);
    }
    //Gets the grid  where the unit  shoot range.
    public  List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {

            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    continue;
                }
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldposition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDir,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldposition()),
                        obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }

        }

        return validGridPositionList;
    }
    //Executes  the action.
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;
        ActionStarts(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    //Gets target and action priority for the enemy.
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    //Counts the grids between the unit and the target unit.
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
       return  GetValidActionGridPositionList(gridPosition).Count;
    }
  //Shows a Fail text when the shot is failed.
  IEnumerator FailText()
    {
      
        targetUnit.fail.SetActive(true);
        yield return new WaitForSeconds(1);
        targetUnit.fail.SetActive(false);

    }


}
