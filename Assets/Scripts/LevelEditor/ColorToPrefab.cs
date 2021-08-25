using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName ="Color-to-Prefab")] 
public class ColorToPrefab : ScriptableObject
{
    public Color prefabColor;
    public GameObject prefab;
}
