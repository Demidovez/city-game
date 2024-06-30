using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSpace
{
    public class AimCamera : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private int _priorityBoost = 10;

        private CinemachineVirtualCamera _virtualCamera;
        private InputAction _aimAction;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();

            _aimAction = _playerInput.actions["Aim"];
        }

        private void OnEnable()
        {
            _aimAction.performed += StartAim;
            _aimAction.canceled += CancelAim;
        }

        private void CancelAim(InputAction.CallbackContext obj)
        {
            _virtualCamera.Priority -= _priorityBoost;
        }

        private void StartAim(InputAction.CallbackContext obj)
        {
            _virtualCamera.Priority += _priorityBoost;
        }

        private void OnDisable()
        {
            _aimAction.performed -= StartAim;
            _aimAction.canceled -= CancelAim;
        }
    }
}

