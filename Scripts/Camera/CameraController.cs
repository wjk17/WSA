//CameraController.cs for UnityChan
//Original Script is here:
//TAK-EMI / CameraController.cs
//https://gist.github.com/TAK-EMI/d67a13b6f73bed32075d
//https://twitter.com/TAK_EMI
//
//Revised by N.Kobayashi 2014/5/15 
//Change : To prevent rotation flips on XY plane, use Quaternion in cameraRotate()
//Change : Add the instrustion window
//Change : Add the operation for Mac
//

using UnityEngine;
using System.Collections;
using System;
namespace Esa.UI
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField]
        private GameObject pivotGO = null;

        public bool showInstWindow = false;

        private Vector3 oldPos;
        public bool trackOnX = true;
        public bool inverseX = false;
        public bool trackOnY = true;
        public bool wheelControlOn = true;
        [HideInInspector]
        public float orthoCamSize;
        public Vector3 orthoCamSizeMOM = new Vector3(0.1f, 0.45f, 10);
        public float tX;
        public float wheelSensitivity = 1f;
        public float dragSensitivity = 0.5f;
        public float trackSensitivity = 1f;
        public float moveSensitivity = 1f;
        private Quaternion originRotation;
        public bool dragging;

        public InputCallBack cb;
        public bool on = true;
        //Show Instrustion Window
        int x = 5;
        int y = 5;
        [HideInInspector] public float timer = 0;
        public Vector3 diff;
        public bool rotateOn = true;
        Transform pivotT { get { return pivotGO == null ? null : pivotGO.transform; } }
        public Vector3 startEuler = new Vector3(0, 0, 0);
        public int CB_Order = -1;

        [Button]
        public void ResetRotation()
        {
            pivotGO.transform.rotation = originRotation;
            tX = 0;
        }
        private void Awake()
        {
            originRotation = transform.rotation;
            var pt = pivotGO.transform;
            pt.rotation = originRotation;//否则轴与摄像机旋转不一致会导致cameraRotate方向异常。
            if (pt.parent == transform) pt.SetParent(transform.parent, true);
            transform.SetParent(pivotGO.transform);
        }

        public void SetEulerWithTx(Vector3 euler)
        {
            pivotT.eulerAngles = euler;
            tX = pivotT.eulerAngles.x;
        }
        private void Start()
        {
            SetEulerWithTx(startEuler);

            //#if UNITY_EDITOR
            orthoCamSize = GetComponent<Camera>().orthographicSize;
            //#else
            //        orthoCamSize = GetComponent<Camera>().orthographicSize = orthoCamSizeMOM.y;
            //#endif
            SyncCamSize();
            cb = this.AddInputCB(GetInput, CB_Order);
        }
        public void SyncCamSize()
        {
            var cams = GetComponentsInChildren<Camera>(true);
            foreach (var cam in cams)
            {
                cam.orthographicSize = orthoCamSize;
            }
        }
        bool mouseInWin
        {
            get
            {
                return MathTool.Between(Input.mousePosition, Vector2.zero,
                    new Vector2(Screen.width, Screen.height));
            }
        }
        void GetInput()
        {
            if (!gameObject.activeSelf || !enabled) return;

            if (on)//&& mouseInWin)
            {
                cb.order = CB_Order;
                mouseEvent();
            }
        }
        void Use()
        {
            cb.order = -100;
            Events.Use();
        }
        void OnGUI()
        {
            if (showInstWindow)
            {
                timer += Time.deltaTime;
                if (GUI.Button(new Rect(x, y, 200, 90), "") && timer > 0.5f) { showInstWindow = false; }
                GUI.Label(new Rect(x + 20, y + 10, 200, 30), "摄像机操作（点击隐藏）");
                GUI.Label(new Rect(x + 10, y + 30, 200, 30), "右键 / Alt+左键: 旋转");
                GUI.Label(new Rect(x + 10, y + 50, 200, 30), "中键 / Alt+Cmd/Ctrl+左键: 移动");
                GUI.Label(new Rect(x + 10, y + 70, 200, 30), "滚轮 / 2 指滑动: 推拉");
            }
        }
        void mouseEvent()
        {
            float delta = Events.Axis("Mouse ScrollWheel");
            if (delta != 0.0f && mouseInWin) mouseWheelEvent(delta * wheelSensitivity);

            if (Events.MouseDown1to3)
            {
                dragging = true;
                oldPos = Input.mousePosition;
            }
            else if (Events.Mouse1to3)
            {
                if (dragging) mouseDragEvent(Input.mousePosition);
            }
            else { dragging = false; }
            oldPos = Input.mousePosition;
        }
        public Camera cam { get { return GetComponent<Camera>(); } }
        public Vector2 size
        {
            get { var h = cam.orthographicSize * 2f; var w = h * cam.aspect; return new Vector2(w, h); }
        }
        void mouseDragEvent(Vector3 mousePos)
        {
            diff = mousePos - oldPos;
            Vector2 diffN = new Vector2(diff.x / Screen.width, diff.y / Screen.height);
            Vector2 diffWorld = Vector2.Scale(diffN, size);
            if (Events.Mouse0)
            {
                //Operation for Mac : "Left Alt + Left Command + LMB Drag" is Track
                if ((Events.Alt && Events.Command) || (Events.Alt && Events.Ctrl))
                {
                    var right = Vector3.Dot(diff, Vector2.right);
                    mouseWheelEvent(right * trackSensitivity);
                    Use();
                }
                else if (Events.Alt && Events.Shift)
                {
                    cameraTranslate(-diffWorld);
                }
                //Operation for Mac : "Left Alt + LMB Drag" is Tumble
                else if (Events.Alt)
                {
                    cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
                }
                //Only "LMB Drag" is no action.
            }
            //Track
            else if (Events.Mouse2)
            {
                cameraTranslate(-diffWorld);
            }
            //Tumble
            else if (Events.Mouse1)
            {
                cameraRotate(new Vector3(diff.y, diff.x, 0.0f));
            }
        }
        public Canvas canvas { get { return UI.canvas; } }
        public void mouseWheelEvent(float delta)
        {
            if (!wheelControlOn) return;

            if (Mathf.Abs(delta) > 0.01f)
            {
                orthoCamSize -= delta;
                if (orthoCamSize > orthoCamSizeMOM.z) orthoCamSize = orthoCamSizeMOM.z;
                else if (orthoCamSize < orthoCamSizeMOM.x) orthoCamSize = orthoCamSizeMOM.x;
                SyncCamSize();
            }
        }
        void cameraTranslate(Vector3 vec)
        {
            Use();

            Transform pivot = pivotGO.transform;

            vec *= moveSensitivity;
            vec.x *= inverseX ? -1 : 1;

            if (trackOnX) transform.Translate(Vector3.right * vec.x, Space.Self);
            if (trackOnY) transform.Translate(Vector3.up * vec.y, Space.Self);
        }
        public void cameraRotate(Vector3 eulerAngle)
        {
            if (!rotateOn) return;

            Use();
            //Use Quaternion to prevent rotation flips on XY plane
            //Quaternion q = Quaternion.identity;

            eulerAngle *= dragSensitivity;

            Transform focusTrans = pivotGO.transform;
            //focusTrans.localEulerAngles = focusTrans.localEulerAngles + eulerAngle;
            //focusTrans.Rotate(eulerAngle);
            //var z = focusTrans.localEulerAngles.z;
            tX -= eulerAngle.x;
            var y = focusTrans.localEulerAngles.y + eulerAngle.y;
            var range = 89;
            if (tX > range) tX = range;
            else if (tX < -range) tX = -range;

            //float limit = 360f;
            //float factor = 1f / limit; // 限制Z轴为360倍数。（不直接==0固定，因为四元数旋转有时会通过将某轴+360°避免欧拉角）
            //var round = Mathf.RoundToInt(z * factor);
            //focusTrans.localEulerAngles = focusTrans.localEulerAngles.SetZ(round * limit);
            focusTrans.localEulerAngles = new Vector3(tX, y, 0);

            //Change this.transform.LookAt(this.focus) to q.SetLookRotation(this.focus)
            //q.SetLookRotation(focus);
        }
    }
}