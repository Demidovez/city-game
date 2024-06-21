using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace PedestrianSpace
{
    [RequireComponent(typeof(CharacterController))]
    public class PedestrianMovement : MonoBehaviour
    {
        [SerializeField] private float _minMoveSpeed = 0.75f;
        [SerializeField] private float _maxMoveSpeed = 1.5f;
        [SerializeField] private float _stopDistance;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private string _debugPointName;
        
        private Vector3 _destination;
        private CharacterController _characterController;
        private float _movementSpeed;

        public bool IsReachedDestination { get; private set; }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _movementSpeed = Random.Range(_minMoveSpeed, _maxMoveSpeed);
        }

        private void Update()
        {
            if (transform.position != _destination)
            {
                Vector3 destinationDirection = _destination - transform.position;
                destinationDirection.y = 0f;

                float destinationDistance = destinationDirection.magnitude;

                if (destinationDistance >= _stopDistance)
                {
                    IsReachedDestination = false;

                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                        _rotationSpeed * Time.deltaTime);

                    Vector3 movement = destinationDirection.normalized * (_movementSpeed * Time.deltaTime);
                    movement.y = -1f;
                    _characterController.Move(movement);
                }
                else
                {
                    IsReachedDestination = true;
                }
            }
        }

        public void SetDestination(Vector3 position, string debugPointName)
        {
            _destination = position;
            _debugPointName = debugPointName;

            IsReachedDestination = false;
        }
    }
}

