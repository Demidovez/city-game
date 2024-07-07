using CarSpace;
using InputActionsSpace;
using PlayerSpace;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Image _aimImage;
        public static GameController Instance;

        private Car _currentCar;

        private bool IsOpenedMenu { get; set; }
        private PossibleActionEnum PossibleAction { get; set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            InputActionsManager.OnPressedEscapeEvent += OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent += OnPressedUseSmth;
            InputActionsManager.OnPressedShootingModeEvent += OnPressedShootingMode;
            
            CarUsingChecker.OnCarWaitActionEvent += OnCarWaitAction;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnPressedShootingMode()
        {
            Player.Instance.IsShootingMode = !Player.Instance.IsShootingMode;
            _aimImage.gameObject.SetActive(Player.Instance.IsShootingMode);
        }
        
        private void OnPressedUseSmth()
        {
            switch (PossibleAction)
            {
                case PossibleActionEnum.GetInCar:
                    Player.Instance.UseCar(true, _currentCar);
                    PossibleAction = PossibleActionEnum.GetOutCar;
                    break;
                case PossibleActionEnum.GetOutCar:
                    Player.Instance.UseCar(false, _currentCar);
                    PossibleAction = PossibleActionEnum.GetInCar;
                    break;
                case PossibleActionEnum.None:
                default:
                    return;
            }
        }

        private void OnCarWaitAction(bool isWait, Car car)
        {
            PossibleAction = isWait ? PossibleActionEnum.GetInCar : PossibleActionEnum.None;

            _currentCar = car;
        }
        
        private void OnToggleMenu()
        {
            IsOpenedMenu = !IsOpenedMenu;
        }
        
        private void OnDestroy()
        {
            InputActionsManager.OnPressedEscapeEvent -= OnToggleMenu;
            InputActionsManager.OnPressedUseSmthEvent -= OnPressedUseSmth;
            InputActionsManager.OnPressedShootingModeEvent -= OnPressedShootingMode;
            
            CarUsingChecker.OnCarWaitActionEvent -= OnCarWaitAction;
        }
    }
}

