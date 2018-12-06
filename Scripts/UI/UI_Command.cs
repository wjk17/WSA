using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public static class Command_Tool
    {
        public static bool Contains<T>(this List<T> list, RectTransform item) where T : CommandHandler
        {
            foreach (T i in list)
            {
                if (i.owner == item) return true;
            }
            return false;
        }
        public static T Ele<T>(this List<T> list, RectTransform item) where T : CommandHandler
        {
            foreach (T i in list)
            {
                if (i.owner == item) return i;
            }
            return null;
        }
        public static bool Contains(this List<CommandHandler> list, RectTransform item)
        {
            foreach (CommandHandler i in list)
            {
                if (i.owner == item) return true;
            }
            return false;
        }
    }
    public partial class UI // Commands
    {
        public static List<GLUIHandler> glHandlers = new List<GLUIHandler>();
        public static List<IMUIHandler> imHandlers = new List<IMUIHandler>();
        public new Camera camera;
        private static RectTransform _owner;
        public static RectTransform owner
        {
            set
            {
                _owner = value;
                if (!glHandlers.Contains(_owner))
                {
                    glHandlers.Add(new GLUIHandler(_owner));
                }
                if (!imHandlers.Contains(_owner))
                {
                    imHandlers.Add(new IMUIHandler(_owner));
                }
            }
            get { return _owner; }
        }
        private void Reset()
        {
            camera = Camera.main;
            if (camera == null) camera = FindObjectOfType<Camera>();
        }
        private void Start()
        {
            var wrapper = camera.GetComOrAdd<CameraEventWrapper>();
            wrapper.onPostRender = CameraPostRender;
        }
        private void OnGUI()
        {
            foreach (var hdl in imHandlers)
            {
                hdl.Execute();
            }
        }
        private void CameraPostRender()
        {
            GLUI.SetLineMaterial();
            foreach (var hdl in glHandlers)
            {
                hdl.Execute();
            }
        }
        public static void ClearCmd()
        {
            glHandlers.Ele(owner).commands.Clear();
            imHandlers.Ele(owner).commands.Clear();
        }
        public static void AddCommand(Command command)
        {
            if (command.GetType() == typeof(GLUICommand))
            {
                glHandlers.Ele(owner).commands.Add(command);
            }
            else if (command.GetType() == typeof(IMUICommand))
            {
                imHandlers.Ele(owner).commands.Add(command);
            }
            else throw null;
        }
    }
}