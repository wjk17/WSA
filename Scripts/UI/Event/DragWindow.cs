using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static class DragWindowTool
    {
        public static bool Hover(this MonoBehaviour mono)
        {
            var rt = new RectTrans(mono);
            var rect = new Rect(rt.center, rt.sizeAbs);
            return rect.Contains(UI.mousePosRef);
        }
        public static List<DragWindow> inss = new List<DragWindow>();
        public static void DoDragWindow(this MonoBehaviour mono)
        {
            var ins = Find(inss, mono);
            if (ins == null)
            {
                ins = inss.Add_(new DragWindow(mono.transform));
            }
            ins.Check();
        }
        public static DragWindow Find(List<DragWindow> list, MonoBehaviour item)
        {
            foreach (var t in list)
            {
                if (t.transform == item.transform) return t;
            }
            return null;
        }
    }
    public class DragWindow
    {
        Vector2 downPos;
        Vector2 downPosM;
        public Transform transform;
        public RectTransform rectT;
        public bool dragging;
        public DragWindow(Transform t)
        {
            transform = t;
            rectT = t as RectTransform;
        }
        public void Check()
        {
            if (Events.mouseDown0)
            {
                var rt = new RectTrans(rectT);
                var rect = new Rect(rt.center, rt.sizeAbs);
                if (rect.Contains(UI.mousePosRef))
                {
                    dragging = true;
                    downPosM = UI.mousePosRef;
                    downPos = transform.UIPos();
                }
            }
            else if (Events.mouse0 && dragging)
            {
                var os = UI.mousePosRef - downPosM;
                transform.SetUIPos(downPos + os);
            }
            else dragging = false;
        }
    }
}
