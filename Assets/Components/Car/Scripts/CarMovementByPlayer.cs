using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public class CarMovementByPlayer : MonoBehaviour
    {
        [SerializeField] private Transform _frontLeftWheel;
        [SerializeField] private Transform _frontRightWheel;
        [SerializeField] private Transform _backLeftWheel;
        [SerializeField] private Transform _backRightWheel;
        
        private Animator _frontLeftWheelAnimator;
        private Animator _frontRightWheelAnimator;
        private Animator _backLeftWheelAnimator;
        private Animator _backRightWheelAnimator;

        private bool _isMovingForward;
        private bool _isMovingBackward;

        private void Start()
        {
            _frontLeftWheelAnimator = _frontLeftWheel.GetComponent<Animator>();
            _frontRightWheelAnimator = _frontRightWheel.GetComponent<Animator>();
            _backLeftWheelAnimator = _backLeftWheel.GetComponent<Animator>();
            _backRightWheelAnimator = _backRightWheel.GetComponent<Animator>();
        }

        private void Update()
        {
            if (!Player.Instance.GetInCar)
            {
                return;
            }
            
            _isMovingForward = PlayerMovement.Instance.MoveInput.y > 0;
            _isMovingBackward = PlayerMovement.Instance.MoveInput.y < 0;
            
            _frontLeftWheelAnimator.SetBool("IsMoveForward", _isMovingForward);
            _frontRightWheelAnimator.SetBool("IsMoveForward", _isMovingForward);
            _backLeftWheelAnimator.SetBool("IsMoveForward", _isMovingForward);
            _backRightWheelAnimator.SetBool("IsMoveForward", _isMovingForward);
            
            _frontLeftWheelAnimator.SetBool("IsMoveBackward", _isMovingBackward);
            _frontRightWheelAnimator.SetBool("IsMoveBackward", _isMovingBackward);
            _backLeftWheelAnimator.SetBool("IsMoveBackward", _isMovingBackward);
            _backRightWheelAnimator.SetBool("IsMoveBackward", _isMovingBackward);
        }
    }
}
