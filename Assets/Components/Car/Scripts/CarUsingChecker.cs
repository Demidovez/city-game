using System;
using Interfaces;
using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public class CarUsingChecker : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private GameObject _pressEText;
        [SerializeField] private Car _car;
        
        public static Action<bool, Car> OnCarWaitActionEvent;

        private void OnEnable()
        {
            _car.OnUpdatedDriver += UpdatedDriver;
        }
        
        private void OnDisable()
        {
            _car.OnUpdatedDriver -= UpdatedDriver;
        }
        
        private void UpdatedDriver(bool hasDriver)
        {
            _pressEText.SetActive(!hasDriver);
        }

        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (other.gameObject == Player.Instance.gameObject)
            {
                OnCarWaitActionEvent?.Invoke(true, _car);
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
