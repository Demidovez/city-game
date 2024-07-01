using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VisibleTargetSpace;

namespace FieldOfViewSpace
{
    public class FieldOfView : MonoBehaviour
    {
        public float ViewRadius;
        [Range(0, 360)] 
        public float ViewAngle;

        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _delayTimeFinding = 0.2f;
        public List<VisibleTarget> VisibleTargets;

        private void Start()
        {
            StartCoroutine(FindVisible());
        }

        private IEnumerator FindVisible()
        {
            while (!gameObject.IsDestroyed())
            {
                yield return new WaitForSeconds(_delayTimeFinding);
                CheckVisibleTargets();
                FindVisibleTargets();
            }
        }

        private void FindVisibleTargets()
        {
            Collider[] targetInRadius = Physics.OverlapSphere(transform.position, ViewRadius, _targetMask);

            for (int i = 0; i < targetInRadius.Length; i++)
            {
                if (targetInRadius[i].TryGetComponent(out VisibleTarget target))
                {
                    if (!target.IsVisible && IsCurrentVisible(target))
                    {
                        target.IsVisible = true;
                        VisibleTargets.Add(target);
                    }
                }
            }
        }
        
        private void CheckVisibleTargets()
        {
            for (int i = 0; i < VisibleTargets.Count; i++)
            {
                VisibleTarget target = VisibleTargets[i];
            
                if (!IsCurrentVisible(target))
                {
                    target.IsVisible = false;
                    VisibleTargets.RemoveAt(i);
                }
            }
        }

        private bool IsCurrentVisible(VisibleTarget target)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            bool inViewAngle = Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2;

            if (!inViewAngle)
            {
                return false;
            }

            Vector3 startRay = transform.position + Vector3.up;
            bool isVisible = Physics.Raycast(startRay, directionToTarget, ViewRadius, _targetMask);

            return isVisible;
        }

        public Vector3 DirectionFromAngle(float angleDegrees, bool isAngleGlobal)
        {
            if (!isAngleGlobal)
            {
                angleDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
        }
    }
}

