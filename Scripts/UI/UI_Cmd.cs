using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    [System.Serializable]
    public class TextureSet
    {
        public Texture2D this[int i] { get { return tex[i]; } }
        public Texture2D[] tex;
        public Texture2D hover { get { return tex[0]; } }
        public Texture2D normal { get { return tex[1]; } }
        public Texture2D down { get { return tex[2]; } }
        public Texture2D focus { get { return tex[3]; } }
    }
    public partial class UI // Commands
    {
        public TextureSet texWindow;
        public TextureSet texButton;
        public TextureSet texButtonCon;
        public int corSizeButton;
        public int corSizeWindow;

        [Header("Command")]
#if UNITY_EDITOR
        public bool updateInEditor;
#endif
        public List<GLHandler> glHandlers;
        public List<IMHandler> imHandlers;
        public new Camera camera;
        public static GLHandler gl;
        public static IMHandler im;

        public object Owner; // for inspect
        private static object _owner;

        public static object owner
        {
            set
            {
                _owner = value;
                gl = I.glHandlers.Get(owner);
                im = I.imHandlers.Get(owner);
            }
            get { return _owner; }
        }
        private void Reset()
        {
            camera = Camera.main;
            if (camera == null) camera = FindObjectOfType<Camera>();
        }
        [Button]
        void Print()
        {
            foreach (var gl in glHandlers)
            {
                var go = gl.owner as GameObject;
                var name = go == null ? "null" : go.name;
                print("glHandler: " + name);
                foreach (var cmd in gl.cmds)
                {
                    print(cmd);
                }
            }
        }
        public Vector2 _mousePosRef;
#if UNITY_EDITOR
        Vector2 mousePosEvent;
        Vector2 gameViewSize;
        Vector2 sceneViewSize;

        private void OnDrawGizmos() // 需要打开SceneView 放在GameView右边
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && !updateInEditor) return;
#endif
            mousePosEvent = Event.current.mousePosition;

            sceneViewSize = screenSize;

            mousePosEvent = mousePosEvent.ReverseX().f_sub_x(gameViewSize.x);

            // sceneview的Size会计算标题栏 高度40
            var titleOs = 40f;
            var osY = (sceneViewSize.y - titleOs - gameViewSize.y) * 0.5f;

            mousePosEvent.y -= osY;
            _mousePosRef = mousePosRef; // for inspect in editor, play or not

            if (!Application.isPlaying)
            {
                UpdateInput();
                //OnGUI();
            }
        }
#endif
        private void OnGUI()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && !updateInEditor) return;
            gameViewSize = screenSize;
#endif
            _mousePosRef = mousePosRef; // for inspect in editor & runtime;
            Owner = owner;
            foreach (var hdr in imHandlers)
            {
                var go = (GameObject)hdr.owner;
                if (go != null && !go.activeInHierarchy) continue;
                hdr.Execute();
            }
        }
        public void CameraPostRender()
        {
            Owner = owner;
            GLUI.SetLineMaterial();

            var checkList = new List<GLHandler>();
            foreach (var hdr in glHandlers)
            {
                var go = hdr.owner as GameObject;
                if (go != null)
                    checkList.Add(hdr);
            }
            glHandlers = checkList;

            glHandlers.Sort(SortList);
            foreach (var hdr in glHandlers)
            {
                var go = (GameObject)hdr.owner;
                if (go == null || go.activeInHierarchy)
                {
                    GL.PushMatrix();
                    hdr.Execute();
                    GL.PopMatrix();
                }
            }
        }
        //private void OnRenderObject()
        //{
        //    CameraPostRender();
        //}
        public static void ClearGL()
        {
            gl.cmds.Clear();
        }
        public static void ClearIM()
        {
            im.cmds.Clear();
        }
        public static void ClearCmd()
        {
            gl.cmds.Clear();
            im.cmds.Clear();
        }
        public static void AddCommand(Cmd cmd)
        {
            cmd.insertOrder = GLUI._insertOrder;
            GLUI._insertOrder++;
            if (cmd.GetType() == typeof(GLCmd))
            {
                gl.cmds.Add(cmd);
            }
            else if (cmd.GetType() == typeof(IMCmd))
            {
                im.cmds.Add(cmd);
            }
            else throw null;
        }
    }
}