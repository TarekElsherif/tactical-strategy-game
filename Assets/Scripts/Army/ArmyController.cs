using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ArmyController : MonoBehaviour
{
    [SerializeField] string _armyName;

    List<UnitController> _activeUnits;
    Dictionary<UnitController, int> _actionsPerUnit;

    int _unitsInAction = 0;
    UnitController _currentSelectedUnit = null;

    public System.Action<UnitController> OnUnitActionDone;
    public System.Action OnAllActionsDone;
    public System.Action OnAllUnitsDied;

    public string ArmyName
    {
        get { return _armyName; }
    }

    public bool IsInAction
    {
        get { return _unitsInAction > 0; }
    }

    public bool IsAlive
    {
        get { return _activeUnits.Count > 0; }
    }

    public UnitController SelectedUnit
    {
        get { return _currentSelectedUnit; }
    }

    public List<UnitController> ActiveUnits
    {
        get { return _activeUnits; }
    }
    
    public int UnitCount
    {
        get { return _activeUnits.Count; }
    }

    public bool HasNoMoreActions
    {
        get
        {
            foreach (KeyValuePair<UnitController, int> item in _actionsPerUnit)
            {
                if (item.Value > 0)
                    return false;
            }
            return true;
        }
    }

    void Awake()
    {
        _activeUnits = new List<UnitController>(GetComponentsInChildren<UnitController>());
        _actionsPerUnit = new Dictionary<UnitController, int>();
        foreach (UnitController unit in _activeUnits)
        {
            _actionsPerUnit[unit] = 0;
        }
    }

    void OnEnable()
    {
        foreach (UnitController unit in _activeUnits)
        {
            unit.OnDie += RemoveUnit;
        }
    }

    void OnDisable()
    {
        foreach (UnitController unit in _activeUnits)
        {
            unit.OnDie -= RemoveUnit;
        }
    }

    public void SetActionsPerUnits(int actionsCount)
    {
        foreach (UnitController unit in _activeUnits)
        {
            _actionsPerUnit[unit] = actionsCount;
        }
    }

    public void SelectUnit(UnitController unit)
    {
        if (_actionsPerUnit[unit] <= 0)
            return;

        if (_currentSelectedUnit == unit)
        {
            unit.Unselect();
            _currentSelectedUnit = null;
        } 
        else if (_currentSelectedUnit != null)
        {
            _currentSelectedUnit.Unselect();
            unit.Select();
            _currentSelectedUnit = unit;
        }
        else
        {
            unit.Select();
            _currentSelectedUnit = unit;
        }
    }

    public void UnselectUnit(UnitController unit)
    {
        if (_currentSelectedUnit != unit)
            return;

        _currentSelectedUnit.Unselect();
        _currentSelectedUnit = null;
    }

    void DecrementUnitActions(UnitController unit)
    {
        _actionsPerUnit[unit]--;
        if (_actionsPerUnit[unit] <= 0)
        {
            unit.Unhighlight();
            if (OnUnitActionDone != null)
                OnUnitActionDone(unit);
            if (HasNoMoreActions)
            {
                if (OnAllActionsDone != null)
                    OnAllActionsDone();
            }
        }
    }

    public void HighlightAvailableUnits()
    {
        foreach (UnitController unit in _activeUnits)
        {
            if (_actionsPerUnit[unit] > 0)
                unit.Highlight();
        }
    }

    public void UnhighlightUnits()
    {
        foreach (UnitController unit in _activeUnits)
        {
            unit.Unhighlight();
        }
    }

    public void MoveUnit(UnitController unit, Vector3 destination, System.Action onDone = null)
    {
        if (_currentSelectedUnit == null)
            return;

        Assert.AreEqual(_currentSelectedUnit, unit);
        Assert.AreNotEqual(_actionsPerUnit[_currentSelectedUnit], 0);
        
        if (Vector3.Distance(unit.transform.position, destination) > unit.MovementRange)
        {
            UnselectUnit(_currentSelectedUnit);
            if (onDone != null)
                onDone();
            return;
        }

        _unitsInAction++;
        System.Action onMoveDone = () =>
        {
            _unitsInAction--;
            DecrementUnitActions(unit);
            if (onDone != null)
                onDone();
        };

        unit.Move(destination, onMoveDone);

        UnselectUnit(_currentSelectedUnit);
    }

    public void ShootUnit(UnitController unit, UnitController victim, System.Action onDone = null)
    {
        if (_currentSelectedUnit == null)
            return;

        Assert.AreEqual(_currentSelectedUnit, unit);
        Assert.AreNotEqual(_actionsPerUnit[_currentSelectedUnit], 0);

        if (Vector3.Distance(unit.transform.position, victim.transform.position) > unit.AttackRange)
        {
            UnselectUnit(_currentSelectedUnit);
            return;
        }

        _unitsInAction++;
        System.Action onAttackDone = () =>
        {
            _unitsInAction--;
            DecrementUnitActions(unit);
            if (onDone != null)
                onDone();
        };

        unit.Shoot(victim, onAttackDone);

        UnselectUnit(_currentSelectedUnit);
    }

    public void SkipTurn(UnitController unit, System.Action onDone = null)
    {
        if (_currentSelectedUnit == null)
            return;

        Assert.AreEqual(_currentSelectedUnit, unit);
        Assert.AreNotEqual(_actionsPerUnit[_currentSelectedUnit], 0);
        DecrementUnitActions(unit);
        if (onDone != null)
            onDone();
        UnselectUnit(_currentSelectedUnit);
    }

    public bool IsFriendlyUnit(UnitController unit)
    {
        return _activeUnits.Contains(unit);
    }

    public UnitController GetFirstAvaiableUnit()
    {
        foreach(UnitController unit in _activeUnits)
        {
            if (_actionsPerUnit[unit] > 0)
                return unit;
        }
        return null;
    }

    void RemoveUnit(UnitController unit)
    {
        unit.OnDie -= RemoveUnit;
        _activeUnits.Remove(unit);
        _actionsPerUnit.Remove(unit);

        if (_activeUnits.Count <= 0)
            if (OnAllUnitsDied != null)
                OnAllUnitsDied();
    }
}
