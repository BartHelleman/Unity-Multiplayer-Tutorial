using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

namespace Version2
{
    public class RoomListItem : MonoBehaviour
    {
        public delegate void JoinRoomDelegate(MatchInfoSnapshot match);
        private JoinRoomDelegate joinRoomDelegate;

        [SerializeField] private Text roomNameText;

        private MatchInfoSnapshot match;

        public void Setup(MatchInfoSnapshot newMatch, JoinRoomDelegate joinRoomCallback)
        {
            match = newMatch;
            joinRoomDelegate = joinRoomCallback;

            roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
        }

        public void JoinRoom()
        {
            joinRoomDelegate.Invoke(match);
        }
    }
}