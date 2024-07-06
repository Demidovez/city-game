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
        public bool GetInCar { get; private set; }
        public bool LockMovement { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            _playerShooting = GetComponent<PlayerShooting>();
        }

        private void Update()
        {
            if (_shouldCheckPosition && Vector3.Distance(transform.position, _targetPosition) > 0.1f )
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, (GetInCar ? 3f : 1f) * Time.deltaTime);
                LockMovement = true;
            }
            else
            {
                _shouldCheckPosition = false;
                LockMovement = GetInCar;
            }
        }

        public void PutInCar(bool isPutIn, Car currentCar)
        {
            GetInCar = isPutIn;

            if (isPutIn)
            {
                _targetPosition = currentCar.DriverSeat.position;
                transform.rotation = Quaternion.LookRotation(currentCar.transform.forward);
            }
            else
            {
                _targetPosition = new Vector3(transform.position.x, 0, transform.position.z - 0.8f);
            }

            currentCar.IsDriving = isPutIn;
            
            _shouldCheckPosition = true;
        }

        public void Shoot()
        {
            if (!IsShootingMode)
            {
                return;
            }
            
            _playerShooting.Shoot();
        }
    }
}

