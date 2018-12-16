using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class HideLayer : MonoBehaviour
    {
        public List<HideOnAwake> list;
        [Button]
        void HaveALook()
        {
            list = TransTool.GetComsScene<HideOnAwake>();
        }
        void Awake()
        {
            list = TransTool.GetComsScene<HideOnAwake>();
            foreach (var hoa in list)
            {
                hoa.SetParent(transform);
            }
            gameObject.SetActive(false);
        }
    }
}