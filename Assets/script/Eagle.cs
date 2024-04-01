using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private SpriteRenderer eagleSeen;
    [SerializeField] private float speed, endPoint, dieTime;
    [SerializeField] private Animator anim;
    [SerializeField]private CapsuleCollider2D capsuleCollider;
    private bool isAlive;
    private Vector3 startPos;
    [Header("sounds\n")]
    [SerializeField] private AudioSource flutterSound;
    [SerializeField] private AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        eagleSeen = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
        anim = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isAlive", isAlive);
        if (Player.position.x > transform.position.x)
            eagleSeen.flipX = true;
        else if (Player.position.x < transform.position.x|| Player.position.x == transform.position.x)
            eagleSeen.flipX = false;
        if (!isAlive)
        { flutterSound.loop=false; StartCoroutine(Death());  }
        else if (isAlive)
            StartCoroutine(EagleMovement());

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireBall"))
        {
            deathSound.Play();
            isAlive =false;
            anim.SetBool("isAlive", isAlive);
        }
    }
    IEnumerator EagleMovement()
    {
        bool isflight = true;
        Vector3 endPos = new (startPos.x, startPos.y+endPoint, startPos.z);
        float temp = 0;
        while(true)
        {
            yield return null;
            if(isflight)
                transform.position = Vector3.Lerp(startPos,endPos,temp);
            else
                transform.position = Vector3.Lerp(endPos, startPos, temp);
            temp+=Time.deltaTime*speed;
            if(temp>1)
            {
                temp=0;
                isflight=!isflight;
            }
        }
    }
    IEnumerator Death()
    {
        
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(.7f);
        Destroy(gameObject);
    }
}

