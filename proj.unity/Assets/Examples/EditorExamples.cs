using UnityEditor;
using UnityEngine;


// Editor Window
public class UserEditorWindow : EditorWindow
{
    private void OnGUI()
    {
        // It's drawing function
    }
}

// Inspector 
[CustomEditor(typeof(object))]
public class UserInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // It's drawing function
        base.OnInspectorGUI();
    }
}

// Property Drawer
[CustomPropertyDrawer(typeof(object))]
public class UserPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}