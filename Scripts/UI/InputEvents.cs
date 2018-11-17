using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Esa.UI
{
    public class InputEvents : Singleton<InputEvents>
    {
        public delegate void SVMouseEvt2D(Vector2 pos);
        public delegate void SVMouseEvtRaycast(Vector3 pos, bool hit);
        public delegate void SVMouseEvtRaycast0(RaycastHit hit);
        public SVMouseEvtRaycast svMouseDownRaycast;
        public SVMouseEvtRaycast0 svMouseDownRaycast0;
        public SVMouseEvt2D svMouseDown2D;
        public SVMouseEvt2D svMouseDown2D_R;
        public SVMouseEvt2D svMouseDownMove2D;
        public SVMouseEvt2D svMouseUp2D;
        public LayerMask mask;
        public float skinThickness;
        public bool skin;
        public void Start()
        {
            this.AddInputCB(GetInput, 1);
        }
        private void GetInput()
        {
            if (!gameObject.activeSelf || !enabled) return;
            if (Events.MouseDown(1))
            {
                var mousePos = Input.mousePosition;
                if (svMouseDown2D_R != null)
                    svMouseDown2D_R(mousePos);
            }
            else if (Events.MouseDown(0))
            {
                var mousePos = Input.mousePosition;
                if (svMouseDown2D != null)
                    svMouseDown2D(mousePos);
                if (svMouseDownRaycast != null)
                {
                    Vector3 p;
                    var hit = SVRaycast(mousePos, out p);
                    svMouseDownRaycast(p, hit);
                }
                //if (svMouseDownRaycast0 !=null)
                //{
                //    RaycastHit hit;
                //    SVRaycast(out hit, mask);
                //}
            }
            else if (Events.Mouse(0))
            {
                var mousePos = Input.mousePosition;
                if (svMouseDownMove2D != null)
                    svMouseDownMove2D(mousePos);
            }
            else if (Events.MouseUp(0))
            {
                var mousePos = Input.mousePosition;
                if (svMouseUp2D != null)
                    svMouseUp2D(mousePos);
                //if (svMouseDownRaycast != null)
                //{
                //    Vector3 p;
                //    var hit = SVRaycast(mousePos, out p);
                //    svMouseDownRaycast(p, hit);
                //}
            }
        }
        public Vector3 SVToRayHit(Vector3 screenPos)
        {
            Vector3 hitPoint;
            SVRaycast(screenPos, out hitPoint);
            return hitPoint;
        }
        public bool SVRaycast(out Transform hitT, int layerMask)
        {
            return SVRaycast(Input.mousePosition, out hitT, layerMask);
        }
        public bool SVRaycast(Vector3 mousePos, out Transform hitT, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                var p = hit.transform;
                hitT = p;
                return true;
            }
            else
            {
                hitT = null;
                return false;
            }
        }
        public RaycastHit[] SVRaycastAll(int layerMask)
        {
            return SVRaycastAll(Input.mousePosition, layerMask);
        }
        public RaycastHit[] SVRaycastAll(int layerMask, Vector3 v)
        {
            return SVRaycastAll(v, layerMask);
        }
        public RaycastHit[] SVRaycastAll(Vector3 mousePos, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            return Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
        }
        public bool SVRaycast(out RaycastHit hit, int layerMask)
        {
            return SVRaycast(Input.mousePosition, out hit, layerMask);
        }
        public bool SVRaycast(Vector3 mousePos, out RaycastHit hit, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SVRaycast(Vector3 mousePos, out Vector3 hitPoint)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
            {
                var p = hit.point;

                if (skin) p += cam.transform.forward * -1 * skinThickness;
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