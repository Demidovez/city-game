using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WayPointsSpace;
using Random = UnityEngine.Random;

namespace TrafficSpace
{
    public class TrafficEntitiesSpawn : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _entityPrefabs;
        [SerializeField] private Transform _entitiesContainer;
        [SerializeField] private int _countEntities;
        [SerializeField] private float _delayTimeSpawn = 0.5f;
        [SerializeField] private LayerMask _entityLayer;

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

                if (wayPoint.IsDisallowSpawn || wayPoint.IsLockSpawn)
                {
                    yield return new WaitForSeconds(_delayTimeSpawn);
                    continue;
                }

                int direction = count % 2;
                Vector3 spawnPosition;
                Quaternion lookRotation;
                
                if (direction == 0)
                {
                    spawnPosition = wayPoint.RightEdge;
                    lookRotation = Quaternion.LookRotation(wayPoint.transform.forward);
                }
                else
                {
                    spawnPosition = wayPoint.LeftEdge;
                    lookRotation = Quaternion.LookRotation(-wayPoint.transform.forward);
                }
                
                spawnPosition.y = 0f;

                if (Physics.Raycast(spawnPosition, wayPoint.transform.up, 10, _entityLayer))
                {
                    yield return new WaitForSeconds(_delayTimeSpawn);
                    continue;
                }
                
                GameObject entityObj = Instantiate(_entityPrefabs[Random.Range(0, _entityPrefabs.Count)], spawnPosition, lookRotation, _entitiesContainer);
                entityObj.TryGetComponent(out EntityNavigation entityNavigation);
                entityNavigation.SetCurrentWayPoint(wayPoint);
                entityNavigation.SetDirection(direction);

                wayPoint.IsDisallowSpawn = true;

                yield return new WaitForSeconds(_delayTimeSpawn);
                count++;
            }
        }
    }
}

