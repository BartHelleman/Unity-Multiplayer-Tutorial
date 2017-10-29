using UnityEngine;

namespace Version2
{
    [System.Serializable]
    public class PlayerWeapon
    {
        public string name = "Blaster";
        public int damage = 5;
        public float range = 100f;
        public float fireRate = 0f;

        public GameObject graphics;
    }
}