using UnityEngine;
using UnityEditor;

namespace MUUG
{
    public static class AssetPathUtility
    {
        private const string COPY_ASSETPATH_PATH = "Assets/Copy/Asset Path";
        private const string COPY_RESOURCES_PATH = "Assets/Copy/Resource Path";

        /// <summary>
        /// Lets the user copy the asset path of a selected asset.
        /// </summary>
        [MenuItem(COPY_ASSETPATH_PATH)]
        public static void CopyAssetPath()
        {
            // Get our active object
            Object selectedObject = Selection.activeObject;
            // Get it's Asset Path
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // Set our clipboard value (this function should not exists in a GUI class)
            EditorGUIUtility.systemCopyBuffer = assetPath;
        }

        /// <summary>
        /// Lets the user right click to copy a path of
        /// an asset in the resources folder.
        /// </summary>
        [MenuItem(COPY_RESOURCES_PATH)]
        public static void CopyResourcePath()
        {
            // Get our active object
            Object selectedObject = Selection.activeObject;
            // Get it's Asset Path
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // Convert out path
            string resourcePath = AssetPathToResourcePath(assetPath);
            // Set our clipbaord
            EditorGUIUtility.systemCopyBuffer = resourcePath;
        }

        /// <summary>
        /// Make sure that we show the menu item disabled
        /// if the user has nothing selected.
        /// </summary>
        [MenuItem(COPY_ASSETPATH_PATH, isValidateFunction: true)]
        public static bool CopyAssetPathValidate()
        {
            return Selection.activeObject != null;
        }

        /// <summary>
        /// Make sure that we show the menu item disabled
        /// if the user has nothing selected or the selected
        /// object is not in a resources folder
        /// </summary>
        [MenuItem(COPY_RESOURCES_PATH, isValidateFunction: true)]
        public static bool CopyResourcePathValidate()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            // Get our active object
            Object selectedObject = Selection.activeObject;
            // Get it's Asset Path
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            // Convert out path
            string resourcePath = AssetPathToResourcePath(assetPath);
            // Return if we have a path
            return !string.IsNullOrEmpty(resourcePath);
        }

        public static string AssetPathToResourcePath(string assetPath)
        {
            const string RESOURCES_FOLDER = "Resources";
            string result = assetPath;
            if(!string.IsNullOrEmpty(assetPath))
            {
                int startingIndex = assetPath.IndexOf(RESOURCES_FOLDER);
                
                if(startingIndex >= 0)
                {
                    startingIndex += RESOURCES_FOLDER.Length;
                    int length = assetPath.Length - startingIndex;
                    result = assetPath.Substring(startingIndex, length);
                }
            }

            return result;
        }
    }
}