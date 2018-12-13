using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class MonoSingletonMgr : MonoBehaviour
    {
        List<MonoSingletonBase> objs;
        [Button]
        void Awake()
        {
            //foreach (var obj in FindObjectsOfType<MonoSingletonBase>())
            objs = TransformTool.GetComsScene<MonoSingletonBase>();
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