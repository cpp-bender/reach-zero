using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Swap Sums/Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    public List<StableStoneLevel> stableStones;
    public List<MovingStoneLevel> movingStones;
}
