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
    public GameObject Enemy;

    private int waveNum = 1;
    [SerializeField] private int NumOfEnemiesLeft = 0;
    [SerializeField] private int NumOfEnemiesWillSpawn = 10;
    private int PlayerHealth1 = 100;
    private int PlayerHealth2 = 100;
    private float timer = 0;

    void Start()
    {
        instance = this;
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(10, 11); // ignore collision (player--bullet), (enenmy--enemyBullet)
        Physics2D.IgnoreLayerCollision(10, 12); // ignore collision (enemy--boundary), enenmy will spawn outside the screen, so that allow enemy enter screen without collisions

        PlayerController.instance.onCollisionWithEnemy1.AddListener(Player1CollidesEnemy);
        PlayerController.instance.onShotPlayer1.AddListener(Player1IsShot);
        //Player2Controller.instance.onCollisionWithEnemy2.AddListener(Player2CollidesEnemy);
        //Player2Controller.instance.onShotPlayer2.AddListener(Player2IsShot);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > Random.Range(5, 15) && NumOfEnemiesWillSpawn > 0)
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
            SceneManager.LoadScene("WinScene");
        }
    }

    void Player1CollidesEnemy()
    {
        PlayerHealth1 -= 50;
        health1.text = "Health: " + PlayerHealth1;
        NumOfEnemiesLeft--;
        if (PlayerHealth1 <= 0)
        {
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
        }
    }

    void Player1IsShot()
    {
        PlayerHealth1 -= EnemiesBullets.damage;
        health1.text = "Health: " + PlayerHealth1;
        if (PlayerHealth1 <= 0)
        {
            PlayerController.instance.isKnockedDown = true;
            PlayerController.instance.moveSpeed = 1;
        }
    }

    //void Player2CollidesEnemy()
    //{
    //    PlayerHealth2 -= 50;
    //    if (PlayerHealth2 <= 0)
    //    {
    //        Player2Controller.instance.isKnockedDown = true;
    //        Player2Controller.instance.moveSpeed = 1;
    //    }
    //}

    //void Player2IsShot()
    //{
    //    PlayerHealth2 -= EnemiesBullets.damage;
    //    if (PlayerHealth2 <= 0)
    //    {
    //        Player2Controller.instance.isKnockedDown = true;
    //        Player2Controller.instance.moveSpeed = 1;
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
}
