using System;
using Interfaces;
using PlayerSpace;
using UnityEngine;

namespace CarSpace
{
    public class Car : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private GameObject _pressEText;
        public bool ShouldStopAndWait { get; set; }
        public bool WantGoCrossRoad { get; set; }
        
        public bool HasDriver { get; private set; }

        public static Action<bool> OnCarWaitActionEvent;

        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (other.gameObject == Player.Instance.gameObject)
            {
                OnCarWaitActionEvent?.Invoke(true);
                _pressEText.SetActive(true);
            }
        }
        
        public void OnTriggerAreaExit(GameObject target, Collider other)
        {
            if (other.gameObject == Player.Instance.gameObject)
            {
                OnCarWaitActionEvent?.Invoke(false);
                _pressEText.SetActive(false);
            }
        }
    }
}

