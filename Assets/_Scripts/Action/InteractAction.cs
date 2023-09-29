using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;
    private float timer;
    private float stateTimer;
    private State state;
    private Vector3 targetPosition;
    private bool canInteract;


    public event EventHandler OnInteraction;

    private enum State
    {
        Looking,
        Interacting,
        Cooloff,
    }
    //Starts the StatesTimer countdown, sets what each state do.
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
             targetPosition = MouseWorld.GetPosition();
        }
        if(!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Looking:
               
                Vector3 aimDir = (targetPosition - unit.GetWorldposition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                break;
            case State.Interacting:
                if (canInteract)
                {

                    canInteract = false;
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
            case State.Looking:
                state = State.Interacting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;

            case State.Interacting:
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
        return "Action";
    }

    //Gets the position of the AiUnit action and set the priority of the action.
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
    //Gets the possible grids that can be interact.
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                Iinteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null)
                {
                    // No Door on this GridPosition
                    continue;
                }



                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;

    }
    //Executes the action, sends the event.
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        OnInteraction?.Invoke(this, EventArgs.Empty);
        Iinteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition);
        state = State.Looking;
        float lookingStateTIme = 1f;
        stateTimer = lookingStateTIme;
        canInteract = true;
     
        interactable.Interact(OnInteractComplete);

        ActionStarts(onActionComplete);
    }
    //Action is finish
    private void OnInteractComplete()
    {
        ActionComplete();
    }
  

}
