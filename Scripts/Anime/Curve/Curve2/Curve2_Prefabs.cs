using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public partial class Curve2
    {
        static Curve2()
        {
            t0v1_t1v1 = new Curve2(Vector2.up, Vector2.right + Vector2.up);

            t0v0_t05v1_t1v0 = new Curve2(Vector2.zero, Vector2.right);
            t0v0_t05v1_t1v0.InsertKey(new Key2(0.5f, 1f));
            t0v0_t05v1_t1v0.time1D = false;

            t0v0_t1v0 = new Curve2(Vector2.zero, Vector2.right);
        }
        static Curve2 t0v0_t05v1_t1v0;
        public static Curve2 T0V1_T05V1_T1V0 { get { return t0v0_t05v1_t1v0.Clone(); } }

        static Curve2 t0v1_t1v1;
        public static Curve2 T0V1_T1V1 { get { return t0v1_t1v1.Clone(); } }

        static Curve2 t0v0_t1v0;
        public static Curve2 T0V0_T1V0 { get { return t0v0_t1v0.Clone(); } }
    }
}