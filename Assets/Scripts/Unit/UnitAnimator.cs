using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : AbstractUnitComponent
{
    const string c_moveParam = "moving";
    const string c_shootParam = "shoot";
    const string c_hitParam = "hit";
    const string c_deadParam = "dead";

    [SerializeField] Animator _animator;

    void OnEnable()
    {
        _unit.OnStartMovement += StartMovement;
        _unit.OnEndMovement += EndMovement;
        _unit.OnStartAttack += Shoot;
        _unit.OnDamageTaken += TakeDamage;
        _unit.OnDie += Die;
    }

    void OnDisable()
    {
        _unit.OnStartMovement -= StartMovement;
        _unit.OnEndMovement -= EndMovement;
        _unit.OnStartAttack -= Shoot;
        _unit.OnDamageTaken -= TakeDamage;
        _unit.OnDie -= Die;
    }

    void StartMovement(Vector3 origin, Vector3 dest)
    {
        _animator.speed = _unit.MovementSpeed / 10;
        _animator.SetBool(c_moveParam, true);
    }

    void EndMovement(Vector3 dest)
    {
        _animator.speed = 1;
        _animator.SetBool(c_moveParam, false);
    }

    void Shoot(UnitController victim)
    {
        _animator.SetTrigger(c_shootParam);
    }

    void TakeDamage(float damage)
    {
        _animator.SetTrigger(c_hitParam);
    }

    void Die(UnitController unit)
    {
        _animator.SetBool(c_deadParam, true);
    }
}
