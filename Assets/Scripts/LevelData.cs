using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Data/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public Sprite Background;
    public List<UnitData> Enemies;
    public List<CardData> Rewards;

    public Level GetLevel()
    {
        var enemies = new List<Unit>();
        foreach (var enemy in Enemies)
        {
            enemies.Add(enemy.GetUnit());
        }
        
        var rewards = new List<Card>();
        foreach (var card in Rewards)
        {
            rewards.Add(card.GetCard());
        }

        return new Level(Background, enemies, rewards);
    }
}