using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Slide : ScriptableObject
{
    public Texture slideImage;
    public Object[] references;

    [MenuItem("Slideshow/Create Slides For Selections")]
    public static void CreateSlides()
    {
        Object[] selections = Selection.objects;

        for (int i = 0; i < selections.Length; i++)
        {
            if(selections[i] is Texture)
            {
                Slide newSlide = CreateInstance<Slide>();
                newSlide.slideImage = selections[i] as Texture;
                string selectionPath = AssetDatabase.GetAssetPath(selections[i]);
                string slidPath = Path.ChangeExtension(selectionPath, ".asset");
                AssetDatabase.CreateAsset(newSlide, slidPath);
            }
        }
        AssetDatabase.Refresh();
    }
}
