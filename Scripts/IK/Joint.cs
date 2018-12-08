using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [Serializable]
    public class Joint
    {
        public RigidbodyConstraints constraints;
        public float maxDeltaAngle;
        public float maxAngle;
        public float minAngle;
        public Transform transform;
        public bool skipClip;
        public DOF dof;
        public string name;
        public Joint Clone()
        {
            return (Joint)MemberwiseClone();
        }
    }
}