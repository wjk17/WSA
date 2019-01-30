using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public partial class CharCtrl
    {
        public KeyCode keyInter; // interact
        public NPC nearNPC;

        Transform _pointingNPC; // previous value
        public Transform pointingNPC; // mouse over    
        Transform Pointing()
        {
            return RaycastTool.SVRaycast(NPC.layerMask);
        }
        void Interact()
        {
            pointingNPC = Pointing();
            if (_pointingNPC != pointingNPC)
            {
                _pointingNPC = pointingNPC;
                var _c = pointingNPC == null ? UI.I.cursorDefault : UI.I.cursorOverNPC;
                Cursor.SetCursor(_c.icon, _c.hotspot, CursorMode.Auto);
            }
            if (pointingNPC != null)
            {
                if (Events.MouseDown0)
                {
                    nearNPC = pointingNPC.GetComponent<NPC>();
                    Chat();
                }
            }
            else if (nearNPC && Events.KeyDown(keyInter))
            {
                Chat();
            }
        }

        void Chat()
        {
            if (nearNPC != null)
            {
                SYS.ShowMsg(nearNPC.datas[0].text[0]);
            }
        }
        private void LateUpdate()
        {
            Interact();
        }
        /// 头发图层
        private void OnTriggerEnter(Collider other)
        {
            if (SYS.debugTrigger) print(other.name);
            nearNPC = other.GetComponent<NPC>();
        }
    }
}