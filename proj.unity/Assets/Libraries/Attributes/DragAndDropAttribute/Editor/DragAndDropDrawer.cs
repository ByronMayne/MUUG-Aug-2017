using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace MUUG
{
    /// <summary>
    /// A simple inspector that allows us to draw a string field
    /// with support for dragging and dropping assets to get their path. 
    /// Keep in mind the reson all functions take a <see cref="SerializedProperty"/>
    /// is because on instance is shared for many elements. This only really happens
    /// int he case for arrays. So local values are pretty much useless. 
    /// </summary>
    [CustomPropertyDrawer(typeof(DragAndDropAttribute))]
    public class DragAndDropDrawer : PropertyDrawer
    {
        /// <summary>
        /// Invoked every event that is fired can happen multiple times
        /// per frame. 
        /// </summary>
        /// <param name="position">The position on the inspector where we are drawing.</param>
        /// <param name="property">The property we are going to draw.</param>
        /// <param name="label">The default prety label Uniy creates for 
        /// you using <see cref="ObjectNames.NicifyVariableName(string)"/></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                ProcessDropEvent(position, property);
                DrawDragVisual(position);
            }
            else
            {
                EditorGUI.HelpBox(position, "[Invalid Property Target, only valid for string fields]", MessageType.Error);
            }
        }

        /// <summary>
        /// Used to capture any drop events that are happening currently. 
        /// </summary>
        /// <param name="position">The rect that we are working with.</param>
        /// <param name="property">The property we are drawing.</param>
        private void ProcessDropEvent(Rect position, SerializedProperty property)
        {
            // A simple cache
            Event current = Event.current;
            // Are we currently executing completed event?
            if (current.type == EventType.DragPerform)
            {
                // If that event happening inside our position rect?
                if (position.Contains(current.mousePosition))
                {
                    // Accept the event
                    DragAndDrop.AcceptDrag();
                    // Grab the array of dragged objects
                    Object[] draggedObjects = DragAndDrop.objectReferences;
                    // Check if we have any (this would be empty if the user dragged files from outside Unity)
                    if (draggedObjects.Length > 0)
                    {
                        // Get our attribute
                        DragAndDropAttribute dragAndDropAttribute = attribute as DragAndDropAttribute;
                        // Create a holder for our dragged object
                        Object draggedObject = draggedObjects[0];
                        // Get a cache of our dragged type
                        Type assetType = dragAndDropAttribute.assetType;
                        // If it's a game object lets try to get it's component 
                        if(draggedObject is GameObject && typeof(Component).IsAssignableFrom(assetType))
                        {
                            // We know it's a game object so we have to cast it. 
                            GameObject asGameObject = draggedObject as GameObject;
                            // Grab our component and save it as our instance
                            draggedObject = asGameObject.GetComponent(assetType);
                        }
                        // Validate the type
                        if (dragAndDropAttribute.assetType.IsAssignableFrom(draggedObject.GetType()))
                        {
                            // Get it's asset path
                            string assetPath = AssetDatabase.GetAssetPath(draggedObject);
                            // Switch our path
                            if (dragAndDropAttribute.pathType == DragAndDropAttribute.PathType.Resource)
                            {
                                // Convert it to a Resource Path using Unity.IO
                                assetPath = AssetPathUtility.AssetPathToResourcePath(assetPath);
                                // If the path is null our asset is not in the resources folder
                                if (string.IsNullOrEmpty(assetPath))
                                {
                                    // ERROR
                                    string title = "Not a Resources";
                                    string message = "The asset " + draggedObject.name + " is not in a resources folder";
                                    string button = "I Understand";
                                    EditorUtility.DisplayDialog(title, message, button);
                                    return;
                                }
                            }
                            // Set our property value.
                            property.stringValue = assetPath;
                            // Apply the changes to the object.
                            property.serializedObject.ApplyModifiedProperties();
                        }
                        else
                        {
                            // ERROR: Wrong type
                            string title = "Type Mismatch";
                            string message = "The asset " + draggedObject.name + " is is not the correct type of asset. Was expecting " + dragAndDropAttribute.assetType.Name + ".";
                            string button = "I Understand";
                            EditorUtility.DisplayDialog(title, message, button);
                            return;
                        }
                    }
                }
            }
        }

        private void DrawDragVisual(Rect position)
        {
            if (Event.current.type == EventType.DragUpdated)
            {
                if (position.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                }
            }
        }
    }
}