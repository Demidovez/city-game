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
        
        public float HorizontalMove { get; private set; }
        public float VerticalMove { get; private set; }

        private CharacterController _characterController;
        private Vector3 _velocity;
        private float _rotationX;
        private float _rotationY;
        private Vector3 _movement;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            bool isStoppingMoveX = Input.GetAxisRaw("Horizontal") == 0;
            bool isStoppingMoveZ = Input.GetAxisRaw("Vertical") == 0;

            float correctMoveX = GetCorrectMoveAxis(isStoppingMoveX, moveX);
            float correctMoveZ = GetCorrectMoveAxis(isStoppingMoveZ, moveZ);
            
            _movement = transform.right * correctMoveX + transform.forward * correctMoveZ;
            
            if(_movement.magnitude > 1) {
                _movement.Normalize();
            }

            if (!_characterController.isGrounded)
            {
                HorizontalMove = Mathf.Lerp(HorizontalMove, 0, 3f * Time.deltaTime);
                VerticalMove = Mathf.Lerp(VerticalMove, -10, 3f * Time.deltaTime);
            } else {
                float forceX = isStoppingMoveX ? 1f : 2f;
                float forceZ = isStoppingMoveZ ? 1f : 2f;
                
                HorizontalMove = Mathf.Clamp(correctMoveX * forceX, -1, 1);
                VerticalMove = Mathf.Clamp(correctMoveZ * forceZ, -1, 1);
            }
        }
        
        private void FixedUpdate()
        {
            Movement();
            Jump();
            ApplyGravity();
            
            Rotation();
        }

        private static float GetCorrectMoveAxis(bool isStopping, float move)
        {
            if (!isStopping)
            {
                return move;
            }
            
            if (move >= 0)
            {
                return Mathf.Clamp(move - 0.5f, 0, 1);
            }
            
            return Mathf.Clamp(move + 0.5f, -1, 0);
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
            if (_characterController.isGrounded && Input.GetAxisRaw("Jump") > 0)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -3 * _gravity);
            }
        }

        private void Movement()
        {
            if (_movement != Vector3.zero)
            {
                _characterController.Move(_movement * (_speed * Time.fixedDeltaTime * (VerticalMove < 0 ? 0.5f : 1f)));
            }
        }
    }
}
