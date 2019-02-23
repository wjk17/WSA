using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class ProcGrass : MonoBehaviour
    {
        public Transform[] bones;

        [Button]
        void GenerateMesh(Transform rig)
        {
            RigData rd = new RigData();
            RigTool.CreateRig(rd, rig, out bones);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}