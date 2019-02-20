using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class PianoMeshCtrl : MonoBehaviour
    {
        public Transform blackKeys;
        public Transform whiteKeys;
        public PosDataLerp blackKeysPos;
        public PosDataLerp whiteKeysPos;

        public Mesh whiteMesh;
        public Mesh blackMesh;
        public Vector3[] whiteMeshVerts;
        public Vector3[] blackMeshVerts;

        public int whiteNearsCount = 42;
        public int blackNearsCount = 56;
        void Start()
        {
            whiteMesh = Instantiate(whiteKeys.GetMesh());
            whiteKeys.SetMesh(whiteMesh);

            whiteMeshVerts = whiteMesh.vertices;

            blackMesh = Instantiate(blackKeys.GetMesh());
            blackKeys.SetMesh(blackMesh);

            blackMeshVerts = blackMesh.vertices;
        }

        // public Note notePress;
        public Vector3 whitePressOffset = new Vector3(0, -0.00017f, 0);
        public Vector3 blackPressOffset = new Vector3(0, -0.00007f, 0);
        public struct DistIdx
        {
            public float dist;
            public int idx;
            public DistIdx(int i, float v) : this()
            {
                this.idx = i;
                this.dist = v;
            }
            public static int Sort(DistIdx a, DistIdx b)
            {
                return a.dist.CompareTo(b.dist);
            }
            public static int SortRev(DistIdx a, DistIdx b)
            {
                return -a.dist.CompareTo(b.dist);
            }
        }
        public bool sortRev = false;
        public bool white = true;

        public PitchRange rng;
        public int os = -3;
        public int idx0;

        public List<Note> notePress;
        void Update()
        {
            if (notePress.Count == 0)
            {
                whiteMesh.vertices = whiteMeshVerts;
                blackMesh.vertices = blackMeshVerts;
                return;
            }

            Vector3[] vsWhite, vsBlack;
            vsWhite = whiteMeshVerts.CloneArray();
            vsBlack = blackMeshVerts.CloneArray();

            var stack = new Stack<Note>(notePress);
            stackLoop:

            var posIdx = stack.Pop() - rng.min;
            idx0 = posIdx;
            if (posIdx > (rng.max - rng.min)) throw new Exception("over range");
            posIdx += os;
            int pitch = posIdx % 12;
            int scale = posIdx / 12;
            var idx = PitchTool.Pitch12To8_5(pitch);
            white = idx > 0;
            posIdx = Mathf.Abs(idx) - 1;


            Vector3 pos;
            if (white)
            {
                posIdx += 2 + scale * 7;

                posIdx = Mathf.Clamp(posIdx, 0, whiteKeysPos.vectors.Count);
                pos = whiteKeysPos.vectors[posIdx];
                var whiteListDI = new List<DistIdx>();
                for (int i = 0; i < whiteMeshVerts.Length; i++)
                {
                    var p = whiteKeys.TransformPoint(whiteMeshVerts[i]);
                    whiteListDI.Add(new DistIdx(i, Vector3.Distance(p.X(), pos.X())));
                }

                if (sortRev) whiteListDI.Sort(DistIdx.SortRev);
                else whiteListDI.Sort(DistIdx.Sort);

                for (int i = 0; i < whiteNearsCount; i++)
                {
                    vsWhite[whiteListDI[i].idx] += whitePressOffset;
                }
            }
            else
            {
                posIdx += 1 + scale * 5;

                posIdx = Mathf.Clamp(posIdx, 0, blackKeysPos.vectors.Count);
                pos = blackKeysPos.vectors[posIdx];
                var blackListDI = new List<DistIdx>();
                for (int i = 0; i < blackMeshVerts.Length; i++)
                {
                    var p = blackKeys.TransformPoint(blackMeshVerts[i]);
                    blackListDI.Add(new DistIdx(i, Vector3.Distance(p.X(), pos.X())));
                }

                if (sortRev) blackListDI.Sort(DistIdx.SortRev);
                else blackListDI.Sort(DistIdx.Sort);

                for (int i = 0; i < blackNearsCount; i++)
                {
                    vsBlack[blackListDI[i].idx] += blackPressOffset;
                }
            }
            if (stack.Count > 0)
            {
                goto stackLoop;
            }

            whiteMesh.vertices = vsWhite;
            blackMesh.vertices = vsBlack;
        }
    }
}