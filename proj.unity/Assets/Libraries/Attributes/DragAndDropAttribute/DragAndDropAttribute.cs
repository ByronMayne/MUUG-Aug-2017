using UnityEngine;
using UnityEditor;
using System;

namespace MUUG
{
    public class DragAndDropAttribute : PropertyAttribute
    {
        /// <summary>
        /// Used to pick which type of path we are using. Used
        /// for validation for the path. 
        /// </summary>
        public enum PathType
        {
            Asset,
            Resource
        }

        private PathType m_PathType;
        private Type m_AssetType;

        /// <summary>
        /// Gets or sets the current
        /// path type of this attribute.
        /// </summary>
        public PathType pathType
        {
            get { return m_PathType; }
            set { m_PathType = value; }
        }

        /// <summary>
        /// Gets or sets the type of asset that we 
        /// are going to draw this drawer for.
        /// </summary>
        public Type assetType
        {
            get { return m_AssetType; }
            set { m_AssetType = value; }
        }


        /// <summary>
        /// Creates a new instance of the AssetPathAttribute and
        /// defaults to a AssetPath path type.
        /// </summary>
        public DragAndDropAttribute(Type assetType) : this(assetType, PathType.Asset)
        { }

        /// <summary>
        /// Creates a new instance of the AssetPathAttribute and lets
        /// you choose the path type.
        /// </summary>
        public DragAndDropAttribute(Type assetType, PathType pathType)
        {
            m_AssetType = assetType;
            m_PathType = pathType;
        }
    }
}