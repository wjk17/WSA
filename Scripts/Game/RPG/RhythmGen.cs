using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.Music
{
    public class RhythmGen : MonoBehaviour
    {
        public int count = 10;
        public Vector2Int range;
        public List<int> nums;
        public string numsStr;
        [Button]
        void Start()
        {
            nums = new List<int>();
            for (int i = 0; i < count; i++)
            {
                nums.Add(range.x + Mathf.RoundToInt((range.y - range.x) * Random.value));
            }
            numsStr = nums.ToStrList().ToStr();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}