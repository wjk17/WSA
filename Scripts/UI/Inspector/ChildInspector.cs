using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [ExecuteInEditMode]
    public class ChildInspector : MonoBehaviour
    {
        public Transform[] childs;
        public int childCount;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            childCount = transform.childCount;
            childs = transform.GetChildsL1().ToArray();
        }
    }
}