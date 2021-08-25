using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCore : MonoBehaviour
{
    private Vector2 position;
    public ColorToPrefab[] colorMappings;
    [SerializeField] private Texture2D levelBlueprint;
    private Color pixelColor;
    private GameObject interfacePrefab;
    private void Start() 
    {
        GenerateInterface();
        ReadBlueprint();
        
    }
    private void ReadBlueprint()
    {
        for (int x = 0; x < levelBlueprint.width; x++)
        {
            for (int y=0; y < levelBlueprint.height; y++)
            {
                GenerateTile(x,y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        pixelColor = levelBlueprint.GetPixel(x,y);
        if (pixelColor.a == 0)
        {
            return;
        }
        foreach(ColorToPrefab colorMapping in colorMappings)
        {
            if(colorMapping.prefabColor.Equals(pixelColor) )
            {
                position = new Vector2(x,y);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }

        Debug.Log("Пук");
    }

    private void GenerateInterface()
    {
        interfacePrefab = Resources.Load<GameObject>("Presets/Interface");
    }
  
}
