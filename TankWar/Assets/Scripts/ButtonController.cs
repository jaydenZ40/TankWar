using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public static int player1 = 0;
    public static int player2 = 0;

    public void OnCharacter1()
    {
        if (player1 == 0)
        {
            player1 = 1;
        }
        else
        {
            player2 = 1;
            SceneManager.LoadScene("Level1");
        }
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
            SceneManager.LoadScene("Level1");
        }
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
            SceneManager.LoadScene("Level1");
        }
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
            SceneManager.LoadScene("Level1");
        }
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnPause()
    {
        
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("ChooseCharacter");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
