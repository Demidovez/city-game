using System.Collections.Generic;
using UnityEngine;

namespace WayPointsSpace
{
    public class WayPoint : MonoBehaviour
    {
        public WayPoint Previous;
        public WayPoint Next;

        public Vector3 LeftEdge => transform.position - (transform.right * Width / 2);
        public Vector3 RightEdge => transform.position + (transform.right * Width / 2);
        
        [Range(0f, 5f)] public float Width = 1f;
        [Range(0f, 1f)] public float BranchRatio = 0.5f;
        

        public List<WayPoint> Branches;
        
        public Vector3 GetPosition()
        {
            return Vector3.Lerp(LeftEdge, RightEdge, Random.Range(0f, 1f));
        }
    }
}

