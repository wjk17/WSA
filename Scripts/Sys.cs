using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa
{
    public class SYS : MonoBehaviour
    {
        public static float Fps
        {
            get { return fps; }
            set { fps = value; }
        }
        static float fps;
        public static float Tpf // timePerFrame
        {
            get { return 1 / Fps; }
        }
        public static bool debugAnime;
        public static bool debugUI;
        public static bool debugSingleton;
        public static bool debugTrigger;
        public bool _debugTrigger;
        public static bool debugDepot;
        public bool _debugDepot;
        public bool debug;

        private void Awake()
        {
            Update();
            if (debug) print("SYS Init");

            SingletonMgr.Init();
            UI.UI.I.Initialize();
        }
        private void Update()
        {
            debugSingleton = debugUI = debugAnime = debug;
            debugDepot = _debugDepot;
            debugTrigger = _debugTrigger;
        }

        public static void ShowMsg(string v)
        {
            UIBlock.I.GetComChild<Text>().text = v;
            UIBlock.I.BlockUI(true);
        }
    }
}