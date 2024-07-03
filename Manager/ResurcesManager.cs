
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResurcesManager : Manager<ResurcesManager>
{

    private string m_abPath = Application.streamingAssetsPath + "/";
    private string m_mainABName = "StandaloneWindows";
    private Dictionary<string, AssetBundle> m_assetBundlesDict = new Dictionary<string, AssetBundle>();
    private AssetBundle m_mainAB = null;
    private AssetBundleManifest m_manifest = null;

    private void OnDestroy()
    {
        AssetBundle.UnloadAllAssetBundles(true);
    }

    public override void Init()
    {
        //m_mainAB = AssetBundle.LoadFromFile(m_abPath + m_mainABName);
        //m_manifest = m_mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    public override void Tick(float dt)
    {
        throw new System.NotImplementedException();
    }

    public override void Uninit()
    {
        throw new System.NotImplementedException();
    }

    public T LoadAsset<T>(string assetPath) where T : Object
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<T>(assetPath);
#else
        return LoadAssetFromAB<T>(assetPath);
#endif
    }

    private T LoadAssetFromAB<T>(string assetPath) where T : Object
    {
        var pathSplit = assetPath.Split("/");
        string assetName = pathSplit.Length > 0 ? pathSplit[pathSplit.Length - 1].Split(".")[0] : null;
        string abName = pathSplit.Length > 2 ? pathSplit[pathSplit.Length - 3] : null;
        if (assetName != null && abName != null)
        {
            m_assetBundlesDict.TryGetValue(abName, out var ab);
            if (ab == null)
            {
                LoadDependencies(abName);
                ab = AssetBundle.LoadFromFile(m_abPath + abName);
                m_assetBundlesDict[abName] = ab;
            }
            return ab.LoadAsset<T>(assetName);
        }
        return null;
    }

    private void LoadDependencies(string abName)
    {
        var dependencies = m_manifest.GetAllDependencies(abName);
        foreach (string depend in dependencies)
        {
            if (!m_assetBundlesDict.ContainsKey(depend))
            {
                var ab = AssetBundle.LoadFromFile(m_abPath + depend);
                m_assetBundlesDict[abName] = ab;
            }
        }
    }
}
