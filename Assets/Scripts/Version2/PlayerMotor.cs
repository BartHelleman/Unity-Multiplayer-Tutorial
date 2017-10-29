using UnityEngine;

namespace Version2
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float cameraRotationLimit = 85f;

        private Vector3 velocity = Vector3.zero;
        private Vector3 rotation = Vector3.zero;
        private float cameraRotation = 0f;
        private float currentCameraRotation = 0f;
        private Vector3 thrusterforce = Vector3.zero;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            PerformMovement();
            PerformRotation();
        }

        public void Move(Vector3 newVelocity)
        {
            velocity = newVelocity;
        }

        public void Rotate(Vector3 newRotation)
        {
            rotation = newRotation;
        }

        public void RotateCamera(float newCameraRotation)
        {
            cameraRotation = newCameraRotation;
        }

        public void ApplyThruster(Vector3 newThrusterForce)
        {
            thrusterforce = newThrusterForce;
        }

        private void PerformMovement()
        {
            if (velocity != Vector3.zero)
            {
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            }

            if (thrusterforce != Vector3.zero)
            {
                rb.AddForce(thrusterforce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }

        private void PerformRotation()
        {
            if (rotation != Vector3.zero)
            {
                rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            }

            if (cam)
            {
                currentCameraRotation -= cameraRotation;
                currentCameraRotation = Mathf.Clamp(currentCameraRotation, -cameraRotationLimit, cameraRotationLimit);

                cam.transform.localEulerAngles = new Vector3(currentCameraRotation, 0f, 0f);
            }
        }

    }
}