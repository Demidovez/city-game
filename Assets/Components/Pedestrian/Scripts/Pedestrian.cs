using System;
using UnityEngine;

namespace PedestrianSpace
{
    public class Pedestrian : MonoBehaviour
    {
        public bool ShouldStopAndWait { get; set; }
        public bool WantGoCrossRoad { get; set; }

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool("IsWait", ShouldStopAndWait);
        }
    } 
}

