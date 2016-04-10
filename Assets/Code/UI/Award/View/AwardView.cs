using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using System;
using mediator;
using helper;

public class AwardView : MonoBehaviour
{

	const int MAX_START = 3;
	const float LATER_TIME = 0.5f;
    private const string BIG_STAR = "Effect/Effect_Prefab/UI/TongGuan_star01";
    private const string SMALL_STAR = "Effect/Effect_Prefab/UI/TongGuan_star02";
	/// <summary>
	/// 评价等级
	/// </summary>
	public enum AssessLevel
	{ 
		NoseOut=1,    //险胜
		Win,        //胜利
		Perfect     //完美胜利
	}
	//评价功能
	private const string assess1 = "tongguan_xiansheng";      //险胜图片名称
	private const string assess2 = "tongguan_tongguan";     //通关
	private const string assess3 = "tongguan_wanmei";       //完美通关
	private const string startName = "tongguan_star";
	private const string startBgName = "tongguan_star_bg";
	private UISprite _assessSp;
	private UILabel _exp;
	private UILabel _gold;
	private UILabel _time;
	private UILabel _title;
	private GameObject itemPrefab;
	private Transform _grid;
	private GameObject _info;
	private GameObject[] _starts;
	private bool isOk = false;
    
	private void Awake ()
	{
		_starts = new GameObject[MAX_START];
		for (int i = 0; i < MAX_START; i++) {
			_starts [i] = transform.FindChild ("Award_Function/Desc/Starts/Start" + (i + 1)).gameObject;
			_starts [i].SetActive (false);
		}
	    if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
	    {
	        BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, BIG_STAR,
	            (asset1) =>
	            {
                    BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, SMALL_STAR,
                    (asset2) =>
                    {
                        setStart((GameObject)asset1, (GameObject)asset2);
                    });
	            });
	    }
	    else
	    {
	        GameObject asset1 = BundleMemManager.Instance.getPrefabByName(BIG_STAR, EBundleType.eBundleUIEffect);
            GameObject asset2 = BundleMemManager.Instance.getPrefabByName(SMALL_STAR, EBundleType.eBundleUIEffect);
            setStart(asset1, asset2);
	    }
		_info = transform.FindChild ("Award_Function/Desc/Info").gameObject;
		_assessSp = transform.FindChild ("Award_Function/Desc/Starts/Assess").GetComponent<UISprite> ();

		_exp = transform.FindChild ("Award_Function/Desc/Info/lbl_Exp").GetComponent<UILabel> ();
		_gold = transform.FindChild ("Award_Function/Desc/Info/lbl_Gold").GetComponent<UILabel> ();
		_time = transform.FindChild ("Award_Function/Desc/Info/lbl_Time").GetComponent<UILabel> ();
		_title = transform.FindChild ("Award_Function/Desc/Info/lbl_Title").GetComponent<UILabel> ();
		itemPrefab = transform.FindChild ("Award_Function/ItemPrefab").gameObject;
		_grid = transform.FindChild ("Award_Function/Items").transform;
		itemPrefab.SetActive (false);
	}

    private void setStart(GameObject bigStar, GameObject samllStar)
    {
        for (int i = 0; i < MAX_START; i++)
        {
            GameObject temp = samllStar;
            if (i == 1)
                temp = bigStar;
            GameObject obj = BundleMemManager.Instance.instantiateObj(temp);
            obj.transform.parent = _starts[i].transform;
            obj.transform.localPosition = Vector3.zero;
        }
    }

	private void Start ()
	{
		isOk = false;
		Gate.instance.sendNotification (MsgConstant.MSG_AWARD_INITIAL_DATA);
		//DisplayStartAssess(AwardManager.Instance.Vo.AssessLvl);
		Hidden ();
		StartCoroutine (LaterDisplay ((int)AwardManager.Instance.Vo.AssessLvl));
		
		switch (AwardManager.Instance.Vo.AssessLvl) {
		case AssessLevel.NoseOut:
			_assessSp.spriteName = "tongguan_xiansheng";
			break;
		case AssessLevel.Win:
			_assessSp.spriteName = "tongguan_tongguan";
			break;
		case AssessLevel.Perfect:
			_assessSp.spriteName = "tongguan_wanmei";
			break;
		
		default:
			break;
		}
		_assessSp.MakePixelPerfect();
		
		
		StartDisplayInfo ();
	}

	private void OnEnable ()
	{
		Gate.instance.registerMediator (new AwardMediator (this));
	}

	private void OnDisable ()
	{
		Gate.instance.removeMediator (mediator.MediatorName.AWARD_MEDIATOR);
	}

	private IEnumerator LaterDisplay (int level)
	{
		int max = level;
		int cur = 0;
		while (true) {
			yield return new WaitForSeconds(LATER_TIME);
			_starts [cur].SetActive (true);
			cur++;
			if (cur >= max) {
				break;
			}
		}
	}

	private void Hidden ()
	{
		_info.SetActive (false);
		_assessSp.alpha = 0;
		itemPrefab.SetActive (false);
	}

	private void Show ()
	{
		_info.SetActive (true);
		_assessSp.alpha = 1;
		itemPrefab.SetActive (true);
	}

	public void StartDisplayInfo ()
	{
		if (!isOk) {
			StartCoroutine (LayerDisplayInfo ());  
		}
	}

	private IEnumerator LayerDisplayInfo ()
	{
		//星星显示时间+1
		float time = ((int)AwardManager.Instance.Vo.AssessLvl + 1) * LATER_TIME;
		yield return new WaitForSeconds(time);
		Show ();
		DisplayAwardInfo ();
		DisplayAwardItems ();
	}

	private void DisplayAwardInfo ()
	{
		_exp.text = AwardManager.Instance.Vo.GetExp.ToString ();
		_gold.text = AwardManager.Instance.Vo.GetMoney.ToString ();
        _time.text = string.Format ("{0}:{1}", AwardManager.Instance.Vo.GetTime / 60, AwardManager.Instance.Vo.GetTime % 60);
        _time.text = PlayTime();
		_title.text = helper.ColorConst.Format (Constant.YELLOW, AwardManager.Instance.Vo.MapName);
	}

    private string PlayTime()
    {
        string s = transform.parent.FindChild("ui_fight/Top/Time_Function/Label").GetComponent<UILabel>().text;
        return s;
    }

	private void DisplayAwardItems ()
	{
		isOk = true;
		int width = 88;
		BetterList<ItemStruct> bs = AwardManager.Instance.Vo.CurAwardItemList;
		if (bs.size == 0) {
			itemPrefab.SetActive (false);
			return;
		}
		bool isJishu = bs .size % 2 == 0 ? false : true;

		Vector3 pos = new Vector3 (0, 0, 0);
		if (bs.size == 1) {
			AddPrefab (pos, bs [0], 0);
			return;
		}
		int count = (bs.size - 1) / 2;
		int lostX = count * -width;
		int ouShuLostX = (bs.size / 2) * -width + width / 2;
		for (int i = 0; i < bs.size; i++) {
			if (isJishu) {
				pos.x = lostX + width * i;
				AddPrefab (pos, bs [i], i);
			} else {
				pos.x = ouShuLostX + width * i;
				AddPrefab (pos, bs [i], i);
			}
		}
        
	}

	private void AddPrefab (Vector3 pos, ItemStruct item, int index)
	{
		itemPrefab.SetActive (true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(itemPrefab);
		obj.transform.parent = _grid;
		obj.name = index.ToString ();
		obj.transform.localPosition = pos;
		obj.transform.localScale = new Vector3 (1, 1, 1);
		obj.transform.FindChild ("Boder").GetComponent<UISprite> ().spriteName = ViewHelper.GetBoderById ((int)item.tempId);
        string iconName = AwardManager.Instance.GetTemplateByTempId(item.tempId).icon;
        Debug.Log("AwardItem:"+iconName);
        obj.transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = SourceManager.Instance.getTextByIconName(iconName);
		obj.transform.FindChild ("Label").GetComponent<UILabel> ().text = string.Format ("x{0}", item.num);
		itemPrefab.SetActive (false);
	}


	void OnClick ()
	{
		if (isOk) {
			isOk = false;
			//发送回城
			Gate.instance.sendNotification (MsgConstant.MSG_AWARD_GO_HOME);
		}
	}

}
