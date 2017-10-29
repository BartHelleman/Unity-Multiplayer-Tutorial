using UnityEngine;

namespace Version2
{
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(ConfigurableJoint))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float mouseSensitivity = 3f;
        [SerializeField] private float thrusterForce = 1000f;
        [SerializeField] private float thrusterFuelBurnSpeed = .8f;
        [SerializeField] private float thrusterFuelRegenSpeed = .3f;
        [SerializeField] private float thrusterFuelAmount = 1f;
        [SerializeField] private LayerMask environmentMask;
        
        [Header("Joint settings: ")]
        [SerializeField] private float jointSpring = 20f;
        [SerializeField] private float jointMaxForce = 40f;

        private PlayerMotor motor;
        private ConfigurableJoint joint;
        private Animator animator;

        public float GetThrusterFuelAmount()
        {
            return thrusterFuelAmount;
        }

        private void Start()
        {
            motor = GetComponent<PlayerMotor>();
            joint = GetComponent<ConfigurableJoint>();
            animator = GetComponent<Animator>();

            SetJointSettings(jointSpring);
        }

        private void Update()
        {
            if (PauseMenu.IsOn)
                return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, environmentMask))
                joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
            else
                joint.targetPosition = new Vector3(0f, 0f, 0f);

            float xMovement = Input.GetAxis("Horizontal");
            float zMovement = Input.GetAxis("Vertical");

            Vector3 moveHorizontal = transform.right * xMovement;
            Vector3 moveVertical = transform.forward * zMovement;

            Vector3 velocity = (moveHorizontal + moveVertical) * speed;

            animator.SetFloat("ForwardVelocity", zMovement);

            motor.Move(velocity);

            float yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 rotation = new Vector3(0f, yRotation, 0f) * mouseSensitivity;
            motor.Rotate(rotation);

            float xRotation = Input.GetAxisRaw("Mouse Y");
            float cameraRotation = xRotation * mouseSensitivity;
            motor.RotateCamera(cameraRotation);

            Vector3 newThrusterForce = Vector3.zero;

            if (Input.GetButton("Jump") && thrusterFuelAmount > 0)
            {
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

                if (thrusterFuelAmount >= 0.01f)
                {
                    newThrusterForce = Vector3.up * thrusterForce;
                    SetJointSettings(0f);
                }
            }
            else
            {
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

                SetJointSettings(jointSpring);
            }

            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

            motor.ApplyThruster(newThrusterForce);
        }

        private void SetJointSettings(float newJointSpring)
        {
            joint.yDrive = new JointDrive {
                positionSpring = newJointSpring, maximumForce = jointMaxForce
            };
        }
    }
}