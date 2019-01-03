using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa
{
    public static class SYSTool
    {
        public static void StartCo(this IEnumerator e)
        {
            UI.UI.I.StartCoro(e);
        }
    }
    public class LayerMasks
    {
        public static string TerrainName = "Terrain";
        public static LayerMask Terrain { get { return LayerMask.GetMask(TerrainName); } }
        public static int TerrainNum { get { return (int)Mathf.Log(Terrain.value, 2); } }


        public static string GridName = "Grid";
        public static LayerMask Grid { get { return LayerMask.GetMask(GridName); } }
        public static int GridNum { get { return (int)Mathf.Log(Grid.value, 2); } }
    }
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