using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecalSpace
{
    public class Decal : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 4f;
            
        private void OnEnable()
        {
            StartCoroutine(InactiveDecal());
        }
        
        private IEnumerator InactiveDecal()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
    }
}

