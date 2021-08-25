using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpFreeBlock : MonoBehaviour
{
    Transform player;
    Rigidbody2D playerRigidBody;
    Collider2D Collider;
    
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerRigidBody = player.gameObject.GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (((player.position.y < transform.position.y) && Collider.enabled)||Controll.GetDown())
        {
            Collider.enabled = false;
        }
        if ((player.position.y > transform.position.y) && !Collider.enabled && (playerRigidBody.velocity.y < 0)&&!Controll.GetDown())
        {
            Collider.enabled = true;
        }
    }
}
