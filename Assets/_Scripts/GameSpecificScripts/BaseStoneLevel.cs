using System;
using UnityEngine;

[Serializable]
public class BaseStoneLevel
{
    [Space(2f)] public GameObject stoneObj;
    [Space(2f)] public Sign textSign;
    [Space(2f)] public int textCount;
    [Space(2f)] public Vector3 initialRot = new Vector3(-90f, 0f, 0f);
}
