using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class ExportAssetBundles 
{
	static int lastVersion = 0;
	[MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
	static void ExportResource () {
		// Bring up save panel
		Caching.CleanCache();
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
		if (path.Length != 0) {
			// Build the resource file from the active selection.
			Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
#if UNITY_IPHONE
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle,BuildTarget.iPhone);
#elif UNITY_ANDROID			
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
#else
			BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone);
#endif
			Selection.objects = selection;
		}
	}
	
	[MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
	static void ExportResourceNoTrack () {
		Caching.CleanCache();
		// Bring up save panel
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");	
		if (path.Length != 0) {
			// Build the resource file from the active selection.
#if UNITY_IPHONE
			BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path,BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone);
#elif UNITY_ANDROID
			BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path,BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android);
#else
			BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path,BuildAssetBundleOptions.CompleteAssets,BuildTarget.iPhone);
#endif
		}
	}

    [MenuItem("Create Bundle/Save Win ICON")]
    static void ExportWinIcon()
    {
        Caching.CleanCache();
        string path = Application.dataPath + "/StreamingAssets/picture.unity3d";
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }
        //这里注意第二个参数就行
        BuildPipeline.BuildAssetBundle(null, selectAssets, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.WebPlayer);
    }

    [MenuItem("Create Bundle/Save IOS ICON")]
    static void ExportIOSICon()
    {
        Caching.CleanCache();
        string path = Application.dataPath + "/StreamingAssets/picture.unity3d";
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }
        //这里注意第二个参数就行
        BuildPipeline.BuildAssetBundle(null, selectAssets, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.iPhone);
    }

    [MenuItem("Create Bundle/Save Win Bundle")]
    static void ExportWinBundle()
    {
        //Caching.CleanCache();
        string path = Application.dataPath + "/StreamingAssets/rename.unity3d";
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }
        BuildPipeline.BuildAssetBundle(null, selectAssets, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.WebPlayer);
    }

    [MenuItem("Create Bundle/Save IOS Bundle")]
    static void ExportIOSBundle()
    {
        Caching.CleanCache();
        string path = Application.dataPath + "/StreamingAssets/rename.unity3d";
        Object[] selectAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object obj in selectAssets)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }
        BuildPipeline.BuildAssetBundle(null, selectAssets, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.iPhone);
    }

    static StreamWriter logWriter = new StreamWriter("Assets/output/log.txt");
    const string SCENE_WIN_PATH = "/output/win_scenes/";
    const string SCENE_IOS_PATH = "/output/ios_scenes/";
    const string PREFAB_WIN_PATH = "/output/win_prefabs/";
    const string PREFAB_IOS_PATH = "/output/ios_prefabs/";

    [MenuItem("Create Bundle/Save Win Scene")]
	static void ExportWinScene(){
		Caching.CleanCache();
		ReadBundle();
        List<BundleItem> items = BundleMemManager.Instance.getBundleItemByType(EBundleType.eBundleScene);
        for (int i = 0; i < items.Count; i++)
        {
            BundleItem item = items[i];
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;
            string[] buildArr = new string[item.subPrefabs.Count];
            for (int j = 0; j < item.subPrefabs.Count; j++)
            {
                buildArr[j] = item.subPrefabs[j] + ".unity";
            }
            string outFile = Application.dataPath + SCENE_WIN_PATH + item.bundleName + ".unity3d";
            BuildPipeline.BuildPlayer(buildArr, outFile, BuildTarget.WebPlayer, BuildOptions.BuildAdditionalStreamedScenes);
        }
	}

    [MenuItem("Create Bundle/Save IOS Scene")]
    static void ExportIOSScene()
    {
        Caching.CleanCache();
        ReadBundle();
        List<BundleItem> items = BundleMemManager.Instance.getBundleItemByType(EBundleType.eBundleScene);
        for (int i = 0; i < items.Count; i++)
        {
            BundleItem item = items[i];
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;
            string[] buildArr = new string[item.subPrefabs.Count];
            for (int j = 0; j < item.subPrefabs.Count; j++)
            {
                buildArr[j] = item.subPrefabs[j] + ".unity";
            }
            string outFile = Application.dataPath + SCENE_IOS_PATH + item.bundleName + ".unity3d";
            BuildPipeline.BuildPlayer(buildArr, outFile, BuildTarget.iPhone, BuildOptions.BuildAdditionalStreamedScenes);
        }
        //Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    }

    [MenuItem("Create Bundle/Save Win Prefabs")]
	static void ExportWinPrefabs()
	{
        string bundlePrefix = "Assets/Resources/";
        ReadBundle();
        string prefabName;
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle;
        List<BundleItem> items = BundleMemManager.Instance.getBundleItemByType(EBundleType.eBundleShader);
        BundleItem shaderItem = items[0];
        //BuildPipeline.PushAssetDependencies();
        Object shaderObj = AssetDatabase.LoadMainAssetAtPath(bundlePrefix + shaderItem.subPrefabs[0] + ".prefab");
        prefabName = Application.dataPath + PREFAB_WIN_PATH + shaderItem.bundleName + ".unity3d";
        BuildPipeline.BuildAssetBundle(shaderObj, null, prefabName, options, BuildTarget.WebPlayer);

        foreach (BundleItem item in BundleMemManager.Instance.AllBundleItems)
        {
            if (item.bundleType == EBundleType.eBundleShader || item.bundleType == EBundleType.eBundleScene 
                || item.bundleType == EBundleType.eBundlePicture || item.bundleType == EBundleType.eBundleUI
                || item.bundleType == EBundleType.eBundleConfig) //忽略场景的bundle
                continue;
            //if (item.bundleVersion <= lastVersion ||(item.bundleType != EBundleType.eBundleBattleEffect 
            //    && item.bundleType != EBundleType.eBundleSelectRole)) 
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;
            int len = item.subPrefabs.Count;
            Object[] childObjs = new Object[len];
            for (int i = 0; i < len; i++)
            {
                string objPath = null;
                if (item.bundleType == EBundleType.eBundleMusic)
                    objPath = bundlePrefix + item.subPrefabs[i];
                else
                    objPath = bundlePrefix + item.subPrefabs[i] + ".prefab";
                childObjs[i] = AssetDatabase.LoadMainAssetAtPath(objPath);
				if(item.bundleType != EBundleType.eBundleMusic)
				{
//                    GameObject testObj = (GameObject)Object.Instantiate(childObjs[i]);
//                    FindShader(testObj, item.bundleName, item.subPrefabs[i]);
				}
                if (childObjs[i] == null)
                {
                    Debug.LogError("The asset: <" + item.bundleName + "> sub prefabs <" +
                                   item.subPrefabs[i] + "> not exist");
                    return;
                }
            }
            //BuildPipeline.PushAssetDependencies();
            prefabName = Application.dataPath + PREFAB_WIN_PATH + item.bundleName + ".unity3d";
            bool success = BuildPipeline.BuildAssetBundle(null, childObjs, prefabName, options, BuildTarget.WebPlayer);
            //BuildPipeline.PopAssetDependencies();
            if (!success)
            {
                Debug.LogError("BuildAsset: <" + item.bundleName + "> error!");
                logWriter.WriteLine("BuildAsset: <" + item.bundleName + "> error!");
            }
        }		

       // BuildPipeline.PopAssetDependencies();
        logWriter.Flush();
	}

    [MenuItem("Create Bundle/Save IOS Prefabs")]
    static void ExportIOSPrefabs()
    {
        string bundlePrefix = "Assets/Resources/";
        ReadBundle();
        bool success;
        string prefabName;
        BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
        List<BundleItem> items = BundleMemManager.Instance.getBundleItemByType(EBundleType.eBundleShader);
        BundleItem shaderItem = items[0];
        //BuildPipeline.PushAssetDependencies();
        Object shaderObj = AssetDatabase.LoadMainAssetAtPath(bundlePrefix + shaderItem.subPrefabs[0] + ".prefab");
        prefabName = Application.dataPath + PREFAB_IOS_PATH + shaderItem.bundleName + ".unity3d";
        BuildPipeline.BuildAssetBundle(shaderObj, null, prefabName, options | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.iPhone);
        
        foreach (BundleItem item in BundleMemManager.Instance.AllBundleItems)
        {
            if (item.bundleType == EBundleType.eBundleShader || item.bundleType == EBundleType.eBundleScene 
                || item.bundleType == EBundleType.eBundlePicture || item.bundleType == EBundleType.eBundleUI
                || item.bundleType == EBundleType.eBundleConfig) //忽略场景的bundle
                continue;
            if (item.bundleVersion <= lastVersion) //如果比导出的上个版本小，那么不用导出
                continue;           
            int len = item.subPrefabs.Count;
            Object[] childObjs = new Object[len];
            for (int i = 0; i < len; i++)
            {
                string objPath = null;
                if (item.bundleType == EBundleType.eBundleMusic)
                    objPath = bundlePrefix + item.subPrefabs[i];
                else
                    objPath = bundlePrefix + item.subPrefabs[i] + ".prefab";
                childObjs[i] = AssetDatabase.LoadMainAssetAtPath(objPath);
                if (childObjs[i] == null)
                {
                    Debug.LogError("The asset: <" + item.bundleName + "> sub prefabs <" +
                                   item.subPrefabs[i] + "> not exist");
                    return;
                }
            }
            //BuildPipeline.PushAssetDependencies();
            prefabName = Application.dataPath + PREFAB_IOS_PATH + item.bundleName + ".unity3d";
            success = BuildPipeline.BuildAssetBundle(null, childObjs, prefabName, options, BuildTarget.iPhone);
            if (!success)
            {
                Debug.LogError("BuildAsset: <" + item.bundleName + "> error!");
                logWriter.WriteLine("BuildAsset: <" + item.bundleName + "> error!");
            }
            //BuildPipeline.PopAssetDependencies();
        }
              
        //BuildPipeline.PopAssetDependencies();
        logWriter.Flush();
    }

    static bool FindShader(GameObject obj, string bundleName, string prefabName)
    {
        bool notExist = false;
        foreach (Transform trans in obj.transform)
        {
            GameObject childObj = trans.gameObject;
            if(childObj.GetComponent<Renderer>() != null)
            {
                Renderer renderer = childObj.GetComponent<Renderer>();
				if (renderer.material != null)
				{
				    Debug.Log(renderer.material.shader.name);
                    if (renderer.material.shader.name.IndexOf("FX PACK 1") != -1)
                    {
                        Debug.Log("The asset: <" + bundleName + "> sub prefabs <" + prefabName + "> use error shader");
                        return false;;
                    }
                    else
                    {
                        notExist = FindShader(childObj, bundleName, prefabName);
                        if(!notExist)
                            break;
                    }
                }
            }
            
        }
        return notExist;
    }

    static void ReadBundle()
    {
        DataReadBundle readBundle = new DataReadBundle();
        readBundle.useInGame = false;
        readBundle.path = PathConst.BUNDLE_PATH;
        readBundle.init();
        ReadConfig(readBundle);
    }

    static void ReadConfig(DataReadBundle readBundle, int special = 0)
    {
        TextAsset ta = Resources.Load(readBundle.path) as TextAsset;
        if (ta)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ta.text);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode(readBundle.getRootNodeName()).ChildNodes;
            for (int k = 0; k < nodeList.Count; k++)
            {
                XmlElement xe = nodeList.Item(k) as XmlElement;
                if (xe == null)
                    continue;
				if(k == 0)
				{
					lastVersion = int.Parse(xe.GetAttribute("lastVersion"));
					continue;
				}
                string key = xe.GetAttribute("ID");
                for (int i = 0; i < xe.Attributes.Count; i++)
                {
                    XmlAttribute attr = xe.Attributes[i];
                    readBundle.appendAttribute(int.Parse(key), attr.Name, attr.Value);
                }
            }			
        }
    }
}
