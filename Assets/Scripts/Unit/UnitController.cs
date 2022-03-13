using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AbstractUnit))]
public class UnitController : MonoBehaviour
{
    AbstractUnit _unit;

    [SerializeField] NavMeshAgent _agent;

    public System.Action OnSelect;
    public System.Action OnUnselect;
    public System.Action OnHighlight;
    public System.Action OnUnhighlight;
    public System.Action<float> OnDamageTaken;
    public System.Action<float> OnHealthGained;
    public System.Action<Vector3, Vector3> OnStartMovement;
    public System.Action<Vector3> OnEndMovement;
    public System.Action<UnitController> OnStartAttack;
    public System.Action<UnitController> OnEndAttack;
    public System.Action<UnitController> OnDie;

    public NavMeshAgent Agent
    {
        get { return _agent; }
    }

    public bool IsAlive
    {
        get { return _unit.IsDead; }
    }

    public float Health
    {
        get { return _unit.Health; }
    }

    public float MovementRange
    {
        get { return _unit.MovementRange; }
    }

    public float AttackRange
    {
        get { return _unit.AttackRange; }
    }

    void Awake()
    {
        _unit = GetComponent<AbstractUnit>();
        _unit.SetHealth(_unit.MaxHealth);
    }

    void Start()
    {
        _agent.speed = _unit.MovementSpeed;
    }

    public void Select()
    {
        if (OnSelect != null)
            OnSelect();
    }

    public void Unselect()
    {
        if (OnUnselect != null)
            OnUnselect();
    }

    public void Highlight()
    {
        if (OnHighlight != null)
            OnHighlight();
    }

    public void Unhighlight()
    {
        if (OnUnhighlight != null)
            OnUnhighlight();
    }

    public void Move(Vector3 destination, System.Action onDone = null)
    {
        float speed = _unit.MovementSpeed;
        System.Action onExecuted = () =>
        {
            if (OnEndMovement != null)
                OnEndMovement(destination);
            if (onDone != null)
                onDone();
        };

        MoveCommand moveCommand = new MoveCommand(this, destination, speed, onExecuted);
        StartCoroutine(moveCommand.Execute());

        if (OnStartMovement != null)
            OnStartMovement(transform.position, destination);
    }

    public void Shoot(UnitController victim, System.Action onDone = null)
    {
        float damage = _unit.AttackDamage;
        float attackDuration = 0.5f;
        System.Action onExecuted = () =>
        {
            if (OnEndAttack != null)
                OnEndAttack(victim);
            if (onDone != null)
                onDone();
        };

        AttackCommand attackCommand = new AttackCommand(this, victim, damage, attackDuration, onExecuted);
        StartCoroutine(attackCommand.Execute());

        if (OnStartAttack != null)
            OnStartAttack(victim);
    }

    public void TakeDamage(float damage)
    {
        float health = _unit.Health;
        health -= damage;
        if (health < 0)
            health = 0;

        _unit.SetHealth(health);

        if (OnDamageTaken != null)
            OnDamageTaken(damage);

        if (health <= 0)
            Die();
    }

    public void GainHealth(float healthGained)
    {
        float health = _unit.Health;
        health += healthGained;

        _unit.SetHealth(health);

        if (OnHealthGained != null)
            OnHealthGained(healthGained);
    }

    public void Die()
    {
        if (OnDie != null)
            OnDie(this);
    }
}
