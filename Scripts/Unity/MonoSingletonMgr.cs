using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class MonoSingletonMgr : MonoBehaviour
    {
        public static List<MonoSingletonBase> objs;
        public static void Init()
        {
            objs = TransformTool.GetComsScene<MonoSingletonBase>();
        }
        [Button]
        void Awake()
        {
            foreach (var obj in objs)
            {
                obj._Awake();
            }

        }
        [Button]
        private void Start()
        {
            foreach (var obj in objs)
            {
                obj._Start();
            }
        }
    }
}