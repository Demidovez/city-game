using UnityEngine;
using WayPointsSpace;
using WheelSpace;

namespace CarSpace
{
    public class CarMovementByNavigation : MonoBehaviour
    {
        // [SerializeField] private float _minMoveSpeed = 0.75f;
        // [SerializeField] private float _maxMoveSpeed = 1.5f;
        
        [SerializeField] private float _motorTorque = 500f;
        [SerializeField] private float _breakTorque = 300f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _maxSteeringSpeed = 10f;
        [SerializeField] private float _steeringRange = 15f;
        [SerializeField] private float _steeringWheelsSpeed = 2f;
        [SerializeField] private string _debugPointName;
        
        private Car _car;
        private Wheel[] _wheels;
        private Rigidbody _rigidBody;
        private Vector3 _destination;

        public bool IsReachedDestination { get; private set; }
        
        private void Awake()
        {
            _car = GetComponent<Car>();
            _rigidBody = GetComponent<Rigidbody>();

            _wheels = GetComponentsInChildren<Wheel>();
        }

        private void Update()
        {
            
            if (Vector3.Distance(_destination, transform.position) < 2f)
            {
                IsReachedDestination = true;
            }
            
            ApplyMovement();
        }
        
        private void ApplyMovement()
        {
            float angle = Vector3.SignedAngle(transform.forward, (_destination - transform.position).normalized, Vector3.up);
            
            float vAxis = 1;
            float hAxis = Mathf.Clamp(angle / 90.0f, -1, 1);
            
            float forwardSpeed = Vector3.Dot(transform.forward, _rigidBody.velocity);
            float speedFactor = Mathf.InverseLerp(0, _maxSpeed, forwardSpeed);
            float currentMotorTorque = Mathf.Lerp(_motorTorque, 0, speedFactor);
            float currentSteerRange = Mathf.Lerp(_steeringRange, _maxSteeringSpeed, speedFactor);

            bool isAcceleration = Mathf.Approximately(Mathf.Sign(vAxis), Mathf.Sign(forwardSpeed));

            foreach (var wheel in _wheels)
            {
                if (wheel.Steerable)
                {
                    wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, hAxis * currentSteerRange, _steeringWheelsSpeed * Time.deltaTime);
                }

                if (isAcceleration && wheel.Motorized)
                {
                    wheel.WheelCollider.motorTorque = vAxis * currentMotorTorque;
                    wheel.WheelCollider.brakeTorque = 0;
                }
                else
                {
                    wheel.WheelCollider.brakeTorque = Mathf.Abs(vAxis) * _breakTorque;
                    wheel.WheelCollider.motorTorque = 0;
                }
            }
        }

        public void SetDestination(Vector3 position, string debugPointName)
        {
            _debugPointName = debugPointName;
            _destination = position;
            
            IsReachedDestination = false;
        }
    }
}

