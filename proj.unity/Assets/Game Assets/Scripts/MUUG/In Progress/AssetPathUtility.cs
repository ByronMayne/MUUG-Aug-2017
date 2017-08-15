using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MUUG.Completed
{
    public static class AssetPathUtility
    {
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
    }
}