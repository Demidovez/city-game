using System;
using System.Collections;
using PedestrianSpace;
using UnityEngine;
using WayPointsSpace;
using Random = UnityEngine.Random;

namespace TrafficSpace
{
    public class TrafficEntitiesSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _entityPrefab;
        [SerializeField] private Transform _entitiesContainer;
        [SerializeField] private int _countEntities;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            int count = 0;

            while (count < _countEntities)
            {
                Transform wayPointTransform = transform.GetChild(Random.Range(0, transform.childCount - 1));
                wayPointTransform.TryGetComponent(out WayPoint wayPoint);
                
                GameObject entityObj = Instantiate(_entityPrefab, wayPointTransform.position, wayPointTransform.rotation, _entitiesContainer);
                entityObj.TryGetComponent(out PedestrianNavigation entityNavigation);
                entityNavigation.SetCurrentWayPoint(wayPoint);
                entityNavigation.SetDirection(count % 2);

                yield return new WaitForEndOfFrame();
                count++;
            }
        }
    }
}

