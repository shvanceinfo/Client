/**该文件实现的基本功能等
function: 实现AssetBundle加载的资源内存管理，去除所有的BundleMemManager.Instance.loadResource
author:ljx
date:2014-05-20
**/

//定义宏来确认是否使用本地Bundle包
//#define USE_LOCAL_BUNDLE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

//bundle的类型
public enum EBundleType
{  
    eBundleCommon = 0,      //Common bundle
    eBundlePicture,         //icon跟场景背景的bundle
    eBundleMusic,           //音乐的bundle
    eBundleWing,            //翅膀bundle
    eBundleSelectRole,      //选角用到的bundle
    eBundleScene,           //场景的 bundle
    eBundleShader,          //常驻内存的shader资源
    eBundleWeapon,          //武器bundle
    eBundleWeaponEffect,    //武器bundle 特效
    eBundleUI,              //UI bundle
    eBundleUIEffect,        //UI bundle 特效
    eBundleTaskEffect,      //任务bundle 特效
    eBundleNPC,             //NPC的 bundle
	eBundleBattleGoBulin,	//哥布林中用到的特殊资源
	eBundleBattleEffect,    //战斗专用的effect特效
    eBundleRoleEffect,      //战斗使用的对应角色prefab
    eBundleMonster,         //怪物bundle
    eBundleMulti,           //多人副本：战斗bundle
    eBundleRaid,            //关卡：战斗bundle
    eBundleGobulin,         //哥布林：战斗bundle
    eBundleTower,           //爬塔：战斗bundle 
    eBundleReward,          //悬赏战斗 bundle
    eBundleScenario,        //剧情战斗 bundle
    eBundleBoss,            //Boss战斗 bundle 
    eBundlePanduoLa,        //潘多拉战斗 bundle 
	eBundlePet,				//宠物模型
    eBundleConfig,          //配置文件
    eBundleOther = 1000,  //没有划分类别的bundle，使用Resources.Load
}

//所有prefab组织的结构
//public struct PrefabVo
//{
//    public string prefabName;               //prefab名称，就是根据prefab名称来查找prefab
//    public GameObject prefab;               //真正对应上的prefab
//    public string belongBundle;             //所属的assetBundle
//    public EBundleType bundleType;          //预取件的类型
//    //public List<Shader> shaders;            //该prefab包含的shader
//    //public CHARACTER_CAREER whichCareer;    //所属职业 
//    //public int reference;                   //该prefab的引用计数
//}

//存储在缓存中的bundle
//public class CacheBundle
//{
//    public float loadedTime;        //从缓存加载到bundle的时间
//    public AssetBundle bundle;      //缓存的实际bundle
//    public string bundleName;       //对应的bundle名称
//}

public class BundleMemManager
{
    private static BundleMemManager _instance = null;
    public static bool debugVersion;   //使用Debug版本，适用于windows调试
    public static bool useLocalServer = true;   //发布本地版本

    private Dictionary<string, GameObject> _scenePrefabs; //进入场景的所有prefab
    private Dictionary<string, List<string>> _prefabsInBundleDic;  //根据bundle的名称存储相应bundle下面的所有prefab名称，方便删除scenePrefab
    private Dictionary<string, System.Object> _allSceneBundles; //整个场景中用到的Bundle  
    private Dictionary<string, AssetBundle> _residentBundles;   //常驻内存的Bundle  
    private EBundleType[] _residentTypes;                       //常驻内存bundle类型 
    private Dictionary<EBundleType, Dictionary<CHARACTER_CAREER,BundleItem>> _cacheBundleDic;      //需要缓存的bundle类型,以bundleType作为key
    private Dictionary<string, CHARACTER_CAREER> _preLoadBundleDic;         //创角场景用到Bundle集合 

    private Dictionary<EBundleType, List<BundleItem>> _bundleByTypeDic;     //根据bundle类型进行加载的预取件，必须线性时间查找
    private Dictionary<string, BundleItem> _bundleByModelDic; 				//根据模型索引添加的bundle Item 
    private Dictionary<int, List<BundleItem>> _modelsByMapID; 				    //根据地图类型添加的bundle Item 			
	private Dictionary<EBundleType, int> _loadUseResource;					//使用Resources.Load 加载的资源
    private List<UnityEngine.Object> _resourceLoadObjs;            //Resources.Load load进来的对象 
    private List<GameObject> _instantiateObjs;                      //Instantiate 进来的对象 
    private Dictionary<string, Texture> _uiTextureDic;              //Texture图集，在切换场景时候释放 
    private bool _isLoading;  //是否已经在加载中
    private AssetBundle _shaderBundle;  //保存shader的bundle
    private AssetBundle _configBundle;  //保存config的bundle

    private static string ASSET_IPHONE_PATH = ASSET_URL_SERVER + "ios/";          //iphone机器bundle 的子路径
    private static string ASSET_WIN_PATH = ASSET_URL_SERVER + "windows/";          //iphone机器bundle 的子路径
    private static string ASSET_ANDROID_PATH = ASSET_URL_SERVER + "android/";
    public static string ASSET_VERSION_FILE = ASSET_URL_SERVER + "AssetBundles.xml";

    public const string BUNDLE_SUFFIX = ".unity3d";
    public const string GOBULIN_MONSTER1 = "huangjingebulin";
    public const string GOBULIN_MONSTER2 = "huangjingebulinJY";
    public const string WORLD_BOSS = "shijieBOSS_aoding";
    public const float CACHE_BUNDLE_TIME = 5f;      //缓存的时间
    //private List<Shader> _sceneShaders;     //场景中用到的所有shader，需要加到场景中precompile

    public static BundleMemManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BundleMemManager();
            return _instance;
        }
    }

    private BundleMemManager()
    {
        _scenePrefabs = new Dictionary<string, GameObject>();
        _prefabsInBundleDic = new Dictionary<string, List<string>>();
        _allSceneBundles = new Dictionary<string, System.Object>();
        _residentBundles = new Dictionary<string, AssetBundle>();
        _bundleByTypeDic = new Dictionary<EBundleType, List<BundleItem>>();
        _bundleByModelDic = new Dictionary<string, BundleItem>();
        _modelsByMapID = new Dictionary<int, List<BundleItem>>();
        _preLoadBundleDic = new Dictionary<string, CHARACTER_CAREER>();
        _resourceLoadObjs = new List<Object>();
        _instantiateObjs = new List<GameObject>();
        _uiTextureDic = new Dictionary<string, Texture>();
        _isLoading = false;
        _configBundle = null;
        _shaderBundle = null;

        //常驻内存的bundle
        _residentTypes = new[]
        {
            EBundleType.eBundleCommon, EBundleType.eBundleShader
        };                

        //缓存中的bundle
        _cacheBundleDic = new Dictionary<EBundleType, Dictionary<CHARACTER_CAREER, BundleItem>>();
		_cacheBundleDic.Add(EBundleType.eBundleMusic, null);  	//音乐放在缓存中
        _cacheBundleDic.Add(EBundleType.eBundleWing, null);	//翅膀也放在缓存中
        _cacheBundleDic.Add(EBundleType.eBundlePet, null);
        _cacheBundleDic.Add(EBundleType.eBundleNPC, null);
        _cacheBundleDic.Add(EBundleType.eBundleUIEffect, null);
        _cacheBundleDic.Add(EBundleType.eBundleWeapon, null);
        _cacheBundleDic.Add(EBundleType.eBundleWeaponEffect, null);
        _cacheBundleDic.Add(EBundleType.eBundleTaskEffect, null);

        //使用Resources.Load的bundle
		_loadUseResource = new Dictionary<EBundleType, int>();
		_loadUseResource.Add(EBundleType.eBundlePicture, 0);  	    //icon 使用 resource.load
		_loadUseResource.Add(EBundleType.eBundleUI, 0);             //UI 适用 resources.load
        _loadUseResource.Add(EBundleType.eBundleOther, 0);        //其它的不知道哪个包的适用load
        if (debugVersion)  //调试版本适用load
        {
            for (int i = (int) EBundleType.eBundleCommon; i <= (int) EBundleType.eBundleConfig; i++)
            {
                if(i != (int)EBundleType.eBundleShader)
                    _loadUseResource[(EBundleType)i] = 0;
            }
            _cacheBundleDic.Clear(); //没有缓存的dictionary
        }
    }

    //根据地图ID获取相应关卡的assetBundle
    public void addSceneBundle(int sceneMapID)
    {
        //获取shader资源
        if (_shaderBundle == null)
            addBundleByType(EBundleType.eBundleShader);
        else
            _shaderBundle.LoadAll();
        //获取其它的例如相机，玩家相机的bundle
        addBundleByType(EBundleType.eBundleCommon);      
        //if (Global.inCityMap())
        //{ 
        //    //获取NPC相应的bundle
        //    addBundleByType(EBundleType.eBundleNPC);
        //    //获取任务相应的bundle
        //    addBundleByType(EBundleType.eBundleTaskEffect);
        //}
		//不在主城中才需要加载地图相关资源
		addBundleByMapID(sceneMapID);
        if (!Global.inCityMap())
        {
            //获取战斗中通用的特效
			addBundleByType(EBundleType.eBundleBattleEffect);
            //获取玩家角色的特效bundle
            addBundleByType(EBundleType.eBundleRoleEffect, CharacterPlayer.character_property.career);            
        }
    }

    //登录之前用到的bundle，先预加载三大角色的bundle，创角界面使用
    public void preLoadBundle()
    {
        //获取shader资源
        addBundleByType(EBundleType.eBundleShader);
        //获取配置文件，使得配置文件热加载，无须重下客户端
        addBundleByType(EBundleType.eBundleConfig);
        //获取其它的例如相机，玩家相机的bundle
        addBundleByType(EBundleType.eBundleCommon);
        //添加创角的bundle
        BundleItem mapBundle = _bundleByModelDic[Constant.CREATE_ROLE_SCENE];
        _allSceneBundles.Add(Constant.CREATE_ROLE_SCENE + BUNDLE_SUFFIX, mapBundle);
        //获取选角界面的bundle
        addBundleByType(EBundleType.eBundleSelectRole, CHARACTER_CAREER.CC_BEGIN, true);       
        //获取各个职业的武器跟武器特效的bundle
        //for (int i = (int) CHARACTER_CAREER.CC_BEGIN + 1; i < (int)CHARACTER_CAREER.CC_END; i++)
        //{
        //    CHARACTER_CAREER career = (CHARACTER_CAREER) i;
        //    addBundleByType(EBundleType.eBundleWeapon, career, true);          
        //    addBundleByType(EBundleType.eBundleWeaponEffect, career, true);
        //}
    }

    //竞技场、多人副本、世界Boss添加其它职业的特效
    public IEnumerator addRoleEffectByCareer(CHARACTER_CAREER career)
    {
        //确保加入全部的场景bundle的dictionary中
        BundleItem item = getBundleByType(EBundleType.eBundleRoleEffect, career);
        string bundleName = item.bundleName + BUNDLE_SUFFIX;
        if (!_allSceneBundles.ContainsKey(bundleName))
        {                
            WWW www = WWW.LoadFromCacheOrDownload(getBundleUrl() + bundleName, item.bundleVersion);
            yield return www;
            AssetBundle assetBundle = www.assetBundle;
            addBundleToMem(bundleName, assetBundle, item.bundleType);
            if (item.subPrefabs.Count > 0)
            {
                for (int j = 0; j < item.subPrefabs.Count; j++)
                {
                    string prefabName = item.subPrefabs[j];
                    GameObject obj = assetBundle.Load(prefabName, typeof(GameObject)) as GameObject;                  
                    addScenePrefab(item.bundleType, obj.name, item.bundleName, obj);
                }
            }  
            assetBundle.Unload(false);  
        }
    }

    //加载武器的模型或武器的特效，UI，翅膀等的Bundle
    public void addBundleByType(EBundleType type, CHARACTER_CAREER career = CHARACTER_CAREER.CC_BEGIN, bool isPreLoad = false)
    {
        BundleItem item = getBundleByType(type, career);
        if (item != null)
        {
            string bundleName = item.bundleName + BUNDLE_SUFFIX;
            if (needResidentInMem(type)) //若是常驻内存的bundle，检查内存中是否存在
            {
                if (_residentBundles.ContainsKey(bundleName))
                {
                    if (!_allSceneBundles.ContainsKey(bundleName))
                        _allSceneBundles.Add(bundleName, _residentBundles[bundleName]);
                }
                else 
                {
                    _residentBundles.Add(bundleName, null);       
                }
            }
            //确保加入全部的场景bundle的dictionary中
            if (!_allSceneBundles.ContainsKey(bundleName))
            {
                _allSceneBundles.Add(bundleName, item);
                if(isPreLoad)
                    _preLoadBundleDic.Add(bundleName, item.career);
            }
        }     
    }  

    //从WWW缓存中加载到的bundle加入列表
    public void addBundleToMem(string bundleName, AssetBundle bundle, EBundleType type)
    {
        _allSceneBundles[bundleName] = bundle;
        if (_residentBundles.ContainsKey(bundleName)) //如果是常驻内存的放入到内存中
        {
            _residentBundles[bundleName] = bundle;
        }
    }

    //获取要释放的bundle
    public List<string> getNeedFreeBundles()
    {
        List<string> removeNames = new List<string>();
        foreach (string bundleName in _allSceneBundles.Keys)
        {
            if (!_residentBundles.ContainsKey(bundleName))
            {
                AssetBundle bundle = _allSceneBundles[bundleName] as AssetBundle;
                if (bundle != null)
                {
                    removeNames.Add(bundleName);
                }
            }
        }
        return removeNames;
    }

    //释放场景的assetBundle
    public void freeOneBundle(string bundleName)
    {
        if (!_residentBundles.ContainsKey(bundleName))
        {
            AssetBundle bundle = _allSceneBundles[bundleName] as AssetBundle;
            if (bundle != null)
            {
                bundle.Unload(true);
            }
            if (_prefabsInBundleDic.ContainsKey(bundleName))
            {
                List<string> prefabList = _prefabsInBundleDic[bundleName];
                foreach (string prefabName in prefabList)
                {
                    if (_scenePrefabs.ContainsKey(prefabName))
                    {
                        GameObject obj = _scenePrefabs[prefabName];
                        Object.Destroy(obj);
                        _scenePrefabs.Remove(prefabName);
                    }
                }
                _prefabsInBundleDic[bundleName] = null;
                _prefabsInBundleDic.Remove(bundleName); //从删除列表中移除prefab信息
            }
            _allSceneBundles.Remove(bundleName);
        }
    }

    //Resources.Load的统一入口
    public Object loadResource(string path, Type classType)
    {
        Object sourceObj = Resources.Load(path, classType);
        _resourceLoadObjs.Add(sourceObj);
        return sourceObj;
    }

    //Resources.Load的统一入口
    public Object loadResource(string path)
    {
        Object sourceObj = Resources.Load(path);
        _resourceLoadObjs.Add(sourceObj);
        return sourceObj;
    }

    //实例化的统一入口
    public GameObject instantiateObj(Object sourceObj)
    {
        GameObject obj = Object.Instantiate(sourceObj) as GameObject;
        _instantiateObjs.Add(obj);
        return obj;
    }

    //实例化的统一入口
    public GameObject instantiateObj(Object sourceObj, Vector3 pos, Quaternion rotate)
    {
        GameObject obj = Object.Instantiate(sourceObj, pos, rotate) as GameObject;
        _instantiateObjs.Add(obj);
        return obj;
    }

    //卸载进入场景的assetBundle
    public void freePreloadBundle()
    {
        List<string> freeBundleNames = new List<string>(); //存储创角界面其它玩家的资源
        foreach (string bundleName in _preLoadBundleDic.Keys)
        {
            CHARACTER_CAREER career = _preLoadBundleDic[bundleName];
            if (_residentBundles.ContainsKey(bundleName))
            {
                if (career != CHARACTER_CAREER.CC_BEGIN && career != CharacterPlayer.character_property.career)
                    freeBundleNames.Add(bundleName);
            }
            else
            {
                freeBundleNames.Add(bundleName);
            }
        }
        foreach (string bundleName in freeBundleNames)
        {
            AssetBundle bundle = _allSceneBundles[bundleName] as AssetBundle;
            if (bundle != null)
            {
                if (_residentBundles.ContainsKey(bundleName))
                    _residentBundles.Remove(bundleName);
                _allSceneBundles.Remove(bundleName);
                List<string> prefabList = _prefabsInBundleDic[bundleName];
                foreach (string prefabName in prefabList)
                {
                    if (_scenePrefabs.ContainsKey(prefabName))
                    {
                        _prefabsInBundleDic.Remove(prefabName);
                    }
                }
                _allSceneBundles.Remove(bundleName);
                bundle.Unload(true); //将选角所有的prefab跟bundle彻底删除
            }
        }  
    }

    //获取相应的assetBundle的URL
    public string getBundleUrl()
    {
        string bundleUrl = null;
#if USE_LOCAL_BUNDLE
    #if UNITY_ANDROID
	        bundleUrl = "jar:file://" + Application.dataPath + "!/assets/";
    #elif UNITY_IPHONE
            bundleUrl = Application.dataPath + "/Raw/";
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	        bundleUrl = "file://" + Application.dataPath + "/StreamingAssets/";
    #endif
#else
    #if   UNITY_STANDALONE_WIN || UNITY_EDITOR
        bundleUrl = ASSET_WIN_PATH;
    #elif UNITY_ANDROID
        bundleUrl = ASSET_ANDROID_PATH;
    #else
        bundleUrl = ASSET_IPHONE_PATH;
    #endif
#endif
        return bundleUrl;
    }

    //根据名称跟类型获取相应的prefab
    public GameObject getPrefabByName(string pathName, EBundleType type, 
        EventDispatcher.EventLoadFromWWW loadFunc = null)
    {
        if (_loadUseResource.ContainsKey(type))
        {
            GameObject obj = loadResource(pathName) as GameObject;
            return obj;
        }
        if (_cacheBundleDic.ContainsKey(type) && loadFunc != null)
        {
            loadPrefabViaWWW<GameObject>(type, pathName, loadFunc);
            return null;
        }      
        string prefabName = ToolFunc.TrimPath(pathName);
        if (_scenePrefabs.ContainsKey(prefabName))
        {           
            return _scenePrefabs[prefabName];
        }
        return null;       
    }

    //开始加载缓存数据
    public void loadPrefabViaWWW<T>(EBundleType type, string origName, EventDispatcher.EventLoadFromWWW loadFunc,
        CHARACTER_CAREER career = CHARACTER_CAREER.CC_BEGIN) where T : UnityEngine.Object
    {
//		if (origName.IndexOf("head_goblin_buff") != -1)
//		{
//            Debug.LogError("ffffffffffffffffffffff");
//        }
        if (_isLoading)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<MonoBehaviour>();
            obj.GetComponent<MonoBehaviour>().StartCoroutine(waitLastCoroutineFinish<T>(type, origName, loadFunc, obj, career));
        }
        else
        {
            MainLogic.sMainLogic.StartCoroutine(loadPrefabFromCache(type, origName, loadFunc, typeof(T), career));
        }
    }
	
	//判断是否在缓存中的bundle
	//是否常驻内存的prefab，例如翅膀，武器, isCache是否存储在缓存中
    public bool needResidentInMem(EBundleType type)
    {
        for (int i = 0; i < _residentTypes.Length; i++)
        {
            if (type == _residentTypes[i])
                return true;
        }
        return false;
    }

    public bool needResidentInMem(string bundleName)
    {
        return _residentBundles.ContainsKey(bundleName);
    }

    //判断是否临时缓存的内存
    public bool isTypeInCache(EBundleType type)
    {
        return _cacheBundleDic.ContainsKey(type);
    }

    //将场景使用的prefab添加到内存中
    public void addScenePrefab(EBundleType type, string prefabName, string bundleName, GameObject prefab)
    {
        List<string> prefabNames;
        if (_prefabsInBundleDic.ContainsKey(bundleName))
        {
            prefabNames = _prefabsInBundleDic[bundleName];
        }
        else
        {
            prefabNames = new List<string>();
            _prefabsInBundleDic.Add(bundleName, prefabNames);
        }
        if (!_scenePrefabs.ContainsKey(prefabName))
        {
            _scenePrefabs.Add(prefabName, prefab);
            prefabNames.Add(prefabName);
        }
    }

    //将bundleItem用type包在Dictionry中就是为了加速查找
    public void addBundleItem(BundleItem item, bool addByType, bool addByMap = false)
    {
        if (addByMap) //根据地图名称打包
        {
            List<BundleItem> models;
            if (_modelsByMapID.ContainsKey(item.mapID))
            {
                models = _modelsByMapID[item.mapID];
            }
            else
            {
                models = new List<BundleItem>();
                _modelsByMapID.Add(item.mapID, models);
            }
            models.Add(item);
        }
        else if(addByType) 
        {
            List<BundleItem> items;
            if (_bundleByTypeDic.ContainsKey(item.bundleType))
            {
                items = _bundleByTypeDic[item.bundleType];
            }
            else
            {
                items = new List<BundleItem>();
                _bundleByTypeDic.Add(item.bundleType, items);
            }
            items.Add(item);
        }
        else //如果不是根据Bundle类型打包的就是根据模型打包
        {
            if (!_bundleByModelDic.ContainsKey(item.bundleName))
            {
                _bundleByModelDic.Add(item.bundleName, item);
            }
        }
    }

    //根据类型获取bundleItem
    public List<BundleItem> getBundleItemByType(EBundleType type)
    {
        if (_bundleByTypeDic.ContainsKey(type))
        {
            return _bundleByTypeDic[type];
        }
        return null;
    }

    //根据model名称获取bundleItem
    public BundleItem getItemByModelName(string modelName)
    {
        if (_bundleByModelDic.ContainsKey(modelName))
        {
            return _bundleByModelDic[modelName];
        }
        return null;
    }

    //根据bundle类型获取相应的特效(武器）
    public BundleItem getBundleByType(EBundleType type, CHARACTER_CAREER career = CHARACTER_CAREER.CC_BEGIN)
    {
        if (_bundleByTypeDic.ContainsKey(type))
        {
            List<BundleItem> items = _bundleByTypeDic[type];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].career == career)
                    return items[i];
            }
        }
        return null;
    }

    //等待上次LoadFromWWW完成
    private IEnumerator waitLastCoroutineFinish<T>(EBundleType type, string origName,
       EventDispatcher.EventLoadFromWWW loadFunc, GameObject obj, CHARACTER_CAREER career) where T : UnityEngine.Object
    {
        while (true)
        {
            yield return 1;
            if (!_isLoading)
            {
                MainLogic.sMainLogic.StartCoroutine(loadPrefabFromCache(type, origName, loadFunc, typeof(T), career));
                obj.GetComponent<MonoBehaviour>().StopAllCoroutines();
                Object.Destroy(obj);
            }
        }
    }

    //获取相应的缓存中的对象
    private IEnumerator loadPrefabFromCache(EBundleType type, string origName,
        EventDispatcher.EventLoadFromWWW loadFromFunc, Type t, CHARACTER_CAREER career)
    {
        if (_cacheBundleDic.ContainsKey(type)) //必须存在缓存中才有效
        {
            _isLoading = true;
            Object obj = null;
            string prefabName = ToolFunc.TrimPath(origName);
            BundleItem  item = _cacheBundleDic[type][career];
            string bundleName = item.bundleName;
            WWW www = WWW.LoadFromCacheOrDownload(getBundleUrl() + bundleName + BUNDLE_SUFFIX, item.bundleVersion);
            yield return www;
            AssetBundle bundle = www.assetBundle;
            if (bundle != null)
            {
                _shaderBundle.LoadAll();
                if (bundle.Contains(prefabName))
                {
                    obj = bundle.Load(prefabName, t);
                }
                else
                {
                    GameObject mainObj = bundle.mainAsset as GameObject;
                    if (mainObj != null)
                    {
                        Transform trans = mainObj.transform.FindChild(prefabName);
                        obj = trans.gameObject;
                        _resourceLoadObjs.Add(mainObj);
                    }
                }
                bundle.Unload(false);
                if (obj != null)
                {
                    _resourceLoadObjs.Add(obj);
                    if (loadFromFunc != null)
                        loadFromFunc(obj);
                }
                else
                {
                    Debug.LogError("aaaaaaaaaaaaaaaaaaaaaaa");
                    Debug.LogError("bundle name <" + bundleName + ">, prefab name <" + prefabName + "> not exist!");
                }
            }
            _isLoading = false;
        }
    }

    //根据地图类型加载战斗控件，加载怪物，怪物技能等等
    private void addBundleByMapID(int sceneMapID)
    {
        //获取场景的资源
        MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(sceneMapID);
        if (_bundleByModelDic.ContainsKey(mapData.sceneName))
        {
            BundleItem mapBundle = _bundleByModelDic[mapData.sceneName];
            if (!_allSceneBundles.ContainsKey(mapBundle.bundleName + BUNDLE_SUFFIX))
            {
                _allSceneBundles.Add(mapBundle.bundleName + BUNDLE_SUFFIX, mapBundle);
            }
        }
        ////在黄金哥布林需要添加哥布林怪
        //if (Global.inGoldenGoblin())
        //{
        //    if (_bundleByModelDic.ContainsKey(GOBULIN_MONSTER1))
        //    {
        //        BundleItem item = _bundleByModelDic [GOBULIN_MONSTER1];
        //        string bundleName = GOBULIN_MONSTER1 + BUNDLE_SUFFIX;
        //        if (!_allSceneBundles.ContainsKey(bundleName))
        //            _allSceneBundles.Add(bundleName, item);
        //    }
        //    if (_bundleByModelDic.ContainsKey(GOBULIN_MONSTER2))
        //    {
        //        BundleItem item = _bundleByModelDic[GOBULIN_MONSTER2];
        //        string bundleName = GOBULIN_MONSTER2 + BUNDLE_SUFFIX;
        //        if (!_allSceneBundles.ContainsKey(bundleName))
        //            _allSceneBundles.Add(bundleName, item);
        //    }
        //}
        ////在世界BOSS需要添加世界boss
        //else if (Global.InWorldBossMap())
        //{
        //    if (_bundleByModelDic.ContainsKey(WORLD_BOSS))
        //    {
        //        BundleItem item = _bundleByModelDic[WORLD_BOSS];
        //        string bundleName = WORLD_BOSS + BUNDLE_SUFFIX;
        //        if (!_allSceneBundles.ContainsKey(bundleName))
        //            _allSceneBundles.Add(bundleName, item);
        //    }
        //}
		if(Global.inCityMap())
			return;
        //就是魔物悬赏，组队，世界Boss，哥布林等非实例ID地图
        if (_modelsByMapID.ContainsKey(sceneMapID))
        {
            List<BundleItem> items = _modelsByMapID[sceneMapID];
            foreach (BundleItem item in items)
            {
                string bundleName = item.bundleName + BUNDLE_SUFFIX;
                if (!_allSceneBundles.ContainsKey(bundleName))
                    _allSceneBundles.Add(bundleName, item);
            }           
        }
        //获取地图上所有怪物模型以及怪物身上绑定的特效
        else if (ConfigDataManager.GetInstance().getMonsterInstanceConfig().m_MapTemplateIDList.ContainsKey(sceneMapID))
        {
            List<int> sceneMonsters =
                ConfigDataManager.GetInstance().getMonsterInstanceConfig().m_MapTemplateIDList[sceneMapID];
            if (sceneMonsters != null && sceneMonsters.Count > 0)
            {
                int len = sceneMonsters.Count;
                for (int i = 0; i < len; i++)
                {
                    MonsterDataItem monsterData =
                        ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(sceneMonsters[i]);
                    string monsterName = ToolFunc.TrimPath(monsterData.name);
                    if (_bundleByModelDic.ContainsKey(monsterName))
                    {
                        BundleItem item = _bundleByModelDic[monsterName];
                        string bundleName = monsterName + BUNDLE_SUFFIX;
                        if (!_allSceneBundles.ContainsKey(bundleName))
                            _allSceneBundles.Add(bundleName, item);
                    }
                    else
                    {
                        Debug.LogError("存在没有打包的怪物模型：" + monsterData.name);
                    }
                }
            }
        }
        //获取地图上的战斗bundle，在主城不用加载战斗Bundle
        if (Global.inTowerMap())
        {
            addBundleByType(EBundleType.eBundleTower);
        }
        else if (Global.inMultiFightMap())
        {
            addBundleByType(EBundleType.eBundleMulti);
        }
        else if (Global.inFightMap())
        {
            addBundleByType(EBundleType.eBundleRaid);
        }
        else if (Global.inGoldenGoblin())
        {
			//获取黄金哥布林中用到的特殊资源
			addBundleByType(EBundleType.eBundleBattleGoBulin);
			addBundleByType(EBundleType.eBundleGobulin);
        }
        else if (Global.InWorldBossMap())
        {
            addBundleByType(EBundleType.eBundleBoss);
        }
		else if(Global.InAwardMap()){
			addBundleByType(EBundleType.eBundleReward);
		}
    }

    //getter and setter

    public static string ASSET_URL_SERVER
    {
        get
        {
            if (useLocalServer)
            {
                return "http://192.168.1.117/AssetBundleResource/";
                //return "http://192.168.1.146/AssetBundleResource/";
            }
            return "http://118.192.89.218/AssetBundleResource/";
        }
    }

    public Dictionary<string, System.Object> AllSceneBundles
    {
        get { return _allSceneBundles; }
    }

    public List<BundleItem> AllBundleItems
    {
        get { return new List<BundleItem>(_bundleByModelDic.Values);}
    }

    public Dictionary<EBundleType, Dictionary<CHARACTER_CAREER, BundleItem>> CacheBunldeDic
    {
        get { return _cacheBundleDic; }
    }

	public Dictionary<EBundleType, int> LoadUseResource
	{
		get { return _loadUseResource; }
	}

    public List<Object> ResourceLoadObjs
    {
        get { return _resourceLoadObjs; }
    }

    public List<GameObject> InstantiateObjs
    {
        get { return _instantiateObjs; }
    }

    public Dictionary<string, Texture> UiTextures
    {
        get { return _uiTextureDic;}
    }

    public AssetBundle ShaderBundle
    {
        set { _shaderBundle = value; }
        get { return _shaderBundle; }
    }

    public AssetBundle ConfigBundle
    {
        set { _configBundle = value; }
        get { return _configBundle; }
    }
}
