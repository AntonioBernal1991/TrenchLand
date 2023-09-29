using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    //Throws grenade.

    public event EventHandler OnThrowGrenade;
    private enum State
    {
        Aiming,
        Throwing,
        Cooloff,
    }

    [SerializeField] private Transform grenadeProjectilePrefab;

    private Vector3 targetPosition;
    private int maxThrowDistance = 5;
    private State state;
    private float stateTimer;
    private bool canThrowGrenade;
    public Transform hand;
    private Unit unit1;
  


    private void Awake()
    {
        unit1 = GetComponent<Unit>();
    }
    //Gets cellgrid the player wants to throw a grenade
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            targetPosition = MouseWorld.GetPosition();
        }
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;

        //Starts the StatesTimer countdown, sets what each state do.
        switch (state)
        {
            case State.Aiming:
                
                Vector3 aimDir = (targetPosition - unit1.GetWorldposition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;

            case State.Throwing:
                if (canThrowGrenade)
                {
                    
                    canThrowGrenade = false;
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
    //Changes state of the action and sets the timer.
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Throwing;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;

            case State.Throwing:
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

   
    public override string GetActionName()
    {
        return "Grenade";
    }
    //Gets the position of the AIUnit action and set the priority of the action.
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    //Gets the possible grids where the grenade can be throwned.
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit1.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
    
    //Executes the action, sends the event .
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
       
        if (Unit.numGrenades > 0)
        {
            OnThrowGrenade?.Invoke(this, EventArgs.Empty);
         

            state = State.Aiming;

            float aimingStateTime = 1f;
            stateTimer = aimingStateTime;
            canThrowGrenade = true;
            StartCoroutine(InstatiateGrenade(gridPosition, onActionComplete));
            Unit.numGrenades--;

            ActionStarts(onActionComplete);
        }
        else
        {
            
            ActionStarts(onActionComplete);
        }     
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
    //Instantiate a grenade
    IEnumerator InstatiateGrenade(GridPosition gridPosition, Action onActionComplete)
    {
        yield return new WaitForSeconds(0.68f);
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, hand.position, Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
    }
  

}

