using System;
using UnityEngine;

namespace PlayerSpace
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        public bool IsRunning { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsFalling { get; private set; }
        
        private CharacterController _characterController;

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
            CheckRunning();
            CheckJumping();
            CheckFalling();
        }

        private void CheckRunning()
        {
            IsRunning = PlayerMovement.Instance.IsRunning;
        }
        
        private void CheckJumping()
        {
            IsJumping = PlayerMovement.Instance.IsJumping;
            IsRunning = IsJumping ? false : IsRunning;
        }
        
        private void CheckFalling()
        {
            IsFalling = PlayerMovement.Instance.IsFalling;
            IsRunning = IsFalling ? false : IsRunning;
        }
    }
}

