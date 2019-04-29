using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalHost : MonoBehaviour
{
    [SerializeField]
    private uint roomSize;

    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void SetRoomSize(uint _size)
    {
        roomSize = _size;
    }

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Creating " + roomName + " for " + roomSize + " people...");
            networkManager.matchMaker.CreateMatch(roomName,roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
        }
    }
}
