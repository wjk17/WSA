using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class CharMgr : Singleton<CharMgr>
    {
        public List<Char> chars;
        // Use this for initialization
        public static Char NewChar(int idx)
        {
            var go = Instantiate(I.chars[idx].gameObject);
            go.Active();
            go.GetComponent<CharCtrl_Auto>().Init();
            return go.GetComponent<Char>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}