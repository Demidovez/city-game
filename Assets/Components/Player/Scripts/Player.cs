using System;
using ObjectPoolSpace;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        private PlayerShooting _playerShooting;
        
        public bool IsShootingMode { get; set; }

        private void Awake()
        {
            Instance = this;
            
            _playerShooting = GetComponent<PlayerShooting>();
        }

        public void Shoot()
        {
            if (!IsShootingMode)
            {
                return;
            }
            
            _playerShooting.Shoot();
        }
    }
}

