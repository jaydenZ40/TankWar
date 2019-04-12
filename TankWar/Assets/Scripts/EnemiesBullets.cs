using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBullets : MonoBehaviour
{
    public float bulletMoveSpeed = 3;
    public static int damage = 5;

    private Vector3 bulletDirection;

    void Start()
    {
        bulletDirection = (PlayerController.instance.transform.position - transform.position).normalized;
    }
    void Update()
    {
        transform.Translate(bulletDirection * Time.deltaTime * bulletMoveSpeed);
        if (IsOutsideRange())
        {
            Destroy(gameObject);
        }
    }

    bool IsOutsideRange()   // the bullet will be destroyed if it is outside the enemies spawning range
    {
        if (transform.position.x < -16 || transform.position.x > 16 
            || transform.position.y < -11 || transform.position.y > 11)
        {
            return true;
        }
        return false;
    }
}
