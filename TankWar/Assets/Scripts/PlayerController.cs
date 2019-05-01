using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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
    public TextMeshProUGUI rescueTime;
    public TextMeshProUGUI overheat;

    private Vector3 oldPosition = Vector3.zero;
    private float timer = 0;
    private int bulletNum = 0;

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
       
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        moveDirection = rb.transform.position - oldPosition;
        oldPosition = rb.transform.position;

        if (moveDirection != Vector3.zero)
        {
            lastNoneZeroDirection = moveDirection;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isKnockedDown && bulletNum < 5)
        {
            if (ButtonController.weaponType1 != 2)
                Instantiate(bullet, rb.transform.position + moveDirection.normalized / 1.5f, Quaternion.identity);
            else
            {
                var numShots = 3;
                var spreadAngle = 2.0f;
                var qAngle = Quaternion.AngleAxis((float)(-(numShots) / 2.0 * spreadAngle), transform.up) * transform.rotation;
                var qDelta = Quaternion.AngleAxis(spreadAngle, transform.up);

                for (var i = 0; i < numShots; i++)
                {
                    GameObject go = Instantiate(bullet, transform.position, qAngle);
                    go.GetComponent<Rigidbody2D>().AddForce(go.transform.forward * 1000);
                    qAngle = qDelta * qAngle;
                }
            }
            bulletNum++;
            Invoke("ShotReady", 3);
        }
        else if (bulletNum >= 5)
        {
            overheat.gameObject.SetActive(true);
        }

        distanceToPlayer2 = (rb.transform.position - Player2Controller.instance.transform.position).magnitude;

        damage = distanceToPlayer2 <= 5 ? powerupDamage : damage;  // Power up when two player are close to each other
        bullet.GetComponent<SpriteRenderer>().color = distanceToPlayer2 <= 5 ? new Color(255, 0, 0) : new Color(255, 255, 255);

        if (distanceToPlayer2 <= 1.5f && Input.GetKey(KeyCode.K) && Player2Controller.instance.isKnockedDown)
        {
            timer += Time.deltaTime;
            rescueTime.gameObject.SetActive(true);
            rescueTime.text = (3 - (int)timer) + "sec";
            Debug.Log("Timer: " + timer + ", distance: " + distanceToPlayer2);
            if (timer >= 3)
            {
                onRescuedPlayer2.Invoke();
                timer = 0;
                rescueTime.gameObject.SetActive(false);
            }
        }

        //if (Input.GetKeyUp(KeyCode.K) || distanceToPlayer2 > 1.5f)    // need to hold the key until another player is recovered??
        //{
        //    timer = 0;
        //}
    }
    void ShotReady()
    {
        bulletNum = 0;
        overheat.gameObject.SetActive(false);
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
