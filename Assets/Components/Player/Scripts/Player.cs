using CarSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        private PlayerShooting _playerShooting;
        private Vector3 _targetPosition;
        private bool _shouldCheckPosition;
        
        public bool IsShootingMode { get; set; }
        public bool IsSittingInCar { get; private set; }
        public bool LockMovement { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            _playerShooting = GetComponent<PlayerShooting>();
        }

        private void Update()
        {
            LockMovement = IsSittingInCar;
            
            CorrectPosition();
        }

        private void CorrectPosition()
        {
            if (_shouldCheckPosition && Vector3.Distance(transform.position, _targetPosition) > 0.1f )
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, (IsSittingInCar ? 3f : 1f) * Time.deltaTime);
                LockMovement = true;
            }
            else
            {
                _shouldCheckPosition = false;
            }
        }

        public void UseCar(bool isUsing, Car currentCar)
        {
            IsSittingInCar = isUsing;

            if (isUsing)
            {
                _targetPosition = currentCar.DriverSeat.position;
                transform.rotation = Quaternion.LookRotation(currentCar.transform.forward);
            }
            else
            {
                _targetPosition = transform.position + transform.TransformDirection(Vector3.left);
                _targetPosition.y = 0;
            }

            currentCar.SetDriver(isUsing ? this : null);
            
            _shouldCheckPosition = true;
        }

        public void TryShoot()
        {
            if (!IsShootingMode)
            {
                return;
            }
            
            _playerShooting.Shoot();
        }
    }
}

