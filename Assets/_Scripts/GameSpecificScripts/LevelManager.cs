using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public LevelData[] levels;
    public int currentLevelIndex;

    private GridManager gridManager;
    private LevelData currentLevel;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitStableStones()
    {
        currentLevel = levels[currentLevelIndex];

        GameObject allStableStones = new GameObject("Stable Stones");

        for (int i = 0; i < currentLevel.stableStones.Count; i++)
        {
            int stableGOGridIndex = currentLevel.stableStones[i].gridIndex;
            GameObject go = currentLevel.stableStones[i].stoneObj;
            Vector3 worldPos = gridManager.GridToWorld(gridManager.virtualGrid[stableGOGridIndex].x, gridManager.virtualGrid[stableGOGridIndex].z);
            var stableGO = Instantiate(go, worldPos, Quaternion.Euler(currentLevel.stableStones[i].initialRot));
            stableGO.GetComponent<BaseStoneController>().SetText((char)currentLevel.stableStones[i].textSign, currentLevel.stableStones[i].textCount);
            stableGO.transform.SetParent(allStableStones.transform);
            gridManager.virtualGrid[stableGOGridIndex].currentObj = stableGO;
        }
    }

    public void InitMovingStones()
    {
        currentLevel = levels[currentLevelIndex];

        GameObject allMovingStones = new GameObject("Moving Stones");

        for (int i = 0; i < currentLevel.movingStones.Count; i++)
        {
            var go = currentLevel.movingStones[i].stoneObj;
            var worldPos = currentLevel.movingStones[i].worldPos;
            var worldRot = Quaternion.Euler(currentLevel.movingStones[i].initialRot);
            var movingGO = Instantiate(go, worldPos, worldRot);
            movingGO.GetComponent<BaseStoneController>().SetText((char)currentLevel.movingStones[i].textSign, currentLevel.movingStones[i].textCount);
            movingGO.transform.SetParent(allMovingStones.transform);
        }
    }

    public void InitLevel()
    {
        gridManager = FindObjectOfType<GridManager>();
        currentLevelIndex = (GameManager.instance.currentLevel - 1) % levels.Length;
        ReferenceManager.Instance.movingStoneCount = levels[currentLevelIndex].movingStones.Count;
        InitStableStones();
        InitMovingStones();
    }
}
