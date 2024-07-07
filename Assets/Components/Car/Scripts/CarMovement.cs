using System;
using PlayerSpace;
using UnityEngine;
using WheelSpace;

namespace CarSpace
{
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private float _motorTorque = 500f;
        [SerializeField] private float _breakTorque = 300f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _maxSteeringSpeed = 10f;
        [SerializeField] private float _steeringRange = 15f;
        
        private Car _car;
        private Wheel[] _wheels;
        private Rigidbody _rigidBody;
        
        private void Awake()
        {
            _car = GetComponent<Car>();
            _rigidBody = GetComponent<Rigidbody>();

            _wheels = GetComponentsInChildren<Wheel>();
        }

        private void Update()
        {
            if (!IsAllowedMove())
            {
                return;
            }

            ApplyMovement();
            FixDriverPosition();
        }

        private bool IsAllowedMove()
        {
            if (!_car.Driver)
            {
                return false;
            }

            if (Vector3.Distance(_car.Driver.transform.position, _car.DriverSeat.position) > 3f)
            {
                return false;
            }

            return true;
        }

        private void ApplyMovement()
        {
            float vAxis = PlayerMovement.Instance.MoveInput.y;
            float hAxis = PlayerMovement.Instance.MoveInput.x;

            float forwardSpeed = Vector3.Dot(transform.forward, _rigidBody.velocity);
            float speedFactor = Mathf.InverseLerp(0, _maxSpeed, forwardSpeed);
            float currentMotorTorque = Mathf.Lerp(_motorTorque, 0, speedFactor);
            float currentSteerRange = Mathf.Lerp(_steeringRange, _maxSteeringSpeed, speedFactor);

            bool isAcceleration = Mathf.Approximately(Mathf.Sign(vAxis), Mathf.Sign(forwardSpeed));

            foreach (var wheel in _wheels)
            {
                if (wheel.Steerable)
                {
                    wheel.WheelCollider.steerAngle = hAxis * currentSteerRange;
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

        private void FixDriverPosition()
        {
            _car.Driver.transform.position = _car.DriverSeat.position;
            _car.Driver.transform.rotation = _car.DriverSeat.rotation;
        }
    }
}

