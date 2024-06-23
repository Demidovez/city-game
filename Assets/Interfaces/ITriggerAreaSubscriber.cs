using UnityEngine;

namespace Interfaces
{
    public interface ITriggerAreaSubscriber
    {
        public void OnTriggerAreaEnter(GameObject target, Collider other);
    }
}