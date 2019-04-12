using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int waveNum = 1;
    public TextMeshProUGUI waveText;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        waveText.text = "Wave: " + waveNum;
    }
}
