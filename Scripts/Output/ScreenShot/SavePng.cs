using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Esa.UI
{
    public class SavePng : MonoBehaviour
    {
        bool grab;
        public Vector3 oPos;
        public float os = 10f;
        private void OnPreRender()
        {

            if (grab)
            {
                oPos = transform.parent.position;
                transform.parent.Translate(Vector3.forward * os, Space.Self);
            }
        }
        private void OnPostRender()
        {
            if (grab)
            {
                Grab();
                transform.parent.position = oPos;

                grab = false;
            }

        }
        [Button]
        public void SavePNG()
        {
            grab = true;
        }

        void Grab()
        {
            // Create a texture the size of the screen, RGB24 format
            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // Read screen contents into the texture
            tex.ReadPixels(new UnityEngine.Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            byte[] bytes = tex.EncodeToPNG();

            if (Application.isPlaying)
                Destroy(tex);
            else DestroyImmediate(tex);

            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
        }
    }
}