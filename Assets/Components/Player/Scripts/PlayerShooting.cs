using System;
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
        
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main?.transform;
        }

        public void Shoot()
        {
            GameObject bulletObj = ObjectPool.Instance.GetPooledObject();

            if (!bulletObj)
            {
                return;
            }

            bulletObj.transform.position = _weaponTransform.position;
            bulletObj.transform.rotation = _weaponTransform.rotation;
                
            bulletObj.SetActive(true);
            
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                bullet.Target = hit.point;
                bullet.Hit = true;
            }
            else
            {
                bullet.Target = _cameraTransform.position + _cameraTransform.forward * _bulletHitMiss;
                bullet.Hit = false;
            }
        }
    }
}


