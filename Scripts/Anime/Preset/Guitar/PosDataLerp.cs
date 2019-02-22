using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    [Serializable]
    public struct TransPair
    {
        public Transform a;
        public Transform b;
    }
    [ExecuteInEditMode]
    public class PosDataLerp : MonoBehaviour
    {
        public TransPair pair;
        public List<Vector3> vectors;
        public int count;
        public float gizmosRadius = 0.5f;
        public Color gizmosColor = Color.red;
        void Start()
        {

        }
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) UpdatePos();
            Gizmos.color = gizmosColor;
            foreach (var v in vectors)
            {
                Gizmos.DrawWireSphere(v, gizmosRadius);
            }
        }
        void Update()
        {
            UpdatePos();
        }
        public int[] skipId;
        public int os;
        public int range = 7;
        void UpdatePos()
        {
            vectors = new List<Vector3>();
            var factor = 1f / (count - 1);
            for (int i = 0; i < count; i++)
            {
                foreach (var id in skipId)
                {
                    if (id == ((i + os) % range)) goto nextloop;
                }
                var t = i * factor;
                vectors.Add(Vector3.Lerp(pair.a.position, pair.b.position, t));

                nextloop: continue;
            }
        }
    }
}