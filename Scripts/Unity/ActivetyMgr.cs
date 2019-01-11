using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class ActivetyMgr : MonoBehaviour
    {
        public List<DisActiveOnAwake> listDis;
        public List<ActiveOnAwake> list;
        [Button("HaveALook")]
        void GetList()
        {
            listDis = TransTool.GetComsScene<DisActiveOnAwake>();
            list = TransTool.GetComsScene<ActiveOnAwake>();
        }
        void Awake()
        {
            GetList();
            foreach (var active in list)
            {
                if (active.on) active.gameObject.SetActive(true);
            }
            foreach (var disact in listDis)
            {
                if (disact.on) disact.gameObject.SetActive(false);
            }
        }
    }
}