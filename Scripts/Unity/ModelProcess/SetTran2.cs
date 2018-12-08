using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class SetTran2 : MonoBehaviour
    {
        public Transform from;
        public Transform to;
        //private void Reset()
        //{
        //    parent = transform;
        //}    
        void Start()
        {
            to.SetTran2(from.Tran2());
        }
    }
}