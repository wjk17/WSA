using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Esa;
public class DOFMgr : MonoXmlSL<DOFData>
{
    public DOF this[Bone bone]
    {
        get { return GetDOF(bone); }
    }
    public DOF GetDOF(Bone bone)
    {
        return data.GetDOF(bone);
    }
    private void Reset()
    {
        folder = "DOF/";
        fileName = "Default.xml";
        Load();
    }
}
