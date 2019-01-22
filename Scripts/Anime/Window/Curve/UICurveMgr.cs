using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    /// <summary>
    /// 统一设置所有曲线的模式
    /// </summary>
    public class UICurveMgr : MonoBehaviour
    {
        public Button setAllCurveToLinear;
        void Start()
        {
            this.AddInput();
            setAllCurveToLinear.onClick.AddListener(SetAllCurveToLinear);
        }
        void SetAllCurveToLinear()
        {
            foreach (var oc in UIClip.I.clip.curves)
            {
                foreach (var curve in oc.curves)
                {
                    foreach (var key in curve.keys)
                    {
                        key.inMode = KeyMode.Linear;
                        key.outMode = KeyMode.Linear;
                    }
                }
            }
        }
    }
}