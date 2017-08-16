using UnityEngine;

using System;

namespace MUUG
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DragAndDropAttribute : PropertyAttribute
    {
        public DragAndDropAttribute(Type objectType)
        {

        }
    }
}