using Interfaces;
using UnityEngine;

namespace UtilsAreaSpace
{
    public class TriggerArea : MonoBehaviour
    {
        [SerializeField] private GameObject _triggerSubscriber;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggerSubscriber.TryGetComponent(out ITriggerAreaSubscriber subscriber))
            {
                subscriber.OnTriggerAreaEnter(gameObject, other);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_triggerSubscriber.TryGetComponent(out ITriggerAreaSubscriber subscriber))
            {
                subscriber.OnTriggerAreaExit(gameObject, other);
            }
        }
    }
}

