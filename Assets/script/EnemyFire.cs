using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private bool toRight = true;
    [SerializeField] private Rigidbody2D RB2;
    [SerializeField] private float Speed;
    [Header("sound\n")]
    [SerializeField] private AudioSource fireSound;
    // Start is called before the first frame update
    private void Start()
    {
        RB2 = GetComponent<Rigidbody2D>();
    }
    //private void Update()
    //{
    //    if (Mathf.Abs(RB2.velocity.x) == 0)
    //        toRight = !toRight;
    //}
    // Update is called once per frame
    private void FixedUpdate()
    {
        if(toRight)
            RB2.velocity = new Vector2(Time.fixedDeltaTime * Speed, RB2.velocity.y);
        else
            RB2.velocity = new Vector2(Time.fixedDeltaTime * Speed*(-1), RB2.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            toRight = !toRight;
        else if(collision.CompareTag("FireBall"))
            transform.localScale=transform.localScale+new Vector3(.05f, .05f, .05f);
    }
}
