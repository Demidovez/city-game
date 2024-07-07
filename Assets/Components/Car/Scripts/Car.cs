using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public delegate void UpdatedDriverDelegate(bool hasDriver);
    
    public class Car : MonoBehaviour
    {
        public Transform DriverSeat;
        public UpdatedDriverDelegate OnUpdatedDriver;

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
    }
}

