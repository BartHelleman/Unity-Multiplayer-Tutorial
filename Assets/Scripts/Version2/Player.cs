using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Version2
{
    [RequireComponent(typeof(PlayerSetup))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private Behaviour[] disableOnDeath;
        [SerializeField] private GameObject[] disableGameObjectsOnDeath;
        [SerializeField] private GameObject deathEffect;
        [SerializeField] private GameObject spawnEffect;

        private bool[] wasEnabled;
        private bool firstSetup = true;

        [SyncVar] private int currentHealth;

        [SyncVar] private bool _isDead = false;

        public bool IsDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

        public void SetupPlayer()
        {
            if (isLocalPlayer)
            {
                GameManager.instance.SetSceneCameraActive(false);
                GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
            }
            
            CmdBroadcastNewPlayerSetup();
        }

        [Command]
        private void CmdBroadcastNewPlayerSetup()
        {
            RpcSetupPlayerOnAllClients();
        }

        [ClientRpc]
        private void RpcSetupPlayerOnAllClients()
        {
            if (firstSetup)
            {
                wasEnabled = new bool[disableOnDeath.Length];

                for (int i = 0; i < wasEnabled.Length; i++)
                {
                    wasEnabled[i] = disableOnDeath[i].enabled;
                }

                firstSetup = false;
            }
            
            SetDefaults();
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Input.GetKeyDown(KeyCode.K))
                RpcTakeDamage(20000);
        }

        [ClientRpc]
        public void RpcTakeDamage(int amount)
        {
            if (IsDead)
                return;

            currentHealth -= amount;

            Debug.Log(transform.name + " has " + currentHealth + " health.");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            IsDead = true;

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = false;
            }

            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(false);
            }

            Collider col = GetComponent<Collider>();

            if (col)
                col.enabled = false;

            GameObject explosionInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 3f);

            if (isLocalPlayer)
            {
                GameManager.instance.SetSceneCameraActive(true);
                GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
            }

            Debug.Log(transform.name + " died");

            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
            
            Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;

            yield return new WaitForSeconds(0.1f);
            SetupPlayer();
        }

        public void SetDefaults()
        {
            IsDead = false;

            currentHealth = maxHealth;

            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                disableOnDeath[i].enabled = wasEnabled[i];
            }

            for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
            {
                disableGameObjectsOnDeath[i].SetActive(true);
            }

            Collider col = GetComponent<Collider>();

            if (col)
                col.enabled = true;
            
            GameObject respawnInstance = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(respawnInstance, 3f);
        }
    }
}