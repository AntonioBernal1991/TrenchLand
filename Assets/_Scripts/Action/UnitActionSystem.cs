
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Controls the selection of the unit and its actions.
public class UnitActionSystem : MonoBehaviour
{
   


    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;




    private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;
    
    //Gets an available unit to start the game with.
    private void Awake()
    {
        GameObject soldier1 = GameObject.Find("Soldier1");
        GameObject soldier2 = GameObject.Find("Soldier2");
        GameObject soldier3 = GameObject.Find("Soldier3");
        GameObject soldier4 = GameObject.Find("Soldier4");


        if (soldier1 != null)
        {
            selectedUnit = soldier1.GetComponent<Unit>();

        }
        else if (soldier2 != null)
        {
            selectedUnit = soldier2.GetComponent<Unit>();

        }
        else if (soldier3 != null)
        {
            selectedUnit = soldier3.GetComponent<Unit>();

        }
        else if (soldier4 != null)
        {
            selectedUnit = soldier4.GetComponent<Unit>();

        }

        {
            if(Instance != null)

            {
                Debug.LogError("Theres a doozi");
                Destroy(gameObject);
                return;

            }
            Instance = this;
       

        }
       
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {

        if(isBusy)
        {
            return;
        }
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }


        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }



        if (TryHandleUnitSelection())
        {   
            return;
        }
        HandleSelectedAction();
       
    }
    //Gets the action selected when the mouse clicks  the UIActionButtons.
    private void HandleSelectedAction()
    {
       
        if(InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if(!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
                              
            }
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }
            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    private int Test(bool b)
    {
        return 0;
    }
    //Sets Busy when a unit is taking action
    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    //Unsets Busy when any unit is not taking action
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    //Gets the unit selected when the mouse clicks it.
    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame()) 
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        return false;
                    }
                    if(unit.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }

            }

        }
        
        return false;

    }
    //Sets who is the selected unit and the move action as default action.
    private void SetSelectedUnit(Unit unit)
    {

        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
      
    }
    //Sets wich is the selected action of the unit
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
    public BaseAction GetSelectedAction()
    {
       
        return selectedAction;
    }
}
