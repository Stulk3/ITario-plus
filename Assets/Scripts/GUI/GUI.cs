using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Texture[] marks;
    public void Start()
    {

        
        
    }
    public void OffFon()
    {
        gameObject.SetActive(false);
        PlayerMove playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        playerMovement.isActive = true;
    }
    public void GetMark()
    {
        GameObject child = GameObject.Find("Mark");
        switch(GlobalData.mark)
        {
            case "None":
                child.GetComponent<RawImage>().texture = marks[0];
                break;
            case "Okay":

                break;
            case "Good":
                
                break;
            case "Great":
                
                break;
        }
        
    }
}
