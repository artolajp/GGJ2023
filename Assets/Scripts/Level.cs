using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public Sprite Background;
    public List<Unit> Enemies;
    public List<Card> Rewards;
    
    public Level(Sprite background, List<Unit> enemies, List<Card> rewards)
    {
        Background = background;
        Enemies = enemies;
        Rewards = rewards;
    }

}