using System;
using InputActionsSpace;
using UnityEngine;

namespace GameControllerSpace
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public bool IsOpenedMenu { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            InputActionsManager.OnPressedEscapeEvent += ToggleMenu;
        }
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void ToggleMenu()
        {
            IsOpenedMenu = !IsOpenedMenu;
        }
        
        private void OnDestroy()
        {
            InputActionsManager.OnPressedEscapeEvent -= ToggleMenu;
        }
    }
}

