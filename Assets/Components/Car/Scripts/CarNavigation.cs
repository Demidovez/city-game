using PlayerSpace;
using UnityEngine;
using WayPointsSpace;

namespace CarSpace
{
    public class CarNavigation : EntityNavigation
    {
        private WayPoint _currentWayPoint;
        private Car _car;
        private CarMovementByNavigation _carMovement;
        private int _direction;
        
        private void Awake()
        {
            _car = GetComponent<Car>();
            _carMovement = GetComponent<CarMovementByNavigation>();
        }
        
        private void Start()
        {
            SetDestination(_currentWayPoint);
        }
        
        private void Update()
        {
            if (_car.Driver == Player.Instance)
            {
                return;
            }
            
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
        
        protected override void SetDestination(WayPoint wayPoint)
        {
            // _car.WantGoCrossRoad = wayPoint.IsCrossingRoad;

            Vector3 position;
            Vector3 secondPosition;

            if (_direction == 0)
            {
                position = wayPoint.RightEdge;
                secondPosition = wayPoint.Next.RightEdge;
            }
            else
            {
                position = wayPoint.LeftEdge;
                secondPosition = wayPoint.Previous.LeftEdge;
            }
            
            _carMovement.SetDestination(position, secondPosition, wayPoint.name);
        }
    }
}

