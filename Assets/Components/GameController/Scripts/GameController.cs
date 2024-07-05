using System;
using CarSpace;
using InputActionsSpace;
using PlayerSpace;
using UnityEngine;

namespace GameControllerSpace
{
    public enum PossibleActionEnum
    {
        None,
        GetInCar,
        GetOutCar,
    }
    
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public bool IsOpenedMenu { get; private set; }
        public PossibleActionEnum PossibleAction { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            InputActionsManager.OnPressedEscapeEvent += OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent += OnPressedUseSmth;
            Car.OnCarWaitActionEvent += OnCarWaitAction;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnPressedUseSmth()
        {
            switch (PossibleAction)
            {
                case PossibleActionEnum.GetInCar:
                    Player.Instance.GetInCar = true;
                    PossibleAction = PossibleActionEnum.GetOutCar;
                    break;
                case PossibleActionEnum.GetOutCar:
                    Player.Instance.GetInCar = false;
                    PossibleAction = PossibleActionEnum.GetInCar;
                    break;
                default:
                    return;
            }
        }

        private void OnCarWaitAction(bool isWait)
        {
            if (isWait)
            {
                PossibleAction = PossibleActionEnum.GetInCar;
            }
            else
            {
                PossibleAction = PossibleActionEnum.None;
            }
        }
        
        private void OnToggleMenu()
        {
            IsOpenedMenu = !IsOpenedMenu;
        }
        
        private void OnDestroy()
        {
            InputActionsManager.OnPressedEscapeEvent -= OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent -= OnPressedUseSmth;
            Car.OnCarWaitActionEvent -= OnCarWaitAction;
        }
    }
}

