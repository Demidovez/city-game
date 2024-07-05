using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolSpace
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject Instantiate(GameObject prefabToPool)
        {
            return Instantiate(prefabToPool, gameObject.transform);
        }
    }
}

