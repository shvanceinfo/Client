/**该文件实现的基本功能等
function: 实现翅膀的培养功能
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System;
using System.Text;
using helper;

public class WingView : MonoBehaviour
{
	const int ATTR_ROW_HEIGHT = 30;	//属性的行高
	const int ATTR_COL_WIDTH = 220; //属性的列宽
	const string YUYI_STAR = "Effect/Effect_Prefab/UI/UI_yuyi_xingxing";
	private Transform _btnTrans;  //按钮控件
	private HealthBar _expBar;      //经验
	private Transform _starTrans;
	private GameObject _evoInfo;	//进阶的相关信息
	private GameObject _cultureInfo; //培养的相关属性
	private GameObject _attrTemplate;	//属性模板
	private GameObject _upArrow; 		//上箭头
    private GameObject _downArrow; 		//下箭头
    private GameObject _timeInfo; 		//时间
//	private UILabel _diamondNum;
//	private UILabel _goldNum;
	private UILabel _wingLadder; //阶数
	private UILabel _wingName; //翅膀名称
	
	private bool _isCulture; //是否是在培养
	private uint _lastStar; //上次的星星
	private string _lastLadder;//上次的阶层
	
	private Transform _timeObj;//时间对象
	private UILabel _time;
	private bool _isAutoTime;  //当前是否在倒计时
	private float countTime = 0;
	private DateTime EndTime;

    private Transform _modelRoot;
    GameObject obj;
    private GameObject prefSuccess;

	void Awake ()
	{
        _btnTrans = transform.Find("middle/right/btn");
        _timeInfo = transform.Find("middle/right/time").gameObject;
		_attrTemplate = transform.Find ("middle/right/attr/attrTemplate").gameObject;
        _evoInfo = transform.Find("middle/right/luckInfo").gameObject;
        _evoInfo.transform.FindChild("luckInfo").GetComponent<UILabel>().text = LanguageManager.GetText("wing_luckinfo_text");
		_cultureInfo = transform.Find ("middle/right/consume").gameObject;
		_starTrans = transform.Find ("middle/right/star");
		_expBar = transform.Find ("middle/right/expBar").GetComponent<HealthBar> ();
//		_diamondNum = transform.Find("top/diamond/num").GetComponent<UILabel>();
//		_goldNum = transform.Find("top/gold/num").GetComponent<UILabel>();	
		_wingLadder = transform.Find ("middle/left/wingLadder").GetComponent<UILabel> ();
		_wingName = transform.Find ("middle/right/title/title").GetComponent<UILabel> ();	
		_upArrow = transform.Find ("background/upArrow").gameObject;
		_downArrow = transform.Find ("background/downArrow").gameObject;
		
		_timeObj = transform.FindChild ("middle/right/time");
		_time = transform.FindChild ("middle/right/time/time").GetComponent<UILabel> ();
        prefSuccess = transform.FindChild("background/SuccessAnim").gameObject;


        _modelRoot = transform.FindChild("background/Sprite");
        obj = Instantiate(_modelRoot.gameObject) as GameObject;
        obj.transform.parent = _modelRoot;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.name = "pos";
        
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new WingMediator (this));
//		WingManager.Instance.initWing(1001, 40, 40);
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.WING_MEDIATOR);
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
	//显示培养还是进化
	public void showView (bool isCulture)
	{
		_isCulture = isCulture;
//		GameObject cultureAuto = _btnTrans.Find("btnAutoCulture").gameObject;
		GameObject evoAuto = _btnTrans.Find ("btnAuto").gameObject;
//		GameObject culture = _btnTrans.Find("btnCulture").gameObject;
		GameObject evo = _btnTrans.Find ("btnEvolution").gameObject;
//		if(isCulture)
//		{
//			_cultureInfo.SetActive(true);
//			culture.SetActive(true);
//			cultureAuto.SetActive(true);
//			culture.GetComponent<BoxCollider>().enabled = true;
//			cultureAuto.GetComponent<BoxCollider>().enabled = true;
//			_evoInfo.SetActive(false);
//			evo.SetActive(false);
//			evoAuto.SetActive(false);
//			evo.GetComponent<BoxCollider>().enabled = false;
//			evoAuto.GetComponent<BoxCollider>().enabled = false;
//		}
//		else
//		{
//			_evoInfo.SetActive(true);
//			evo.SetActive(true);
//			evoAuto.SetActive(true);
//			evo.GetComponent<BoxCollider>().enabled = true;
//			evoAuto.GetComponent<BoxCollider>().enabled = true;
//			_cultureInfo.SetActive(false);
//			culture.SetActive(false);
//			cultureAuto.SetActive(false);
//			culture.GetComponent<BoxCollider>().enabled = false;
//			cultureAuto.GetComponent<BoxCollider>().enabled = false;
//		}
		WingManager.Instance.previewLadder = WingManager.Instance.CurrentLevel;
		
		enableButton ();
		
		hiddenArrow ();
		updateAttr (WingManager.Instance.CurrentWing.attrTypes, WingManager.Instance.CurrentWing.attrValues);
		updateMoney (); //初始化获取人物身上货币
		updateMaterial (WingManager.Instance.CurrentWing);
	}

    public void SuccessPlayAnim()
    {
        prefSuccess.SetActive(true);
        GameObject obj = BundleMemManager.Instance.instantiateObj(prefSuccess);
        obj.transform.parent = transform.FindChild("background");
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0,25,0);
        prefSuccess.SetActive(false);
        SkillTalentManager.Instance.effectObjList.Add(obj);
        Destroy(obj,1.5f);
    }

	public void SetCamera (bool isactive)
	{
		NPCManager.Instance.ModelCamera.gameObject.SetActive (isactive);
	}
	//名称，阶数，翅膀模型的变化
	public void updateLadder (string wingName, string ladder, string modelName, Vector3 modelPos,Vector3 modelScale)
	{
		_wingName.text = wingName;
		_wingLadder.text = ladder;
		NPCManager.Instance.createCamera (false); //先消除原来的3D相机
	    GameObject model = null;
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleWing))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleWing, modelName,
                (obj) =>
                {
                    _modelRoot = transform.Find("background/Sprite");
                    _modelRoot.localPosition = _modelRoot.localPosition + modelPos;

                    Vector3 pos = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                        NPCManager.Instance.ModelCamera, _modelRoot.position);

                    _modelRoot.localPosition = _modelRoot.localPosition - modelPos;

                    model = BundleMemManager.Instance.instantiateObj(obj, Vector3.zero, Quaternion.identity);
                    model.name = "wing_ui";
                    model.transform.parent = NPCManager.Instance.ModelCamera.transform;
                    model.transform.position = pos;
                    model.transform.localScale = modelScale;
                    model.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));	
                });
        }
        else
        {
            _modelRoot = transform.Find("background/Sprite");
            _modelRoot.localPosition = _modelRoot.localPosition + modelPos;

            Vector3 pos = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
                NPCManager.Instance.ModelCamera, _modelRoot.position);

            _modelRoot.localPosition = _modelRoot.localPosition - modelPos;

            GameObject asset = BundleMemManager.Instance.getPrefabByName(modelName, EBundleType.eBundleWing);
            model = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            model.name = "wing_ui";
            model.transform.parent = NPCManager.Instance.ModelCamera.transform;
            model.transform.position = pos;
            model.transform.localScale = modelScale;
            model.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            ToolFunc.SetLayerRecursively(model, LayerMask.NameToLayer("TopUI"));	
        }				
	}
	
	//升级的时候触发的升阶效果
	public void updateLevelUpLadder (string wingName, string ladder, string modelName)
	{
		if (!string.IsNullOrEmpty (this._lastLadder) && ladder != this._lastLadder) { //升阶层的时候触发
			FloatMessage.GetInstance ().PlayImageAdvanceFloatMessage (UIManager.Instance.getRootTrans ()); //字体进阶效果
		}
		this._lastLadder = ladder;
		
		
		#region 星星显示
		for (int i=1; i<=10; i++) {
			GameObject star = _starTrans.Find ("star" + i).gameObject;
			if (i <= WingManager.Instance.CurrentLevel)
				star.SetActive (true);
			else
				star.SetActive (false);
		}
		if (this._lastStar != 0 && WingManager.Instance.CurrentLevel > this._lastStar) { //升级时候出现的粒子效果
			Transform star = _starTrans.Find ("star" + WingManager.Instance.CurrentLevel);
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, YUYI_STAR,
                    (asset) =>
                    {
                        GameObject yuyiStar = BundleMemManager.Instance.instantiateObj(asset, star.position, Quaternion.identity); //星星效果
                        Destroy(yuyiStar, 2.3f);
                        FloatMessage.GetInstance().PlayImageLevelUpFloatMessage(UIManager.Instance.getRootTrans()); //字体升级效果
                    });
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(YUYI_STAR, EBundleType.eBundleUIEffect);
                GameObject yuyiStar = BundleMemManager.Instance.instantiateObj(asset, star.position, Quaternion.identity); //星星效果
                Destroy(yuyiStar, 2.3f);
                FloatMessage.GetInstance().PlayImageLevelUpFloatMessage(UIManager.Instance.getRootTrans()); //字体升级效果
            }
		}
		this._lastStar = WingManager.Instance.CurrentLevel;// 设置最后更新的星星数量
		#endregion
		
		
	}
	
	
//	//经验条的变化
//	public void updateExp(uint exp, uint maxExp, uint starNum)
//	{
//        _expBar.MaxValue = (int)maxExp;
//        _expBar.Value = (int)exp;
//		for(int i=1; i<=10; i++)
//		{
//			GameObject star = _starTrans.Find("star"+i).gameObject;
//			if(i<=starNum)
//				star.SetActive(true);
//			else
//				star.SetActive(false);
//		}
//		if (this._lastStar!=0 && starNum>this._lastStar) { //升级时候出现的粒子效果
//			Transform star = _starTrans.Find("star"+starNum);
//			GameObject yuyiStar  = Instantiate(BundleMemManager.Instance.loadResource(YUYI_STAR,typeof(GameObject)),star.position,Quaternion.identity) as GameObject; //星星效果
//			Destroy(yuyiStar,2.3f);
//			FloatMessage.GetInstance().PlayImageLevelUpFloatMessage(UIManager.Instance.getRootTrans()); //字体升级效果
//		}
//		this._lastStar = starNum;// 设置最后更新的星星数量
//	}
	
	//成功率，幸运点的变化
	public void updateLuckPoint (uint luckNum, uint maxLuckNum, string successRate)
	{
		if (WingManager.Instance.CurrentLevel == 10) {
			_expBar.MaxValue = 999;
			_expBar.Value = 999;
		} else {
			_expBar.MaxValue = (int)maxLuckNum;
			_expBar.Value = (int)luckNum;
		}
 
		_evoInfo.transform.Find ("successState").GetComponent<UILabel> ().text = successRate;
	}
	
	//金钱钻石的变化
	public void updateMoney ()
	{
//		_goldNum.text = CharacterPlayer.character_asset.gold.ToString();
//		_diamondNum.text = CharacterPlayer.character_asset.diamond.ToString();
	}
	
	//自动培养，自动进阶
	public void autoCultureEvo (bool isCulture)
	{
		changeBtn (isCulture, true);
//		if(isCulture)
//			StartCoroutine(autoCulture());
//		else
		StartCoroutine (this.autoEvolution ());
	}
	
	//切换按钮的状态
	public void changeBtn (bool isCulture, bool beginAuto)
	{
		UILabel btnLabel;
//		if(isCulture)
//		{
//			btnLabel = _btnTrans.Find("btnAutoCulture/Label").GetComponent<UILabel>();
//			if(beginAuto)
//				btnLabel.text = LanguageManager.GetText("wing_stop_culture");
//			else
//				btnLabel.text = LanguageManager.GetText("wing_auto_culture");
//		}
//		else
//		{
		btnLabel = _btnTrans.Find ("btnAuto/Label").GetComponent<UILabel> ();
        if (beginAuto)
        {
            btnLabel.text = LanguageManager.GetText("wing_stop_evolution");
            _btnTrans.Find("btnAuto/Label").GetComponent<UILabel>().effectColor = Color.yellow;
            _btnTrans.FindChild("btnAuto/Sprite").GetComponent<UISprite>().spriteName = "shop_btn1";
        }
        else
        {
            btnLabel.text = LanguageManager.GetText("wing_auto_evolution");
            _btnTrans.Find("btnAuto/Label").GetComponent<UILabel>().effectColor = Color.cyan ;
            //_btnTrans.FindChild("btnAuto/Sprite").GetComponent<UISprite>().spriteName = "common_button1";
        }
//		}
	}
	
	//自动培养，自动进阶时提示相关错误信息
	public void showErrMsg (string msgSymbol)
	{
		string errMsg = LanguageManager.GetText (msgSymbol);
		FloatMessage.GetInstance ().PlayFloatMessage (errMsg, UIManager.Instance.getRootTrans (), 
		                                            new Vector3 (0f, 200f, -150f), new Vector3 (0f, 280f, -150f));
	}
	
	//显示点击后的效果
	public void showEffect (string doubleStr, string floatMsg, string effectName)
	{

		int doubleNum = int.Parse (doubleStr);
		if (doubleNum == 1)
			FloatMessage.GetInstance ().PlayFloatMessage (floatMsg, UIManager.Instance.getRootTrans (), 
		                                            new Vector3 (0f, -160f, -50f), new Vector3 (0f, -100f, -50f));
		else {
			floatMsg = "暴击X" + doubleStr + " " + floatMsg;
			FloatMessage.GetInstance ().PlayFloatMessage (floatMsg, UIManager.Instance.getRootTrans (), 
		                                            new Vector3 (0f, -160f, -50f), new Vector3 (0f, -100f, -50f));
		}
	}
	
	//按钮变灰或者变正常
	public void enableButton ()
	{
		if (WingManager.Instance.previewLadder == WingManager.Instance.CurrentLevel && WingManager.Instance.CurrentLevel < 10) { //当前阶数就是预览阶数就使能按钮
//			if(_isCulture)
//			{
//				GameObject cultureAuto = _btnTrans.Find("btnAutoCulture").gameObject;
//				GameObject culture = _btnTrans.Find("btnCulture").gameObject;
//				culture.GetComponent<BoxCollider>().enabled = true;
//				cultureAuto.GetComponent<BoxCollider>().enabled = true;
//				_btnTrans.Find("btnAutoCulture/Sprite").GetComponent<UISprite>().spriteName = ACTIVE_BTN_NAME;
//				_btnTrans.Find("btnCulture/Sprite").GetComponent<UISprite>().spriteName = ACTIVE_BTN_NAME;
//			}
//			else
//			{
			GameObject evoAuto = _btnTrans.Find ("btnAuto").gameObject;
			GameObject evo = _btnTrans.Find ("btnEvolution").gameObject;
			evo.GetComponent<BoxCollider> ().enabled = true;
			evoAuto.GetComponent<BoxCollider> ().enabled = true;
            if (evoAuto.transform.FindChild("Label").GetComponent<UILabel>().text.Equals("自动进阶"))
               _btnTrans.Find("btnAuto/Sprite").GetComponent<UISprite>().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
            _btnTrans.Find("btnEvolution/Sprite").GetComponent<UISprite>().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
            _btnTrans.FindChild("Label").gameObject.SetActive(false);


//			}

            _timeInfo.SetActive(true);
            _evoInfo.transform.FindChild("consumeLbl").gameObject.SetActive(true);
            _evoInfo.transform.FindChild("stone").gameObject.SetActive(true);
            _evoInfo.transform.FindChild("stoneNum").gameObject.SetActive(true);
            _btnTrans.transform.FindChild("btnEvolution").gameObject.SetActive(true);
            _btnTrans.transform.FindChild("btnAuto").gameObject.SetActive(true);
            _btnTrans.transform.FindChild("btnRet").gameObject.SetActive(false);
            _evoInfo.transform.FindChild("info").gameObject.SetActive(false);


		} else { //按钮变灰
//			if(_isCulture)
//			{
//				GameObject cultureAuto = _btnTrans.Find("btnAutoCulture").gameObject;
//				GameObject culture = _btnTrans.Find("btnCulture").gameObject;
//				culture.GetComponent<BoxCollider>().enabled = false;
//				cultureAuto.GetComponent<BoxCollider>().enabled = false;
//				_btnTrans.Find("btnAutoCulture/Sprite").GetComponent<UISprite>().spriteName = IN_ACTIVE_BTN_NAME;
//				_btnTrans.Find("btnCulture/Sprite").GetComponent<UISprite>().spriteName = IN_ACTIVE_BTN_NAME;
//			}
//			else
//			{
			GameObject evoAuto = _btnTrans.Find ("btnAuto").gameObject;
			GameObject evo = _btnTrans.Find ("btnEvolution").gameObject;
			evo.GetComponent<BoxCollider> ().enabled = false;
			evoAuto.GetComponent<BoxCollider> ().enabled = false;

			_btnTrans.Find ("btnAuto/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			_btnTrans.Find ("btnEvolution/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
//			}

            _timeInfo.SetActive(false);
            _evoInfo.transform.FindChild("consumeLbl").gameObject.SetActive(false);
            _evoInfo.transform.FindChild("stone").gameObject.SetActive(false);
            _evoInfo.transform.FindChild("stoneNum").gameObject.SetActive(false);
            _btnTrans.transform.FindChild("btnEvolution").gameObject.SetActive(false);
            _btnTrans.transform.FindChild("btnAuto").gameObject.SetActive(false);


            if (WingManager.Instance.previewLadder != 10 || WingManager.Instance.CurrentLevel != 10)
                _btnTrans.transform.FindChild("btnRet").gameObject.SetActive(true);
            _evoInfo.transform.FindChild("info").gameObject.SetActive(true);
            _evoInfo.transform.FindChild("info").GetComponent<UILabel>().text = LanguageManager.GetText("wing_luckinfo_info");
            _btnTrans.FindChild("Label").gameObject.SetActive(false);

            if (WingManager.Instance.CurrentLevel == 10)
                _btnTrans.FindChild("Label").gameObject.SetActive(true);
        }
		hiddenArrow ();
	}
	
	//属性的添加修改
	public void updateAttr (BetterList<int> attrNames, BetterList<int> attrValues)
	{
		int len = attrNames.size < attrValues.size ? attrNames.size : attrValues.size;
		float posX = 0;
		float posY = 0;
		_attrTemplate.SetActive (true); //先激活模板
		Transform trans = _attrTemplate.transform;
		foreach (Transform child in trans.parent) {
			if (child != trans)
				Destroy (child.gameObject);
		}
		for (int i=0; i<len; i++) {
			GameObject go = NGUITools.AddChild (trans.parent.gameObject, _attrTemplate);
			int rows = i / 2;
			if (i % 2 == 0) {
				posX = trans.localPosition.x;
				posY = trans.localPosition.y - rows * ATTR_ROW_HEIGHT;
			} else {
				posX = trans.localPosition.x + ATTR_COL_WIDTH;
				posY = trans.localPosition.y - rows * ATTR_ROW_HEIGHT;
			}
			go.transform.localPosition = new Vector3 (posX, posY, trans.localPosition.z);
			go.transform.localScale = new Vector3 (trans.localScale.x, trans.localScale.y, trans.localScale.z);
			UILabel title = go.transform.Find ("attrTitle").GetComponent<UILabel> ();
			UILabel num = go.transform.Find ("attrNum").GetComponent<UILabel> ();
			title.text = EquipmentManager.GetEquipAttributeName ((eFighintPropertyCate)attrNames [i], true);
            string s = PowerManager.Instance.ChangeInfoData((eFighintPropertyCate)attrNames[i], attrValues[i]);
			num.text = "+" + s;
			go.name = "attr" + (i + 1);
		}
		_attrTemplate.SetActive (false);
	}



	//培养丹，洗炼丹的变化
	public void updateMaterial (WingVO wing)
	{
		uint ownerNum = 0;
		uint needNum = 0;
//		if(_isCulture)
//		{
//			ownerNum = ItemManager.GetInstance().GetItemNumById(WingManager.Instance.CurrentWing.cultureItemID); //得到当前培养石
//			needNum = wing.cultureItemNum;
////			_rightCulture.transform.Find("possess/num").GetComponent<UILabel>().text = "X" + ownerNum;
//			if (ownerNum>needNum) {
//				_cultureInfo.transform.Find("stoneNum").GetComponent<UILabel>().text = "[00ff00]"+ ownerNum +"[-]" + "/" + needNum;  //需要的培养石显示
//			}else{
//				_cultureInfo.transform.Find("stoneNum").GetComponent<UILabel>().text = "[ff0000]"+ownerNum +"[-]" + "/" + needNum;  //需要的培养石显示
//			}
//			_cultureInfo.transform.Find("goldNum").GetComponent<UILabel>().text = "X"+wing.costGold;       //需要花费的金币
////			_cultureInfo.transform.Find("stone").GetComponent<UISprite>().spriteName = "";
////			UITexture icon1 = _cultureInfo.transform.Find("possess/stone").GetComponent<UITexture>();
//			UITexture icon2 = _cultureInfo.transform.Find("stone").GetComponent<UITexture>();		
//			ItemTemplate item = ItemManager.GetInstance().GetTemplateByTempId(WingManager.Instance.CurrentWing.cultureItemID);	
////			DealTexture.Instance.setTextureToIcon(icon1, item, false);
//			DealTexture.Instance.setTextureToIcon(icon2, item, false); //设置ICON的图片
//		}
//		else
//		{
		ownerNum = ItemManager.GetInstance ().GetItemNumById (WingManager.Instance.CurrentWing.evoCostItem); //得到当前的进阶石
		needNum = wing.evoNum;
//			_rightEvolution.transform.Find("possess/num").GetComponent<UILabel>().text = "X" + ownerNum;
		if (ownerNum >= needNum) { //需要的进阶石显示
			_evoInfo.transform.Find ("stoneNum").GetComponent<UILabel> ().text = "[00ff00]" + ownerNum + "[-]" + "/" + needNum;  //需要的进阶石显示
		} else {
			_evoInfo.transform.Find ("stoneNum").GetComponent<UILabel> ().text = "[ff0000]" + ownerNum + "[-]" + "/" + needNum;  //需要的进阶石显示
		}
		_evoInfo.transform.Find ("goldNum").GetComponent<UILabel> ().text = "X" + wing.costGold;
//			_evoInfo.transform.Find("stone").GetComponent<UISprite>().spriteName = "";
//			UITexture icon1 = _rightEvolution.transform.Find("possess/stone").GetComponent<UITexture>();
		UITexture icon2 = _evoInfo.transform.Find ("stone").GetComponent<UITexture> ();
		ItemTemplate item = ItemManager.GetInstance ().GetTemplateByTempId (WingManager.Instance.CurrentWing.evoCostItem);
//			DealTexture.Instance.setTextureToIcon(icon1, item, false);
		DealTexture.Instance.setTextureToIcon (icon2, item, false);
//		}
	}
	
	//显示隐藏上线箭头
	private void hiddenArrow ()
	{
		if (WingManager.Instance.previewLadder <= 1) {
			_downArrow.SetActive (true);
			_upArrow.SetActive (false);
		} else if (WingManager.Instance.previewLadder >= 10) {
			_downArrow.SetActive (false);
			_upArrow.SetActive (true);
		} else {
			_downArrow.SetActive (true);
			_upArrow.SetActive (true);
		}
	}
	
	/// <summary>
	/// 显示倒计时
	/// </summary>
	/// <param name='luckNum'>
	/// Luck number.
	/// </param>
	/// <param name='isLimit'>
	/// Is limit.
	/// </param>
	public void ShowTime (uint luckNum, bool isLimit)
	{
		
		if (luckNum > 0) {
            this._timeObj.gameObject.SetActive(true);
            this._timeObj.transform.FindChild("info").GetComponent<UILabel>().text = LanguageManager.GetText("wing_time_control");
		} else {
			this._timeObj.gameObject.SetActive (false);
			this._isAutoTime = false;
		} //为0则隐藏
		
		
		if (isLimit) {	
			if (_isAutoTime == false) {
				this._isAutoTime = true;
				
				EndTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
				EndTime = EndTime.AddDays(1);
				TimeSpan dt = EndTime - DateTime.Now;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				this._time.text = sb.Append(dt.Hours).Append("时").Append(dt.Minutes).Append("分").Append(dt.Seconds).Append("秒").ToString();
			}
		} else {  //无限制的时候关闭协同
			this._time.text = "无限制";
		}
		
	}
	
 
	
//	//自动培养
//	IEnumerator autoCulture()
//	{
//		while (true) 
//		{
//			WingManager.Instance.beginCulture(true);
//			yield return new WaitForSeconds(0.2f);
//		}	
//	}
	
	//自动进阶
	IEnumerator autoEvolution ()
	{
		while (true) {
			WingManager.Instance.beginEvolute (true);
			yield return new WaitForSeconds(0.2f);
		}	
	}
 
	void Update ()
	{
 
		if (_isAutoTime) {
			countTime += Time.deltaTime;
			if (countTime > 1) {
				TimeSpan dt = EndTime - DateTime.Now;
				
				if (dt.Ticks<=0) {
					this._timeObj.gameObject.SetActive (false);
					this._isAutoTime = false;
					return;
				}
				StringBuilder sb = new StringBuilder();
				this._time.text = sb.Append(dt.Hours).Append("时").Append(dt.Minutes).Append("分").Append(dt.Seconds).Append("秒").ToString();
				countTime -=1;
	 
			}
		}



        //if (NPCManager.Instance.ModelCamera.transform.childCount > 0)
        //{

        //    _modelRoot = transform.FindChild("background/Sprite");


        //    Vector3 v3 = transform.FindChild("background/Sprite/pos").localPosition;
        //    _modelRoot.localPosition = _modelRoot.localPosition + v3;
        //    Vector3 pos = ViewHelper.UIPositionToCameraPosition(UIManager.Instance.getRootTrans().FindChild("Camera").GetComponent<Camera>(),
        //        NPCManager.Instance.ModelCamera, _modelRoot.position);
        //    _modelRoot.localPosition = _modelRoot.localPosition - v3;
        //    GameObject model = NPCManager.Instance.ModelCamera.transform.GetChild(0).gameObject;
        //    model.transform.position = pos;
        //}
	}
	
 
	
}
