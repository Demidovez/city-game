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
        private InputAction _actionMove;
        private InputAction _actionJump;
        private InputAction _actionSlowly;
        private InputAction _actionShoot;
        private InputAction _actionMouseLook;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
            _actionMove = _playerInput.actions["Move"];
            _actionJump = _playerInput.actions["Jump"];
            _actionSlowly = _playerInput.actions["Slowly"];
            _actionShoot = _playerInput.actions["Shoot"];
            _actionMouseLook = _playerInput.actions["MouseLook"];
        }
        
        private void OnEnable()
        {
            _actionShoot.performed += Shooting;
            _actionJump.performed += Jump;
        }

        private void Update()
        {
            PlayerMovement.Instance.MoveInput = _actionMove.ReadValue<Vector2>();
            PlayerMovement.Instance.MouseLook = _actionMouseLook.ReadValue<Vector2>();
            PlayerMovement.Instance.Slowly = _actionSlowly.IsPressed();
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            PlayerMovement.Instance.Jump();
        }

        private void Shooting(InputAction.CallbackContext obj)
        {
            Player.Instance.Shoot();
        }

        private void OnDisable()
        {
            _actionShoot.performed -= Shooting;
            _actionJump.performed -= Jump;
        }
    }

}