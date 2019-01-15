using System;
using UnityEngine;
namespace Esa._UI
{
    [ExecuteInEditMode]
    public class CameraEventWrapper : MonoBehaviour
    {
        public Action onRenderObject;
        public Action onPostRender;
        public Action onWillRenderObject;
        public Action<RenderTexture, RenderTexture> onRenderImage;
        public Action onPreRender;
        public Action onPreCull;
        public bool debug;
        public bool updateInEditor = true;
        //public void OnRenderObject()
        //{
        //    if (!Application.isPlaying && !updateInEditor) return;
        //    if (onRenderObject != null) { if (debug) print("OnRenderObject"); onRenderObject(); }
        //}
        void OnPostRender()
        {
            if (!Application.isPlaying && !updateInEditor) return;
            if (onPostRender != null) { if (debug) print("OnPostRender"); onPostRender(); }
        }
        //private void OnWillRenderObject()
        //{
        //    if (onWillRenderObject != null) onWillRenderObject();
        //}
        //private void OnRenderImage(RenderTexture source, RenderTexture destination)
        //{
        //    if (onRenderImage != null) onRenderImage(source, destination);
        //    else Graphics.Blit(source, destination);
        //}
        //private void OnPreRender()
        //{
        //    if (onPreRender != null) onPreRender();
        //}
        //private void OnPreCull()
        //{
        //    if (onPreCull != null) onPreCull();
        //}
    } }