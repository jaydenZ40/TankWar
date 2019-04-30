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
    public TextMeshProUGUI shield1;
    public TextMeshProUGUI shield2;
    public TextMeshProUGUI rescueTime;
    public GameObject Enemy;
    public GameObject line;
    public GameObject Heal;
    public GameObject Shield;
    public GameObject Bullet;
    public GameObject Bullet2;
    public Sprite tank1, tank2;
    public GameObject player1, player2;
    public Sprite tank1Down, tank2Down;

    private int waveNum = 1;
    [SerializeField] private int NumOfEnemiesLeft = 0;
    [SerializeField] private int NumOfEnemiesWillSpawn = 10;
    private int PlayerHealth1 = 100;
    private int PlayerHealth2 = 100;
    private int PlayerMaxHealth1 = 100;
    private int PlayerMaxHealth2 = 100;
    private int PlayerShield1 = 0;
    private int PlayerShield2 = 0;
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
        PlayerController.instance.onHealPlayer1.AddListener(AddPlayer1Health);
        PlayerController.instance.onShieldPlayer1.AddListener(AddPlayer1Shield);

        Player2Controller.instance.onCollisionWithEnemy2.AddListener(Player2CollidesEnemy);
        Player2Controller.instance.onShotPlayer2.AddListener(Player2IsShot);
        Player2Controller.instance.onRescuedPlayer1.AddListener(Player1Recover);
        Player2Controller.instance.onHealPlayer2.AddListener(AddPlayer2Health);
        Player2Controller.instance.onShieldPlayer2.AddListener(AddPlayer2Shield);

        //EnemiesController.instance.onShotEnemy.AddListener(RandomSpawnHealthOrShield);

        tmpColor = PlayerController.instance.GetComponent<SpriteRenderer>().color;
        tmpColor2 = Player2Controller.instance.GetComponent<SpriteRenderer>().color;

        if (ButtonController.weaponType1 == 2)
        {
            player1.GetComponent<SpriteRenderer>().sprite = tank2;
        }
        if (ButtonController.weaponType2 == 2)
        {
            player2.GetComponent<SpriteRenderer>().sprite = tank2;
        }

        //Debug.Log("P1:" + ButtonController.player1 + ", P2:" + ButtonController.player2);
    }

    void Update()
    {
        SpawnEnemies();

        NextWave();

        CheckWin();

        DrawLine();
    }

    void SpawnEnemies()
    {

        timer += Time.deltaTime;
        if (timer > Random.Range(2, 10) && NumOfEnemiesWillSpawn > 0)   // spawn enemies
        {
            timer = 0;
            Instantiate(Enemy, GetEnemyRandomSpawnPosition(), Quaternion.identity);
            NumOfEnemiesWillSpawn--;
            NumOfEnemiesLeft++;
        }
    }

    void NextWave()
    {
        if (NumOfEnemiesLeft == 0 && NumOfEnemiesWillSpawn == 0 && waveNum < 10)  // wave1: 10 enemies, wave2: 15 enemies, wave3: 20 enemies....
        {
            waveNum++;
            waveText.text = "Wave: " + waveNum;
            NumOfEnemiesWillSpawn += 10 + (waveNum - 1) * 5;

            EnemiesController.instance.enemyMoveSpeed += 0.3f;
        }
    }

    void CheckWin()
    {
        if (NumOfEnemiesLeft == 0 && NumOfEnemiesWillSpawn == 0 && waveNum == 10) // win!
        {
            SceneManager.LoadScene("Win");
        }
    }

    void DrawLine()
    {
        float distance = PlayerController.instance.distanceToPlayer2;
        //float scaleY;
        //if (distance > 2)
        //    scaleY = 50 / distance + 5;
        //else scaleY = 25;

        Vector3 P1 = PlayerController.instance.transform.position;
        Vector3 P2 = Player2Controller.instance.transform.position;
        float deltaX = P2.x - P1.x;
        float deltaY = P2.y - P1.y;

        float rotateZ = Mathf.Atan(deltaY / deltaX);
        line.transform.rotation = Quaternion.AxisAngle(new Vector3(0, 0, 1), rotateZ);
        line.transform.localScale = new Vector3(distance, 5, 1);
        line.transform.position = (P2 + P1) / 2;

        //Debug.Log(distance);
        //Debug.Log(line.GetComponent<SpriteRenderer>().color.a);
        if (distance > 5)
            line.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 0);
        else line.GetComponent<SpriteRenderer>().color = new Color(255, 255 / distance, 0, 255 - 51 * distance);
        //Debug.Log(line.GetComponent<SpriteRenderer>().color);
    }

    void Player1CollidesEnemy()
    {
        PlayerShield1 -= 25;
        if (PlayerShield1 < 0)
        {
            PlayerHealth1 += PlayerShield1;
            PlayerShield1 = 0;
        }
        NumOfEnemiesLeft--;
        if (PlayerHealth1 <= 0)
        {
            PlayerHealth1 = 0;
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
            CheckGameover();
            if (ButtonController.weaponType1 == 1)
            {
                player1.GetComponent<SpriteRenderer>().sprite = tank1Down;
            }
            else player1.GetComponent<SpriteRenderer>().sprite = tank2Down;
        }
        health1.text = "Health: " + PlayerHealth1;
        shield1.text = "Shield: " + PlayerShield1;
    }

    void Player1IsShot()
    {
        PlayerShield1 -= EnemiesBullets.damage;
        if (PlayerShield1 < 0)
        {
            PlayerHealth1 += PlayerShield1;
            PlayerShield1 = 0;
        }
        if (PlayerHealth1 <= 0)
        {
            PlayerHealth1 = 0;
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
            CheckGameover();

            if (ButtonController.weaponType1 == 1)
            {
                player1.GetComponent<SpriteRenderer>().sprite = tank1Down;
            }
            else player1.GetComponent<SpriteRenderer>().sprite = tank2Down;
        }
        health1.text = "Health: " + PlayerHealth1;
        shield1.text = "Shield: " + PlayerShield1;
    }

    void Player2CollidesEnemy()
    {
        PlayerShield2 -= 25;
        if (PlayerShield2 < 0)
        {
            PlayerHealth2 += PlayerShield2;
            PlayerShield2 = 0;
        }
        NumOfEnemiesLeft--;
        if (PlayerHealth2 <= 0)
        {
            PlayerHealth2 = 0;
            Player2Controller.instance.isKnockedDown = true;
            Player2Controller.instance.moveSpeed = 1;
            CheckGameover();
            if (ButtonController.weaponType2 == 1)
            {
                player2.GetComponent<SpriteRenderer>().sprite = tank1Down;
            }
            else player2.GetComponent<SpriteRenderer>().sprite = tank2Down;
        }
        health2.text = "Health: " + PlayerHealth2;
        shield2.text = "Shield: " + PlayerShield2;
    }

    void Player2IsShot()
    {
        PlayerShield2 -= EnemiesBullets.damage;
        if (PlayerShield2 < 0)
        {
            PlayerHealth2 += PlayerShield2;
            PlayerShield2 = 0;
        }
        if (PlayerHealth2 <= 0)
        {
            PlayerHealth2 = 0;
            Player2Controller.instance.isKnockedDown = true;
            Player2Controller.instance.moveSpeed = 1;
            CheckGameover();
            if (ButtonController.weaponType2 == 1)
            {
                player2.GetComponent<SpriteRenderer>().sprite = tank1Down;
            }
            else player2.GetComponent<SpriteRenderer>().sprite = tank2Down;
        }
        health2.text = "Health: " + PlayerHealth2;
        shield2.text = "Shield: " + PlayerShield2;
    }

    void CheckGameover()    // Game over if two player is knocked down at same time
    {
        if (PlayerController.instance.isKnockedDown && Player2Controller.instance.isKnockedDown)
        {
            SceneManager.LoadScene("Gameover");
        }
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
        Invoke("SetColliderActive", 3); // invincible for 3 sec after being saved
        if (ButtonController.weaponType1 == 1)
            player1.GetComponent<SpriteRenderer>().sprite = tank1;
        else player1.GetComponent<SpriteRenderer>().sprite = tank2;
    }

    void Player2Recover()
    {
        Player2Controller.instance.GetComponent<SpriteRenderer>().color /= 2;
        Player2Controller.instance.collider.enabled = false;
        PlayerHealth2 = 30;
        health2.text = "Health: " + PlayerHealth2;
        Player2Controller.instance.isKnockedDown = false;
        Player2Controller.instance.moveSpeed = 5;
        Invoke("SetColliderActive", 3); // invincible for 3 sec after being saved
        if (ButtonController.weaponType2 == 1)
            player2.GetComponent<SpriteRenderer>().sprite = tank1;
        else player2.GetComponent<SpriteRenderer>().sprite = tank2;
    }

    void SetColliderActive()
    {
        PlayerController.instance.collider.enabled = true;
        PlayerController.instance.GetComponent<SpriteRenderer>().color = tmpColor;
        Player2Controller.instance.collider.enabled = true;
        Player2Controller.instance.GetComponent<SpriteRenderer>().color = tmpColor2;
    }

    void AddPlayer1Health()
    {
        PlayerHealth1 += 30;
        if (PlayerHealth1 >= PlayerMaxHealth1)
            PlayerHealth1 = PlayerMaxHealth1;
        health1.text = "Health: " + PlayerHealth1;
    }

    void AddPlayer1Shield()
    {
        PlayerShield1 += 30;
        shield1.text = "Shield: " + PlayerShield1;
    }

    void AddPlayer2Health()
    {
        PlayerHealth2 += 30;
        if (PlayerHealth1 >= PlayerMaxHealth2)
            PlayerHealth1 = PlayerMaxHealth2;
        health2.text = "Health: " + PlayerHealth2;
    }

    void AddPlayer2Shield()
    {
        PlayerShield2 += 30;
        shield2.text = "Shield: " + PlayerShield2;
    }

    //void RandomSpawnHealthOrShield(Vector3 pos)
    //{
    //    //if (Random.Range(0, 10) == 0)
    //    if (true)
    //    {
    //        if (Random.Range(0, 2) == 0)
    //            Instantiate(Heal, pos, Quaternion.identity);
    //        else Instantiate(Shield, pos, Quaternion.identity);
    //    }
    //}

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

    void InitCharacterProperty()
    {
        switch (ButtonController.player1)
        {
            case 1:
                PlayerMaxHealth1 = 100;
                PlayerHealth1 = 100;
                health1.text = "Health: 100";
                PlayerController.instance.damage = 1;
                PlayerController.instance.powerupDamage = 2;
                Bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                PlayerController.instance.moveSpeed = 3;
                PlayerController.instance.bulletMoveSpeed = 5;
                break;
            case 2:
                PlayerMaxHealth1 = 50;
                PlayerHealth1 = 50;
                health1.text = "Health: 50";
                PlayerController.instance.damage = 2;
                PlayerController.instance.powerupDamage = 3;
                Bullet.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                PlayerController.instance.moveSpeed = 3;
                PlayerController.instance.bulletMoveSpeed = 15;
                break;
            case 3:
                PlayerMaxHealth1 = 75;
                PlayerHealth1 = 75;
                health1.text = "Health: 75";
                PlayerController.instance.damage = 1;
                PlayerController.instance.powerupDamage = 2;
                Bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                PlayerController.instance.moveSpeed = 10;
                PlayerController.instance.bulletMoveSpeed = 10;
                break;
            case 4:
                PlayerMaxHealth1 = 200;
                PlayerHealth1 = 200;
                health1.text = "Health: 200";
                PlayerController.instance.damage = 1;
                PlayerController.instance.powerupDamage = 2;
                Bullet.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                PlayerController.instance.moveSpeed = 1.5f;
                PlayerController.instance.bulletMoveSpeed = 5;
                break;
        }

        switch (ButtonController.player2)
        {
            case 1:
                PlayerMaxHealth2 = 100;
                PlayerHealth2 = 100;
                health2.text = "Health: 100";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.powerupDamage = 2;
                Bullet2.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                Player2Controller.instance.moveSpeed = 3;
                Player2Controller.instance.bulletMoveSpeed = 5;
                break;
            case 2:
                PlayerMaxHealth2 = 50;
                PlayerHealth2 = 50;
                health2.text = "Health: 50";
                Player2Controller.instance.damage = 2;
                Player2Controller.instance.powerupDamage = 3;
                Bullet2.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                Player2Controller.instance.moveSpeed = 3;
                Player2Controller.instance.bulletMoveSpeed = 15;
                break;
            case 3:
                PlayerMaxHealth2 = 75;
                PlayerHealth2 = 75;
                health2.text = "Health: 75";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.powerupDamage = 2;
                Bullet2.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                Player2Controller.instance.moveSpeed = 10;
                Player2Controller.instance.bulletMoveSpeed = 10;
                break;
            case 4:
                PlayerMaxHealth2 = 200;
                PlayerHealth2 = 200;
                health2.text = "Health: 200";
                Player2Controller.instance.damage = 1;
                Player2Controller.instance.powerupDamage = 2;
                Bullet2.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                Player2Controller.instance.moveSpeed = 1.5f;
                Player2Controller.instance.bulletMoveSpeed = 5;
                break;
        }
    }
}
