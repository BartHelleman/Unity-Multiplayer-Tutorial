using System.Collections.Generic;
using UnityEngine;

namespace Version2
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject sceneCamera;

        public static GameManager instance;

        private void Awake()
        {
            if (instance)
            {
                Debug.LogError("More than one GameManager in scene");
            }
            else
            {
                instance = this;
            }
        }

        public void SetSceneCameraActive(bool isActive)
        {
            if (sceneCamera == null)
                return;

            sceneCamera.SetActive(isActive);
        }

        #region ...

        public MatchSettings matchSettings;
        #endregion

        #region Player tracking

        private const string PLAYER_ID_PREFIX = "Player ";
        private static Dictionary<string, Player> players = new Dictionary<string, Player>();

        public static void RegisterPlayer(string netId, Player player)
        {
            string playerId = PLAYER_ID_PREFIX + netId;
            players.Add(playerId, player);
            player.transform.name = playerId;
        }

        public static void UnregisterPlayer(string playerId)
        {
            players.Remove(playerId);
        }

        public static Player GetPlayer(string playerId)
        {
            return players[playerId];
        }

        /*private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(200, 200, 200, 500));
            GUILayout.BeginVertical();

            foreach (string playerId in players.Keys)
            {
                GUILayout.Label(playerId + " - " + players[playerId].transform.name);
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }*/

        #endregion
    }
}