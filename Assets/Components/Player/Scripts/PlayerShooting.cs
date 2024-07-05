using System;
using System.Collections.Generic;
using BulletSpace;
using GameControllerSpace;
using ObjectPoolSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform _weaponTransform;
        [SerializeField] private float _bulletHitMiss = 25f;
        
        [Header("Pools")]
        [SerializeField] private GameObject _prefabBulletToPool;
        [SerializeField] private GameObject _prefabDecalToPool;
        [SerializeField] private int _amountBulletsInPool;
        [SerializeField] private int _amountDecalsInPool;
        
        private ObjectPool _bulletsPool;
        private ObjectPool _decalsPool;
        
        private Transform _cameraTransform;
        
        private void Awake()
        {
            _cameraTransform = Camera.main?.transform;

            _bulletsPool = new ObjectPool(_prefabBulletToPool, _amountBulletsInPool);
            _decalsPool = new ObjectPool(_prefabDecalToPool, _amountDecalsInPool);
        }

        private void Update()
        {
            _weaponTransform.gameObject.SetActive(Player.Instance.IsShootingMode);
        }

        public void Shoot()
        {
            GameObject bulletObj = _bulletsPool.GetPooledObject();

            if (!bulletObj)
            {
                return;
            }

            bulletObj.transform.position = _weaponTransform.position;
            bulletObj.transform.rotation = _cameraTransform.rotation;
                
            bulletObj.SetActive(true);
            
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                bullet.Target = hit.point;
                bullet.Hit = true;
                bullet.OnBulletCollision = OnBulletCollision;
            }
            else
            {
                bullet.Target = _cameraTransform.position + _cameraTransform.forward * _bulletHitMiss;
                bullet.Hit = false;
                bullet.OnBulletCollision = null;
            }
        }

        private void OnBulletCollision(ContactPoint contact)
        {
            GameObject decal = _decalsPool.GetPooledObject();
            
            decal.transform.position = contact.point + contact.normal * 0.001f;
            decal.transform.rotation = Quaternion.LookRotation(contact.normal);
            decal.SetActive(true);
        }
    }
}


