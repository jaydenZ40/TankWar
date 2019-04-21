using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI health1;
    public TextMeshProUGUI health2;
    public GameObject Enemy;
    public GameObject line;

    private int waveNum = 1;
    [SerializeField] private int NumOfEnemiesLeft = 0;
    [SerializeField] private int NumOfEnemiesWillSpawn = 10;
    private int PlayerHealth1 = 100;
    private int PlayerHealth2 = 100;
    private float timer = 0;
    private Color tmpColor;
    private Color tmpColor2;

    void Start()
    {
        instance = this;

        InitCharacterProperty();

        Physics2D.IgnoreLayerCollision(8, 9);   // ignore collision (player--bullet)
        Physics2D.IgnoreLayerCollision(10, 11); // ignore collision (enenmy--enemyBullet)
        Physics2D.IgnoreLayerCollision(10, 12); // ignore collision (enemy--boundary), enenmy will spawn outside the screen, so that allow enemy enter screen without collisions

        PlayerController.instance.onCollisionWithEnemy1.AddListener(Player1CollidesEnemy);
        PlayerController.instance.onShotPlayer1.AddListener(Player1IsShot);
        PlayerController.instance.onRescuedPlayer2.AddListener(Player2Recover);

        Player2Controller.instance.onCollisionWithEnemy2.AddListener(Player2CollidesEnemy);
        Player2Controller.instance.onShotPlayer2.AddListener(Player2IsShot);
        Player2Controller.instance.onRescuedPlayer1.AddListener(Player1Recover);

        tmpColor = PlayerController.instance.GetComponent<SpriteRenderer>().color;
        tmpColor2 = Player2Controller.instance.GetComponent<SpriteRenderer>().color;

        Debug.Log("P1:" + ButtonController.player1 + ", P2:" + ButtonController.player2);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > Random.Range(5, 15) && NumOfEnemiesWillSpawn > 0)   // spawn enemies
        {
            timer = 0;
            Instantiate(Enemy, GetEnemyRandomSpawnPosition(), Quaternion.identity);
            NumOfEnemiesWillSpawn--;
            NumOfEnemiesLeft++;
        }

        if (NumOfEnemiesLeft == 0 && NumOfEnemiesWillSpawn == 0 && waveNum < 10)  // wave1: 10 enemies, wave2: 15 enemies, wave3: 20 enemies....
        {
            waveNum++;
            waveText.text = "Wave: " + waveNum;
            NumOfEnemiesWillSpawn += 10 + (waveNum - 1) * 5;
        }

        if (NumOfEnemiesLeft == 0 && NumOfEnemiesWillSpawn == 0 && waveNum == 10) // win!
        {
            SceneManager.LoadScene("Win");
        }

        DrawLine();
    }

    void DrawLine()
    {
        float distance = PlayerController.instance.distanceToPlayer2;
        float scaleY;
        if (distance > 2)
            scaleY = 100 / distance + 5;
        else scaleY = 50;

        Vector3 P1 = PlayerController.instance.transform.position;
        Vector3 P2 = Player2Controller.instance.transform.position;
        float deltaX = P2.x - P1.x;
        float deltaY = P2.y - P1.y;

        float rotateZ = Mathf.Atan(deltaY / deltaX);
        line.transform.rotation = Quaternion.AxisAngle(new Vector3(0, 0, 1), rotateZ);
        line.transform.localScale = new Vector3(distance, scaleY, 1);
        line.transform.position = (P2 + P1) / 2;

        //Debug.Log(distance);
        //Debug.Log(line.GetComponent<SpriteRenderer>().color.a);
        if (distance > 5)
            line.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0);
        else line.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255 - 51 * distance);
    }

    void Player1CollidesEnemy()
    {
        PlayerHealth1 -= 50;
        NumOfEnemiesLeft--;
        if (PlayerHealth1 <= 0)
        {
            PlayerHealth1 = 0;
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
            CheckGameover();
        }
        health1.text = "Health: " + PlayerHealth1;
    }

    void Player1IsShot()
    {
        PlayerHealth1 -= EnemiesBullets.damage;
        if (PlayerHealth1 <= 0)
        {
            PlayerHealth1 = 0;
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
            CheckGameover();
        }
        health1.text = "Health: " + PlayerHealth1;
    }

    void Player2CollidesEnemy()
    {
        PlayerHealth2 -= 50;
        NumOfEnemiesLeft--;
        if (PlayerHealth2 <= 0)
        {
            PlayerHealth2 = 0;
            Player2Controller.instance.isKnockedDown = true;
            Player2Controller.instance.moveSpeed = 1;
            CheckGameover();
        }
        health2.text = "Health: " + PlayerHealth2;
    }

    void Player2IsShot()
    {
        PlayerHealth2 -= EnemiesBullets.damage;
        if (PlayerHealth2 <= 0)
        {
            PlayerHealth2 = 0;
            Player2Controller.instance.isKnockedDown = true;
            Player2Controller.instance.moveSpeed = 1;
            CheckGameover();
        }
        health2.text = "Health: " + PlayerHealth2;
    }

    void Player1Recover()
    {
        PlayerController.instance.GetComponent<SpriteRenderer>().color = new Color(150, 255, 255, 150);
        Debug.Log(PlayerController.instance.GetComponent<SpriteRenderer>().color.a);
        PlayerController.instance.collider.enabled = false;
        PlayerHealth1 = 30;
        health1.text = "Health: " + PlayerHealth1;
        PlayerController.instance.isKnockedDown = false;
        PlayerController.instance.moveSpeed = 5;
        Invoke("SetColliderActive", 3); // invincible for 3 sec

    }

    void Player2Recover()
    {
        Player2Controller.instance.GetComponent<SpriteRenderer>().color /= 2;
        Player2Controller.instance.collider.enabled = false;
        PlayerHealth2 = 30;
        health2.text = "Health: " + PlayerHealth2;
        Player2Controller.instance.isKnockedDown = false;
        Player2Controller.instance.moveSpeed = 5;
        Invoke("SetColliderActive", 3); // invincible for 3 sec
    }

    void SetColliderActive()
    {
        PlayerController.instance.collider.enabled = true;
        PlayerController.instance.GetComponent<SpriteRenderer>().color = tmpColor;
        Player2Controller.instance.collider.enabled = true;
        Player2Controller.instance.GetComponent<SpriteRenderer>().color = tmpColor2;
    }

    Vector3 GetEnemyRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        float x= 0, y = 0;
        while ((x > -10 && x < 10) && (y > -6 && y < 6))
        {
            x = Random.Range(-15, 15);
            y = Random.Range(-10, 10);
        }
        randomPosition.x = x;
        randomPosition.y = y;
        return randomPosition;
    }

    public void DecNumOfEnemiesLeft()
    {
        NumOfEnemiesLeft--;
    }

    void CheckGameover()    // Game over if two player is knocked down at same time
    {
        if (PlayerController.instance.isKnockedDown && Player2Controller.instance.isKnockedDown)
        {
            SceneManager.LoadScene("Gameover");
        }
    }

    void InitCharacterProperty()
    {
        switch (ButtonController.player1)
        {
            case 1:
                PlayerHealth1 = 100;
                health1.text = "Health: 100";
                PlayerController.instance.damage = 1;
                PlayerController.instance.bulletMoveSpeed = 10;
                break;
            case 2:
                PlayerHealth1 = 100;
                health1.text = "Health: 102";
                PlayerController.instance.damage = 1;
                PlayerController.instance.bulletMoveSpeed = 10;
                break;
            case 3:
                PlayerHealth1 = 100;
                health1.text = "Health: 103";
                PlayerController.instance.damage = 1;
                PlayerController.instance.bulletMoveSpeed = 10;
                break;
            case 4:
                PlayerHealth1 = 100;
                health1.text = "Health: 104";
                PlayerController.instance.damage = 1;
                PlayerController.instance.bulletMoveSpeed = 10;
                break;
        }

        switch (ButtonController.player2)
        {
            case 1:
                PlayerHealth2 = 100;
                health2.text = "Health: 100";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.bulletMoveSpeed = 10;
                break;
            case 2:
                PlayerHealth2 = 100;
                health2.text = "Health: 102";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.bulletMoveSpeed = 10;
                break;
            case 3:
                PlayerHealth2 = 100;
                health2.text = "Health: 103";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.bulletMoveSpeed = 10;
                break;
            case 4:
                PlayerHealth2 = 100;
                health2.text = "Health: 104";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.bulletMoveSpeed = 10;
                break;
        }
    }
}
