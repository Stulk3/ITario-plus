using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSides : MonoBehaviour
{
    private bool isLeft=false;

    PlayerMove parent;
    private void Start()
    {
        parent = gameObject.GetComponentInParent<PlayerMove>();
        if ((transform.localScale.x > 0) && isLeft)
        {
            isLeft = false;
        }
        if ((transform.localScale.x < 0) && !isLeft)
        {
            isLeft = true;
        }
    }
   
    private void OnCollisionStay2D(Collision2D collision)
    {
         if (collision.gameObject.tag == ("ground"))
        {
            parent.SetStopSpeed(isLeft,false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("ground"))
        {
            parent.SetStopSpeed(isLeft, true);
        }
    }
    

}
