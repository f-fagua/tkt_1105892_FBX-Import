using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HashPrinter : MonoBehaviour
{
    [MenuItem("UnitySupport/Force Asset Serialization", false, 2)]
    public static void ForceAssetSerialization()
    {
        var filePath = GetFilePath();
        var assetPath = RemoveAppDataPath(filePath);

        var stringList = new List<string> {assetPath};

        AssetDatabase.ForceReserializeAssets(stringList);
    }

    [MenuItem("UnitySupport/Get Asset Hash", false, 1)]
    public static void PrintAssetHash()
    {
        var filePath = GetFilePath();
        var assetPath = RemoveAppDataPath(filePath);
        LogHashes(assetPath);
    }

    private static string GetFilePath()
    {
        var filePath = EditorUtility.OpenFilePanel("FBX Test", Path.GetDirectoryName(Application.dataPath), "");
        return File.Exists(filePath) == false ? null : filePath;
    }
    
    private static string RemoveAppDataPath(string filePath)
    {
        return "Assets" + filePath.Substring(Application.dataPath.Length);
    }

    private static void LogHashes(string assetPath) 
    {
        var guid = GetAssetGUID(assetPath);
        var hash = AssetDatabase.GetAssetDependencyHash(assetPath);

        var message = $"Main asset path: {assetPath}";
        message += $"\nMain asset guid: {guid}";
        message += $"\nMain asset hash: {hash}";

        //TODO: Change this to the real player dependencies.
        var dependencies = AssetDatabase.GetDependencies(assetPath, true);
        GetDependenciesInfo(dependencies, ref message);
        Debug.Log(message);
    }

    private static void GetDependenciesInfo(string[] dependencies, ref string message)
    {
        for (int i = 0; i < dependencies.Length; i++)
        {
            message += $"\n- Dependency {(i+1)}: ";
            message += $"\n\tpath: {dependencies[i]}";
            message += $"\n\tguid: {GetAssetGUID(dependencies[i])}";
            message += $"\n\thash: {AssetDatabase.GetAssetDependencyHash(dependencies[i])}";
        }
    }
    
    private static string GetAssetGUID(string assetPath)
    {
#if (UNITY_2019 )
        return AssetDatabase.AssetPathToGUID(assetPath);
#else
        return AssetDatabase.GUIDFromAssetPath(path);
#endif
    }
}
