using UnityEngine;
using UnityEngine.Networking;

namespace Version2
{
    public class WeaponManager : NetworkBehaviour
    {
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private PlayerWeapon primaryWeapon;

        private PlayerWeapon currentWeapon;
        private WeaponGraphics currentWeaponGraphics;

        private const string WEAPON_LAYER_NAME = "Weapon";

        private void Start()
        {
            EquipWeapon(primaryWeapon);
        }

        public PlayerWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public WeaponGraphics GetCurrentWeaponGraphics()
        {
            return currentWeaponGraphics;
        }

        private void EquipWeapon(PlayerWeapon weapon)
        {
            currentWeapon = weapon;

            GameObject weaponInstance = Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation);
            weaponInstance.transform.SetParent(weaponHolder);

            currentWeaponGraphics = weaponInstance.GetComponent<WeaponGraphics>();

            if (currentWeaponGraphics == null)
                Debug.LogError("No WeaponGraphics component on the weapon: " + weaponInstance.name);

            if (isLocalPlayer)
                Util.SetLayerRecursive(weaponInstance, LayerMask.NameToLayer(WEAPON_LAYER_NAME));
        }
    }
}