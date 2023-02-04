using System;

public abstract class Card
{
    public string Name;
    public string Description;

    private bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            isSelected = value;
            if (isSelected)
            {
                OnCardSelected?.Invoke();
            }
            else
            {
                OnCardDeselected?.Invoke();
            }
        }
    }

    public event Action OnCardSelected;
    public event Action OnCardDeselected;
    
    public Card(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract void Use(Unit unit, Unit other);
    
}

public class AttackCard : Card
{
    public int damage;

    public AttackCard(string name, string description, int damage) : base(name, description)
    {
        this.damage = damage;
    }

    public override void Use(Unit unit, Unit other)
    {
        unit.Attack(other, this);
    }
}
