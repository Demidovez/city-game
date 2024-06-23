using UnityEngine;
using WayPointsSpace;

namespace CarSpace
{
    [RequireComponent(typeof(CarMovement))]
    public class CarNavigation : EntityNavigation
    {
        private WayPoint _currentWayPoint;
        private Car _car;
        private CarMovement _carMovement;
        private int _direction;

        private void Awake()
        {
            _car = GetComponent<Car>();
            _carMovement = GetComponent<CarMovement>();
        }

        private void Start()
        {
            SetDestination(_currentWayPoint);
        }

        private void Update()
        {
            if (_carMovement.IsReachedDestination)
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
                else if (_currentWayPoint.IsCrossingRoad)
                {
                    if (_currentWayPoint.Next)
                    {
                        _currentWayPoint = _currentWayPoint.Next;
                    }
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

        public override void SetCurrentWayPoint(WayPoint wayPoint)
        {
            _currentWayPoint = wayPoint;
        }
        
        public override void SetDirection(int direction)
        {
            _direction = direction;
        }

        private void SetDestination(WayPoint wayPoint)
        {
            _car.WantGoCrossRoad = wayPoint.IsCrossingRoad;
            _carMovement.SetDestination(wayPoint.GetPosition(), wayPoint.name);
        }
    }  
}

