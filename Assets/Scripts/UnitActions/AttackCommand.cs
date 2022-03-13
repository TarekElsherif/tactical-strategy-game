using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : IUnitCommand
{
    System.Action _onExectuted;
    UnitController _attacker;
    UnitController _victim;
    float _attackDamage;
    float _attackDuration;

    public AttackCommand(UnitController attacker, UnitController victim, float damage, float duration, Action onExecuted = null)
    {
        _attacker = attacker;
        _victim = victim;
        _attackDamage = damage;
        _attackDuration = duration;
        _onExectuted = onExecuted;
    }

    public Action OnExecuted
    {
        get => _onExectuted;
    }

    public IEnumerator Execute()
    {
        _attacker.transform.LookAt(_victim.transform);
        yield return new WaitForSeconds(_attackDuration);
        _victim.TakeDamage(_attackDamage);

        if (_onExectuted != null)
            _onExectuted();
    }
}
