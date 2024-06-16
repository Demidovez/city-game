using System;
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

        public static PlayerMovement Instance;
        
        public bool IsRunning { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsFalling { get; private set; }

        private CharacterController _characterController;
        private Vector3 _velocity;
        private float _rotationX;
        private float _rotationY;

        private void Awake()
        {
            Instance = this;
        }

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
            
            IsFalling = !_characterController.isGrounded && _velocity.y < -2f;
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
            if (_characterController.isGrounded && (Input.GetAxisRaw("Jump") > 0))
            {
                IsJumping = true;
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravity);
            } else if (IsJumping && _velocity.y < 0)
            {
                IsJumping = false;
            }
        }

        private void Movement()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");
            
            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            if (move != Vector3.zero)
            {
                IsRunning = true;
                _characterController.Move(move.normalized * (_speed * Time.deltaTime));
            }
            else
            {
                IsRunning = false;
            }
        }
    }
}
