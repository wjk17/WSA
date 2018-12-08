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
            if (!Application.isPlaying && updateInEditor) Start();
        }
        [Button]
        private void Start()
        {
            glHandlers = new List<GLHandler>();
            imHandlers = new List<IMHandler>();
            var wrapper = camera.GetComOrAdd<CameraEventWrapper>();
            wrapper.onPostRender = CameraPostRender;
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
        private void OnGUI()
        {
            if (!Application.isPlaying && !updateInEditor) return;
            foreach (var hdr in imHandlers)
            {
                hdr.Execute();
            }
        }
        private void CameraPostRender()
        {
            GLUI.SetLineMaterial();
            foreach (var hdr in glHandlers)
            {
                hdr.Execute();
            }
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