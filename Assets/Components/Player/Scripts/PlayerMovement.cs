using System;
using UnityEngine;

namespace PlayerSpace
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;
        
        [SerializeField] private float _speed;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _gravityForce = 1f;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _animSmoothTime = 1f;
        [SerializeField] private float _rotationSpeed = 2f;
        
        public Vector2 MoveInput { get; set; }
        public bool Slowly { get; set; }
        
        public Vector2 CurrentBlendAnim { get; private set; }

        private CharacterController _characterController;
        private Vector3 _velocity;
        private float _rotationX;
        private float _rotationY;
        private Vector3 _movement;
        private Vector2 _animVelocity;

        private void Awake()
        {
            Instance = this;

            _characterController = GetComponent<CharacterController>();
        }
        
        private void FixedUpdate()
        {
            Movement();
            ApplyGravity();
            UpdateAnimBlend();
        }

        private void LateUpdate()
        {
            Rotation();
        }

        private void Movement()
        {
            _movement = _cameraTransform.right * MoveInput.x + _cameraTransform.forward * MoveInput.y;
            
            if(_movement.magnitude > 1) {
                _movement.Normalize();
            }
            
            _movement.y = 0;

            float resultSpeed = _speed * (MoveInput.y < 0 ? 0.5f : 1f) * (Slowly ? 0.3f : 1f);

            _characterController.Move(_movement * (resultSpeed * Time.fixedDeltaTime));
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }
            else
            {
                _velocity.y += _gravity * Time.fixedDeltaTime;
                _characterController.Move(_velocity * (Time.fixedDeltaTime * _gravityForce));
            }
        }

        private void UpdateAnimBlend()
        {
            Vector2 target;
            float speed;
            
            if (!_characterController.isGrounded)
            {
                target = new Vector2(0, -10);
                speed = (_animSmoothTime / 0.1f) * Time.fixedDeltaTime;
            }
            else
            {
                target = MoveInput;
                speed = _animSmoothTime * Time.fixedDeltaTime;
            }
            
            CurrentBlendAnim = Vector2.SmoothDamp(CurrentBlendAnim, target, ref _animVelocity, speed);
        }
        
        private void Rotation()
        {
            Quaternion rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
        } 
        
        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -3 * _gravity);
            }
        }
    }
}
