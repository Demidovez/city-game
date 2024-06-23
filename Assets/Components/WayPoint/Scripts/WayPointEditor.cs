using UnityEditor;
using UnityEngine;

namespace WayPointsSpace
{
    [InitializeOnLoad()]
    public class WayPointEditor : MonoBehaviour
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(WayPoint wayPoint, GizmoType gizmoType)
        {
            bool isSelected = (gizmoType & GizmoType.Selected) != 0;
            
            Gizmos.color = Color.yellow * (isSelected ? 1 : 0.5f);
            Gizmos.DrawSphere(wayPoint.transform.position, 0.1f);
            
            Gizmos.color = Color.white;
            Gizmos.DrawLine(wayPoint.LeftEdge, wayPoint.RightEdge);

            if (wayPoint.Previous)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(wayPoint.RightEdge, wayPoint.Previous.RightEdge);
            }
            
            if (wayPoint.Next)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(wayPoint.LeftEdge, wayPoint.Next.LeftEdge);
            }

            if (wayPoint.Branches != null)
            {
                Gizmos.color = Color.blue;
                
                foreach (WayPoint branch in wayPoint.Branches)
                {
                    if (branch)
                    {
                        Gizmos.DrawLine(wayPoint.transform.position, branch.transform.position);
                    }
                }
            }
        }
    }
}

