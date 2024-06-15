using UnityEngine;

namespace PlayerSpace
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _mouseSensitivity = 100f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _gravityForce = 1f;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private Transform _cameraTransform;

        private CharacterController _characterController;
        private Vector3 _velocity;
        private float _rotationX = 0;
        private float _rotationY = 0;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }
        
        private void FixedUpdate()
        {
            Movement();
            Jump();
            ApplyGravity();
            
            Rotation();
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
                _characterController.Move(_velocity * (Time.deltaTime * _gravityForce));
            }
        }

        private void Rotation()
        {
            float sensitive = _mouseSensitivity * Time.deltaTime;
            
            float mouseX = Input.GetAxis("Mouse X") * sensitive;
            float mouseY = Input.GetAxis("Mouse Y") * sensitive;

            _rotationY += mouseX;
            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, -60f, 60f);
            
            transform.localRotation = Quaternion.Euler(0f, _rotationY, 0f);
            _cameraTransform.localRotation =  Quaternion.Euler(_rotationX, 0f, 0f);
        }
        
        private void Jump()
        {
            if (_characterController.isGrounded && (Input.GetAxis("Jump") > 0))
            {
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            }
        }

        private void Movement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            
            if (move != Vector3.zero)
            {
                _characterController.Move(move * (_speed * Time.deltaTime));
            }
        }
    }
}
