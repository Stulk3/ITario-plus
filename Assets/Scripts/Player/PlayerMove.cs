//Legacy Code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    //Приватные поля для управления персонажем и взаимодействия с механиками
    [SerializeField] private Collider2D small;
    [SerializeField] private Collider2D big;
    [SerializeField] private GameObject smallFoot;
    [SerializeField] private GameObject bigFoot;
    [SerializeField] private float MaxSpeed = 1;
    [SerializeField] private float accelerate = 1;
    [SerializeField] private float maxJumpStrengh = 1;
    [SerializeField] private float maxJumpHold = 1;
    public Camera mainCamera;
    [SerializeField] private bool freezeCam=false;
    [Header("Коэффициент для прекращения прыжка при отжатии кнопки")]
    [SerializeField] private float kFall = 1;
    [SerializeField] private CheckGround[] checker;
    //Публичные поля
    public int hp = 3;
    public int coins = 0;
    public int enemyKill = 0;
    public bool bigState = false;
    public bool isActive=false;
    
    public Sound sound;
    public AudioSource GlobalAudio;

    private float speed = 0;
    private float jumpHold = 1;
    private Rigidbody2D physics;
    private bool freeJump = true;
    private bool unmortal = false;
    private Animator animator;
    private float maxLeftSpeed;
    private float maxRightSpeed;
    private Text moneyDisplay;
    private Text enemyDisplay;
    private string tube;
    private float startTube;
    private GameObject teleport;

    private void Start()
    {
        physics = gameObject.GetComponent<Rigidbody2D>();
        moneyDisplay = GameObject.Find("CoinCount").GetComponent<Text>();
        enemyDisplay = GameObject.Find("EnemyCount").GetComponent<Text>();
        animator = GetComponent<Animator>();
        SetStopSpeed(true, true);
        SetStopSpeed(false, true);
        //Сет Камеры
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        if (PlayerPrefs.HasKey("Hp"))
        {
            hp = PlayerPrefs.GetInt("Hp");
            enemyKill = PlayerPrefs.GetInt("Enemy");
            coins = PlayerPrefs.GetInt("Coins");
            moneyDisplay.text = "x " + coins.ToString();
            enemyDisplay.text= "x " + enemyKill.ToString();
            GUI g = GameObject.Find("BlackFon").GetComponent<GUI>();
            if (PlayerPrefs.HasKey("Big"))
            {
                PlayerPrefs.DeleteKey("Big");
                bigState = true;
                animator.SetBool("BigState", true);
                smallFoot.SetActive(false);
                bigFoot.SetActive(true);
                small.enabled = false;
                big.enabled = true;
                transform.position = transform.position + new Vector3(0, 0.5f, 0);
            }
            //g.SetMark(hp);
        }
    }
    private bool CheckContact(Collision2D collision)
    {
        bool answer = false;
        ContactPoint2D[] points= collision.contacts;
        foreach (ContactPoint2D point in points)
        {
            if ((point.point.y<transform.position.y)&& (Mathf.Abs(point.point.x - transform.position.x) < 0.5))
            {
                answer = true;
                break;
            }
        }
        return answer;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if ((collision.transform.tag == "ground")&&!(animator.GetBool("OnGround"))&&(checker[0].stay|| checker[1].stay))
        {
            //physics.velocity = new Vector2(physics.velocity.x, 0f);
            freeJump = true;
            animator.SetBool("OnGround", true);
            jumpHold = maxJumpHold;

        }
        if (collision.transform.tag == "Money")
        {
            if (!bigState)
            {
                sound.Play("Powerup2");
                Time.timeScale = 0.1f;
                bigState = true;
                GameObject.Destroy(collision.gameObject);
                animator.SetBool("BigState", true);
                smallFoot.SetActive(false);
                bigFoot.SetActive(true);
                small.enabled = false;
                big.enabled = true;
                transform.position = transform.position + new Vector3(0, 0.5f, 0);
            }
            else
            {
                ChangeCoin(10);
            }
                
            
        }
        }
        


        

    public void SetStopSpeed (bool isLeft,bool isMax)
    {
        if (transform.localScale.x < 0) isLeft = !isLeft;
        if (isLeft)
        {
            if (isMax) maxLeftSpeed = -Mathf.Abs(MaxSpeed);
            else maxLeftSpeed = 0;
        }
        else
        {
            if (isMax) maxRightSpeed = Mathf.Abs(MaxSpeed);
            else maxRightSpeed = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if ((collision.transform.tag == "Teleport") && animator.GetBool("OnGround") && Controll.GetDown()&&isActive)
        {
            isActive = false;
            teleport = collision.gameObject;
            sound.Play("Powerup7");
            physics.gravityScale = 0;

            big.enabled = false;
            small.enabled = false;
            foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
            {
                child.enabled = false;
            }
            transform.position = new Vector3(teleport.transform.position.x, transform.position.y, transform.position.z);
            startTube = SetDirect(teleport.GetComponent<Teleport>().tube);
            tube = teleport.GetComponent<Teleport>().tube;
        }
    }


        private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "ground")
        {

            animator.SetBool("OnGround", false);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.tag == "damageZone")&& !unmortal && Time.timeScale>0.5f)
        {
            
            unmortal = true;
            if (!bigState) Death();
            else
            {
                PlayerPrefs.DeleteKey("Big");
                sound.Play("Powerup2");
                Time.timeScale = 0.1f;
                bigState = false;
                animator.SetBool("BigState", false);
                smallFoot.SetActive(true);
                bigFoot.SetActive(false);
                small.enabled = true;
                big.enabled = false;
            }
        }
        if (collision.transform.tag == "deatchZone")
        {
            Death();
            
        }
       
    }

    public void Death()
    {
        sound.Play("Hit_Hurt3");
        GlobalAudio.Stop();
        if (!animator.GetBool("Death"))
        {
            if (hp > 1)
            {
                PlayerPrefs.SetInt("Hp", hp - 1);
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt("Enemy", enemyKill);
            }
            else
            {
                PlayerPrefs.DeleteKey("Hp");
                PlayerPrefs.DeleteKey("Coins");
                PlayerPrefs.DeleteKey("Enemy");

            }
            Time.timeScale = 0.01f;
            animator.SetBool("Death", true);
            physics.gravityScale = 0;
            small.enabled = false;
            foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
            {
                child.enabled = false;
            }

        }
    }

    public void DownDeatchAnimation()
    {
        sound.Play("Hit_Hurt10");
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        Time.timeScale = 1f;
        physics.velocity= (new Vector2(0, 9));
        //physics.AddForce(new Vector2(0, 300));
        physics.gravityScale = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //speed = Mathf.Clamp(speed + Controll.GetHorizontalMove()* accelerate*Time.deltaTime,-MaxSpeed,MaxSpeed);
        //anim.SetFloat("speedPanda", speed);
        if (!animator.GetBool("Death")&&isActive)
        {
            speed = Mathf.Clamp(Mathf.Lerp(speed, Controll.GetHorizontalMove() * MaxSpeed, 0.2f),maxLeftSpeed,maxRightSpeed);
            animator.SetFloat("speedPanda", speed);
            if (speed < 0) transform.localScale= new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (transform.position.x > 185)//только для уровня 1!!!
            {
                speed = MaxSpeed;
            }
            transform.position = new Vector3(Mathf.Max(mainCamera.transform.position.x-8, transform.position.x + speed * Time.deltaTime), transform.position.y, transform.position.z);
            if (Input.GetKeyDown(KeyCode.C)) mainCamera.GetComponent<KinectManager>().Clear();
            if (animator.GetBool("OnGround") && Controll.GetJumpStart()&&(transform.position.x < 185))
            {
                
                //physics.AddForce(new Vector2(0f, maxJumpStrengh));
                physics.velocity=new Vector2(0f, maxJumpStrengh);
                sound.Play("Jump");
            }
            if ((Controll.GetJumpHold()) && (jumpHold > 0))
            {
                //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, maxJumpStrengh));
                //jumpHold = jumpHold - Time.deltaTime;
            }
            if (Controll.GetJumpStop()&& (physics.velocity.y>0) && (freeJump))
            {
                physics.AddForce(new Vector2(0f,-physics.velocity.y*kFall));
            }
            if ((mainCamera.transform.position.x<transform.position.x-1)&&!freezeCam)        mainCamera.transform.position= new Vector3(gameObject.transform.position.x-1, mainCamera.transform.position.y,mainCamera.transform.position.z);
            if ((transform.position.x > 200)&&!freezeCam)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                freezeCam = true;

            }
            if (transform.position.x > 215)
            {
                PlayerPrefs.SetInt("Hp", hp);
                PlayerPrefs.SetInt("Coins", coins);
                PlayerPrefs.SetInt("Enemy", enemyKill);
                if (bigState) PlayerPrefs.SetInt("Big", 1);

                mainCamera.GetComponent<KinectManager>().Clear();
                SceneManager.LoadScene(2);
            }
        }

        if (transform.position.y < -25)
        {
            
            //Deatch();
            if (hp > 1)
            {
                //PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
                mainCamera.GetComponent<KinectManager>().Clear();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                mainCamera.GetComponent<KinectManager>().Clear();
                Application.Quit();
            }


        }
        if (tube != "")
        {
            if (tube == "right")
            {
                transform.position = transform.position + new Vector3(Time.deltaTime, 0, 0);
                if (transform.position.x > startTube + 1.5)
                {
                    if (teleport != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "left")
            {
                transform.position = transform.position - new Vector3(Time.deltaTime, 0, 0);
                if (transform.position.x < startTube - 1.5)
                {
                    if (teleport != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "up")
            {
                transform.position = transform.position + new Vector3(0, Time.deltaTime, 0);
                if (transform.position.y > startTube + 1.5)
                {
                    if (teleport != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
            if (tube == "down")
            {
                transform.position = transform.position - new Vector3(0,Time.deltaTime, 0);
                if (transform.position.y < startTube - 1.5)
                {
                    if (teleport != null)
                    {
                        Teleportation();
                    }
                    else
                    {
                        ContinueAdvanture();
                    }
                }
            }
        }
    }

    private float SetDirect(string direction)
    {
        if ((direction == "right") || (direction == "left"))
        {
            return transform.position.x;
        }
        else
        {
            return transform.position.y;
        }
    }

    private void ContinueAdvanture()
    {
        isActive = true;
        physics.gravityScale = 2;
        
        foreach (Collider2D child in GetComponentsInChildren<Collider2D>())
        {
            child.enabled = true;
        }
        big.enabled = false;
        small.enabled = false;
        if (bigState) big.enabled = true;
        else small.enabled = true;
        tube = "";
        SetStopSpeed(true, true);
        SetStopSpeed(false, true);
    }

    private void Teleportation()
    {
        Teleport trash = teleport.GetComponent<Teleport>();
        freezeCam = trash.setFreezeCam;
        transform.position =new Vector3( trash.teleportTarget.x,trash.teleportTarget.y,transform.position.z);
        mainCamera.transform.position=new Vector3(trash.targetCam.x, trash.targetCam.y, mainCamera.transform.position.z);
        Debug.Log(trash.tubeExit);
        Debug.Log(SetDirect(trash.tubeExit));
        startTube = SetDirect(trash.tubeExit);
        tube = trash.tubeExit;
        sound.Play("Powerup7");

        teleport = null;
    }

    public void Setmortal()
    {
        unmortal = false;
        
    }
    public void setNormalSpeed()
    { Time.timeScale = 1f; }

    public void MustJump(float strenghtJump)
    {
        //physics.AddForce(new Vector2(0f, strenghtJump));
        if (Controll.GetJumpHold()) strenghtJump = strenghtJump * 1.5f;
        physics.velocity = new Vector2(0f, strenghtJump);
       
        freeJump = false;
    }

    public void ChangeCoin(int coinsCount)
    {
        coins = coins + coinsCount;
        moneyDisplay.text ="x "+coins.ToString();
    }
    public void ChangeEnemy()
    {
        enemyKill++;
        enemyDisplay.text = "x " + enemyKill.ToString();
    }
}
