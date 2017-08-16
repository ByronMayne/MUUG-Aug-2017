using System;
using UnityEngine;

namespace MUUG
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetPathAttribute : PropertyAttribute
    {
        public AssetPathAttribute(Type type)
        {

        }
    }
}