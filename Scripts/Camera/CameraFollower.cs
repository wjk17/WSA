using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class CameraFollower : Singleton<CameraFollower>
    {
        public Transform target;
        public Vector3 offset;
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            transform.position = target.position + offset;
        }
    }
}