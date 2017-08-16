
using UnityEditor;
using UnityEngine;

public class AssetDatabaseUsage
{
    public static void FrequentFunction()
    {
        Object assetObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath:"");

        string path = AssetDatabase.GetAssetPath(assetObject);

        string[] guids = AssetDatabase.FindAssets(filter: "", searchInFolders: new string[0]);

        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

        AssetDatabase.Refresh();

        AssetDatabase.CreateAsset(asset: null, path: "");

        AssetDatabase.ImportAsset(path: "");

        AssetDatabase.StartAssetEditing();
        {
            // Do Work
        }
        AssetDatabase.StopAssetEditing();
    }
}
