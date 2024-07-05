using System;
using System.Collections.Generic;
using GameControllerSpace;
using UnityEngine;

namespace ObjectPoolSpace
{
    public class ObjectPool
    {
        private List<GameObject> _poolObjects;
        private GameObject _prefabToPool;
        private int _amountToPool;

        public ObjectPool(GameObject prefabToPool, int amountToPool)
        {
            _poolObjects = new List<GameObject>();
            _prefabToPool = prefabToPool;
            _amountToPool = amountToPool;

            GameObject temp;
            
            for (int i = 0; i < _amountToPool; i++)
            {
                temp = ObjectPoolManager.Instance.Instantiate(_prefabToPool);
                temp.SetActive(false);
                
                _poolObjects.Add(temp);
            }
        }
        
        public GameObject GetPooledObject()
        {
            foreach (var poolObject in _poolObjects)
            {
                if (!poolObject.activeInHierarchy)
                {
                    return poolObject;
                }
            }

            GameObject temp = ObjectPoolManager.Instance.Instantiate(_prefabToPool);
            temp.SetActive(false);
            _poolObjects.Add(temp);
            
            return temp;
        }
    }
}

