using System;
using System.Collections.Generic;

public class Unit
{
    public string PlayerName;
    public int PlayerHealth;
    public int PlayerResource;
    public string PlayerASCII;

    public List<Card> Cards = new List<Card>();

    public event Action<Unit> OnUnitChanged;

    public void Attack(Unit other, AttackCard card)
    {
        other.ApplyCard(this, card);
    }

    public void ApplyCard(Unit other, AttackCard card)
    {
        PlayerHealth -= card.damage;
        OnUnitChanged?.Invoke(this);
    }
}