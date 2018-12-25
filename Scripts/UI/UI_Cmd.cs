using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public partial class UI // Commands
    {
        [Header("Command")]
        public bool updateInEditor;
        public List<GLHandler> glHandlers;
        public List<IMHandler> imHandlers;
        public new Camera camera;
        public static GLHandler gl;
        public static IMHandler im;

        public RectTransform Owner; // for inspect
        private static RectTransform _owner;

        public static RectTransform owner
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
                print(gl.owner.name);
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
            gameViewSize = screenSize;
            if (!Application.isPlaying && !updateInEditor) return;
#endif
            _mousePosRef = mousePosRef; // for inspect in editor & runtime;
            Owner = owner;
            foreach (var hdr in imHandlers)
            {
                if (hdr.owner.gameObject.activeInHierarchy)
                    hdr.Execute();
            }
        }
        private void CameraPostRender()
        {
            Owner = owner;
            GLUI.SetLineMaterial();
            foreach (var hdr in glHandlers)
            {
                if (hdr.owner.gameObject.activeInHierarchy)
                    hdr.Execute();
            }
        }
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