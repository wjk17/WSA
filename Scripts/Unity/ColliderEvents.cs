using System;
using UnityEngine;
namespace Esa
{
    /// <summary>
    /// TODO 处理RaycastAll：单次点击击中多个碰撞体时轮流切换
    /// </summary>
    public class ColliderEvents : MonoBehaviour
    {
        public Action onRaycastHit;
        public Action onUpdate;
        public float boxSize
        {
            set
            {
                var box = this.GetComOrAdd<BoxCollider>();
                box.size = Vector3.one * value;
            }
        }
        private void Update()
        {
            onUpdate();
        }
        public void OnRaycastHit()
        {
            if (onRaycastHit != null) onRaycastHit();
        }
    }
}