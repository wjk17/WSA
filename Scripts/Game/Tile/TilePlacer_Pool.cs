using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public partial class TilePlacer // Pool
    {
        public Transform poolSelection
        {
            get
            {
                if (_poolSelection == null)
                {
                    var p = transform.Search("_poolSelection");
                    if (p == null) p = new GameObject("_poolSelection").transform;
                    p.SetParent(transform);
                    p.ResetTransform();
                    _poolSelection = p;
                }
                return _poolSelection;
            }
        }
        Transform _poolSelection;
        public Transform poolGrid
        {
            get
            {
                if (_poolGrid == null)
                {
                    var p = transform.Search("_poolGrid");
                    if (p == null) p = new GameObject("_poolGrid").transform;
                    p.SetParent(transform);
                    p.ResetTransform();
                    _poolGrid = p;
                }
                return _poolGrid;
            }
        }
        Transform _poolGrid;
        public Transform poolPrefab
        {
            get
            {
                if (_poolPrefab == null)
                {
                    var p = transform.Search("_poolPrefab");
                    if (p == null) p = new GameObject("_poolPrefab").transform;
                    p.SetParent(transform);
                    p.ResetTransform();
                    _poolPrefab = p;
                }
                return _poolPrefab;
            }
        }
        Transform _poolPrefab;
    }
}