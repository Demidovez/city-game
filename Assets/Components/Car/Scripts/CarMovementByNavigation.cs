using Interfaces;
using PlayerSpace;
using UnityEngine;
using WayPointsSpace;
using WheelSpace;

namespace CarSpace
{
    public class CarMovementByNavigation : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private float _speedMove = 5f;
        [SerializeField] private float _speedRotation = 5f;
        [SerializeField] private float _gravity = -40f;
        [SerializeField] private string _debugPointName;
        [SerializeField] private LayerMask _carLayer;
        
        private Car _car;
        private Rigidbody _rigidBody;
        private Vector3 _destination;
        private Vector3 _secondDestination;
        private int _collisionObjectsCount;
        private float _timeOfStopping;

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

            if (IsAllowMove())
            {
                ApplyMovement();
            }
        }

        private bool IsAllowMove()
        {
            if (_collisionObjectsCount > 0 || _car.ShouldStopAndWait)
            {
                _rigidBody.velocity = Vector3.zero;
                return false;
            }

            Debug.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) + transform.forward * 6, Color.red);
            Debug.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) + Quaternion.Euler(0, 20, 0) * transform.forward * 6, Color.red);

            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hitFront,6, _carLayer))
            {
                if (hitFront.collider.gameObject.TryGetComponent(out Car car))
                {
                    if (car.IsMoving && car.gameObject != gameObject)
                    {
                        _rigidBody.velocity = Vector3.zero;
                        return false;
                    }
                }
            }
            
            if (Physics.Raycast(transform.position + Vector3.up, Quaternion.Euler(0, 20, 0) * transform.forward, out RaycastHit hitRight,6, _carLayer))
            {
                if (hitRight.collider.gameObject.TryGetComponent(out Car car))
                {
                    if (car.IsMoving && car.gameObject != gameObject)
                    {
                        _rigidBody.velocity = Vector3.zero;
                        return false;
                    }
                }
            }

            return true;
        }

        private void ApplyMovement()
        {
            Vector3 moveDirection = (_destination - transform.position).normalized;
            Vector3 lookDirection = new Vector3(_secondDestination.x, 0f, _secondDestination.z) - new Vector3(transform.position.x, 0f, transform.position.z); 
            
            Debug.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) + lookDirection.normalized * 6, Color.red);
            if (Physics.Raycast(transform.position + Vector3.up, lookDirection.normalized, out RaycastHit hitLook,6, _carLayer))
            {
                if (hitLook.collider.gameObject.TryGetComponent(out Car car))
                {
                    if (car.IsMoving && car.gameObject != gameObject)
                    {
                        _rigidBody.velocity = Vector3.zero;
                        return;
                    }
                }
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), _speedRotation * Time.fixedDeltaTime);

            Vector3 velocity = moveDirection * (Time.fixedDeltaTime * _speedMove);

            if (transform.position.y > 1)
            {
                velocity.y = _gravity;
            }
            else
            {
                velocity.y = 0f;
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

        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (other.gameObject.TryGetComponent(out Car _) || other.gameObject == Player.Instance.gameObject)
            {
                _collisionObjectsCount++;
            }
        }
        
        public void OnTriggerAreaExit(GameObject target, Collider other)
        {
            if (other.gameObject.TryGetComponent(out Car _) || other.gameObject == Player.Instance.gameObject)
            {
                _collisionObjectsCount--;
            }
        }
    }
}

