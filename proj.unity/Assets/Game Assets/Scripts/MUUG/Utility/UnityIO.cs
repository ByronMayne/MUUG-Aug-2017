/*>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
UnityIO was released with an MIT License.
Arther: Byron Mayne
Twitter: @ByMayne
MIT License
Copyright(c) 2016 Byron Mayne
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/

using UnityEngine;

/// <summary>
/// Taken from Unity.IO on github because I hate
/// doing AssetDatabase stuff so I built a whole library for it.
/// </summary>
namespace UnityIO
{
    public static class IO
    {
        /// <summary>
        /// A list of chars that are not valid for file names in Unity. 
        /// </summary>
        public static readonly char[] INVALID_FILE_NAME_CHARS = new char[] { '/', '\\', '<', '>', ':', '|', '"' };
     
        /// <summary>
        /// The char we use to split our paths.
        /// </summary>
        public const char PATH_SPLITTER = '/';

        /// <summary>
        /// The starting part of a path that is required for it to be an Asset Path. 
        /// </summary>
        public const string ROOT_FOLDER_NAME = "Assets";

        /// <summary>
        /// The name of the folder in Unity for resources.
        /// </summary>
        public const string RESOURCE_FOLDER_NAME = "/Resources/";

        /// <summary>
        /// A function used to make sure that paths being sent to UnityIO are the correct type. One common
        /// problem with Unity's own file system is that every function expects a different format. So if
        /// we force the user to way and explode with fire every other way it will become easier to use. This
        /// will throw exceptions if the path is not valid.
        /// </summary>
        /// <param name="path">The path you want to check.</param>
        public static void ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new System.IO.IOException("UnityIO. A path can not be null or empty when searching the project");
            }

            if (path[path.Length - 1] == PATH_SPLITTER)
            {
                throw new System.IO.IOException("UnityIO: All directory paths are expected to not end with a leading slash. ( i.e. the '/' character )");
            }
        }

        /// <summary>
        /// Checks to see if the file name contains any invalid chars that Unity does not accept.
        /// </summary>
        /// <remarks>Path.GetInvalidFileNameChars() works on Windows but only returns back '/' on Mac so we have to make our own version.</remarks>
        /// <returns><c>true</c> if is valid file name otherwise, <c>false</c>.</returns>
        /// <param name="name">Name.</param>
        public static bool IsValidFileName(string name)
        {
            for (int i = 0; i < INVALID_FILE_NAME_CHARS.Length; i++)
            {
                if (name.IndexOf(INVALID_FILE_NAME_CHARS[i]) != -1)
                {
                    return false;
                }
            }
            return true;
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
        /// Appends a string to the end of the file name. This is before the extension.
        /// </summary>
        public static string AppendName(string assetPath, string appendedText)
        {
            // Create a holder for our name 
            char[] appendedPath = new char[assetPath.Length + appendedText.Length];
            // Get the index of our extension
            int extensionIndex = assetPath.LastIndexOf('.');
            // Copy it into our new path
            assetPath.CopyTo(0, appendedPath, 0, extensionIndex);
            // Append our new text
            appendedText.CopyTo(0, appendedPath, extensionIndex + 1, appendedText.Length);
            // Get the length of our starting extension
            int extensionLength = assetPath.Length - extensionIndex;
            // Add back on the extension
            assetPath.CopyTo(extensionIndex, appendedPath, extensionIndex + appendedText.Length, extensionLength);
            // Return the result
            return new string(appendedPath);
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
}