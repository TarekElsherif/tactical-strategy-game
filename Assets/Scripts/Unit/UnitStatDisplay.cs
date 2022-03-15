using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatDisplay : AbstractUnitComponent
{
    [SerializeField] Slider _healthDisplay;
    [SerializeField] GameObject _movementIndicator;
    [SerializeField] LineRenderer _movementRangeCircle;
    [SerializeField] LineRenderer _attackRangeCircle;
    [SerializeField] bool _showMovementRange;
    [SerializeField] bool _showAttackRange;

    void OnEnable()
    {
        _unit.OnDamageTaken += UpdateHealthDisplay;
        _unit.OnHealthGained += UpdateHealthDisplay;
        _unit.OnStartMovement += StartMovement;
        _unit.OnEndMovement += EndMovement;
        _unit.OnSelect += DisplayRangeCircles;
        _unit.OnUnselect += HideRangeCircles;

        _healthDisplay.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        _unit.OnDamageTaken -= UpdateHealthDisplay;
        _unit.OnHealthGained -= UpdateHealthDisplay;
        _unit.OnStartMovement -= StartMovement;
        _unit.OnEndMovement -= EndMovement;
        _unit.OnSelect -= DisplayRangeCircles;
        _unit.OnUnselect -= HideRangeCircles;

        _healthDisplay.gameObject.SetActive(false);
        _movementIndicator.gameObject.SetActive(false);
        _movementRangeCircle.gameObject.SetActive(false);
        _attackRangeCircle.gameObject.SetActive(false);
    }

    void Start()
    {
        UpdateHealthDisplay(100);
    }

    void StartMovement(Vector3 start, Vector3 dest)
    {
        _movementIndicator.SetActive(true);
        _movementIndicator.transform.parent = null;
        _movementIndicator.transform.position = dest;
    }

    void EndMovement(Vector3 dest)
    {
        _movementIndicator.transform.parent = transform;
        _movementIndicator.SetActive(false);
    }

    void DisplayRangeCircles()
    {
        if (_showMovementRange)
        {
            _movementRangeCircle.gameObject.SetActive(true);
            _movementRangeCircle.DrawCircle(_unit.MovementRange, .04f);
        }
        if (_showAttackRange)
        {
            _attackRangeCircle.gameObject.SetActive(true);
            _attackRangeCircle.DrawCircle(_unit.AttackRange, .04f);
        }
    }

    void HideRangeCircles()
    {
        _movementRangeCircle.gameObject.SetActive(false);
        _attackRangeCircle.gameObject.SetActive(false);
    }

    void UpdateHealthDisplay(float input)
    {
        float value = _unit.Health / 100;
        _healthDisplay.value = value;
    }
}
