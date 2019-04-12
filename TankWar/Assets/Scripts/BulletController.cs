using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletMoveSpeed = 10;
    public static int damage = 1;

    private Vector3 bulletDirection;

    void Start()
    {
        bulletDirection = PlayerController.instance.lastNoneZeroDirection.normalized;
    }

    void Update()
    {
        transform.Translate(bulletDirection * Time.deltaTime * bulletMoveSpeed);
        if (IsOutsideScreen())
        {
            Destroy(gameObject);
        }
    }
    bool IsOutsideScreen()
    {
        if (transform.position.x < -9 || transform.position.x > 9
            || transform.position.y < -5 || transform.position.y > 5)   // screen size x = (-9, 9), y = (-5, 5)
        {
            return true;
        }
        return false;
    }
}
