using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraOnCanvas : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject interfacePrefab;
    private Canvas interfaceCanvas;
    void Start()
    {
        mainCamera = Camera.main;
        interfacePrefab = GameObject.Find("Interface");
        interfaceCanvas = interfacePrefab.GetComponent<Canvas>();
        interfaceCanvas.worldCamera = mainCamera;
    }
   
}
