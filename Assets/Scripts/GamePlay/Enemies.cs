using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int hp=1;
    public Vector2 patrulPointOne;
    public Vector2 patrulPointTwo;
    public float speed;
    public bool leftOrientationSprite = true;
    public GameObject[] parts;
    public bool isJamping;
    public float YcoordJump;
    public float jumpTrigger = 15;
    public Sound sound;

    private Transform player;
    private Transform cam;
    public bool sayd;
    private Animator anim;


   

    private void Start()
    {
        if (((speed>0)&&leftOrientationSprite) || ((speed < 0) && !leftOrientationSprite)) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        anim=gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    
    private void Update()
    {
        if (Mathf.Abs(cam.position.x - transform.position.x) < 13 && hp>0)
        {
            if ((!sayd)&& (Mathf.Abs(cam.position.x - transform.position.x) < 10))
            {
                sayd = true;
                int r = Random.Range(1, 20);
                if (r<=11)  sound.Play("voice"+r.ToString());
            }
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            if (((transform.localPosition.x < patrulPointOne.x) && (speed < 0)) || ((transform.localPosition.x > patrulPointTwo.x) && (speed > 0)))
            {
                speed = -speed;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                if (isJamping)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpTrigger);
                    hp = hp - 1;
                    if (hp <= 0) Damage(1);
                }
            }
        }
        if (isJamping && (transform.position.y < YcoordJump))
        {
            GetComponent<Rigidbody2D>().velocity=new Vector2(0, jumpTrigger);
            sound.Play("trigJump");
        }
        if (transform.position.y < -5)
        {
            GameObject.Destroy(gameObject);
            player.gameObject.GetComponent<PlayerMove>().ChangeEnemy();
        }
        }


    public void Damage(int strenght)
    {
        sound.Play("Hit_Hurt3");
        hp = hp - strenght;
        if (isJamping )hp = 0;
        
        if (hp <= 0)
        {
            
            if (anim!=null) anim.SetTrigger("die");
            if (isJamping) GetComponent<Rigidbody2D>().velocity = new Vector2(0, -15);
            isJamping = false;
            //GetComponent<Collider2D>().enabled = false;
            GetComponentInChildren<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            foreach (GameObject part in parts) GameObject.Destroy(part);
        }
    }

    public void TurnDamage()
    {
        transform.localScale =new Vector3 (transform.localScale.x,-Mathf.Abs(transform.localScale.y),transform.localScale.z);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200));
        patrulPointOne.x = -1000;
        patrulPointTwo.x = 1000;
        GetComponent<Collider2D>().enabled = false;
        foreach (Collider2D cld in GetComponentsInChildren<Collider2D>())
        {
            cld.enabled = false;
        }
        
    }

    public void Deatch()
    {
        
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != ("ground"))
        {
            speed = -speed;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (collision.gameObject.tag == ("ground")&& ((collision.contacts[0].point.y > transform.position.y - 0.4)|| (collision.contacts[collision.contactCount-1].point.y > transform.position.y - 0.4)))
        {
            if (collision.contacts[0].point.x > transform.position.x ) speed = -Mathf.Abs(speed);
            else speed = Mathf.Abs(speed);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
