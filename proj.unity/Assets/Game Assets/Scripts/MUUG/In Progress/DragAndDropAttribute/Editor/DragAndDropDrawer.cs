using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace MUUG
{
    [CustomPropertyDrawer(typeof(DragAndDropAttribute))]
    public class DragAndDropDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}