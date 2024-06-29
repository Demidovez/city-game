using System;
using System.Collections.Generic;
using GameControllerSpace;
using UnityEngine;

namespace ObjectPoolSpace
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;
        
        [SerializeField] private GameObject _prefabToPool;
        [SerializeField] private int _amountToPool;
        
        private List<GameObject> _poolObjects;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _poolObjects = new List<GameObject>();

            for (int i = 0; i < _amountToPool; i++)
            {
                _poolObjects.Add(Instantiate(_prefabToPool, GameController.Instance.BulletsContainer, false));
            }
        }

        public GameObject GetPooledObject()
        {
            for (int i = 0; i < _amountToPool; i++)
            {
                if (!_poolObjects[i].activeInHierarchy)
                {
                    return _poolObjects[i];
                }
            }

            return null;
        }
    }
}

