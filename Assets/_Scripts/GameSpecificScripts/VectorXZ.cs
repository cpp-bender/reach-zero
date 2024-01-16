using System;
using UnityEngine;

[Serializable]
public class VectorXZ
{
    public int x;
    public int z;
    public GameObject currentObj;

    public VectorXZ(float worldX, float worldZ)
    {
        x = (int)worldX;
        z = (int)worldZ;
    }

    public void SetObject(GameObject go)
    {
        currentObj = go;
    }
}
