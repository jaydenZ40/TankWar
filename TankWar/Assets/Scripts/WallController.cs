using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Bullet") || other.transform.CompareTag("Bullet2") || other.transform.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
