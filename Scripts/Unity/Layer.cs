using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomPropertyDrawer(typeof(Layer))]
public class LayerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty layerProp = property.FindPropertyRelative("m_layer");
        layerProp.intValue = EditorGUI.LayerField(position, label, layerProp.intValue);
    }    
}
#endif




[Serializable]
public struct Layer
{
    //void a()
    //{
    //    int layerNumber = 0;
    //    int layer = myLayer.value;
    //    while (layer > 0)
    //    {
    //        layer = layer >> 1;
    //        layerNumber++;
    //    }
    //    return layerNumber;
    //}


//    Ntero said: ↑
//You could do something like this:

//Code(csharp):


//int layerNumber = 0;
//    int layer = myLayer.value;
//while(layer > 0)
//{
//    layer = layer >> 1;
//    layerNumber++;
//}
//return layerNumber;
 
//Essentially it just counts the number of bitshifts to get clear the flag.And unless you have multiple bit flags active it should return 20, to your 1 << 20.
//Also in Unity 5 (as far as I know, havent checked the unity 4), the layer index start from 0, so you might wanna do
//Code(csharp):
//return layerNumber - 1;
//to get correct result
//or
//Code(csharp):
//while(layer > 1)


    [SerializeField]
    int m_layer;

    public int Index { get { return m_layer; } set { m_layer = value; } }
    public int Mask { get { return 1 << m_layer; } }
    public string Name
    {
        get { return LayerMask.LayerToName(m_layer); }
        set { m_layer = LayerMask.NameToLayer(value); }
    }

    public static implicit operator int(Layer l)
    {
        return l.Index;
    }

    public static implicit operator Layer(int i)
    {
        return new Layer() { Index = i };
    }

    public Layer(int index)
    {
        m_layer = index;
    }
    public Layer(string name)
    {
        m_layer = LayerMask.NameToLayer(name);
    }
}