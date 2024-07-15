using System;
using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public delegate void UpdatedDriverDelegate(bool hasDriver);
    
    public class Car : MonoBehaviour
    {
        public Transform DriverSeat;
        public UpdatedDriverDelegate OnUpdatedDriver;

        public bool ShouldStopAndWait { get; set; }
        public bool WantGoCrossRoad { get; set; }
        public bool IsMoving => _rigidBody.velocity != Vector3.zero;

        private Player _driver;
        public Player Driver
        {
            get => _driver;
            set
            {
                _driver = value;
                OnUpdatedDriver?.Invoke(value);
            }
        }

        private Rigidbody _rigidBody;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
    }
}

