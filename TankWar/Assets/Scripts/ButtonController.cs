using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public static ButtonController instance;
    public static int player1 = 0;
    public static int player2 = 0;
    public static int weaponType1 = 0;
    public static int weaponType2 = 0;
    public UnityEvent onButtons = new UnityEvent();

    void Awake()
    {
        instance = this;
    }

    public void OnCharacter1()
    {
        if (player1 == 0)
        {
            player1 = 1;
        }
        else
        {
            player2 = 1;
            SceneManager.LoadScene("ChooseWeapons");
        }
        onButtons.Invoke();
    }

    public void OnCharacter2()
    {
        if (player1 == 0)
        {
            player1 = 2;
        }
        else
        {
            player2 = 2;
            SceneManager.LoadScene("ChooseWeapons");
        }
        onButtons.Invoke();
    }

    public void OnCharacter3()
    {
        if (player1 == 0)
        {
            player1 = 3;
        }
        else
        {
            player2 = 3;
            SceneManager.LoadScene("ChooseWeapons");
        }
        onButtons.Invoke();
    }

    public void OnCharacter4()
    {
        if (player1 == 0)
        {
            player1 = 4;
        }
        else
        {
            player2 = 4;
            SceneManager.LoadScene("ChooseWeapons");
        }
        onButtons.Invoke();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("ChooseCharacter");
        onButtons.Invoke();
    }

    public void OnPause()
    {
        Pause.instance.TogglePause();
        onButtons.Invoke();
    }

    public void OnRestart()
    {
        player1 = 0; player2 = 0;
        SceneManager.LoadScene("ChooseCharacter");
        Time.timeScale = 1;
        onButtons.Invoke();
    }

    public void OnExit()
    {
        Application.Quit();
        onButtons.Invoke();
    }
    public void OnMachineGun()
    {
        if (weaponType1 == 0)
            weaponType1 = 1;
        else
        {
            weaponType2 = 1;
            SceneManager.LoadScene("Level1");
        }
        onButtons.Invoke();
    }
    public void OnShotGun()
    {
        if (weaponType1 == 0)
            weaponType1 = 2;
        else
        {
            weaponType2 = 2;
            SceneManager.LoadScene("Level1");
        }
        onButtons.Invoke();
    }
}
