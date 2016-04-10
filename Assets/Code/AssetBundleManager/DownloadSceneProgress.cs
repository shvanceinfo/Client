using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DownloadSceneProgress : MonoBehaviour 
{
	private UILabel 		m_ProgressLabel;
	private UISlider 		m_ProgressSlider;
	private float 			m_ProgressVal;
	private bool			m_bDownloadFinish;
    private string _assetSize;
	public 	Transform		m_MainGameObject;
    public bool debugVersion = true;   //使用Debug版本，适用于windows调试
    public bool useLocalServer = true;   //发布本地版本
    private bool _loadBundleSuccess;  //成功加载bundle到内存中

    private const string BUNDLE_VERSION = "bundle_version"; //对应所有的bundle版本号，可以用json存储
	
	void Awake()
	{
        _loadBundleSuccess = false;
		m_bDownloadFinish = false;
		m_ProgressVal = 0;
		m_ProgressLabel = transform.FindChild("InitGameGround/Background/Child/InitLabel").GetComponent<UILabel>();
		m_ProgressSlider = transform.FindChild("InitGameGround/Background/Child/ProgressBar").GetComponent<UISlider>();
	    //BundleMemManager.useLocalServer = useLocalServer;
	    BundleMemManager.debugVersion = debugVersion;
        MessageManager.Instance.doNothing(); //这里初始化messageManager
	}
	
	void OnEnable()
	{
		EventDispatcher.GetInstance().DialogSure += DialogSure;
		EventDispatcher.GetInstance().DialogCancel += DialogCancel;
	}
	
	void OnDisable()
	{
		EventDispatcher.GetInstance().DialogSure -= DialogSure;
		EventDispatcher.GetInstance().DialogCancel -= DialogCancel;
	}
	
	void Start()
	{
		//下载场景资源
		//DownloadSceneAssets();	
		//setBarPos(0, true);
		if (debugVersion)
		{
			_loadBundleSuccess = true;
			m_bDownloadFinish = true;
			setBarPos(1);
			m_ProgressVal = 1.0f;
		}
		else
		{
			DownloadSceneAssets();
			setBarPos(0, true);
		}
	}

    private void Update()
    {
        if (m_bDownloadFinish)
        {
            m_ProgressVal += Time.deltaTime;
            if (m_ProgressVal >= 1.0f)
            {
                m_ProgressVal = 1.0f;
            }
            if (_loadBundleSuccess && m_ProgressVal >= 1.0f)
            {
                _loadBundleSuccess = false;
                //启动游戏
                m_MainGameObject.gameObject.SetActive(true);
            }
            setBarPos(m_ProgressVal);
        }
    }

    //设置加载条的跟随
	void setBarPos(float currentPos, bool bDownloadAsset=false)
	{
		m_ProgressSlider.value = currentPos;
		if (bDownloadAsset)
		{
			m_ProgressLabel.text = string.Format("正在更新资源包...({0}%)", (int)(currentPos * 100));
		}
		else 
		{
			m_ProgressLabel.text = string.Format("正在初始化...", (int)(currentPos * 100));
		}
	}
	
	public void DialogSure(eDialogSureType eType)
	{
		if (eType == eDialogSureType.eDownloadAsset)
		{
            StartCoroutine(DownloadAndCache());
		}
	}
	
	public void DialogCancel(eDialogSureType eType)
	{
		if (eType == eDialogSureType.eDownloadAsset)
		{
			Application.Quit();
		}
	}
	  
	// Use this for initialization
	void DownloadSceneAssets() 
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			UIManager.Instance.ShowMessageBox("没有可用网络,请打开wifi或3g");
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			UIManager.Instance.ShowDialog(eDialogSureType.eDownloadAsset, "未检测到wifi网络,是否使用现有网络下载,更新资源大小: "+_assetSize);
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			StartCoroutine (DownloadAndCache());
		}
	}
	
    //开始下载AssetBundle并且缓存起来
	IEnumerator DownloadAndCache ()
    {
		// Wait for the Caching system to be ready
		while (!Caching.ready)
		yield return null;

        WWW bundleFile = new WWW(BundleMemManager.ASSET_VERSION_FILE);
        yield return bundleFile;
		
		//Caching.CleanCache();
		//读取预制件下载列表 
        DataReadBundle readBundle = new DataReadBundle();
		readBundle.init();
        read_config(readBundle, bundleFile.text);

	    int storeVersion = 0; //存储的版本
	    int maxVersion = 0;  //存储的最大版本号
		//PlayerPrefs.SetInt(BUNDLE_VERSION, maxVersion);
	    if (PlayerPrefs.HasKey(BUNDLE_VERSION)) //获取存储在本地的版本号
	        storeVersion = PlayerPrefs.GetInt(BUNDLE_VERSION);
        //场景，特效，人物，武器的bundle都在一个文件中
		List<BundleItem> bundleItems = BundleMemManager.Instance.AllBundleItems;
	    int bundleLen = bundleItems.Count;
	    for (int i = 0; i < bundleLen; ++i)
	    {
	        BundleItem item = bundleItems[i];
            if(BundleMemManager.Instance.LoadUseResource.ContainsKey(item.bundleType)) //忽略Resource的资源
                continue;
			if (BundleMemManager.Instance.CacheBunldeDic.ContainsKey(item.bundleType))
			{
			    Dictionary<CHARACTER_CAREER, BundleItem> itemDics = BundleMemManager.Instance.CacheBunldeDic[item.bundleType];
			    if (itemDics == null)
			    {
			        itemDics = new Dictionary<CHARACTER_CAREER, BundleItem>();
			        BundleMemManager.Instance.CacheBunldeDic[item.bundleType] = itemDics;
			    }
                itemDics[item.career] = item;
	        }
            if (item.bundleVersion > maxVersion)
                maxVersion = item.bundleVersion;
            if (item.bundleVersion <= storeVersion) //比上一个下载的版本号下那么就无需下载
			{
				continue;
			}						
			string urlPath = BundleMemManager.Instance.getBundleUrl() + item.bundleName + BundleMemManager.BUNDLE_SUFFIX;
__Retry:
            WWW www = WWW.LoadFromCacheOrDownload(urlPath, item.bundleVersion);
			yield return www;
			if (www.error != null)
			{
                Debug.LogError("WWW download scene <" + item.bundleName + "> had an error:" + www.error);
				yield return new WaitForSeconds(1.0f);
				goto __Retry;
			}
			else 
			{
				www.assetBundle.Unload(true);
				Resources.UnloadUnusedAssets();
                Debug.Log("WWW download scene <" + item.bundleName + "> success!");
                setBarPos((float)(i + 1) / (float)bundleLen, true);
			}
		}
        PlayerPrefs.SetInt(BUNDLE_VERSION, maxVersion);
	    yield return new WaitForSeconds(0.1f);
		m_bDownloadFinish = true;
        BundleMemManager.Instance.preLoadBundle();
		StartCoroutine(LoadSceneAssetsBundle());
	}
	
    //读取加载bundle的配置文件
    void read_config(DataReadBase dataBase, string text)
    {
        Debug.Log("begin read config : " + dataBase.path);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode(dataBase.getRootNodeName()).ChildNodes;
        for (int k = 0; k < nodeList.Count; k++)
        {
            XmlElement xe = nodeList.Item(k) as XmlElement;
            if (xe == null)
                continue;
            if (k == 0)
            {
                continue;
            }
            string key = xe.GetAttribute("ID");
            for (int i = 0; i < xe.Attributes.Count; i++)
            {
                XmlAttribute attr = xe.Attributes[i];
                dataBase.appendAttribute(int.Parse(key), attr.Name, attr.Value);
            }
        }
    }


    //这里加载的是选择创角的场景
    IEnumerator LoadSceneAssetsBundle()
    {
        int len = BundleMemManager.Instance.AllSceneBundles.Count;
        string[] keys = BundleMemManager.Instance.AllSceneBundles.Keys.ToArray();
        for (int i = 0; i < len; i++)
        {
            string bundleName = keys[i];
            System.Object value = BundleMemManager.Instance.AllSceneBundles[bundleName];
            if (value is BundleItem) //就是assetBundle还没有加载
            {
                BundleItem item = value as BundleItem;
                 if (BundleMemManager.Instance.LoadUseResource.ContainsKey(item.bundleType)) //忽略Resource的资源
                    continue;
                string urlPath = BundleMemManager.Instance.getBundleUrl() + bundleName;
                float t1 = Time.realtimeSinceStartup;
                WWW www = WWW.LoadFromCacheOrDownload(urlPath, item.bundleVersion);
                yield return www;
                if (www.error != null)
                {
					Debug.LogError(bundleName + " the www error is " + www.error);
                    continue;
                }
                float t2 = Time.realtimeSinceStartup;
                //Debug.LogError("load asset " + bundleName + " time is " + (t2 - t1));
                AssetBundle assetBundle = www.assetBundle;
                BundleMemManager.Instance.addBundleToMem(bundleName, assetBundle, item.bundleType);
                setBarPos(i / ((float)len));
                if (item.subPrefabs.Count > 0 && item.bundleType != EBundleType.eBundleShader
				    && item.bundleType != EBundleType.eBundleConfig && item.bundleType != EBundleType.eBundleScene)
                {
                    for (int j = 0; j < item.subPrefabs.Count; j++)
                    {
                        string prefabName = item.subPrefabs[j];
                        GameObject obj = assetBundle.Load(prefabName, typeof(GameObject)) as GameObject;
						if(obj == null)
							Debug.LogError("the null object name is: " + prefabName);
                        BundleMemManager.Instance.addScenePrefab(item.bundleType, obj.name, bundleName, obj);
                    }
                }             
                if (item.bundleType == EBundleType.eBundleShader)
                    BundleMemManager.Instance.ShaderBundle = assetBundle;
                else if (item.bundleType == EBundleType.eBundleConfig)
                    BundleMemManager.Instance.ConfigBundle = assetBundle;
            }
            yield return new WaitForSeconds(0.01f);
        }
        _loadBundleSuccess = true;
    }
}
