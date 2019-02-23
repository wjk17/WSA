using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [ExecuteInEditMode]
    public partial class TilePlacer : MonoBehaviour
    {
        public GridSized gridSelection; // 选中的网格空间
        public GridSized gridTerrain; // 地形网格（5号尺寸）
        public Vector3Int pos;
        public Vector3Int[] poss;
        public int id;
        public int maxPosY = 1;

        public List<GridUnit> prefabs; // 预置
        public Vector3Int lastPos;
        public bool replace;
        public bool select;
        public string folder = "Grid/";
        public string fileName = "City1.grid";
        public float eulerStart;
        public float speed = 1f;
        public bool dragAsClick = true;
        public string path { get { return System.IO.Path.Combine(Application.streamingAssetsPath, folder) + fileName; } }

        [Button]
        private void UpdateList()
        {
            var ps = poolPrefab.GetChildsL1();
            for (int i = 0; i < ps.Count; i++)
            {
                if (i >= prefabs.Count)
                {
                    prefabs.Add(new GridUnit(i, Vector3Int.zero, ps[i].gameObject));
                }
                else prefabs[i].go = ps[i].gameObject;
            }
        }
        [Button]
        void Clear()
        {
            poolGrid.ClearChildren();
            gridTerrain.Clear();
        }
        [Button]
        void Save()
        {
            // Clear deleted unit
            var n = new List<GridUnit>();
            foreach (var u in gridTerrain.units)
            {
                if (u.id > -1) n.Add(u);
            }
            gridTerrain.units = n;
            Serializer.XMLSerialize(gridTerrain, path);
        }
        [Button]
        void Load()
        {
            Clear();
            Serializer.XMLDeSerialize(out gridTerrain, path);
            UpdateGridGO();
        }
        bool MissPrefab()
        {
            foreach (var prefab in prefabs)
            {
                if (prefab.go == null) return true;
            }
            return false;
        }
        [Button]
        public void UpdateGOs()
        {
            poolGrid.ClearChildren();
            UpdateGridGO();
        }
        public void UpdateGridGO()
        {
            if (MissPrefab()) UpdateList();
            foreach (var u in gridTerrain.units)
            {
                // 不检测，使用前先Clear()
                //if (u.go != null) { ComTool.DestroyAuto(u.go); u.go = null; }
                if (u.id > -1)
                    u.go = Instantiate(prefabs[u.id].go, poolGrid, true);
            }
            Arrange();
        }
        [Button]
        private void Arrange()
        {
            gridTerrain.Arrange();
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            Raycast();
            var dir = KeyMgr.dir;
            if (dir > -1)
            {
                var t = Camera.main.transform.parent;
                var rot = eulerStart + dir * -45f;
                var v = Quaternion.AngleAxis(rot, Vector3.up) * Vector3.forward;
                t.Translate(v * speed, Space.Self);
            }
        }
        private void OnGUI()
        {
            if (!Application.isPlaying) Raycast();
        }
        private void Raycast()
        {
            RaycastHit hit;
            var _hit = RaycastTool.SVRaycast(out hit, LayerMasks.Grid);
            if (_hit)
            {
                pos = hit.transform.position.DivideToInt(gridTerrain.gridUnitSize);
                pos = gridTerrain[pos].pos;
            }
            else
            {
                _hit = RaycastTool.SVRaycast(out hit, LayerMasks.Terrain);
                if (_hit)
                {
                    pos = hit.point.DivideToInt(gridTerrain.gridUnitSize);
                    pos.y = -1;
                }
            }
            if (_hit)
            {
                if (select)
                {
                    if (Events.MouseDown0) poss[0] = pos;
                    else if (Events.MouseDown1) poss[1] = pos;
                }
                else if (Events.MouseDown0 || (Events.Mouse0 && dragAsClick))
                {
                    if (lastPos == pos && !Events.MouseDown0) return;
                    if (replace)
                    {
                        if (pos.y == -1) return;
                        EmptyGrid(pos);
                    }
                    else
                    {
                        if (pos.y >= maxPosY) return;
                        pos.y++;
                    }
                    FillGrid(pos);
                }
                else if (Events.MouseDown1 || (Events.Mouse1 && dragAsClick))
                {
                    if (lastPos == pos && !Events.MouseDown1) return;
                    if (pos.y > -1)
                        EmptyGrid(pos);
                }
            }
        }
        public void EmptyGrid(Vector3Int pos)
        {
            var unit = gridTerrain[pos];
            if (unit != null)
            {
                lastPos = pos;
                lastPos.y--;
                gridTerrain.Remove(pos);
                ComTool.DestroyAuto(unit.go);
            }
        }
        void FillGrid(Vector3Int pos)
        {
            FillGrid(pos, id);
        }
        public void FillGrid(Vector3Int pos, int id, bool replace)
        {
            var unit = gridTerrain[pos];
            if (unit == null)
            {
                if (id >= prefabs.Count) UpdateList();
                if (id >= prefabs.Count) { print("Invalid Tile id."); return; }

                gridTerrain.Add(newGrid(id, pos));
            }
            else if (replace)
            {
                ComTool.DestroyAuto(unit.go);
                unit.id = id;
                unit.go = Instantiate(prefabs[id].go, poolGrid, true);
            }
        }
        public void FillGrid(Vector3Int pos, int id)
        {
            var unit = gridTerrain[pos];
            if (unit == null)
            {
                if (id >= prefabs.Count) UpdateList();
                if (id >= prefabs.Count) { print("Invalid Tile id."); return; }
                lastPos = pos;
                gridTerrain.Add(newGrid(id, pos));
            }
        }
        GridUnit newGrid(int id, Vector3Int pos)
        {
            var u = new GridUnit(id, pos);
            u.go = Instantiate(prefabs[id].go, poolGrid, true);
            return u;
        }
    }
}