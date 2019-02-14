using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    using System.IO;
    using UI_;
    using ENC = System.Text.Encoding;
    public enum Encoding
    {
        UTF8,
        UTF7,
        UTF32,
        Unicode,
        BigEndianUnicode,
        ASCII,
        Default
    }
    public static class TextTool
    {
        public static ENC ToENC(this Encoding encoding)
        {
            switch (encoding)
            {
                case Encoding.UTF8: return ENC.UTF8;
                case Encoding.UTF7: return ENC.UTF7;
                case Encoding.UTF32: return ENC.UTF32;
                case Encoding.Unicode: return ENC.Unicode;
                case Encoding.BigEndianUnicode: return ENC.BigEndianUnicode;
                case Encoding.ASCII: return ENC.ASCII;
                case Encoding.Default: return ENC.Default;
                default: throw new Exception();
            }
        }
    }
    public class ScoreLoader : MonoBehaviour
    {
        public string filePath;
        public string folder = @"\Score\";
        public string fileName = ".txt";
        public Encoding encoding = Encoding.UTF8;
        [TextArea(1, 50)] public string text;

        [Button]
        void GetPath()
        {
            filePath = Application.streamingAssetsPath + folder + fileName;
        }

        [Button]
        void Start()
        {
            var enc = encoding.ToENC();
            if (File.Exists(filePath))
            {
                text = File.ReadAllText(filePath, enc);
                //using (StreamReader sr = new StreamReader(filePath, enc))
                //{
                //    text = sr.ReadToEnd();
                //    byte[] mybyte = enc.GetBytes(text);
                //    text = enc.GetString(mybyte);
                //}
            }
            //print(text);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}