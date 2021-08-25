using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlock : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D playerRigidBody;
    private Sprite blockSprite;
    private Collider2D Collider;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerRigidBody = player.gameObject.GetComponent<Rigidbody2D>();
        Collider= GetComponent<Collider2D>();
        blockSprite = GetComponent<Sprite>();
    }


    void Update()
    {
        if ((player.position.y < transform.position.y)&&!Collider.enabled && (playerRigidBody.velocity.y > 0))
        {
            Collider.enabled = true;
        }
        if ((player.position.y > transform.position.y) && Collider.enabled&&(playerRigidBody.velocity.y<0)&&(GetComponent<SpriteRenderer>().sprite == null))
        {
            Collider.enabled = false;
        }
    }
}
