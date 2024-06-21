using System;
using System.Linq;
using UnityEngine;
using WayPointsSpace;
using Random = UnityEngine.Random;

namespace PedestrianSpace
{
    [RequireComponent(typeof(PedestrianMovement))]
    public class PedestrianNavigation : MonoBehaviour
    {
        private WayPoint _currentWayPoint;
        private PedestrianMovement _pedestrianMovement;
        private int _direction;

        private void Awake()
        {
            _pedestrianMovement = GetComponent<PedestrianMovement>();
        }

        private void Start()
        {
            SetDestination(_currentWayPoint);
        }

        private void Update()
        {
            if (_pedestrianMovement.IsReachedDestination)
            {
                bool shouldUseBranch = false;

                if (_currentWayPoint.Branches != null && _currentWayPoint.Branches.Count > 0)
                {
                    shouldUseBranch = Random.Range(0, 2) <= _currentWayPoint.BranchRatio;
                }

                if (shouldUseBranch)
                {
                    _currentWayPoint = _currentWayPoint.Branches[Random.Range(0, _currentWayPoint.Branches.Count - 1)];
                }
                else if (_direction == 0)
                {
                    if (_currentWayPoint.Next)
                    {
                        _currentWayPoint = _currentWayPoint.Next;
                    }
                    else
                    {
                        _direction = 1;
                        _currentWayPoint = _currentWayPoint.Previous;
                    }
                }
                else
                {
                    if (_currentWayPoint.Previous)
                    {
                        _currentWayPoint = _currentWayPoint.Previous;
                    }
                    else
                    {
                        _direction = 0;
                        _currentWayPoint = _currentWayPoint.Next;
                    }
                }
                
                SetDestination(_currentWayPoint);
            }
        }

        public void SetCurrentWayPoint(WayPoint wayPoint)
        {
            _currentWayPoint = wayPoint;
        }
        
        public void SetDirection(int direction)
        {
            _direction = direction;
        }

        private void SetDestination(WayPoint wayPoint)
        {
            _pedestrianMovement.SetDestination(wayPoint.GetPosition(), wayPoint.name);
        }
    }
}

