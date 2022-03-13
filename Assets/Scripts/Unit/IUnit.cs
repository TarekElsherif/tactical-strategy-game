public interface IUnit
{
    public float Health { get; }
    public float MaxHealth { get; }
    public float AttackDamage { get; }
    public float AttackRange { get; }
    public float MovementSpeed { get; }
    public float MovementRange { get; }
    public bool IsDead { get; }
    public void SetHealth(float health);
}