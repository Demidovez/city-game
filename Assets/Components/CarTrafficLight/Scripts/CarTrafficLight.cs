using System.Collections;
using System.Collections.Generic;
using CarSpace;
using Interfaces;
using UnityEngine;

namespace TrafficLightSpace
{
    public class CarTrafficLight : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private int _greenLightActiveTime = 10;
        [SerializeField] private int _greenLightInactiveTime = 10;
        [SerializeField] private int _delayOfStart;
        [SerializeField] private Light _greenLight;
        [SerializeField] private bool _defaultActive;

        private bool _isActiveGreen;
        private readonly List<Car> _waitCars = new List<Car>();

        private void Start()
        {
            StartCoroutine(ToggleActiveState());
        }
        
        private void Update()
        {
            _greenLight.enabled = _isActiveGreen;

            if (_isActiveGreen && _waitCars.Count > 0)
            {
                for (var i = 0; i < _waitCars.Count; i++)
                {
                    StartCoroutine(AllowMove(_waitCars[i]));
                    _waitCars.RemoveAt(i);
                }
            }
        }

        private IEnumerator AllowMove(Car car)
        {
            yield return new WaitForSeconds(5);
            car.ShouldStopAndWait = false;
        }
        
        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (_isActiveGreen)
            {
                return;
            }

            if (other.TryGetComponent(out Car car))
            {
                if (car.WantGoCrossRoad)
                {
                    car.ShouldStopAndWait = true;
                    _waitCars.Add(car);
                }
            }
        }
        
        private IEnumerator ToggleActiveState()
        {
            yield return new WaitForSeconds(_delayOfStart);
            
            while (gameObject)
            {
                _isActiveGreen = _defaultActive;
                yield return new WaitForSeconds(_greenLightActiveTime);
                
                _isActiveGreen = !_defaultActive;
                yield return new WaitForSeconds(_greenLightInactiveTime);
            }
        }
    }
}
