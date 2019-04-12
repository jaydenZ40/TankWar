using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletMoveSpeed = 10;

    private Vector3 bulletDirection;
    private float damage = 1;

    void Start()
    {
        bulletDirection = PlayerController.instance.transform.up;
    }

    // Update is called once per frame
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
        if (transform.position.x < -8.5 || transform.position.x > 8.5
            || transform.position.y < -4.5 || transform.position.y > 4.5)
        {
            return true;
        }
        return false;
    }
}
