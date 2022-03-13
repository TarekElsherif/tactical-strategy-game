using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class AICommandInput : AbstractCommandInput
{
    [Header("References:")]
    [SerializeField] List<ArmyController> _enemyArmies;

    [Header("Settings:")]
    [SerializeField] [Range(0, 100)] int aggression = 50;

    enum AIAction
    {
        AttackNearestEnemy,
        AttackWeakestEnemy,
        RunAway,
        SkipTurn,
        MoveRandomly
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DoSequencialSteps();
    }

    public void DoSequencialSteps()
    {
        UnitController unit = _army.GetFirstAvaiableUnit();
        if (unit == null)
            return;

        float health = unit.Health;
        int rand = Random.Range(0, 100) - (int)health / 2;
        bool attack = (rand < aggression);
        List<UnitController> enemies = GetAllEnemyUnits();

        AIAction action = MakeDecision(unit, enemies);
        switch (action)
        {
            case AIAction.AttackNearestEnemy:
                AttackNearestEnemy(unit, enemies, DoSequencialSteps);
                break;
            case AIAction.AttackWeakestEnemy:
                AttackWeakestEnemy(unit, enemies, DoSequencialSteps);
                break;
            case AIAction.RunAway:
                RunAwayFromEnemies(unit, DoSequencialSteps);
                break;
            case AIAction.MoveRandomly:
                MoveRandomly(unit, DoSequencialSteps);
                break;
            case AIAction.SkipTurn:
                SkipTurn(unit, DoSequencialSteps);
                break;
            default:
                MoveRandomly(unit, DoSequencialSteps);
                break;
        }
    }

    AIAction MakeDecision(UnitController unit, List<UnitController> enemies)
    {
        if (enemies.Count <= 0)
            return AIAction.SkipTurn;

        float health = unit.Health;
        int rand = Random.Range(0, 200);
        bool attack = rand < (health + aggression);

        if (attack)
        {
            rand = Random.Range(0, 100);
            if (rand < 50)
                return AIAction.AttackWeakestEnemy;
            else
                return AIAction.AttackNearestEnemy;
        } 
        else
        {
            rand = Random.Range(0, 100);
            if (rand < health)
                return AIAction.MoveRandomly;
            else
                return AIAction.RunAway;
        }
    }

    void AttackNearestEnemy(UnitController unit, List<UnitController> enemies, System.Action onDone)
    {
        _army.SelectUnit(unit);

        UnitController nearestEnemy = FindNearestUnit(unit, enemies);
        AttackEnemy(unit, nearestEnemy, onDone);
    }

    void AttackWeakestEnemy(UnitController unit, List<UnitController> enemies, System.Action onDone)
    {
        _army.SelectUnit(unit);

        UnitController weakestEnemy = FindWeakestUnit(enemies);
        AttackEnemy(unit, weakestEnemy, onDone);
    }

    void RunAwayFromEnemies(UnitController unit, System.Action onDone)
    {
        _army.SelectUnit(unit);

        Vector3 summedVector = Vector3.zero;
        foreach (ArmyController enemyArmy in _enemyArmies)
        {
            foreach (UnitController enemy in enemyArmy.ActiveUnits)
            {
                Vector3 dir = unit.transform.position - enemy.transform.position;
                summedVector += dir;
            }
        }
        Vector3 dirNorm = summedVector.normalized * unit.AttackRange;
        dirNorm += unit.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(dirNorm, out hit, unit.AttackRange, 1);
        Vector3 finalPosition = hit.position;

        _army.MoveUnit(unit, finalPosition, onDone);
    }

    void AttackEnemy(UnitController unit, UnitController enemy, System.Action onDone)
    {
        float dist = Vector3.Distance(unit.transform.position, enemy.transform.position);
        Vector3 dir = enemy.transform.position - unit.transform.position;

        if (dist > unit.AttackRange)
        {
            float randDistance = Random.Range(0.5f,1f);
            float range = Mathf.Min(unit.AttackRange, (dist - unit.AttackRange / 2));
            Vector3 dirNorm = dir.normalized * range;
            dirNorm += unit.transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(dirNorm, out hit, unit.AttackRange, 1);
            Vector3 finalPosition = hit.position;

            _army.MoveUnit(unit, finalPosition, onDone);
        }
        else
        {
            _army.ShootUnit(unit, enemy, onDone);
        }
    }

    void MoveRandomly(UnitController unit, System.Action onDone)
    {
        _army.SelectUnit(unit);

        Vector3 randomDirection = Random.insideUnitSphere * unit.AttackRange;
        randomDirection += unit.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, unit.AttackRange, 1);
        Vector3 finalPosition = hit.position;

        _army.MoveUnit(unit, finalPosition, onDone);
    }

    void SkipTurn(UnitController unit, System.Action onDone)
    {
        _army.SelectUnit(unit);

        _army.SkipTurn(unit, onDone);
    }

    List<UnitController> GetAllEnemyUnits()
    {
        List<UnitController> enemies = new List<UnitController>();
        foreach (ArmyController enemyArmy in _enemyArmies)
        {
            enemyArmy.ActiveUnits.ForEach(enemy => enemies.Add(enemy));
        }
        return enemies;
    }

    UnitController FindWeakestUnit(List<UnitController> units)
    {
        Assert.AreNotEqual(units.Count, 0);

        UnitController weakestUnit = units.OrderBy(t => t.Health).FirstOrDefault();

        return weakestUnit;
    }

    UnitController FindNearestUnit(UnitController unit, List<UnitController> otherUnits)
    {
        Assert.AreNotEqual(otherUnits.Count, 0);

        UnitController nearestUnit = otherUnits.OrderBy(
            t => Vector3.Distance(unit.transform.position, t.transform.position))
            .FirstOrDefault();

        return nearestUnit;
    }
}
