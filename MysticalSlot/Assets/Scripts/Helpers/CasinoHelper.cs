using UnityEngine;
using System.Linq;

public static class CasinoHelper {

    public static AssetBundle assetBundle;
    
    public static T loadAssetOfKind<T>(AssetBundle bundle, string filePath, string fileName, string ext = "png") where T : UnityEngine.Object {

        T t = null;
        bool contains = filePath.Contains("Resources/");
        if (bundle == null || contains) {
            if (contains) {
                t = Resources.Load<T>(filePath.Replace("Resources/", "") + fileName);
            } else {
#if UNITY_EDITOR
                t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(filePath + fileName + "." + ext);
#endif
            }
        } else {
            t = bundle.LoadAsset<T>(fileName);
        }
        return t;
    }

}