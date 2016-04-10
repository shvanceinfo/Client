#define USE_ASSETBUNDLE
/**该文件实现的基本功能等
function:控制Loading的进度,并且保证loading条不小于1s
author:ljx
date:2011-11-14
**/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class LoadingProgress : MonoBehaviour 
{
	const float FINISH_LOAD_TIME = 1f; 
	const float TIPS_CHANGE_TIME = 3.0f;
	const float U3D_MAGIC_NUM = .9f; //magic number activation
	
	private UISprite _halfBar1;  //前背景
	private UISprite _halfBar2;	 //后背景
    private UILabel _loadFixTips;    //固定值
	private UILabel _loadTips;		//加载的TIP中
    private UILabel _loadProgress;  //加载的百分比
	private float _updateTimes; 	//加载的进度条总共话费的时间
	private float _currentTimePos;	//当前的Progress的时间进度,保证loading加载至少一秒
	private bool _loadScenefinish;   //是否加载场景完毕
    private bool _loadBundleFinish;  //加载不能动了结束
    private bool _freeMemSuccess;       //成功释放了内存
	private AsyncOperation _async;	//异步加载场景的同步操作  
	private bool _clearScene;
    private HealthBar _bar;
	private float m_fProgress;
    private int _totalBundleLen;    //所有的bundle数目
    private int _currentLoadNum;    //加载计数

	void Awake()
	{
		_bar = transform.FindChild("empty/loadingBar").GetComponent<HealthBar>();
		_bar.MaxValue = 100;
		_halfBar1 = transform.FindChild("empty/loadingBar/halfBar").GetComponent<UISprite>();
		_halfBar2 = transform.FindChild("empty/loadingBar/halfBar").GetComponent<UISprite>();
		_halfBar2.type = _halfBar1.type; //UISprite.Type.Filled; 
		_halfBar2.fillDirection = _halfBar1.fillDirection;
		_loadProgress = transform.FindChild("empty/loadProgress").GetComponent<UILabel>();
		_loadTips = transform.FindChild("empty/loadTip").GetComponent<UILabel>();
		_loadFixTips = transform.FindChild("empty/background/Label").GetComponent<UILabel>();
		SceneManager.Instance.clearSceneOver = false;
        m_fProgress = 0;
	    _currentLoadNum = 0;
	}
	
	void Start()
	{
		_halfBar1.fillAmount = 0f;
		_halfBar2.fillAmount = 0f;
		_currentTimePos = 0f;
		_updateTimes = 0;
        _loadScenefinish = false;
	    _loadBundleFinish = false;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);
		_loadTips.text = ConfigDataManager.GetInstance().getLoadingTipsConfig().getTipData(getRandom()).tip;
        _loadFixTips.text = ConfigDataManager.GetInstance().getLoadingTipsConfig().getTipData(Constant.LOAD_TIP_FIX).tip;
		_clearScene = true;
		NPCManager.Instance.hasInstantiate = false; //加载新场景销毁原NPC
		UIManager.Instance.closeAllUI(); //进入loading关闭所有UI，清理完真正释放
        BundleMemManager.Instance.addSceneBundle(CharacterPlayer.character_property.getServerMapID());
        List<string> removeList = BundleMemManager.Instance.getNeedFreeBundles();
        _totalBundleLen = removeList.Count + SourceManager.Instance.IconList.Count + BundleMemManager.Instance.UiTextures.Count
            + 1 + 1 + 1 + 1 + 1; //loadBundle, resources, intantiate, unloadunUsed, gc.collect
        StartCoroutine(freeAllMemory()); //先清除原来场景的Bundle
	    _freeMemSuccess = false;       
	}
	
	void Update()
	{
		//if(_clearTime > Constant.CLEAR_TIME && _clearScene)
        if(_clearScene)
		{
			SceneManager.Instance.changeScene();
			_clearScene = false;
		}
        if (_freeMemSuccess)
	    {
	        StartCoroutine(LoadSceneAssetsBundle());
	        _freeMemSuccess = false;
	    }
	    if (_loadBundleFinish)
	    {
            StartCoroutine(loadNewScene());
	        _loadBundleFinish = false;
	    }
		else if(_async != null && !_loadScenefinish) //没有加载完成才需要更新
		{
            _loadScenefinish = _async.progress >= U3D_MAGIC_NUM; 
			//updateProgress(_async.progress);			
		}
	    if (_loadScenefinish)
	    {
	        StartCoroutine(awakeScene());
	    }
	}
	
	void updateProgress(float currentPos)
	{
        //if(_currentTimePos >= FINISH_LOAD_TIME) //时间消耗已经超过一秒还在update
        //{
        //    if (_loadScenefinish) //加载场景结束，重置进度条满进度加载
        //    {
        //        setBarPos(FINISH_LOAD_TIME);
        //        //StartCoroutine(clearSceneOver());
        //    }			
        //    else
        //    {
        //        setBarPos(currentPos); //直接更新进度条
        //        if (_updateTimes > 2.0f)
        //        {
        //            _loadTips.text = ConfigDataManager.GetInstance().getLoadingTipsConfig().getTipData(getRandom()).tip;
        //        }
        //    }
        //} 
        //else //时间还在一秒内
        //{
        //    if (_loadScenefinish)//已经真正加载完毕
        //    {
        //        StartCoroutine(progressStatus());
        //    }
        //    else //没有真正加载完毕
        //    {
        //        int timeUpdateNum = (int)(_updateTimes/Constant.PROGRESS_UPDATE) + 1;  //下取整来判断使用时间基数加载还是原来的进度加载
        //        if(currentPos < timeUpdateNum*Constant.PROGRESS_UPDATE) //没有时间更新的快
        //        {
        //            setBarPos(currentPos);		
        //        }
        //        else //超过时间更新的时间
        //        {
        //            setBarPos(_currentTimePos);
        //        }
        //        _currentTimePos = timeUpdateNum*Constant.PROGRESS_UPDATE;
        //        _updateTimes += Time.deltaTime;
        //    }
        //}
	}
	
	//处理加载的线程
    //IEnumerator progressStatus()
    //{
    //    while (_currentTimePos < FINISH_LOAD_TIME) 
    //    {
    //        int random = getRandom();
    //        int smoothTime = 1;
    //        if(random < (int)(Constant.LOAD_TIP_MAX/8))
    //            smoothTime = 2*Constant.LOAD_TIP_MAX;
    //        yield return new WaitForSeconds(smoothTime*Constant.PROGRESS_UPDATE);
    //        if(random >= (int)(Constant.LOAD_TIP_MAX/8))
    //        {
    //            float deltaTime = Random.Range(0f, Constant.PROGRESS_UPDATE);
    //            _currentTimePos += deltaTime;	
    //            if(_currentTimePos >= FINISH_LOAD_TIME)
    //            {
    //                _currentTimePos = FINISH_LOAD_TIME;
    //                setBarPos(FINISH_LOAD_TIME);
    //                //StartCoroutine(clearSceneOver());
    //            }
    //            else
    //            {
    //                setBarPos(_currentTimePos);
    //            }
    //       }
    //    }		
    //}
	
	//场景清除完毕
    //IEnumerator clearSceneOver()
    //{
    //    while (true)
    //    {
    //        if (SceneManager.Instance.clearSceneOver && m_PreloadFinish) //线程已经停止
    //        {
    //            StopAllCoroutines();
    //            SourceManager.Instance.removeAllTexture(); //移除所有下载的icon
    //            Resources.UnloadUnusedAssets();
    //            System.GC.Collect();
    //            _async.allowSceneActivation = true;
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    IEnumerator freeAllMemory()
    {
        //释放bundle
        List<string> removeList = BundleMemManager.Instance.getNeedFreeBundles();     
        foreach (string bundleName in removeList)
        {
            BundleMemManager.Instance.freeOneBundle(bundleName);
            yield return new WaitForSeconds(0.1f); //每个bundle释放0.1s
            _currentLoadNum++;
            setBarPos(_currentLoadNum / (float)_totalBundleLen);
        }

        //释放icon texture 图集
        foreach (Texture2D texture in SourceManager.Instance.IconList.Values)
        {
            Resources.UnloadAsset(texture);
            yield return new WaitForSeconds(0.05f); //每个texture释放0.05s
            _currentLoadNum++;
            setBarPos(_currentLoadNum / (float)_totalBundleLen);
        }
        SourceManager.Instance.IconList.Clear();

        //释放UI prefab的图集
        UIDrawCall.ReleaseInactive();
        foreach (Texture texture in BundleMemManager.Instance.UiTextures.Values)
        {
            Resources.UnloadAsset(texture);
            yield return new WaitForSeconds(0.1f); //每个atlas释放0.1s
            _currentLoadNum++;
            setBarPos(_currentLoadNum / (float)_totalBundleLen);
        }
        BundleMemManager.Instance.UiTextures.Clear();

        //释放Reource load的资源
        for(int i=0; i<BundleMemManager.Instance.ResourceLoadObjs.Count; i++)
        {
            BundleMemManager.Instance.ResourceLoadObjs[i] = null;
        }   
        BundleMemManager.Instance.ResourceLoadObjs.Clear();
        _currentLoadNum++;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);
        yield return new WaitForSeconds(0.05f);

        int len = BundleMemManager.Instance.InstantiateObjs.Count;
		int freeCount = 0;
        for (int i = 0; i < len; i++)
        {
            GameObject obj = BundleMemManager.Instance.InstantiateObjs[i];
            if (obj != null)
            {
                Destroy(obj);
                obj = null;
				freeCount++;
            }
            if (i%20 == 0) //每0.1秒释放20个
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        BundleMemManager.Instance.InstantiateObjs.Clear();
        _currentLoadNum++;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);
 
        Resources.UnloadUnusedAssets(); //为了释放加载的texture资源
        yield return new WaitForSeconds(0.1f); //unloadUnused 释放 100ms
        _currentLoadNum++;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);
        System.GC.Collect();
        yield return new WaitForSeconds(0.1f);
        _currentLoadNum++;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);
        _freeMemSuccess = true;
    }

    //唤醒场景
    IEnumerator awakeScene()
    {
        //Shader.WarmupAllShaders();
        yield return new WaitForSeconds(0.1f);
        _async.allowSceneActivation = true;
        StopAllCoroutines();
    }
	
	//加载新场景时候的返回
	IEnumerator loadNewScene()
	{
        MapDataItem mapData = ConfigDataManager.GetInstance().getMapConfig().getMapData(CharacterPlayer.character_property.getServerMapID());
        _async = Application.LoadLevelAsync(mapData.sceneNO);
        if (EasyTouchJoyStickProperty.sJoystickProperty)
        {
            EasyTouchJoyStickProperty.SetJoyStickEnable(false);
            if(CharacterPlayer.sPlayerMe != null)
                CharacterPlayer.sPlayerMe.GetComponent<InputProperty>().ResetData();
        }
	    _async.allowSceneActivation = false;
		yield return _async;	
	}
	
	//设置加载条的跟随
	void setBarPos(float currentPos, string szAsset="")
	{
		if (m_fProgress >= currentPos && m_fProgress >0)
		{
			return;
		}
		m_fProgress = currentPos;
	    if (currentPos > 1f)
	        currentPos = 1f;
        _loadProgress.text = string.Format("{0}% ", (int)(currentPos * 100))+szAsset;

        _bar.Value = (int)(currentPos * 100);
        _bar.MaxValue = 100;
		if(currentPos <= .5f)
			_halfBar1.fillAmount = currentPos*2;
		else
		{
			_halfBar1.fillAmount = 1f;
			_halfBar2.fillAmount = (currentPos - .5f)*2;
		}
	}
	
	//随机数
	int getRandom()
    {
		return  Random.Range(Constant.LOAD_TIP_MIN, Constant.LOAD_TIP_MAX);
    }
	
    //加载场景的中需要的bundle(包括场景本身)
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
                    continue;
                }
				float t2 = Time.realtimeSinceStartup;
				//Debug.LogError("load asset " + bundleName + " time is " + (t2 - t1));
                AssetBundle assetBundle = www.assetBundle;
                BundleMemManager.Instance.addBundleToMem(bundleName, assetBundle, item.bundleType);
				if (item.bundleType != EBundleType.eBundleScene && item.subPrefabs.Count > 0)
                {
                    for (int j = 0; j < item.subPrefabs.Count; j++)
                    {
                        string prefabName = item.subPrefabs[j];
                        GameObject obj = assetBundle.Load(prefabName, typeof(GameObject)) as GameObject;
                        if(obj == null)
                            Debug.LogError("bundle name <"+ item.bundleName + "> prefab name <" + prefabName + "> not exist!");
                        //存在cache中bundle不保存prefab
                        if (!BundleMemManager.Instance.needResidentInMem(item.bundleType))
                            BundleMemManager.Instance.addScenePrefab(item.bundleType, obj.name, bundleName, obj);
                    }
                }                                
                //if (assetBundle.mainAsset != null)
                //{
                //    GameObject obj = assetBundle.mainAsset as GameObject;
                //    foreach (Transform childTrans in obj.transform)
                //    {
                //        GameObject childObj = childTrans.gameObject;
                //        //存在缓存中bundle不保存prefab
                //        if (!BundleMemManager.Instance.needResidentInMem(item.bundleType, true))
                //            BundleMemManager.Instance.addScenePrefab(item.bundleType, childObj.name, bundleName, childObj);
                //    }
                //}
				float t3 = Time.realtimeSinceStartup;
				//Debug.LogError("initiate bundle " + bundleName + " time is " + (t3-t2));			
	        }
			yield return new WaitForSeconds(0.01f);
	    }
        _currentLoadNum++;
        setBarPos(_currentLoadNum / (float)_totalBundleLen);      
		float t4 = Time.realtimeSinceStartup;
	    _loadBundleFinish = true;
	    _currentTimePos = FINISH_LOAD_TIME;
	    //t4 = Time.realtimeSinceStartup - t4;
	    //Debug.LogError("warmup shader  time is " + t4);
	}
}
