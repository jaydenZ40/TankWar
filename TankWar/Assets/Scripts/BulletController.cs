using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletMoveSpeed = 10;
    private Vector3 bulletDirection;

    void Start()
    {
        bulletDirection = this.gameObject.name == "bullet(Clone)" ? 
            PlayerController.instance.lastNoneZeroDirection.normalized : Player2Controller.instance.lastNoneZeroDirection.normalized;
        //bulletMoveSpeed = this.gameObject.name == "Bullet(Clone)" ? PlayerController.instance.bulletMoveSpeed: Player2Controller.instance.bulletMoveSpeed;
        Debug.Log(this.gameObject.name);
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
        if (transform.position.x < -18 || transform.position.x > 18
            || transform.position.y < -10 || transform.position.y > 10)   // screen size x = (-18, 18), y = (-10, 10)
        {
            return true;
        }
        return false;
    }
}
