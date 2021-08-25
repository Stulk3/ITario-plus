using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public bool stay;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "ground") stay = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "ground") stay = false;
    }

}
