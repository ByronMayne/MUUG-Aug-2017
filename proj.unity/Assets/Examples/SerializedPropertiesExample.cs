using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class Human : MonoBehaviour
{
    public string name;

    [SerializeField]
    private int m_Age;
    
    [SerializeField]
    protected internal Shirt m_Shirt;
}

[Serializable]
public class Shirt
{
    [SerializeField]
    protected Color m_Color;
}


public class SerializedPropertiesUsage : Editor
{
    public void Do()
    {
        Human human = new Human();
        SerializedObject serializedObject = new SerializedObject(obj: human);

        SerializedProperty name = serializedObject.FindProperty("name");
        SerializedProperty age = serializedObject.FindProperty("m_Age");
        SerializedProperty shirt = serializedObject.FindProperty("m_Shirt");

        SerializedProperty color = shirt.FindPropertyRelative("m_Color");
    }
}
