using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Esa.UI_;
namespace Esa
{
    [ExecuteInEditMode]
    public class TilePlacer_Bat : MonoBehaviour
    {
        TilePlacer o { get { return GetComponent<TilePlacer>(); } }
        public int cutAboveNum = 6;
        public int fillId = 0;
        public bool fillReplace;
        public bool dulicateReplace;
        public bool dulicateEmpty;
        [Header("Replace")]
        public int from = 0;
        public int to = 0;

        public Vector3Int boundA { get { return o.poss[0]; } }
        public Vector3Int boundB { get { return o.poss[1]; } }

        public Vector3Int min { get { return Vector3Int.Min(boundA, boundB); } }
        public Vector3Int max { get { return Vector3Int.Max(boundA, boundB); } }

        public Vector3Int os;

        [Button]
        void Duplicate()
        {
            var listId = new List<int>();
            var grid = o.gridTerrain;
            // get id list
            for (int z = min.z; z <= max.z; z++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int x = min.x; x <= max.x; x++)
                    {
                        var unit = grid[new Vector3Int(x, y, z)];
                        listId.Add(unit == null ? -1 : unit.id);
                    }
                }
            }
            // duplicate to os
            var _min = min + os;
            var _max = max + os;
            int i = 0;
            for (int z = _min.z; z <= _max.z; z++)
            {
                for (int y = _min.y; y <= _max.y; y++)
                {
                    for (int x = _min.x; x <= _max.x; x++)
                    {
                        var pos = new Vector3Int(x, y, z);
                        if (listId[i] == -1) { if (dulicateEmpty) o.EmptyGrid(pos); }
                        else o.FillGrid(pos, listId[i], dulicateReplace);
                        i++;
                    }
                }
            }
            grid.Arrange();
        }
        [Button]
        void Replace()
        {
            foreach (var unit in o.gridTerrain.units)
            {
                if (unit.id == from)
                {
                    ComTool.DestroyAuto(unit.go);
                    unit.id = to;
                    unit.go = Instantiate(o.prefabs[to].go, o.poolGrid, true);
                }
            }
            o.gridTerrain.Arrange();
        }
        [Button]
        void Fill()
        {
            var list = new List<GridUnit>();
            var grid = o.gridTerrain;
            for (int z = min.z; z <= max.z; z++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int x = min.x; x <= max.x; x++)
                    {
                        var pos = new Vector3Int(x, y, z);
                        if (fillId == -1) o.EmptyGrid(pos);
                        else o.FillGrid(pos, fillId, fillReplace);
                    }
                }
            }
            grid.Arrange();
        }
        [Button]
        void Move()
        {
            var list = new List<GridUnit>();
            var grid = o.gridTerrain;
            for (int z = min.z; z <= max.z; z++)
            {
                for (int y = min.y; y <= max.y; y++)
                {
                    for (int x = min.x; x <= max.x; x++)
                    {
                        list.Add(grid[new Vector3Int(x, y, z)]);
                    }
                }
            }
            foreach (var unit in list)
            {
                if (unit != null) unit.pos += os;
            }
            grid.Arrange();
        }
        [Button]
        private void CutAbove()
        {
            foreach (var unit in o.gridTerrain.units)
            {
                if (unit.pos.y > cutAboveNum)
                {
                    ComTool.DestroyAuto(unit.go);
                    unit.go = null;
                    unit.id = -1;
                }
            }
        }
    }
}