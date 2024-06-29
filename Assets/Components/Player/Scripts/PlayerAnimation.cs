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
            _animator.SetFloat("Horizontal", PlayerMovement.Instance.HorizontalMove);
            _animator.SetFloat("Vertical", PlayerMovement.Instance.VerticalMove);
        }
    }
}

