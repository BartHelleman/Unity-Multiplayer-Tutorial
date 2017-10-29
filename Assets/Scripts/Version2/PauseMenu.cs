using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Version2
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool IsOn = false;

        private NetworkManager networkManager;

        private void Start()
        {
            networkManager = NetworkManager.singleton;
        }

        public void Disconnect()
        {
            MatchInfo matchInfo = networkManager.matchInfo;
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
    }
}