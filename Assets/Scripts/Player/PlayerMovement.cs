using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 1;
        public float turnSmoothTime = 0.1f;
        public FloatingJoystick joystick;
        
        private CharacterController characterController;
        private InputAction moveAction;
        private Vector3 direction;
        private float turnSmoothVelocity;
        private Animator animator;
        
        void Start()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }
        
        void Update()
        {
            var input = moveAction.ReadValue<Vector2>();
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;
            
            direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Moving", true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    ref turnSmoothVelocity,
                    turnSmoothTime
                );
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                characterController.Move(direction.normalized * speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }
}
