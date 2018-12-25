using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class DisActiveMgr : MonoBehaviour
    {
        public List<DisActiveOnAwake> list;
        [Button]
        void HaveALook()
        {
            list = TransTool.GetComsScene<DisActiveOnAwake>();
        }
        void Awake()
        {
            list = TransTool.GetComsScene<DisActiveOnAwake>();
            foreach (var hoa in list)
            {
                hoa.gameObject.SetActive(false);
            }
        }
    }
}