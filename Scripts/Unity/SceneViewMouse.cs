using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
namespace Esa.UI_
{
    [CustomEditor(typeof(SceneViewMouse))]
    public class SceneViewMouseEditor : Editor
    {
        public delegate void SVMouseDown2D(Vector2 pos);
        public delegate void SVMouseDownRaycast(Vector3 pos, bool hit);
        SceneViewMouse obj { get { return (SceneViewMouse)target; } }

        void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Camera cam = SceneView.lastActiveSceneView.camera;
                var mousePos = obj.SVToScreen(Event.current.mousePosition);

                if (obj.svMouseDown2D != null) obj.svMouseDown2D(mousePos);

                if (obj.svMouseDownRaycast != null)
                {
                    Vector3 p;
                    var hit = obj.SVRaycast(mousePos, out p);
                    obj.svMouseDownRaycast(p, hit);
                }
            }
        }
    }
    public class SceneViewMouse : Singleton<SceneViewMouse>
    {
        public SceneViewMouseEditor.SVMouseDownRaycast svMouseDownRaycast;
        public SceneViewMouseEditor.SVMouseDown2D svMouseDown2D;
        public LayerMask mask;
        public float skinThickness;
        public Vector2 SVToScreen(Vector3 screenPos)
        {
            //Camera cam = SceneView.lastActiveSceneView.camera;
            Vector2 mousePos = screenPos;// Event.current.mousePosition;
                                         //mousePos.z = -cam.worldToCameraMatrix.MultiplyPoint(transform.position).z;
            mousePos.y = Screen.height - mousePos.y - 36.0f; // ??? Why that offset?!
                                                             /// maybe is the title bar of sceneview
            //mousepos = cam.ScreenToWorldPoint(mousepos);
            return mousePos;
        }
        public Vector3 SVToRayHit(Vector3 screenPos)
        {
            Vector3 hitPoint;
            SVRaycast(screenPos, out hitPoint);
            return hitPoint;
        }
        public bool SVRaycast(Vector3 screenPos, out Vector3 hitPoint)
        {
            Camera cam = SceneView.lastActiveSceneView.camera;
            Ray ray = cam.ScreenPointToRay(screenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
            {
                var p = hit.point;

                p += cam.transform.forward * -1 * skinThickness;
                hitPoint = p;
                return true;
            }
            else
            {
                hitPoint = Vector3.zero;
                return false;
            }
        }
    }
}
#endif

