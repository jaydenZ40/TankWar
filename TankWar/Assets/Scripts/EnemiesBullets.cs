using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBullets : MonoBehaviour
{
    public float bulletMoveSpeed = 3;
    public static int damage = 5;

    private Vector3 bulletDirection;
    private float distanceToPlayer1;
    private float distanceToPlayer2;

    void Start()
    {
        // enemy's bullet will be shot to the player who is closer to itself.
        distanceToPlayer1 = (PlayerController.instance.transform.position - transform.position).magnitude;
        distanceToPlayer2 = (Player2Controller.instance.transform.position - transform.position).magnitude;
        bulletDirection = distanceToPlayer1 <= distanceToPlayer2 ?
            (PlayerController.instance.transform.position - transform.position).normalized : (Player2Controller.instance.transform.position - transform.position).normalized;
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
