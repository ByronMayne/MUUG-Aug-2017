using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MUUG
{
    /// <summary>
    /// Tells Unity that this is the editor for any field
    /// with an <see cref="AssetPathAttribute"/> above it.
    /// </summary>
    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    public class AssetPathDrawer : PropertyDrawer
    {
        /// <summary>
        /// A map used to keep track of our object
        /// references so we don't have to keep loading them
        /// every frame.
        /// </summary>
        private IDictionary<string, Object> m_InstanceMap;

        /// <summary>
        /// The constructor is used to set up our fields
        /// </summary>
        public AssetPathDrawer()
        {
            m_InstanceMap = new Dictionary<string, Object>();
        }

        /// <summary>
        /// Used to tell Unity how much space we will need to draw our
        /// drawer. In this case we just need the default height.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        /// <summary>
        /// Invoked every event that happens. We use this to draw our drawer. No local
        /// variables should be used since this instance is shared.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Make sure we are above a string
            if (property.propertyType == SerializedPropertyType.String)
            {
                // The propertyPath will always be unique it's the path
                // the serializer has to go to find the value. Example 'player.stats.level'
                string propertyPath = property.propertyPath;
                // The asset path we are working with.
                string assetPath = property.stringValue;
                // A holder for our instance
                Object instance = null;
                // We need our attribute to get the type of the field
                AssetPathAttribute assetPathAttribute = attribute as AssetPathAttribute;

                // Check if we have key for the instance and if we don't create a null one.
                if (!m_InstanceMap.ContainsKey(propertyPath))
                {
                    // Create a new value in our map
                    m_InstanceMap.Add(propertyPath, null);
                    // Do we have any path set?
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        // We do so lets try to load it
                        if (assetPathAttribute.pathType == AssetPathAttribute.PathType.Asset)
                        {
                            // Load the instance with the asset database
                            instance = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
                        }
                        else if (assetPathAttribute.pathType == AssetPathAttribute.PathType.Resource)
                        {
                            instance = Resources.Load(assetPath);
                        }

                        // Check if we were able to load anything.
                        if (instance == null)
                        {
                            // ERROR: Missing asset
                        }
                        else
                        {
                            // Validate it's type
                            if (assetPathAttribute.assetType.IsAssignableFrom(instance.GetType()))
                            {
                                // Set our instance map
                                m_InstanceMap[propertyPath] = instance;
                            }
                            else
                            {
                                // ERROR: Type mismatch   
                            }
                        }
                    }
                }
                else
                {
                    // Load the value in the instance map
                    instance = m_InstanceMap[propertyPath];
                }

                // We only want to do work if we have a change
                EditorGUI.BeginChangeCheck();
                {
                    instance = EditorGUI.ObjectField(position, label, instance, assetPathAttribute.assetType, false);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    // Did they clear the refernce?
                    if (instance == null)
                    {
                        // Clear our path/
                        property.stringValue = string.Empty;
                    }
                    else
                    {
                        // They assigned a value so we want to get the path
                        string newAssetPath = AssetDatabase.GetAssetPath(instance);
                        // Set our path based on the value defined in our attribute
                        if (assetPathAttribute.pathType == AssetPathAttribute.PathType.Asset)
                        {
                            property.stringValue = newAssetPath;
                        }
                        else if (assetPathAttribute.pathType == AssetPathAttribute.PathType.Resource)
                        {
                            // We have to convert our path
                            string resourcePath = AssetPathUtility.AssetPathToResourcePath(newAssetPath);
                            // If it's empty it's not in resources
                            if (string.IsNullOrEmpty(resourcePath))
                            {
                                // ERROR: Asset not in resources folder
                            }
                            else
                            {
                                // Assing the value
                                property.stringValue = resourcePath;
                            }
                        }
                    }
                    // Set our new object to our instance map.
                    m_InstanceMap[propertyPath] = instance;
                }
            }
            else
            {
                // Wrong field type
                EditorGUI.HelpBox(position, "[Invalid Property Target]", MessageType.Error);
            }
        }
    }
}