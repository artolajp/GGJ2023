using System;

public class AttackCard : Card
{
    public int damage;

    public AttackCard(string name, string description, int damage, int cost) : base(name, description, cost)
    {
        this.damage = damage;
    }

    public override bool TryUse(Unit unit, Unit other)
    {
        if (!base.TryUse(unit, other))
        {
            return false;
        }
        
        unit.Attack(other, this);
        return true;
    }

    public override string Text
    {
        get => damage.ToString();
    }
}