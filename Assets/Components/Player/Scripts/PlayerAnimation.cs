using System;
using UnityEngine;

namespace PlayerSpace
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat("Horizontal", PlayerMovement.Instance.CurrentBlendAnim.x);
            _animator.SetFloat("Vertical", PlayerMovement.Instance.CurrentBlendAnim.y);
            _animator.SetBool("Slowly", PlayerMovement.Instance.Slowly);
        }
    }
}

