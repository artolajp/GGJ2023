using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Data/Card", order = 1)]
public class CardData : ScriptableObject
{
    public string Name;
    public string Description;
    public int Cost;

    public virtual Card GetCard()
    {
        return new Card(Name, Description, Cost);
    }
}