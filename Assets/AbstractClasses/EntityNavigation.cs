using UnityEngine;
using WayPointsSpace;

public abstract class EntityNavigation : MonoBehaviour
{
    public abstract void SetCurrentWayPoint(WayPoint wayPoint);

    public abstract void SetDirection(int direction);
}
