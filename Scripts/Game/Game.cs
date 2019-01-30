using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    using UI_;
    public class Game : Singleton<Game>
    {
        public static bool pause; // 播放技能动画时使用
        public static float totalTime;
        public List<TilePlacer> tiles
        {
            get { return TileSceneMgr.I.tiles; }
        }
        public override void _Awake()
        {
            base._Awake();
            pause = false;
        }
    }
}