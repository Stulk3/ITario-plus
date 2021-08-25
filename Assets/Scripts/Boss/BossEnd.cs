using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnd : MonoBehaviour
{
    float time;
    public bool active = false;
    void Update()
    {
        if (active)
        {
            time = time + Time.deltaTime;
            
            if (time >3) SceneManager.LoadScene(3);
        }
    }
}
