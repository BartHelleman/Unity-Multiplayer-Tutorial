using UnityEngine;
using UnityEngine.Networking;

namespace Version2
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField] private Behaviour[] componentsToDisable;
        [SerializeField] private string remoteLayerName = "RemotePlayer";
        [SerializeField] private string dontDrawLayerName = "DontDraw";
        [SerializeField] private GameObject playerGraphics;
        [SerializeField] private GameObject playerUIPrefab;
        
        [HideInInspector] public GameObject playerUIInstance;

        private void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
                AssignRemoteLayer();
            }
            else
            {
                Util.SetLayerRecursive(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

                playerUIInstance = Instantiate(playerUIPrefab);
                playerUIInstance.name = playerUIPrefab.name;

                PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();

                if (ui == null)
                    Debug.LogError("No PlayerUI component on PlayerUI prefab.");
                else
                    ui.SetController(GetComponent<PlayerController>());

                GetComponent<Player>().SetupPlayer();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            string netId = GetComponent<NetworkIdentity>().netId.ToString();
            Player player = GetComponent<Player>();

            GameManager.RegisterPlayer(netId, player);
        }

        private void DisableComponents()
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }

        private void AssignRemoteLayer()
        {
            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
        }

        private void OnDisable()
        {
            Destroy(playerUIInstance);

            if (isLocalPlayer)
                GameManager.instance.SetSceneCameraActive(true);

            GameManager.UnregisterPlayer(transform.name);
        }

    }
}