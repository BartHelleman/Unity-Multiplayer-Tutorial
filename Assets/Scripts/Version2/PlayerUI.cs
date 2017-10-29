using UnityEngine;

namespace Version2
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform thrusterFuelAmount;
        [SerializeField] private GameObject pauseMenu;

        private PlayerController controller;

        private void Start()
        {
            PauseMenu.IsOn = false;
        }

        private void Update()
        {
            SetFuelAmount(controller.GetThrusterFuelAmount());

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void SetController(PlayerController newController)
        {
            controller = newController;
        }

        private void SetFuelAmount(float amount)
        {
            thrusterFuelAmount.localScale = new Vector3(1f, amount, 1f);
        }

        private void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            PauseMenu.IsOn = pauseMenu.activeSelf;
        }
    }
}