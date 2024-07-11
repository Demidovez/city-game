using PlayerSpace;
using UnityEngine;
using WayPointsSpace;
using WheelSpace;

namespace CarSpace
{
    public class CarMovementByNavigation : MonoBehaviour
    {
        [SerializeField] private float _speedMove = 5f;
        [SerializeField] private float _speedRotation = 5f;
        [SerializeField] private float _gravity = -40f;
        [SerializeField] private string _debugPointName;
        
        private Car _car;
        private Rigidbody _rigidBody;
        private Vector3 _destination;
        private Vector3 _secondDestination;

        public bool IsReachedDestination { get; private set; }
        
        private void Awake()
        {
            _car = GetComponent<Car>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_car.Driver == Player.Instance)
            {
                return;
            }
            
            if (Vector3.Distance(_destination, transform.position) < 2f)
            {
                IsReachedDestination = true;
            }
            
            ApplyMovement();
        }
        
        private void ApplyMovement()
        {
            
            Vector3 moveDirection = (_destination - transform.position).normalized;
            Vector3 lookDirection = new Vector3(_secondDestination.x, 0f, _secondDestination.z) - new Vector3(transform.position.x, 0f, transform.position.z); 
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), _speedRotation * Time.fixedDeltaTime);

            Vector3 velocity = moveDirection * (Time.fixedDeltaTime * _speedMove);

            if (transform.position.y > 1)
            {
                velocity.y = _gravity;
            }
            else
            {
                velocity.y = -1f;
            }
            
            _rigidBody.velocity = velocity;
        }

        public void SetDestination(Vector3 position, Vector3 secondPosition, string debugPointName)
        {
            _debugPointName = debugPointName;
            _destination = position;
            _secondDestination = secondPosition;
            
            IsReachedDestination = false;
        }
    }
}

