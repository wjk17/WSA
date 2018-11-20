using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class MonoSingletonMgr : MonoBehaviour
    {
        [Button]
        void Awake()
        {
            foreach (var obj in FindObjectsOfType<MonoSingletonBase>())
            {
                obj.Init();
            }
        }
    }
}