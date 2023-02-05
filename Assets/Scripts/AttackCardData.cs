using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Data/Card Attack", order = 1)]
public class AttackCardData : CardData
{
    public int Power;
    
    public override Card GetCard()
    {
        return new AttackCard(Name, Description,Power, Cost);
    }
}