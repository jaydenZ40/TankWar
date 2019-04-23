using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject bullet;
    public Rigidbody2D rb;
    public Collider2D collider;
    public float moveSpeed = 5;
    public Vector3 moveDirection = Vector3.zero;
    public Vector3 lastNoneZeroDirection = new Vector3(1, 0, 0); // default bullet direction is right
    public UnityEvent onShotPlayer1 = new UnityEvent();
    public UnityEvent onCollisionWithEnemy1 = new UnityEvent();
    public UnityEvent onRescuedPlayer2 = new UnityEvent();    // save player2's life
    public UnityEvent onHealPlayer1 = new UnityEvent();
    public UnityEvent onShieldPlayer1 = new UnityEvent();
    public bool isKnockedDown = false;
    public int damage = 1;
    public int powerupDamage = 2;
    public float bulletMoveSpeed = 5;
    public float distanceToPlayer2;

    private Vector3 oldPosition = Vector3.zero;
    private float timer = 0;

    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
        moveDirection = rb.transform.position - oldPosition;
        oldPosition = rb.transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (moveDirection != Vector3.zero)
        {
            lastNoneZeroDirection = moveDirection;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isKnockedDown)
        {
            Instantiate(bullet, rb.transform.position + moveDirection.normalized / 1.5f, Quaternion.identity);
        }

        distanceToPlayer2 = (rb.transform.position - Player2Controller.instance.transform.position).magnitude;

        damage = distanceToPlayer2 <= 5 ? powerupDamage : damage;  // Power up when two player are close to each other

        if (distanceToPlayer2 <= 1.5f && Input.GetKey(KeyCode.K) && Player2Controller.instance.isKnockedDown)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                onRescuedPlayer2.Invoke();
                timer = 0;
            }
            Debug.Log("Timer: " + timer + ", distance: " + distanceToPlayer2);
        }

        //if (Input.GetKeyUp(KeyCode.K) || distanceToPlayer2 > 1.5f)    // need to hold the key until another player is recovered??
        //{
        //    timer = 0;
        //}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy") && !isKnockedDown)
        {
            Destroy(other.gameObject);
            onCollisionWithEnemy1.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("EnemyBullet") && !isKnockedDown)
        {
            Destroy(other.gameObject);
            onShotPlayer1.Invoke();
        }

        if (other.transform.CompareTag("Heal"))
        {
            Destroy(other.gameObject);
            onHealPlayer1.Invoke();
        }

        if (other.transform.CompareTag("Shield"))
        {
            Destroy(other.gameObject);
            onShieldPlayer1.Invoke();
        }
    }
}
