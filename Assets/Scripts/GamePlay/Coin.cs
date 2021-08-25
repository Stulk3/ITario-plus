using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Animator anim;
    public bool inBlock = false;
    public Sound sound;

    private void Awake()

    {
        anim = GetComponent<Animator>();
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Collect();
        }
    }

    public void Destroy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponentInParent<PlayerMove>().ChangeCoin(1);

        GameObject.Destroy(transform.parent.gameObject);
    }

    public void Collect()
    {
        anim.SetTrigger("collect");
        sound.Play("Pickup_Coin");
    }
    private void Update()
    {
        if (inBlock) Collect();
    }
}
