using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    public class ReturnButton : MonoBehaviour
    {
        public GameObject charCam;
        public bool showCam;
        public GameObject tips;
        public CharCtrl charControl;
        public float originLightY;
        public float yOffset;
        void Start()
        {
            this.AddInput(Input, 0, false);
            originLightY = FindObjectOfType<Light>().transform.eulerAngles.y;
        }
        public void OnClick()
        {
            showCam = !showCam;
            UICharColorMod.I.gameObject.SetActive(showCam);
            UIBag.I.gameObject.SetActive(showCam);
            UIEquip.I.gameObject.SetActive(showCam);
            UIEquip.I.transform.parent.gameObject.SetActive(showCam);
            tips.SetActive(showCam);
            charControl.enabled = !showCam;
            charControl.animator.SetClip("Idle");
            charCam.gameObject.SetActive(showCam);

            y = prevY = 0;
            charCam.transform.localRotation = Quaternion.AngleAxis(y, Vector3.up);

            FindObjectOfType<Light>().SetLocalEulerY(showCam ?
                charControl.transform.eulerAngles.y + yOffset : originLightY);
        }

        Vector2 prevPos;
        float prevY;
        float y;
        public float sensy = 1;
        public bool dirRev;
        bool dragging;
        void Input()
        {
            if (showCam)
            {
                if (Events.mouseDown1to3 && this.Hover())
                {
                    dragging = true;
                    prevPos = UI.mousePosRef;
                    prevY = y;
                }
                else if (Events.mouse1to3 && dragging)
                {
                    var os = UI.mousePosRef - prevPos;
                    if (dirRev) os = -os;
                    y = prevY + os.x * sensy;
                    charCam.transform.localRotation = Quaternion.AngleAxis(y, Vector3.up);
                }
                else dragging = false;
            }
        }
    }
}