using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pausePanel;
    public static Pause instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            pausePanel.SetActive(true);
        }
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
