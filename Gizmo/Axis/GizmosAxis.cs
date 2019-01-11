using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{    
    public partial class GizmosAxis : Singleton<GizmosAxis>
    {
        public float drawAxisLength = 100f;
        public bool drawAxisProj;
        Vector2 screenThisPos;
        Vector2 screenAxis;
        Vector2 mousePosProj;
        public int CB_Order = 10;
        private void Start()
        {
            originSize = cam.orthographicSize;
            this.AddInput(Input, CB_Order);
        }
        private void LateUpdate()
        {
            Draw();
            //AdjustSize
            var ratio = cam.orthographicSize / originSize;
            transform.localScale = Vector3.one * gizmosSize * ratio;
        }
        void Input()
        {
            Draw();
            deltaPosition = Vector3.zero;
            if (!dragging) CheckHover();
            CheckClick();
            if (dragging) MouseDragEvent(UnityEngine.Input.mousePosition);
        }
        void DrawLineScreen(Vector2 a, Vector2 b, Color color)
        {
            Matrix4x4 m = Matrix4x4.identity;
            var screenSize = new Vector2(Screen.width, Screen.height);
            a /= screenSize;
            b /= screenSize;
            DrawLines.DoDrawLines(color, new Vector2[] { a, b }, new int[] { 0, 1 }, m);
        }
        public Vector3 gridSize;
        public float smallStep;
        public Color color;
        private void Draw()
        {
            this._StartGL();
            GLUI.DrawGrid(gridSize, smallStep, color);

            if (dragging && drawAxisProj)
            {
                DrawLineScreen(screenThisPos, screenThisPos + screenAxis.normalized * drawAxisLength, Color.white);
                DrawLineScreen(screenThisPos, mousePosProj, Color.red);
                DrawLineScreen(screenThisPos, UnityEngine.Input.mousePosition, Color.black);
            }
        }

        Vector3 GetProjPos()
        {
            var axis = (Axis)axisIndex;
            Transform plane1, plane2;
            Vector3 axisV;
            Vector3 axisLocal;
            switch (axis)
            {
                case Axis.x:
                    plane1 = planes[1]; plane2 = planes[2];
                    axisLocal = Vector3.right; axisV = transform.right; break;

                case Axis.y:
                    plane1 = planes[2]; plane2 = planes[0];
                    axisLocal = Vector3.up; axisV = transform.up; break;

                case Axis.z:
                    plane1 = planes[0]; plane2 = planes[1];
                    axisLocal = Vector3.forward; axisV = transform.forward; break;

                default: throw new Exception("Wrong Axis Index");
            }
            /// 投射射线前，先在屏幕空间里投影到轴上
            screenThisPos = Camera.main.WorldToScreenPoint(transform.position);
            screenAxis = Camera.main.worldToCameraMatrix.MultiplyVector(axisV);

            var v = (Vector2)UnityEngine.Input.mousePosition - screenThisPos;
            var n = screenAxis;
            mousePosProj = screenThisPos + (Vector2)Vector3.Project(v, n);

            var hits = this.SVRaycastAll(mask.value, mousePosProj);
            if (dragging)
            {
                int i = 0;
                foreach (var p in planes)
                {
                    p.GetComponent<MeshRenderer>().enabled = showPlane && p == plane1;
                    axes[i == 0 ? 2 : i - 1].GetComponent<MeshRenderer>().enabled = showAxes && p == plane1;
                    i++;
                }
            }
            foreach (var hit in hits)
            {
                if (hit.transform == plane1 || hit.transform == plane2)
                {
                    var lp = transform.InverseTransformPoint(hit.point);
                    var proj = Vector3.Project(lp, axisLocal);
                    var wp = transform.TransformPoint(proj);
                    return wp;
                }
            }
            throw new Exception("not hit");
        }
        private void MouseDragEvent(Vector3 mousePos)
        {
            var os = GetProjPos() - downPosWorld;

            transform.position = downPosAxisWorld + os;
            if (_controlObj != null) _controlObj.position = transform.position;


            for (int i = 0; i < handles.Length; i++)
            {
                if (i == axisIndex) continue;
                SetMats(handles[i], unSelected);
            }
        }
        private void CheckHover()
        {
            if (!dragging)
            {
                for (int i = 0; i < handles.Length; i++)
                {
                    SetMats(handles[i], mats[i]);
                }
                foreach (var p in planes)
                {
                    p.GetComponent<MeshRenderer>().enabled = false;
                }
                foreach (var a in axes)
                {
                    a.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            var hits = this.SVRaycastAll(mask.value);
            foreach (var hit in hits)
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform == planes[0] || hit.transform == planes[1]
                    || hit.transform == planes[2]) continue;
                dragging = true;
                var n = hit.transform.name;
                switch (n)
                {
                    case "x": axisIndex = 0; break;
                    case "y": axisIndex = 1; break;
                    case "z": axisIndex = 2; break;
                    default: Debug.LogError(n); return;
                }
                for (int i = 0; i < handles.Length; i++)
                {
                    if (i == axisIndex) continue;
                    SetMats(handles[i], mats[i]);
                }
                SetMats(handles[axisIndex], selected);
                return;
            }
        }
        void CheckClick()
        {
            if (dragging && Events.Mouse0 == false)
            {
                dragging = false;
            }
            if (dragging && Events.MouseDown0)
            {
                downPosAxisWorld = transform.position;
                downPosWorld = GetProjPos();
            }
        }
    }
}