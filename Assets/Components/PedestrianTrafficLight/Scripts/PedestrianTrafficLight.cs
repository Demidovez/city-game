using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using PedestrianSpace;
using UnityEngine;

namespace TrafficLightSpace
{
    public class PedestrianTrafficLight : MonoBehaviour, ITriggerAreaSubscriber
    {
        [SerializeField] private int _greenLightActiveTime = 10;
        [SerializeField] private int _greenLightInactiveTime = 10;
        [SerializeField] private Light _greenLight;
        [SerializeField] private bool _defaultActive;

        private bool _isActiveGreen;
        private readonly List<Pedestrian> _waitPedestrians = new List<Pedestrian>();

        private void Start()
        {
            StartCoroutine(ToggleActiveState());
        }
        
        private void Update()
        {
            _greenLight.enabled = _isActiveGreen;

            if (_isActiveGreen && _waitPedestrians.Count > 0)
            {
                for (var i = 0; i < _waitPedestrians.Count; i++)
                {
                    _waitPedestrians[i].ShouldStopAndWait = false;
                    _waitPedestrians.RemoveAt(i);
                }
            }
        }

        public void OnTriggerAreaEnter(GameObject target, Collider other)
        {
            if (_isActiveGreen)
            {
                return;
            }

            if (other.TryGetComponent(out Pedestrian pedestrian))
            {
                if (pedestrian.WantGoCrossRoad)
                {
                    pedestrian.ShouldStopAndWait = true;
                    _waitPedestrians.Add(pedestrian);
                }
            }
        }

        private IEnumerator ToggleActiveState()
        {
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

