using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject bullet;
    public Rigidbody2D rb;
    public float moveSpeed = 5;

    private Vector3 oldPosition = Vector3.zero;

    void Awake()
    {
        instance = this;
        rb = this.transform.GetComponent<Rigidbody2D>();
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
        Vector3 moveDirection = rb.transform.position - oldPosition;
        oldPosition = rb.transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(bullet, rb.transform.position + rb.transform.up, Quaternion.identity);
        }
    }
}
