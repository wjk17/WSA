using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public struct Bool2
{
    public bool bool1;
    public string bool2Label;
    public bool bool2;
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Bool2))]
public class Bool2Drawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var b1 = new Rect(position.x, position.y, 30, position.height);
        var b2Name = new Rect(position.x + 35, position.y, 50, position.height);
        var b2 = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(b1, property.FindPropertyRelative("bool1"), GUIContent.none);
        EditorGUI.LabelField(b2Name, property.FindPropertyRelative("bool2Label").stringValue);
        EditorGUI.PropertyField(b2, property.FindPropertyRelative("bool2"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif