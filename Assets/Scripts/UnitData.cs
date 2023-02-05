using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Data/Unit", order = 1)]
public class UnitData : ScriptableObject
{
    public string Name;
    public int Health;
    public int Resources;
    public string ASCII;
    public int HandSize;

    public List<CardData> Cards;

    public Unit GetUnit()
    {
        var unit = new Unit();
        unit.Health = Health;
        unit.Resources = Resources;
        unit.CurrentResources = Resources;
        unit.Name = Name;
        unit.ASCII = ASCII;
        unit.HandSize = HandSize;
        
        unit.Cards = new List<Card>();
        foreach (var card in Cards)
        {
            unit.Cards.Add(card.GetCard());
        }
        
        return unit;
    }
}