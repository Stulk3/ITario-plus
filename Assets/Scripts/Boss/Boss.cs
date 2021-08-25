using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public Enemies enem;
    public GameObject triger;
    public GameObject jerjinsky;
    public float YcoordJump=0.2f;
    public Transform deatch;
    public BossEnd be;
    public Sound sound;

    private int waitCount=0;
    private int waitCountDance=-4;
    private Animator anim;
    private int readyHP = 2;
    private bool biger = false;
    private bool downDeatch = false;
    
    private float jumpTrigger = 15;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SpawnTrigger(int speed)
    {
        waitCount++;
        if ((waitCount >= 5)&&(readyHP>=0))
        {
            callSound("bossAtake");
            waitCount = waitCount - 5;
            GameObject[] trigers = GameObject.FindGameObjectsWithTag("triggers");
            if (trigers.Length < 8-readyHP*2)
            {
                GameObject trash = Instantiate(triger, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f), Quaternion.identity);
                trash.GetComponent<Enemies>().speed = speed;
                trash.GetComponent<Enemies>().jumpTrigger = jumpTrigger;
                trash.GetComponent<Enemies>().YcoordJump = YcoordJump;
            }
            GameObject[] en = GameObject.FindGameObjectsWithTag("EnemyDown");
            /*if (en.Length < 6 - readyHP*2)
            {
                GameObject trash = Instantiate(jerjinsky, new Vector3(14, -2, transform.position.z - 0.01f), Quaternion.identity);
                trash.GetComponent<Enemies>().speed = -3;
                trash.GetComponent<Enemies>().patrulPointOne = new Vector2(-9, 0);
                trash.GetComponent<Enemies>().patrulPointTwo = new Vector2(13, 0);
            }*/
        }
            
        
        
    }
    public void Dancing()
    {
        waitCountDance++;
        if (waitCountDance >= 7)
        {
            waitCountDance = waitCount - 7;
            anim.SetTrigger("Dance");
        }
    }
    private void Update()
    {
        if (enem.hp <= readyHP)
        {
            enem.hp = readyHP;
            readyHP = readyHP - 1;
            anim.SetTrigger("Damage");
        }
        if ((biger && (transform.localScale.y < 1.5))||transform.localScale.y<0)
        {
            transform.localScale = transform.localScale + transform.localScale * 0.005f;
            if (transform.localScale.y >= 1.5)
            {
                biger = false;
                transform.localScale = Vector3.one * 1.5f;
            }
        }
        if ((transform.localScale.y > 1.3) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") && (enem.speed == 0)) enem.speed = 2;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") ) enem.speed = 0;
        if (downDeatch && (deatch.position.y > 6.5))
        {
            deatch.position = deatch.position - new Vector3(0, Time.deltaTime*3, 0);
            if (deatch.position.y <= 6.5)
            {
                deatch.position = new Vector3(deatch.position.x, 6.5f, deatch.position.z);
                downDeatch = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "deatchZone")
        {
            readyHP = -1;
            transform.position = new Vector3(transform.position.x,transform.position.y, -5);
            enem.TurnDamage();
            GameObject[] triggers = GameObject.FindGameObjectsWithTag("triggers");
            foreach (GameObject trig in triggers)
            {
                trig.GetComponent<Enemies>().Deatch();
            }
            be.active = true;
            callSound("bossDeatch");
        }
    }
    

    public void NewPhase()
    {
        if (readyHP == 1)
        {
            biger = true;
        }
        if (readyHP == 0)
        {
            anim.SetBool("hat", true);
            jumpTrigger = 20;
            YcoordJump = -2.7f;
            GameObject[] trashs = GameObject.FindGameObjectsWithTag("triggers");
            foreach (GameObject trash in trashs)
            {
                trash.GetComponent<Enemies>().jumpTrigger = jumpTrigger;
                trash.GetComponent<Enemies>().YcoordJump = YcoordJump;
            }
            downDeatch = true;
        }
    }
    public void callSound(string name)
    {
        sound.Play(name);
    }
}
