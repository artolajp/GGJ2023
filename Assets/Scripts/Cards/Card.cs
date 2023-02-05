using System;

public class Card
{
    public string Name;
    public string Description;
    public int Cost;

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
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

    public virtual string Text
    {
        get => "";
    }

    public event Action OnCardSelected;
    public event Action OnCardDeselected;
    
    public Card(string name, string description, int cost)
    {
        Name = name;
        Description = description;
        Cost = cost;
    }

    public virtual bool TryUse(Unit unit, Unit other)
    {
        if (Cost > unit.CurrentResources)
        {
            return false;
        }
        
        unit.CurrentResources -= Cost;
        return true;
    }
    
}