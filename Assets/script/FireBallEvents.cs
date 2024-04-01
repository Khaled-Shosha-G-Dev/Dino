using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEvents : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
            Destroy(gameObject);
        else if (collision.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
