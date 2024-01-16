using System.Collections.Generic;
using UnityEngine;

public class GridManager : SingletonMonoBehaviour<GridManager>
{
    public GridSettings gridSettings;
    public List<VectorXZ> virtualGrid;

    private List<Vector3> gridWorldPos;
    private List<GameObject> gridObjects;
    private LevelManager levelManager;
    private const int maxPossibleNeighbourCount = 4;
    private int[] neighboursX;
    private int[] neighboursZ;

    protected override void Awake()
    {
        base.Awake();
        gridWorldPos = new List<Vector3>();
        gridObjects = new List<GameObject>();
    }

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        InitNeighboursIndexes();
        InitVirtualGrid();
        levelManager.InitLevel();
    }

    private void InitVirtualGrid()
    {
        Vector3 worldPos = Vector3.zero;

        for (int i = 0; i < gridSettings.height; i++)
        {
            for (int k = 0; k < gridSettings.width; k++)
            {
                worldPos = new Vector3(k * gridSettings.widthThreshold, 0f, i * gridSettings.heightThreshold);
                var go = Instantiate(gridSettings.cubePrefab, worldPos, Quaternion.identity, transform);
                gridObjects.Add(go);
                var vectorXZ = new VectorXZ(k, i);
                virtualGrid.Add(vectorXZ);
                gridWorldPos.Add(worldPos);
            }
        }
    }

    private void InitNeighboursIndexes()
    {
        neighboursX = new int[] { -1, 1, 0, 0 };
        neighboursZ = new int[] { 0, 0, 1, -1 };
    }

    public List<GameObject> GetNeighboursObject(int x, int z)
    {
        var neighbours = new List<GameObject>();
        for (int i = 0; i < maxPossibleNeighbourCount; i++)
        {
            int nx = x + neighboursX[i];
            int nz = z + neighboursZ[i];
            for (int k = 0; k < virtualGrid.Count; k++)
            {
                if (virtualGrid[k].x == nx && virtualGrid[k].z == nz)
                {
                    if (virtualGrid[k].currentObj != null)
                    {
                        neighbours.Add(virtualGrid[k].currentObj);
                    }
                }
            }
        }
        return neighbours;
    }

    public List<VectorXZ> GetNeighboursGrid(int x, int z)
    {
        var neighbours = new List<VectorXZ>();
        for (int i = 0; i < maxPossibleNeighbourCount; i++)
        {
            int nx = x + neighboursX[i];
            int nz = z + neighboursZ[i];
            for (int k = 0; k < virtualGrid.Count; k++)
            {
                if (virtualGrid[k].x == nx && virtualGrid[k].z == nz)
                {
                    if (virtualGrid[k].currentObj == null)
                    {
                        neighbours.Add(virtualGrid[k]);
                    }
                }
            }
        }
        return neighbours;
    }

    public int GetNeighboursCount(int x, int z)
    {
        int neighbourCount = 0;
        for (int i = 0; i < maxPossibleNeighbourCount; i++)
        {
            int nx = x + neighboursX[i];
            int nz = z + neighboursZ[i];
            for (int k = 0; k < virtualGrid.Count; k++)
            {
                if (virtualGrid[k].x == nx && virtualGrid[k].z == nz)
                {
                    if (virtualGrid[k].currentObj == null)
                    {

                    }
                    else
                    {
                        neighbourCount++;
                    }
                }
            }
        }
        return neighbourCount;
    }

    public List<VectorXZ> GetAllPossibleNeighbourGrid()
    {
        // Objesi olmayan nesnenin komsulari

        var list = new List<VectorXZ>();
        var allGrid = new List<VectorXZ>();

        for (int i = 0; i < virtualGrid.Count; i++)
        {
            if (virtualGrid[i].currentObj != null)
            {
                allGrid.Add(virtualGrid[i]);
            }
        }

        for (int i = 0; i < allGrid.Count; i++)
        {
            var tempList = GetNeighboursGrid(allGrid[i].x, allGrid[i].z);

            for (int k = 0; k < tempList.Count; k++)
            {
                list.Add(tempList[k]);
            }
        }

        return list;
    }

    public Vector3 GetNearestPossibleGridPos(Vector3 mousePos, List<VectorXZ> allPossibleGridPos)
    {
        Vector3 snapVector = Vector3.zero;
        for (int i = 0; i < allPossibleGridPos.Count; i++)
        {
            float x = mousePos.x;
            float y = 0f;
            float z = mousePos.z;

            mousePos = new Vector3(x, y, z);

            float dist = Vector3.Distance(mousePos, GridToWorld(allPossibleGridPos[i].x, allPossibleGridPos[i].z));

            if (dist < 0.45f)
            {
                snapVector = GridToWorld(allPossibleGridPos[i].x, allPossibleGridPos[i].z);
            }
        }
        return snapVector;
    }

    public VectorXZ WorldToGrid(Vector3 worldPos)
    {
        int index = 0;
        for (int i = 0; i < gridWorldPos.Count; i++)
        {
            if (worldPos.x == gridWorldPos[i].x && worldPos.y == gridWorldPos[i].y && worldPos.z == gridWorldPos[i].z)
            {
                index = i;
                break;
            }
        }
        return virtualGrid[index];
    }

    public Vector3 GridToWorld(int x, int z)
    {
        int index = 0;
        for (int i = 0; i < gridWorldPos.Count; i++)
        {
            if (x == virtualGrid[i].x && z == virtualGrid[i].z)
            {
                index = i;
                break;
            }
        }
        return gridWorldPos[index];
    }

    public void SwitchGrid(bool turnOn)
    {
        for (int i = 0; i < gridObjects.Count; i++)
        {
            gridObjects[i].GetComponent<SphereCollider>().enabled = turnOn;
        }
    }

    public List<GameObject> GetAllGridObjects()
    {
        var tempList = new List<GameObject>();

        for (int i = 0; i < virtualGrid.Count; i++)
        {
            if (virtualGrid[i].currentObj != null)
            {
                tempList.Add(virtualGrid[i].currentObj);
            }
        }
        return tempList;
    }
}
