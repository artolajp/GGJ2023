using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Data/Stage", order = 1)]
public class StageData : ScriptableObject
{
    public List<LevelData> levels;
    
}
