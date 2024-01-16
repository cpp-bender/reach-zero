using UnityEngine;

[CreateAssetMenu(fileName = "Grid Data", menuName = "Swap Sums/Grid Data")]
public class GridSettings : ScriptableObject
{
    public GameObject cubePrefab;
    public Vector3 initialPos = Vector3.zero;
    public float width = 5;
    public float height = 5;
    public float widthThreshold = 2f;
    public float heightThreshold = 2f;
}