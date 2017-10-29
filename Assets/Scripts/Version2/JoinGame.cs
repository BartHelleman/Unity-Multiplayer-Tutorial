using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Version2
{
    public class JoinGame : MonoBehaviour
    {
        [SerializeField] private GameObject roomListItemPrefab;
        [SerializeField] private Transform roomListParent;

        private NetworkManager networkManager;
        private List<GameObject> roomList = new List<GameObject>();

        private void Start()
        {
            networkManager = NetworkManager.singleton;

            if (networkManager.matchMaker == null)
                networkManager.StartMatchMaker();

            RefreshRoomList();
        }

        public void RefreshRoomList()
        {
            ClearRoomList();
            networkManager.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            if (matchList == null)
                return;

            ClearRoomList();

            foreach (MatchInfoSnapshot match in matchList)
            {
                GameObject roomListItemGameObject = Instantiate(roomListItemPrefab);
                roomListItemGameObject.transform.SetParent(roomListParent);

                RoomListItem roomListItem = roomListItemGameObject.GetComponent<RoomListItem>();
                if (roomListItem != null)
                {
                    roomListItem.Setup(match, JoinRoom);
                }

                roomList.Add(roomListItemGameObject);
            }
        }

        private void ClearRoomList()
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Destroy(roomList[i]);
            }

            roomList.Clear();
        }

        public void JoinRoom(MatchInfoSnapshot match)
        {
            networkManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
            ClearRoomList();
        }
    }
}
