using System;
using System.Collections;
using GameControllerSpace;
using UnityEngine;

namespace BulletSpace
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletDecal;
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _bulletLifeTime = 5f;
        
        public Vector3 Target { get; set; }
        public bool Hit { get; set;  }

        private void OnEnable()
        {
            StartCoroutine(DestroyBullet());
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_bulletLifeTime);
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

            GameObject decal = Instantiate(_bulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
            decal.transform.SetParent(GameController.Instance.BulletsContainer);
            
            gameObject.SetActive(false);
        }
    }
}
