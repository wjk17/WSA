using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static class DragWindowTool
    {
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

        public DragWindow(Transform t)
        {
            transform = t;
        }
        public void Check()
        {
            if (Events.MouseDown0)
            {
                downPosM = UI.mousePosRef;
                downPos = transform.UIPos();
            }
            else if (Events.Mouse0)
            {
                var os = UI.mousePosRef - downPosM;
                transform.SetUIPos(downPos + os);
            }
        }
    }
}
