using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class SingletonMgr : MonoBehaviour
    {
        public static List<SingletonBase> objs;
        public static void Init()
        {
            if (SYS.debugSingleton) print("Singleton Init");
            objs = TransTool.GetComsScene<SingletonBase>();
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