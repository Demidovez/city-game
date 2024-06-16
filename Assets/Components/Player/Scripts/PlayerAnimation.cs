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
            _animator.SetBool("IsRunning", Player.Instance.IsRunning);
            _animator.SetBool("IsJumping", Player.Instance.IsJumping);
            _animator.SetBool("IsFalling", Player.Instance.IsFalling);
        }
    }
}

