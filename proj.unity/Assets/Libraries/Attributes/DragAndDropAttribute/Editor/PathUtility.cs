using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathUtility
{
    public const char PATH_SPLITTER = '/';
    public const string ROOT_FOLDER_NAME = "Assets";
    public const string RESOURCE_FOLDER_NAME = "/Resources/";
    private const string COPY_ASSETPATH_PATH = "Assets/Copy/Asset Path";
    private const string COPY_RESOURCES_PATH = "Assets/Copy/Resource Path";

    //[MenuItem(COPY_ASSETPATH_PATH)]
    public static void CopyAssetPath()
    {
    }

    //[MenuItem(COPY_RESOURCES_PATH)]
    public static void CopyResourcePath()
    {
    }

    //[MenuItem(COPY_ASSETPATH_PATH, isValidateFunction: true)]
    public static bool CopyAssetPathValidate()
    {
        throw new System.NotImplementedException();
    }

    //[MenuItem(COPY_RESOURCES_PATH, isValidateFunction: true)]
    public static bool CopyResourcePathValidate()
    {
        throw new System.NotImplementedException();
    }



    /// <summary>
    /// Converts a a full system path to a local asset path. 
    /// <example>
    /// Input (SystemPath): C:/Users/Projects/MyProject/Asset/Images/Bear.png
    /// Output  (AssetPath) : Assets/Images/Bear.png
    /// </example>
    /// </summary>
    /// <param name="systemPath"></param>
    /// <returns></returns>
    public static string SystemToAssetPath(string systemPath)
    {
        string assetPath = string.Empty;

        // We have to make sure we are not working on a useless string. 
        if (string.IsNullOrEmpty(systemPath))
        {
            throw new System.ArgumentNullException("SystemPath", "The asset path that was sent in was null or empty. Can not convert");
        }

        // Make sure we are in the right directory
        if (!systemPath.StartsWith(Application.dataPath))
        {
            throw new System.InvalidOperationException(string.Format("The path {0} does not start with {1} which is our current directory. This can't be converted", systemPath, Application.dataPath));
        }

        // Get the number of chars in our system directory path
        int systemDirNameLength = Application.dataPath.Length;

        // Subtract the name of the asset folder since we want to keep that part. 
        systemDirNameLength -= ROOT_FOLDER_NAME.Length;

        // Get the count of the number of letter left in our path starting from '/Assets'
        int assetDirNameLength = systemPath.Length - systemDirNameLength;

        // Substring to get our result. 
        assetPath = systemPath.Substring(systemDirNameLength, assetDirNameLength);

        // Return
        return assetPath;
    }

    /// <summary>
    /// Takes a Unity asset path and converts it to a system path.
    /// <example>
    /// Input  (AssetPath) : Assets/Images/Bear.png
    /// Output (SystemPath): C:/Users/Projects/MyProject/Asset/Images/Bear.png
    /// </example>
    /// </summary>
    /// <param name="assetPath">The Unity asset path you want to convert.</param>
    /// <returns>The newly created system path.</returns>
    public static string AssetPathToSystemPath(string assetPath)
    {
        // A local holder for our working path. 
        string systemPath = string.Empty;

        // We have to make sure we are not working on a useless string. 
        if (string.IsNullOrEmpty(assetPath))
        {
            throw new System.ArgumentNullException("AssetPath", "The asset path that was sent in was null or empty. Can not convert");
        }

        // Get the index of 'Asset/' part of the path 
        if (!assetPath.StartsWith(ROOT_FOLDER_NAME + PATH_SPLITTER))
        {
            // This is returned by Unity and we must have it to convert
            throw new System.InvalidOperationException(string.Format("Can't convert '{0}' to a System Path since it does not start with '{1}'", assetPath, ROOT_FOLDER_NAME + PATH_SPLITTER));
        }

        // Get our starting length.
        int pathLength = assetPath.Length;

        // Now subtract the root folder name 
        pathLength -= ROOT_FOLDER_NAME.Length;

        // Now get the substring
        assetPath = assetPath.Substring(ROOT_FOLDER_NAME.Length, pathLength);

        // Combine them
        systemPath = Application.dataPath + assetPath;

        // Return the result
        return systemPath;
    }

    /// <summary>
    /// Taking in a Unity Asset Path we get out the resource
    /// path if it's in one or an empty string.
    /// </summary>
    public static string AssetPathToResourcePath(string assetPath)
    {
        // Make sure it's not empty
        if (!string.IsNullOrEmpty(assetPath))
        {
            // Get the starting index in the string
            int resourceIndex = assetPath.LastIndexOf(RESOURCE_FOLDER_NAME);
            // If it's -1 it does not exist. 
            if (resourceIndex > -1)
            {
                // Increase our index by the length
                resourceIndex += RESOURCE_FOLDER_NAME.Length;
                // Get the new length of the string.
                int resourcePathLength = assetPath.Length - resourceIndex;
                // Get the extension index
                int extensionIndex = assetPath.Length - assetPath.LastIndexOf('.');
                // Calculate the new length
                int length = resourcePathLength - extensionIndex;
                // Split out the part that we need.
                assetPath = assetPath.Substring(resourceIndex, length);
                // Return the result.
                return assetPath;
            }
        }

        return "";
    }
}
