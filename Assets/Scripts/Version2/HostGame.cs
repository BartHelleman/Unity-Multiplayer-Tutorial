﻿using UnityEngine;
using UnityEngine.Networking;

namespace Version2
{
    public class HostGame : MonoBehaviour
    {
        [SerializeField] private uint roomSize = 4;
        private string roomName;

        private NetworkManager networkManager;

        private void Start()
        {
            networkManager = NetworkManager.singleton;

            if (networkManager.matchMaker == null)
            {
                networkManager.StartMatchMaker();
            }
        }

        public void SetRoomName(string name)
        {
            roomName = name;
        }

        public void CreateRoom()
        {
            if (roomName != "" && roomName != null)
            {
                Debug.Log("Creating room: " + roomName + " with room for " + roomSize + " players.");
                networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
            }
        }
    }
}