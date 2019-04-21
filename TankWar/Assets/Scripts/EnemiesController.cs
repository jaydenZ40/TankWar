using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesController : MonoBehaviour
{
    public static EnemiesController instance;

    public Vector3 bulletDirection;
    public float shootIntervalMin = 5;
    public float shootIntervalMax = 15;
    public GameObject enemyBullet;
    public UnityEvent onShotEnemy = new UnityEvent();
    public int enemyHealth = 5;
    public float enemyMoveSpeed = 0.5f;

    private float timer = 0;
    private float distanceToPlayer1;
    private float distanceToPlayer2;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // enemy will move forward the player who is closer to itself.
        distanceToPlayer1 = (PlayerController.instance.transform.position - transform.position).magnitude;
        distanceToPlayer2 = (Player2Controller.instance.transform.position - transform.position).magnitude;
        bulletDirection = distanceToPlayer1 <= distanceToPlayer2 ? 
            (PlayerController.instance.transform.position - transform.position).normalized : (Player2Controller.instance.transform.position - transform.position).normalized;

        timer += Time.deltaTime;

        if (timer > Random.Range(shootIntervalMin, shootIntervalMax))
        {
            timer = 0;
            Instantiate(enemyBullet, transform.position + bulletDirection, Quaternion.identity);
        }

        transform.Translate(bulletDirection * Time.deltaTime * enemyMoveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            enemyHealth -= PlayerController.instance.damage;
            if (enemyHealth <= 0)
            {
                Destroy(this.gameObject);
                GameManager.instance.DecNumOfEnemiesLeft();
            }
            onShotEnemy.Invoke();
        }

        if (other.transform.CompareTag("Bullet2"))
        {
            Destroy(other.gameObject);
            enemyHealth -= Player2Controller.instance.damage;
            if (enemyHealth <= 0)
            {
                Destroy(this.gameObject);
                GameManager.instance.DecNumOfEnemiesLeft();
            }
            onShotEnemy.Invoke();
        }
    }
}
