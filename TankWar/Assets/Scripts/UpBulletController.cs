using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBulletController : MonoBehaviour
{
    private float bulletMoveSpeed;
    private Vector3 bulletDirection;

    void Start()
    {
        bulletDirection = this.gameObject.name == "Bullet(Clone)" ? PlayerController.instance.lastNoneZeroDirection : Player2Controller.instance.lastNoneZeroDirection;
        bulletMoveSpeed = this.gameObject.name == "Bullet(Clone)" ? PlayerController.instance.bulletMoveSpeed: Player2Controller.instance.bulletMoveSpeed;
        //Debug.Log(this.gameObject.name);
        bulletDirection += new Vector3(0, 15, 0);
        bulletDirection = bulletDirection.normalized;
    }

    void Update()
    {
        transform.Translate(bulletDirection * Time.deltaTime * 1);
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
