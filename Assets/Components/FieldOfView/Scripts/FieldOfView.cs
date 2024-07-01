using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        public List<Transform> VisibleTargets;

        private void Start()
        {
            StartCoroutine(FindVisible());
        }

        private IEnumerator FindVisible()
        {
            while (!gameObject.IsDestroyed())
            {
                yield return new WaitForSeconds(_delayTimeFinding);
                FindVisibleTarget();
            }
        }

        private void FindVisibleTarget()
        {
            Collider[] targetInRadius = Physics.OverlapSphere(transform.position, ViewRadius, _targetMask);

            for (int i = 0; i < targetInRadius.Length; i++)
            {
                Transform target = targetInRadius[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < ViewAngle / 2)
                {
                    if (Physics.Raycast(transform.position, directionToTarget, _targetMask))
                    {
                        VisibleTargets.Add(target);
                    }
                }
            }
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

