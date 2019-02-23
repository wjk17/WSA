using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa.UI_
{
    public class SceneTransition : Singleton<SceneTransition>
    {
        ST_UpDownToCenter st;
        public float t
        {
            get { return st.t; }
            set { st.t = value; }
        }
        private void Start()
        {
            st = new ST_UpDownToCenter();
            st.t = 2;
            this.AddInput(Draw, -10000, true);
        }
        public void Go(Action action)
        {
            st.action = action;
            st.t = 0;
        }
        void Draw()
        {
            this.BeginOrtho(10000);
            st.Draw();
        }
    }
    [Serializable]
    // 过场：上下向中间
    public class ST_UpDownToCenter
    {
        public float t = 0;
        public float speed = 1;
        public Action action;
        public void Draw()
        {
            t += Time.deltaTime * speed;
            if (t > 1)
            {
                action.SafeInvoke();
                action = null;
                return;
            }
            var size = new Vector2(1, t * 0.5f);

            Rect up = new Rect(Vector2.up.MulRef(), size.MulRef(), Vector2.up);
            Rect down = new Rect(Vector2.zero, size.MulRef(), Vector2.zero);
            GLUI.DrawQuad(Color.black, up.ToCWA());
            GLUI.DrawQuad(Color.black, down.ToCWA());
        }
    }
}