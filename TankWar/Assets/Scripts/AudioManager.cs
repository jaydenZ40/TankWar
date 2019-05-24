using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource m_BGMPlayer, m_buttonPlayer, m_getShotPlayer, m_machineGunPlayer, m_shotGunPlayer, m_pickItemPlayer;

    void Start()
    {
        if (null == instance)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        ButtonController.instance.onButtons.AddListener(PlayButton);
    }

    public void LevelStart()
    {
        PlayerController.instance.onShotPlayer1.AddListener(PlayGetShot);
        Player2Controller.instance.onShotPlayer2.AddListener(PlayGetShot);
        PlayerController.instance.onMachineGun.AddListener(PlayMachineGun);
        Player2Controller.instance.onMachineGun.AddListener(PlayMachineGun);
        PlayerController.instance.onShotGun.AddListener(PlayShotGun);
        Player2Controller.instance.onShotGun.AddListener(PlayShotGun);
        PlayerController.instance.onHealPlayer1.AddListener(PlayPickItem);
        Player2Controller.instance.onHealPlayer2.AddListener(PlayPickItem);
        PlayerController.instance.onShieldPlayer1.AddListener(PlayPickItem);
        Player2Controller.instance.onShieldPlayer2.AddListener(PlayPickItem);
    }

    public void PlayBGM()
    {
        m_BGMPlayer.GetComponent<AudioSource>().Play();
    }

    public void PlayButton()
    {
        m_buttonPlayer.GetComponent<AudioSource>().Play();
    }

    public void PlayGetShot()
    {
        m_getShotPlayer.GetComponent<AudioSource>().Play();
    }

    public void PlayMachineGun()
    {
        m_machineGunPlayer.GetComponent<AudioSource>().Play();
    }

    public void PlayShotGun()
    {
        m_shotGunPlayer.GetComponent<AudioSource>().Play();
    }

    public void PlayPickItem()
    {
        m_pickItemPlayer.GetComponent<AudioSource>().Play();
    }
}
