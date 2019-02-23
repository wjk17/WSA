using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class TileSceneMgr : Singleton<TileSceneMgr>
    {
        public List<TilePlacer> tiles;
        [Button]
        void GetTiles()
        {
            GetComponentsInChildren(true, tiles);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}