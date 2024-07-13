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
                     SetCurrentWayPoint(_currentWayPoint.Branches[Random.Range(0, _currentWayPoint.Branches.Count)]);
                }
                else if (_currentWayPoint.IsCrossingRoad)
                {
                    if (_currentWayPoint.Next)
                    {
                        SetCurrentWayPoint(_currentWayPoint.Next);
                    }
                }
                else if (_direction == 0)
                {
                    if (_currentWayPoint.Next)
                    {
                        SetCurrentWayPoint(_currentWayPoint.Next);
                    }
                    else
                    {
                        _direction = 1;
                        SetCurrentWayPoint(_currentWayPoint.Previous);
                    }
                }
                else
                {
                    if (_currentWayPoint.Previous)
                    {
                        SetCurrentWayPoint(_currentWayPoint.Previous);
                    }
                    else
                    {
                        _direction = 0;
                        SetCurrentWayPoint(_currentWayPoint.Next);
                    }
                }

                CheckChangeDirectionFromBranch();
                SetDestination(_currentWayPoint);
            }
        }
        
        public override void SetCurrentWayPoint(WayPoint wayPoint)
        {
            if (_currentWayPoint)
            {
                _currentWayPoint.ConnectedEntity = null;
                _currentWayPoint.IsDisallowSpawn = false;
            }
            
            _currentWayPoint = wayPoint;
            _currentWayPoint.ConnectedEntity = this;

            _currentWayPoint.IsDisallowSpawn = true;
        }

        public override void SetDirection(int direction)
        {
            _direction = direction;
        }

        private void CheckChangeDirectionFromBranch()
        {
            if (_direction == 0 && _currentWayPoint.Next)
            {
                bool isReverseDirection = Vector3.Dot(_currentWayPoint.transform.forward, _currentWayPoint.Next.transform.forward) < 0;

                if (isReverseDirection)
                {
                    _direction = 1;
                    SetCurrentWayPoint(_currentWayPoint.Next.Previous);
                }
            } else if (_direction == 1 && _currentWayPoint.Previous)
            {
                bool isReverseDirection = Vector3.Dot(_currentWayPoint.transform.forward , _currentWayPoint.Previous.transform.forward) < 0;
                    
                if (isReverseDirection)
                {
                    _direction = 0;
                    SetCurrentWayPoint(_currentWayPoint.Previous.Next);
                }
            }
        }
        
        protected override void SetDestination(WayPoint wayPoint)
        {
            // _car.WantGoCrossRoad = wayPoint.IsCrossingRoad;

            Vector3 position;
            Vector3 secondPosition;

            if (_direction == 0)
            {
                position = wayPoint.RightEdge;
                secondPosition = wayPoint.Next ? wayPoint.Next.RightEdge : position;
            }
            else
            {
                position = wayPoint.LeftEdge;
                secondPosition = wayPoint.Previous ? wayPoint.Previous.LeftEdge : position;
            }
            
            _carMovement.SetDestination(position, secondPosition, wayPoint.name);
        }
    }
}

