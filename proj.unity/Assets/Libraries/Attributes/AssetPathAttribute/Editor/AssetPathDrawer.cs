using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MUUG
{
    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    public class AssetPathDrawer : PropertyDrawer
    {
        public AssetPathDrawer()
        {
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}