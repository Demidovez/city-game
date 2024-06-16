using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WayPointsSpace
{
    public class WayPointManager : EditorWindow
    {
        public Transform WayPointRoot;
        
        [MenuItem("Tools/WayPoint Editor")]
        public static void Open()
        {
            GetWindow<WayPointManager>();
        }
        
        void OnSelectionChange()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.TryGetComponent(out WayPoint wayPoint))
            {
                WayPointRoot = wayPoint.gameObject.transform.parent;
            }
            
            Repaint();
        }

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);
            EditorGUILayout.PropertyField(obj.FindProperty(nameof(WayPointRoot)));
            
            if (!WayPointRoot)
            {
                EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DrawButtons();
                EditorGUILayout.EndVertical();  
            }
            
            obj.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Create WayPoint"))
            {
                CreateWayPoint();
            }

            if (Selection.activeGameObject && Selection.activeGameObject.TryGetComponent(out WayPoint wayPoint))
            {
                if (GUILayout.Button("Create WayPoint Before"))
                {
                    CreateWayPointBefore(wayPoint);
                }
                
                if (GUILayout.Button("Create WayPoint After"))
                {
                    CreateWayPointAfter(wayPoint);
                }
                
                if (GUILayout.Button("Remove WayPoint"))
                {
                    RemoveWayPoint(wayPoint);
                }
            }
        }

        private Vector3 GenerateOffsetFrom(WayPoint fromPoint, WayPoint toPoint = null)
        {
            float distance = 1f;
            
            if (toPoint)
            {
                distance = Vector3.Distance(fromPoint.transform.position, toPoint.transform.position) / 2f;
            }

            return fromPoint.transform.forward * distance;
        }

        private void RemoveWayPoint(WayPoint selectedWayPoint)
        {
            GameObject selectedObj = null;
            
            if (selectedWayPoint.Previous)
            {
                selectedWayPoint.Previous.Next = selectedWayPoint.Next;
                selectedObj = selectedWayPoint.Previous.gameObject;
            }
            
            if (selectedWayPoint.Next)
            {
                selectedWayPoint.Next.Previous = selectedWayPoint.Previous;
                selectedObj = selectedWayPoint.Next.gameObject;
            }
            
            DestroyImmediate(selectedWayPoint.gameObject);

            if (selectedObj)
            {
                Selection.activeGameObject = selectedObj;
            }
            else if (WayPointRoot.childCount > 0)
            {
                Selection.activeGameObject = WayPointRoot.GetChild(WayPointRoot.childCount - 1).gameObject;
            }
            else
            {
                Selection.activeGameObject = WayPointRoot.gameObject;
            }
        }

        private void CreateWayPointAfter(WayPoint selectedWayPoint)
        {
            GameObject newWayPointObj = new GameObject("WayPoint_" + WayPointRoot.childCount, typeof(WayPoint));
            newWayPointObj.transform.SetParent(WayPointRoot, false);
            
            newWayPointObj.TryGetComponent(out WayPoint newWayPoint);
            
            newWayPoint.transform.position = selectedWayPoint.transform.position + GenerateOffsetFrom(selectedWayPoint, selectedWayPoint.Next);
            newWayPoint.transform.forward = selectedWayPoint.transform.forward;
            
            if (selectedWayPoint.Next)
            {
                newWayPoint.Next = selectedWayPoint.Next;
                newWayPoint.Next.Previous = newWayPoint;
            }

            newWayPoint.Previous = selectedWayPoint;
            selectedWayPoint.Next = newWayPoint;
            newWayPoint.Width = selectedWayPoint.Width;
            
            newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex() + 1);

            Selection.activeGameObject = newWayPointObj;
        }

        private void CreateWayPointBefore(WayPoint selectedWayPoint)
        {
            GameObject newWayPointObj = new GameObject("WayPoint_" + WayPointRoot.childCount, typeof(WayPoint));
            newWayPointObj.transform.SetParent(WayPointRoot, false);
            
            newWayPointObj.TryGetComponent(out WayPoint newWayPoint);
            
            newWayPoint.transform.position = selectedWayPoint.transform.position - GenerateOffsetFrom(selectedWayPoint, selectedWayPoint.Previous);
            newWayPoint.transform.forward = selectedWayPoint.transform.forward;
            
            if (selectedWayPoint.Previous)
            {
                newWayPoint.Previous = selectedWayPoint.Previous;
                newWayPoint.Previous.Next = newWayPoint;
            }
            
            newWayPoint.Next = selectedWayPoint;
            selectedWayPoint.Previous = newWayPoint;
            newWayPoint.Width = selectedWayPoint.Width;
            
            newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());

            Selection.activeGameObject = newWayPointObj;
        }

        private void CreateWayPoint()
        {
            GameObject newWayPointObj = new GameObject("WayPoint_" + WayPointRoot.childCount, typeof(WayPoint));
            
            if (WayPointRoot.childCount > 0)
            {
                newWayPointObj.TryGetComponent(out WayPoint newWayPoint);
                WayPointRoot.GetChild(WayPointRoot.childCount - 1).TryGetComponent(out WayPoint previousWayPoint);

                newWayPoint.Previous = previousWayPoint;
                newWayPoint.Previous.Next = newWayPoint;
                newWayPoint.Width = previousWayPoint.Width;
                    
                newWayPointObj.transform.SetParent(WayPointRoot, false);
                
                newWayPoint.transform.position = previousWayPoint.transform.position + GenerateOffsetFrom(previousWayPoint);
                newWayPoint.transform.forward = newWayPoint.Previous.transform.forward;
            }
            else
            {
                newWayPointObj.transform.SetParent(WayPointRoot, false);
            }
            
            Selection.activeGameObject = newWayPointObj;
        }
    }
}

