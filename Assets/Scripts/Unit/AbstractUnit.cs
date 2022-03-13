using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractUnit : MonoBehaviour, IUnit 
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _attackDamage;
    [SerializeField] float _attackRange;
    [SerializeField] float _movementSpeed;
    [SerializeField] float _movementRange;

    protected float _health;

    public float Health
    {
        get => _health;
    }

    public float AttackDamage
    {
        get => _attackDamage;
    }

    public float MovementSpeed
    {
        get => _movementSpeed;
    }

    public bool IsDead
    {
        get => _health == 0;
    }

    public float AttackRange
    {
        get => _attackRange;
    }

    public float MovementRange
    {
        get => _movementRange;
    }

    public float MaxHealth
    {
        get => _maxHealth;
    }

    public void SetHealth(float health)
    {
        _health = health;
    }
}
