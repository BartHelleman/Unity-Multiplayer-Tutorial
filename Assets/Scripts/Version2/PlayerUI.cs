using UnityEngine;

namespace Version2
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] RectTransform thrusterFuelAmount;

        private PlayerController controller;

        private void Update()
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());
        }

        public void SetController(PlayerController newController)
        {
            controller = newController;
        }

        private void SetFuelAmount(float amount)
        {
            thrusterFuelAmount.localScale = new Vector3(1f, amount, 1f);
        }
    }
}