using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    [Header("Player\n")]
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private SpriteRenderer SR;
    [SerializeField] private Animator anim;
    [SerializeField] private float speed, Jump , animTime ;
    [SerializeField] private bool  isJump,shootAnim,isAlive;
    private int Souls;
    private int D=0;
    private bool isCrash;
    private bool gameIsEnd;
    [Header("FireBall\n")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform fireBallLocation;
    [SerializeField] private float ballSpeed;
    [SerializeField] private float fireBallLife;
    private int counterToAnimShoot;
    [Header("UI\n")]
    [SerializeField] private Transform Panel;
    private TextMeshProUGUI healthText;
    private Image healthBar;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI gameOver;
    private TextMeshProUGUI youWon;
    public Button restartButton;
    public Button menuButton;
    private TextMeshProUGUI Info;
    private int Score;
    [Header("Sounds\n")]
    //[SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource takeCherrySound;
    [SerializeField] private AudioSource takeGemSound;
    [SerializeField] private AudioSource completeHealthSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource gameOverSound;
    [SerializeField] private AudioSource crashSound;
    //[Header("Dashwind\n")]
    //[SerializeField] private GameObject dashWind;
    //[SerializeField] private Transform dashWindLoaction;
    //[SerializeField] private float windTime;
    // Update is called once per frame
    private void Start()
    {
        //moving& actions

        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        isJump = false;
        shootAnim = false;
        counterToAnimShoot = 0;
        gameIsEnd = false;
        
        //events with another object
        
        Souls = 5;
        isCrash = false;
        isAlive = true;
        Score = 0;

        //text & gui
 
        healthText = Panel.Find("TextHealth").GetComponent<TextMeshProUGUI>();
        healthText.text = Souls * 20 + "%";
        healthBar = Panel.Find("HealthBar").GetComponent<Image>();
        healthBar.fillAmount = (Souls * 20) / 100;
        scoreText= Panel.Find("Score").GetComponent<TextMeshProUGUI>();
        scoreText.text = "Score : " + Score;
        gameOver = Panel.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameOver.enabled = false;
        youWon = Panel.Find("YouWon").GetComponent<TextMeshProUGUI>();
        youWon.enabled = false;
        Info = Panel.Find("Info").GetComponent<TextMeshProUGUI>();
        Info.enabled = true;
        restartButton.enabled = false;
        menuButton.enabled = false;
        restartButton.image.enabled = false;
        menuButton.image.enabled = false;
    }
    private void Update()
    {
                    ///Player Controler
                    ///Jump
        if (Input.GetButtonDown("Jump")&&!isJump && !isCrash && !gameIsEnd)
        {
            Info.enabled=false;
            jumpSound.Play();
            RB.velocity = new Vector2(0, Jump);
            isJump = true;
        }
        if(Mathf.Abs(RB.velocity.y) < 0.01)
            isJump = false;

                    ///Player Direction move 

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))&& !gameIsEnd && !isCrash)
        { SR.flipX = true; Info.enabled = false; }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !gameIsEnd && !isCrash)
        { SR.flipX = false; Info.enabled = false; }
                    ///Player Shooting

        if(Input.GetKeyDown(KeyCode.F)&&!shootAnim&&!isCrash && !gameIsEnd)
        {
            shootSound.Play();
            Info.enabled = false;
            shootAnim = true;
            counterToAnimShoot = 0;
        }
        if (counterToAnimShoot >= animTime)
            shootAnim = false;
        FireBallDirection();

        ///Player out of range

        if (transform.position.y < -18)
        {
            gameOverSound.Play();
            gameOver.text = "Game Over";
            gameOver.enabled = true;
            SR.enabled= false;
            StartCoroutine(OutOFRange());
        }
         

                    ///Crash with Enemy
        if (isCrash)
            StartCoroutine(LostSoul());
        if(!isAlive)
            StartCoroutine(Death());

                    ///HealthBarColor
        
        switch(Souls)
        {
            case 5:
                {
                    healthBar.color = Color.Lerp(Color.gray, Color.green, .5f);
                    break;
                }
            case 4:
                {
                    healthBar.color = Color.Lerp(Color.green,Color.yellow,.7f);
                    break;
                }
            case 3:
                {
                    healthBar.color = Color.yellow;
                    break;
                }
            case 2:
                {
                    healthBar.color = Color.Lerp(Color.yellow, Color.red, .5f);
                    break;
                }
            case 1:
                {
                    healthBar.color = Color.red;
                    break;
                }
        }
        if(Input.GetKey(KeyCode.M))
        {
            restartButton.image.enabled = true;
            menuButton.image.enabled = true;
            restartButton.enabled = true;
            menuButton.enabled = true;
            gameIsEnd = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            restartButton.enabled = false;
            menuButton.enabled = false;
            restartButton.image.enabled = false;
            menuButton.image.enabled = false;
            gameIsEnd = false; 
        }
    }
    void FixedUpdate()
    {
            counterToAnimShoot++;

                        ///Player Velocity when Run
            if(!isCrash && !gameIsEnd)
                RB.velocity = new Vector2(speed * Input.GetAxis("Horizontal") * Time.deltaTime, RB.velocity.y);
                        ///Var to animator

            anim.SetFloat("Speed", Mathf.Abs(RB.velocity.x));
            anim.SetFloat("Jump", Mathf.Abs(RB.velocity.y));
            anim.SetBool("Shoot", shootAnim);
            anim.SetBool("isCrash", isCrash);
            anim.SetBool("isAlive", isAlive);
            anim.SetInteger("Souls",Souls);
            anim.SetTrigger("TouchFire");
                        ///Fire the FireBall
       
        if (shootAnim && (counterToAnimShoot == 1))  
            ShootBall();
            
     }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire")&&Souls!=1&&!isCrash)
        {
            anim.SetTrigger("TouchFire");
            crashSound.Play();
            Souls--;
            isCrash = true;
            healthText.text = Souls * 20 + "%";
            healthBar.fillAmount = (Souls * 20.0f) / 100;
        }
        else if(collision.CompareTag("Fire")&&Souls==1 && !isCrash)
        {
            anim.SetTrigger("TouchFire");
            crashSound.Play();
            isAlive = false;
            Souls--;
            healthText.text = Souls * 20 + "%";
            healthBar.fillAmount = (Souls * 20.0f) / 100;
            gameOverSound.Play();
            gameOver.text = "Game Over";
            gameOver.enabled = true;
        }
        else if (collision.CompareTag("Cherry"))
        {
            takeCherrySound.Play();
            if(Souls < 5)
            {
                Souls++;
                if(Souls==5)
                completeHealthSound.Play();
                healthText.text = Souls * 20 + "%";
                healthBar.fillAmount = (Souls * 20.0f) / 100;
            }
            else if(Souls==5)
            {
                Score += 10;
                scoreText.text = "Score : " + Score;
            }
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Gem"))
        {
            Score++;
            takeGemSound.Play();
            scoreText.text = "Score : "+Score;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EndLine"))
        {
            gameIsEnd=true;
            youWon.text = "YOU WON";
            winSound.Play();
            youWon.enabled = true;
            restartButton.image.enabled = true;
            menuButton.image.enabled = true;
            restartButton.enabled = true;
            menuButton.enabled = true;
            RB.velocity = new Vector2(0, 0);
        }
    }
    IEnumerator OutOFRange()
    {
        yield return new WaitForSeconds(2.5f);
        restart();
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(.95f);
        Destroy(gameObject);
        restart();
    }
    IEnumerator LostSoul()
    {
        yield return new WaitForSeconds(1f);
        isCrash = false;
    }
    private void restart()
    {
        SceneManager.LoadScene("level 01");
    }
    private void ShootBall()
    {
        GameObject Ball= Instantiate(fireBall, fireBallLocation.position, fireBallLocation.rotation);
        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(ballSpeed * FireBallDirection()* Time.deltaTime, 0f);
        Ball.transform.localScale = new Vector2(Ball.transform.localScale.x* FireBallDirection(),Ball.transform.localScale.y);
        Destroy(Ball,fireBallLife);
    }
    int FireBallDirection()
    {
        if (SR.flipX)
            D= -1;
        else if (!SR.flipX)
            D= 1;
        return D;
    }
}
