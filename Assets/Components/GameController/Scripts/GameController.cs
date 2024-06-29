using System;
using UnityEngine;

namespace GameControllerSpace
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        
        [SerializeField] private Transform _bulletsContainer;
        public Transform BulletsContainer => _bulletsContainer;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

