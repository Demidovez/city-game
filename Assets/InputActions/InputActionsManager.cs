using System;
using PlayerSpace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace InputActionsSpace
{
    public class InputActionsManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputAction _actionRun;
        private InputAction _actionJump;
        private InputAction _actionWalk;
        private InputAction _actionShoot;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            _actionRun = _playerInput.actions["Run"];
            _actionJump = _playerInput.actions["Jump"];
            _actionWalk = _playerInput.actions["Walk"];
            _actionShoot = _playerInput.actions["Shoot"];
        }
        
        private void OnEnable()
        {
            _actionRun.performed += Run;
            _actionRun.canceled += Run;
            
            _actionShoot.performed += Shooting;
            _actionJump.performed += Jump;
        }
        
        private void Update()
        {
            PlayerMovement.Instance.IsWalking = _actionWalk.IsPressed();
        }
        
        private void Run(InputAction.CallbackContext obj)
        {
            PlayerMovement.Instance.MoveInput = obj.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            PlayerMovement.Instance.Jump();
        }

        private int count;

        private void Shooting(InputAction.CallbackContext obj)
        {
            Player.Instance.Shoot();
        }

        private void OnDisable()
        {
            _actionRun.performed -= Run;
            _actionRun.canceled -= Run;
            
            _actionShoot.performed -= Shooting;
            _actionJump.performed -= Jump;
        }
    }
}
