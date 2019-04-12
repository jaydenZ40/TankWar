using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public static Vector3 bulletDirection;

    public float shotInterval = 3;
    public GameObject enemyBullet;

    private float timer = 0;

    void Update()
    {
        bulletDirection = (PlayerController.instance.transform.position - transform.position).normalized;
        timer += Time.deltaTime;

        if (timer > shotInterval)
        {
            timer -= shotInterval;
            Instantiate(enemyBullet, transform.position + bulletDirection, Quaternion.identity);
        }
    }
}
