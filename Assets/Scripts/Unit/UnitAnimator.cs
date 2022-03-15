using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : AbstractUnitComponent
{
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
        _animator.SetBool("moving", true);
    }

    void EndMovement(Vector3 dest)
    {
        _animator.SetBool("moving", false);
    }

    void Shoot(UnitController victim)
    {
        _animator.SetTrigger("shoot");
    }

    void TakeDamage(float damage)
    {
        _animator.SetTrigger("hit");
    }

    void Die(UnitController unit)
    {
        _animator.SetBool("dead", true);
    }
}
