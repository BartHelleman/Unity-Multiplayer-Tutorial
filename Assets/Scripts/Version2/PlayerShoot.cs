using UnityEngine;
using UnityEngine.Networking;

namespace Version2
{
    [RequireComponent(typeof(WeaponManager))]
    public class PlayerShoot : NetworkBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask mask;

        private const string PLAYER_TAG = "Player";

        private WeaponManager weaponManager;
        private PlayerWeapon currentWeapon;

        private void Start()
        {
            if (!cam)
            {
                Debug.LogError("PlayerShoot: No camera reference");
                this.enabled = false;
            }

            weaponManager = GetComponent<WeaponManager>();
        }

        private void Update()
        {
            currentWeapon = weaponManager.GetCurrentWeapon();

            if (currentWeapon.fireRate <= 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                }
            }

        }

        [Command]
        private void CmdOnShoot()
        {
            RpcDoShootEffect();
        }

        [Command]
        private void CmdOnHit(Vector3 pos, Vector3 normal)
        {
            RpcDoHitEffect(pos, normal);
        }

        [ClientRpc]
        private void RpcDoShootEffect()
        {
            weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();
        }

        [ClientRpc]
        private void RpcDoHitEffect(Vector3 pos, Vector3 normal)
        {
            GameObject hitEffect = Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
            Destroy(hitEffect, 2f);
        }

        [Client]
        private void Shoot()
        {
            if (!isLocalPlayer)
                return;

            CmdOnShoot();

            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
            {
                if (hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(hit.collider.name, currentWeapon.damage);
                }

                CmdOnHit(hit.point, hit.normal);
            }
        }

        [Command]
        private void CmdPlayerShot(string playerId, int damage)
        {
            Debug.Log(playerId + " has been shot.");

            Player player = GameManager.GetPlayer(playerId);
            player.RpcTakeDamage(damage);
        }
    }
}