using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public partial class Curve3Edit
    {
        /// 曲线锚点图层，编辑曲线时显示的点（小正方块） 的图层
        string layerKeyName = "Keys";
        LayerMask layerKey { get { return LayerMask.GetMask(layerKeyName); } }
        int layerKeyNum { get { return (int)Mathf.Log(layerKey.value, 2); } }

        public Curve3 curve;

        Transform keysContainer;
        string keysContainerName = "CurveKeys_ToClick";
        public float keyClickSize = 0.05f;

        void Start()
        {
            InputEvents.I.svMouseDown2D_R = SVMouseDown2D_R;
        }

        private void SVMouseDown2D_R(Vector2 pos2d)
        {
            if (editCurve)
            {
                RaycastHit hit;
                if (InputEvents.I.SVRaycast(out hit, layerKey.value))
                {
                    hit.transform.GetComponent<ColliderEvents>().OnRaycastHit();
                }
            }
        }
    }
}