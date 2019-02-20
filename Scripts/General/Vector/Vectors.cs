using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static class Vectors
    {
        /// <summary>
        /// Vector3.one * 0.5f
        /// </summary>
        public readonly static Vector3 half = Vector3.one * 0.5f;
        public readonly static Vector3 halfRight = Vector3.right * 0.5f;
        public readonly static Vector3 halfLeft = Vector3.left * 0.5f;
        public readonly static Vector3 halfDown = Vector3.down * 0.5f;
        public readonly static Vector3 halfUp = Vector3.up * 0.5f;

        public readonly static Vector2 half2d = half;
        public readonly static Vector2 halfRight2d = Vector2.right * 0.5f;
        public readonly static Vector2 halfLeft2d = Vector2.left * 0.5f;
        public readonly static Vector2 halfDown2d = Vector2.down * 0.5f;
        public readonly static Vector2 halfUp2d = Vector2.up * 0.5f;
    }
}