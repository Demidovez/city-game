using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace PedestrianSpace
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PedestrianMovement : MonoBehaviour
    {
        [SerializeField] private float _minMoveSpeed = 0.75f;
        [SerializeField] private float _maxMoveSpeed = 1.5f;
        [SerializeField] private string _debugPointName;
        
        private Pedestrian _pedestrian;
        private NavMeshAgent _navMeshAgent;

        public bool IsReachedDestination { get; private set; }

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = Random.Range(_minMoveSpeed, _maxMoveSpeed);
            
            _pedestrian = GetComponent<Pedestrian>();
        }

        private void Update()
        {
            _navMeshAgent.isStopped = _pedestrian.ShouldStopAndWait;

            if (_navMeshAgent.isStopped)
            {
                IsReachedDestination = false;
            }
            else
            {
                IsReachedDestination = _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
            }
        }

        public void SetDestination(Vector3 position, string debugPointName)
        {
            _debugPointName = debugPointName;

            _navMeshAgent.SetDestination(position);
            IsReachedDestination = false;
        }
    }
}

