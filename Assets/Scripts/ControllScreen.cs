using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllScreen : MonoBehaviour
{
   float time;
   
   void Start()
    {
        PlayerPrefs.DeleteKey("Hp");
        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("Enemy");
    }
    void Update()
    {
        time=time+Time.deltaTime;
        if (time > 5) SceneManager.LoadScene(1);
    }
}
