using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
namespace Esa
{
    using UnityEditor;
    public class CleanupMissingScript : MonoBehaviour
    {
        public Component[] components;
        [Button]
        void DoCleanUpChilds()
        {
            foreach (var t in GetComponentsInChildren<Transform>(true))
            {
                DoCleanUp(t.gameObject);
            }
        }
        [Button]
        void DoCleanUp()
        {
            DoCleanUp(gameObject);
        }
        void DoCleanUp(GameObject go)
        {
            components = go.GetComponents<Component>();
            var serializedObject = new SerializedObject(go);
            var prop = serializedObject.FindProperty("m_Component");
            for (int i = 0, j = 0; j < components.Length; j++)
            {
                if (components[j] == null)
                {
                    prop.DeleteArrayElementAtIndex(j - i);
                    i++;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif