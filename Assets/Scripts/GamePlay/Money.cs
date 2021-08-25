using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public Collider2D colldr;
    public float speed;
    public Rigidbody2D rb;
    
    private float startY;
    private void Awake()
    {
        colldr.enabled = false;
        rb.gravityScale = 0;
        startY = transform.position.y;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.15f);
    }
    private void Update()
    {
        if (colldr.enabled == false)
        {
            colldr.transform.position = colldr.transform.position + new Vector3(0,Mathf.Abs(speed)*Time.deltaTime, 0);
            if (transform.position.y> startY + 0.9)
            {
                colldr.enabled = true;
                rb.gravityScale = 1;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
           // if (!onGround) transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y- speed * Time.deltaTime, transform.position.z);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("ground") && ((collision.contacts[0].point.y > transform.position.y) || (collision.contacts[collision.contactCount - 1].point.y > transform.position.y )))
        {
            //colldr.transform.position = colldr.transform.position + new Vector3(0, speed * Time.deltaTime / 2, 0);
            if (collision.contacts[0].point.x > transform.position.x) speed = -Mathf.Abs(speed);
            else speed = Mathf.Abs(speed);
        }
    }
    
    
}
