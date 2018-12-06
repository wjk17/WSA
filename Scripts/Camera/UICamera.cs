using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    /// <summary>
    /// 控制摄像头的UI，6视角快捷切换
    /// </summary>
    public class UICamera : Singleton<UICamera>
    {
        public Button buttonReset;
        public Toggle toggleRotate;

        public RectTransform rectView;
        void Start()
        {
            this.AddInput();
            buttonReset.onClick.AddListener(ResetCam);
        }
        private void Update()
        {
            //if (ASUI.MouseOver(rectView) && Events.Key(KeyCode.Keypad1)) ResetCam();
            if (Events.Key(KeyCode.Keypad1)) ResetCam();
        }
        void ResetCam()
        {
            CameraController.I.ResetRotation();
        }
    }
}