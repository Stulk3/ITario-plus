using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour
{
    public float steep;
    public Image loadIcon;
    public GameObject layer;

    float progress = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Controll.GetExit())
        {
            if (!layer.gameObject.activeSelf) layer.gameObject.SetActive(true);
            progress = progress + steep;
            loadIcon.fillAmount =Mathf.Clamp( progress,0,100);

            if (progress >= 1)
            {
                
                Application.Quit();
            }
        }
        else
        {
            if (layer.gameObject.activeSelf)
            {
                progress = 0;
                layer.gameObject.SetActive(false);
            }
        }
    }
}
