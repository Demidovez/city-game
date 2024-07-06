using System;
using Interfaces;
using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public class Car : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private GameObject _pressEText;
        public Transform DriverSeat;

        public static Action<bool, Car> OnCarWaitActionEvent;

        public bool IsDriving
        {
            get => _isDriving;
            set
            {
                _isDriving = value; 
                _pressEText.SetActive(!value);
            }
        }

        private bool _isDriving;
        
        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (other.gameObject == Player.Instance.gameObject)
            {
                OnCarWaitActionEvent?.Invoke(true, this);
                _pressEText.SetActive(true);
            }
        }
        
        public void OnTriggerAreaExit(GameObject target, Collider other)
        {
            if (other.gameObject == Player.Instance.gameObject)
            {
                OnCarWaitActionEvent?.Invoke(false, null);
                _pressEText.SetActive(false);
            }
        }
    }
}

