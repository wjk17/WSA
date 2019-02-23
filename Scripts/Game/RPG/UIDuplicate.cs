using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    [ExecuteInEditMode]
    public class UIDuplicate : MonoBehaviour
    {
        public string[] txts;
        public Vector2 offset = new Vector2(0, 20);
        public Vector2 factor = new Vector2(0, 1);
        public bool updateInEditor = true;
        public bool updateImmd;
        public bool updateOnStart = true;
        public Transform prefab;
        [Button]
        void Start()
        {
            if (updateOnStart) Duplicate();
        }
        private void Update()
        {
            if ((updateInEditor && !Application.isPlaying) || updateImmd)
            {
                Duplicate();
            }
        }
        void Duplicate()
        {
            prefab = transform.GetChild(0);
            var cs = transform.GetChildsL1();
            for (int c = 1; c < cs.Count; c++)
            {
                cs[c].SetParent(null, false);
                ComTool.DestroyAuto(cs[c].gameObject);
            }
            var os = offset + (prefab as RectTransform).rect.size;
            int i = 0;
            foreach (var txt in txts)
            {
                if (i == 0)
                {
                    i++;
                    prefab.GetComponentInChildren<Text>().text = txt;
                    continue;
                }

                var t = Instantiate(prefab, transform, true);
                t.GetComponentInChildren<Text>().text = txt;

                var rt = (t as RectTransform);
                rt.anchoredPosition += os * i++ * factor;
            }
        }
    }
}