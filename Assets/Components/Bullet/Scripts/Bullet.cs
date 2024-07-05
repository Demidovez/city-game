using System;
using System.Collections;
using GameControllerSpace;
using UnityEngine;

namespace BulletSpace
{
    public delegate void OnBulletCollision(ContactPoint contact);
    
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _lifeTime = 5f;

        public OnBulletCollision OnBulletCollision;
        
        public Vector3 Target { get; set; }
        public bool Hit { get; set;  }

        private void OnEnable()
        {
            StartCoroutine(InactiveBullet());
        }

        private IEnumerator InactiveBullet()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, _speed * Time.deltaTime);
            
            if (!Hit && Vector3.Distance(transform.position, Target) < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.GetContact(0);
            
            OnBulletCollision?.Invoke(contact);
            
            gameObject.SetActive(false);
        }
    }
}
