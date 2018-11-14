using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class MonoXmlSL<T> : MonoBehaviour
{
    [Header("ReadOnly")]
    public string _path;
    public string path
    {
        get
        {
            _path = System.IO.Path.Combine(Application.streamingAssetsPath, folder + fileName);
            return _path;
        }
    }
    public string folder = "folder/";
    public string fileName = "file.xml";
    public T data;
    [Button]
    public void Save()
    {
        Serializer.XMLSerialize(data, path);
    }
    [Button]
    public void Load()
    {
        data = Serializer.XMLDeSerialize<T>(path);
    }
}
