using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kostil : MonoBehaviour
{
    void Start()
    {
        int scene = PlayerPrefs.GetInt("Scene");
        PlayerPrefs.DeleteKey("Scene");
        SceneManager.LoadScene(scene);

    }

   
}
