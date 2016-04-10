/**该文件实现的基本功能等
function: 实现宠物的培养功能
author:zyl
date:2014-06-03
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System;
using System.Text;
using System.Collections.Generic;
using helper;

public class PetView : MonoBehaviour
{
	const int ATTR_ROW_HEIGHT = 30;	//属性的行高
	const int ATTR_COL_WIDTH = 200; //属性的列宽
	 
	private Transform _trans;
	private Transform _btnTrans;  //按钮控件
	private HealthBar _expBar;      //经验

	private GameObject _evoInfo;	//进阶的相关信息
	private GameObject _attrTemplate;	//属性模板
	private GameObject _upArrow; 		//上箭头
	private GameObject _downArrow; 		//下箭头
	private Transform _btnFollow;	//宠物跟随
	
	
	private UILabel _petName; //宠物名称
	private UILabel _petTitle; //宠物名称与等级
	

	private string _lastPet;//上次的宠物
	private Transform _timeObj;//时间对象
	private UILabel _time;
	private bool _isAutoTime;  //当前是否在倒计时
	private float countTime = 0;
	private DateTime EndTime;
	private Transform _modelRoot;
	GameObject obj;
	private GameObject prefSuccess;

	#region tab
	Transform _right;
	Transform _rightBag;
	UICheckBoxColor _checkBoxTab1;
	UICheckBoxColor _checkBoxTab2;
	IList<ItemInfo> _petItemInfoList;
	public float Height = -98;
	public int pageSize = 4;
	int currentPagePetEquip = 1;
	Transform _itemContainer;
	Transform _itemTemp;
	UILabel _petEquipInfo;					//数量信息
	UIPanel _petEquipItemList;				//pet Panel
	UIScrollView _petEquipScrollView;		//pet Scroll View

	UISprite _toothBg;				//牙齿
	UITexture _toothItem;
	UISprite _clawBg;				//爪子
	UITexture _clawItem;
	UISprite _eyeBg;				//眼睛
	UITexture _eyeItem;
	UISprite _jewelryBg;			//宝珠
	UITexture _jewelryItem;
	UILabel _hiddenSale;
	UITexture _skillImg;		//图片				
	UILabel _skillName;			//技能名称
	UILabel _skillEffect;		//技能效果
	UILabel _skillUse;			//技能使用法


	#endregion

	#region 技能面板
	UITexture _skillImgBg;	  //技能图片
	UILabel _petSkillName;
	UILabel _petOnlyName;
	UILabel _petContent;
	UILabel _petEffectName;
	UILabel _petEffectContent;
	UILabel _petDescName;
	UILabel _petDescConent;
	GameObject _skillPanel;

	#endregion


	void Awake ()
	{
		_trans = this.transform;
		_btnTrans = _trans.Find ("middle/right/btn");
		_attrTemplate = _trans.Find ("middle/right/attr/attrTemplate").gameObject;
		_evoInfo = _trans.Find ("middle/right/luckInfo").gameObject;
		_evoInfo.transform.FindChild ("luckInfo").GetComponent<UILabel> ().text = LanguageManager.GetText ("pet_luckinfo_text");
		_expBar = _trans.Find ("middle/right/expBar").GetComponent<HealthBar> ();
		_petName = _trans.Find ("middle/left/petName").GetComponent<UILabel> ();
		_petTitle = _trans.Find ("middle/right/title/title").GetComponent<UILabel> ();	
		_upArrow = _trans.Find ("middle/left/upArrow").gameObject;
		_downArrow = _trans.Find ("middle/left/downArrow").gameObject;
		_timeObj = _trans.FindChild ("middle/right/time");
		_time = _trans.FindChild ("middle/right/time/time").GetComponent<UILabel> ();
		_btnFollow = _trans.FindChild ("middle/left/btnFollow");
		prefSuccess = transform.FindChild ("background/SuccessAnim").gameObject;


		_modelRoot = transform.FindChild ("background/Sprite");
		obj = Instantiate (_modelRoot.gameObject) as GameObject;
		obj.transform.parent = _modelRoot;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.name = "pos";


		this._right = this._trans.FindChild ("middle/right");
		this._rightBag = this._trans.FindChild ("middle/rightbag");
		_checkBoxTab1 = this._trans.FindChild ("Table/Table1").GetComponent<UICheckBoxColor> ();
		_checkBoxTab2 = this._trans.FindChild ("Table/Table2").GetComponent<UICheckBoxColor> ();
		this._itemTemp = this._trans.FindChild ("middle/rightbag/content/itemlist/gird/itemTemp");
		this._petEquipInfo = this._trans.FindChild ("middle/rightbag/content/btn/info").GetComponent<UILabel> ();
		this._petEquipItemList = this._trans.FindChild ("middle/rightbag/content/itemlist").GetComponent<UIPanel> ();
		this._petEquipScrollView = this._trans.FindChild ("middle/rightbag/content/itemlist").GetComponent<UIScrollView> ();

		this._toothBg = this._trans.FindChild ("middle/left/equip/tooth/bg").GetComponent<UISprite> ();
		this._toothItem = this._trans.FindChild ("middle/left/equip/tooth/item").GetComponent<UITexture> ();
		this._clawBg = this._trans.FindChild ("middle/left/equip/claw/bg").GetComponent<UISprite> ();
		this._clawItem = this._trans.FindChild ("middle/left/equip/claw/item").GetComponent<UITexture> ();
		this._eyeBg = this._trans.FindChild ("middle/left/equip/eye/bg").GetComponent<UISprite> ();
		this._eyeItem = this._trans.FindChild ("middle/left/equip/eye/item").GetComponent<UITexture> ();
		this._jewelryBg = this._trans.FindChild ("middle/left/equip/jewelry/bg").GetComponent<UISprite> ();
		this._jewelryItem = this._trans.FindChild ("middle/left/equip/jewelry/item").GetComponent<UITexture> ();

		this._hiddenSale = this._trans.FindChild ("middle/rightbag/content/itemlist/gird/hiddenSale").GetComponent<UILabel> ();


		this._skillImg = this._trans.FindChild ("middle/left/skill/skill").GetComponent<UITexture> ();		
		this._skillName = this._trans.FindChild ("middle/left/skill/name").GetComponent<UILabel> ();		
		this._skillEffect = this._trans.FindChild ("middle/left/skill/effect/content").GetComponent<UILabel> ();		
		this._skillUse = this._trans.FindChild ("middle/left/skill/use/content").GetComponent<UILabel> ();		

		this._petSkillName = this._trans.FindChild ("skillpanel/name").GetComponent<UILabel> ();	  
		this._skillImgBg = this._trans.FindChild ("skillpanel/skill/skill").GetComponent<UITexture> ();	  
		this._petOnlyName = this._trans.FindChild ("skillpanel/pet/name").GetComponent<UILabel> ();	  
		this._petContent = this._trans.FindChild ("skillpanel/pet/content").GetComponent<UILabel> ();	  
		this._petEffectName = this._trans.FindChild ("skillpanel/effect/name").GetComponent<UILabel> ();	  
		this._petEffectContent = this._trans.FindChild ("skillpanel/effect/content").GetComponent<UILabel> ();	  
		this._petDescName = this._trans.FindChild ("skillpanel/desc/name").GetComponent<UILabel> ();	  
		this._petDescConent = this._trans.FindChild ("skillpanel/desc/content").GetComponent<UILabel> ();	 
		this._skillPanel = this._trans.FindChild ("skillpanel").gameObject;	 

	}
	
	void OnEnable ()
	{
		BagManager.Instance.RegisterEvent ();
		Gate.instance.registerMediator (new PetMediator (this));
	}
	
	void OnDisable ()
	{
		BagManager.Instance.CancelEvent ();
		Gate.instance.removeMediator (MediatorName.PET_MEDIATOR);
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
	
	//显示进化
	public void ShowView ()
	{
		PetManager.Instance.PreviewLadder = PetManager.Instance.CurrentLevel;
		EnableButton ();
		this.SetFollowPet ();
		HiddenArrow ();
		UpdateAttr (PetManager.Instance.CurrentPet.AttrTypes, PetManager.Instance.CurrentPet.AttrValues);
		UpdateMoney (); //初始化获取人物身上货币
		UpdateMaterial (PetManager.Instance.MaxPet);
		ChangeFollowColor (true);
		this.ShowSkill (PetManager.Instance.CurrentPet);
	}

	public void SetCamera (bool isactive)
	{
		NPCManager.Instance.ModelCamera.gameObject.SetActive (isactive);
	}

	static string GetPetNameAndLevel (string petName)
	{
		StringBuilder sb = new StringBuilder ();
        sb.Append(petName).Append(" ").Append(LanguageManager.GetText("pet_" + PetManager.Instance.PreviewLadder + "_level"));
		return  sb.ToString ();
	}
	
	//名称，阶数， 模型的变化
	public void UpdateLadder (string petName, string modelName, Vector3 modelPos, Vector3 rotate, Vector3 scale)
	{
		var pName = GetPetNameAndLevel (petName);
		this._petName.text = pName;
		this._petTitle.text = pName;
		NPCManager.Instance.createCamera (false); //先消除原来的3D相机
		GameObject model = null;
		if (BundleMemManager.Instance.isTypeInCache (EBundleType.eBundlePet)) {
			BundleMemManager.Instance.loadPrefabViaWWW<GameObject> (EBundleType.eBundlePet, modelName,
                (obj) =>
			{
				_modelRoot = transform.FindChild ("background/Sprite");
				_modelRoot.localPosition = _modelRoot.localPosition + modelPos;

				Vector3 pos = ViewHelper.UIPositionToCameraPosition (UIManager.Instance.getRootTrans ().FindChild ("Camera").GetComponent<Camera> (),
                        NPCManager.Instance.ModelCamera, _modelRoot.position);

				_modelRoot.localPosition = _modelRoot.localPosition - modelPos;

				model = BundleMemManager.Instance.instantiateObj (obj, Vector3.zero, Quaternion.identity);
				model.name = "pet_ui";
				model.transform.parent = NPCManager.Instance.ModelCamera.transform;
				model.transform.position = pos;
				model.transform.localEulerAngles = rotate;
				model.transform.localScale = scale;
				model.GetComponent<CharacterPet> ().enabled = false;
				ToolFunc.SetLayerRecursively (model, LayerMask.NameToLayer ("TopUI"));
			});
		} else {
			_modelRoot = transform.FindChild ("background/Sprite");
			_modelRoot.localPosition = _modelRoot.localPosition + modelPos;

			Vector3 pos = ViewHelper.UIPositionToCameraPosition (UIManager.Instance.getRootTrans ().FindChild ("Camera").GetComponent<Camera> (),
                NPCManager.Instance.ModelCamera, _modelRoot.position);

			_modelRoot.localPosition = _modelRoot.localPosition - modelPos;

			GameObject asset = BundleMemManager.Instance.getPrefabByName (modelName, EBundleType.eBundlePet);
			model = BundleMemManager.Instance.instantiateObj (asset, Vector3.zero, Quaternion.identity);
			model.name = "pet_ui";
			model.transform.parent = NPCManager.Instance.ModelCamera.transform;
			model.transform.position = pos;
			model.transform.localEulerAngles = rotate;
			model.transform.localScale = scale;
			model.GetComponent<CharacterPet> ().enabled = false;
			ToolFunc.SetLayerRecursively (model, LayerMask.NameToLayer ("TopUI"));
		}				
	}
	
	//升级的时候触发的升阶效果
	public void UpdateLevelUpLadder (string petName)
	{
		if (!string.IsNullOrEmpty (this._lastPet) && petName != this._lastPet) { //升阶层的时候触发
			FloatMessage.GetInstance ().PlayImageAdvanceFloatMessage (UIManager.Instance.getRootTrans ()); //字体进阶效果
		}
		this._lastPet = petName;
  
	}
	
 
	
	//成功率，幸运点的变化
	public void UpdateLuckPoint (uint luckNum, uint maxLuckNum)
	{
		if (PetManager.Instance.PreviewLadder == PetManager.Instance.DictionaryPet.Count) {
			_expBar.MaxValue = 999;
			_expBar.Value = 999;
		} else {
			_expBar.MaxValue = (int)maxLuckNum;
			_expBar.Value = (int)luckNum;
		}
 
	}
	
	//金钱钻石的变化
	public void UpdateMoney ()
	{
 
	}
	
	//自动培养，自动进阶
	public void AutoCultureEvo ()
	{
		ChangeBtn (true);
		StartCoroutine (this.AutoEvolution ());
	}

	public void SuccessPlayAnim ()
	{
		prefSuccess.SetActive (true);
		GameObject obj = BundleMemManager.Instance.instantiateObj (prefSuccess);
		obj.transform.parent = transform.FindChild ("background");
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3 (0, 25, 0);
		prefSuccess.SetActive (false);
        SkillTalentManager.Instance.effectObjList.Add(obj);
		Destroy (obj, 1.5f);
	}
	

	//切换按钮的状态
	public void ChangeBtn (bool beginAuto)
	{
		UILabel btnLabel;
		btnLabel = _btnTrans.Find ("btnAuto/Label").GetComponent<UILabel> ();
		if (beginAuto) {
			btnLabel.text = LanguageManager.GetText ("pet_stop_evolution");
			_btnTrans.Find ("btnAuto/Label").GetComponent<UILabel> ().effectColor = Color.yellow;
			_btnTrans.FindChild ("btnAuto/Sprite").GetComponent<UISprite> ().spriteName = "shop_btn1";
		} else {
			btnLabel.text = LanguageManager.GetText ("pet_auto_evolution");
			_btnTrans.Find ("btnAuto/Label").GetComponent<UILabel> ().effectColor = Color.cyan;
			_btnTrans.FindChild ("btnAuto/Sprite").GetComponent<UISprite> ().spriteName = "common_button1";
		}
 
	}
	
	//自动培养，自动进阶时提示相关错误信息
	public void ShowErrMsg (string msgSymbol)
	{
		string errMsg = LanguageManager.GetText (msgSymbol);
		FloatMessage.GetInstance ().PlayFloatMessage (errMsg, UIManager.Instance.getRootTrans (), 
		                                            new Vector3 (0f, 200f, -150f), new Vector3 (0f, 280f, -150f));
	}
	
	//显示点击后的效果
	public void ShowEffect (string doubleStr, string floatMsg)
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
	public void EnableButton ()
	{
 
		if (PetManager.Instance.MaxLadder == PetManager.Instance.PreviewLadder && PetManager.Instance.PreviewLadder < PetManager.Instance.DictionaryPet.Count) { //当前阶数就是预览阶数就使能按钮

			_btnTrans.Find ("btnAuto").gameObject.SetActive (true);
			_btnTrans.Find ("btnEvolution").gameObject.SetActive (true);
			
			Transform luckInfoTrans = _evoInfo.transform;
			
			luckInfoTrans.FindChild ("Info").gameObject.SetActive (false);
			luckInfoTrans.FindChild ("consumeLbl").gameObject.SetActive (true);
			luckInfoTrans.FindChild ("stone").gameObject.SetActive (true);
			luckInfoTrans.FindChild ("stoneNum").gameObject.SetActive (true);
			
			_btnTrans.Find ("btnRet").gameObject.SetActive (false);
			PetManager.Instance.ShowTime ();

			if (PetManager.Instance.PreviewLadder == PetManager.Instance.CurrentLevel) {
				ChangeFollowColor (true);
			} else {
				ChangeFollowColor (false);
			}

			_btnTrans.FindChild ("Label").gameObject.SetActive (false);
		} else if (PetManager.Instance.MaxLadder == PetManager.Instance.PreviewLadder && PetManager.Instance.PreviewLadder == PetManager.Instance.DictionaryPet.Count) {
			_btnTrans.Find ("btnAuto").gameObject.SetActive (false);
			_btnTrans.Find ("btnEvolution").gameObject.SetActive (false);
			_btnTrans.Find ("btnRet").gameObject.SetActive (false);

			Transform luckInfoTrans = _evoInfo.transform;
			luckInfoTrans.FindChild ("Info").gameObject.SetActive (true);
			_evoInfo.transform.FindChild ("Info").GetComponent<UILabel> ().text = LanguageManager.GetText ("pet_luckinfo_info");

			luckInfoTrans.FindChild ("consumeLbl").gameObject.SetActive (false);
			luckInfoTrans.FindChild ("stone").gameObject.SetActive (false);
			luckInfoTrans.FindChild ("stoneNum").gameObject.SetActive (false);
			this._timeObj.gameObject.SetActive (false);

			ChangeFollowColor (true);
			_btnTrans.FindChild ("Label").gameObject.SetActive (true);
		} else { 
			_btnTrans.Find ("btnAuto").gameObject.SetActive (false);
			_btnTrans.Find ("btnEvolution").gameObject.SetActive (false);
			_btnTrans.Find ("btnRet").gameObject.SetActive (true);
			
			Transform luckInfoTrans = _evoInfo.transform;
			
			luckInfoTrans.FindChild ("Info").gameObject.SetActive (true);
			_evoInfo.transform.FindChild ("Info").GetComponent<UILabel> ().text = LanguageManager.GetText ("pet_luckinfo_info");

			luckInfoTrans.FindChild ("consumeLbl").gameObject.SetActive (false);
			luckInfoTrans.FindChild ("stone").gameObject.SetActive (false);
			luckInfoTrans.FindChild ("stoneNum").gameObject.SetActive (false);
			this._timeObj.gameObject.SetActive (false);
			_btnTrans.FindChild ("Label").gameObject.SetActive (false);
			if (PetManager.Instance.PreviewLadder == PetManager.Instance.CurrentLevel) {
				ChangeFollowColor (true);
			} else {
				ChangeFollowColor (false);
			}
		}
		this.HiddenArrow ();
	}

	private void ChangeFollowColor (bool isChanged)
	{
		if (isChanged) {
			_btnFollow.FindChild ("Label").GetComponent<UILabel> ().text = "跟随中";
			_btnFollow.FindChild ("Label").GetComponent<UILabel> ().effectColor = Color.yellow;
			_btnTrans.Find ("btnAuto/Label").GetComponent<UIWidget> ().color = Color.black;

			_btnFollow.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = "shop_btn1";
		} else {
			_btnFollow.FindChild ("Label").GetComponent<UILabel> ().text = "选择跟随";
			_btnFollow.FindChild ("Label").GetComponent<UILabel> ().effectColor = Color.cyan;
			_btnTrans.Find ("btnAuto/Label").GetComponent<UIWidget> ().color = Color.black;

			_btnFollow.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = "common_button1";
		}
	}

	public void SetFollowPet ()
	{
		if (PetManager.Instance.PreviewLadder <= PetManager.Instance.MaxLadder) {
			_btnFollow.GetComponent<BoxCollider> ().enabled = true;
			_btnFollow.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (true);
			if (PetManager.Instance.PreviewLadder == PetManager.Instance.CurrentLevel) {
				ChangeFollowColor (true);
			} else {
				ChangeFollowColor (false);
			}
		} else {
			_btnFollow.GetComponent<BoxCollider> ().enabled = false;
			_btnFollow.FindChild ("Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus (false);
		}
		
	}
	
	
	//属性的添加修改
	public void UpdateAttr (List<int> attrNames, List<int> attrValues)
	{
		int len = attrNames.Count < attrValues.Count ? attrNames.Count : attrValues.Count;
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
			string s = PowerManager.Instance.ChangeInfoData ((eFighintPropertyCate)attrNames [i], attrValues [i]);
			num.text = "+" + s;

			num.text += FormatCountAttrVal((eFighintPropertyCate)attrNames [i]);


			go.name = "attr" + (i + 1);
		}

		this.AddExtAttr (len,attrNames);





		_attrTemplate.SetActive (false);
	}

	#region 扩展属性
	private void AddExtAttr(int len,List<int> attrNames){
		Dictionary<eFighintPropertyCate,AttributeValue> dic = new Dictionary<eFighintPropertyCate, AttributeValue> ();
		this.GetExtAttr (eEquipPart.eTooth, dic,len,attrNames);
		this.GetExtAttr (eEquipPart.eClaw, dic,len,attrNames);
		this.GetExtAttr (eEquipPart.eEye, dic,len,attrNames);
		this.GetExtAttr (eEquipPart.eJewelry, dic,len,attrNames);


		List<AttributeValue> attrList = new List<AttributeValue> ();
		foreach (var item in dic.Values) {
			attrList.Add(item);
		} 
		
		this.AddExtAttrRow (len, attrList); //添加行
	}
	
	/// <summary>
	/// 得到基本属性不存在的属性
	/// </summary>
	/// <param name="part">Part.</param>
	/// <param name="dic">Dic.</param>
	/// <param name="len">Length.</param>
	/// <param name="attrNames">Attr names.</param>
	private void GetExtAttr(eEquipPart part,Dictionary<eFighintPropertyCate,AttributeValue> dic,int len,List<int> attrNames){
		if (BagManager.Instance.EquipData.ContainsKey (part)) {
			var equip = BagManager.Instance.EquipData [part];	
			for (int i = 0,max = equip.BaseAtrb.size; i < max; i++) {
				bool isHave= false;
				for (int j = 0 ; j < len; j++) {
					if (equip.BaseAtrb[i].Type== (eFighintPropertyCate)attrNames [j]) {
						isHave=true;
						break;
					}
				}
				if (isHave==false) { //得到不存在于原始属性的属性
					if (dic.ContainsKey(equip.BaseAtrb[i].Type)) { //如果包含则叠加原始值
						AttributeValue attrVal = dic[equip.BaseAtrb[i].Type];
						attrVal.Value += equip.BaseAtrb[i].Value;
						dic[equip.BaseAtrb[i].Type] = attrVal;//重新覆盖
					}else{ //如果不包含则添加
						dic.Add(equip.BaseAtrb[i].Type,equip.BaseAtrb[i]);
					}
				}
			}
		}
	}

	/// <summary>
	/// 添加行
	/// </summary>
	/// <param name="len">Length.</param>
	/// <param name="attrList">Attr list.</param>
	private void AddExtAttrRow(int len,List<AttributeValue> attrList){
		float posX = 0;
		float posY = 0;
		Transform trans = _attrTemplate.transform;
		for (int i=0,max=attrList.Count; i<max; i++,len++) {
			GameObject go = NGUITools.AddChild (trans.parent.gameObject, _attrTemplate);
			int rows = len / 2;
			if (len % 2 == 0) {
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
			
			title.text = EquipmentManager.GetEquipAttributeName (attrList[i].Type , true);
			num.text =  GetCountAttrValString(attrList[i].Type, attrList[i].Value);
			
			go.name = "attr" + (len + 1);
		}
	}


	#endregion


	#region 得到宠物装备的增加值

	private string GetCountAttrValString (eFighintPropertyCate attr,int val){
		string str = string.Empty;
		if (val != 0) {
			str = "(" + string.Format(LanguageManager.GetText("pet_plus_color"),"+"+PowerManager.Instance.ChangeInfoData (attr, val))  + ")";
		}
		return str;
	}

	private string FormatCountAttrVal (eFighintPropertyCate attr)
	{
		int val = this.CountAttrVal (attr);
		return	this.GetCountAttrValString (attr, val);
	}
	
	private int CountAttrVal (eFighintPropertyCate attr)
	{
		int count = GetAttrValue (eEquipPart.eTooth, attr) +
			GetAttrValue (eEquipPart.eClaw, attr) +
				GetAttrValue (eEquipPart.eEye, attr) +
				GetAttrValue (eEquipPart.eJewelry, attr);
		return count;
	}
	
	private int GetAttrValue (eEquipPart part, eFighintPropertyCate attr)
	{
		int val = 0;
		if (BagManager.Instance.EquipData.ContainsKey (part)) {
			var equip = BagManager.Instance.EquipData [part];
			for (int i = 0,max = equip.BaseAtrb.size; i < max; i++) {
				if (equip.BaseAtrb [i].Type == attr) { 
					val = equip.BaseAtrb [i].Value;
					break;
				}
			}
		}
		return val;
	}
	#endregion

 

	//材质变化
	public void UpdateMaterial (PetVo nextPet)
	{
		uint ownerNum = 0;
		uint needNum = 0;
 		
		ownerNum = ItemManager.GetInstance ().GetItemNumById (PetManager.Instance.MaxPet.EvoCostItem); //得到当前的进阶石
		needNum = nextPet.EvoNum;
 
		if (ownerNum >= needNum) { //需要的进阶石显示
			_evoInfo.transform.Find ("stoneNum").GetComponent<UILabel> ().text = "[00ff00]" + ownerNum + "[-]" + "/" + needNum;  //需要的进阶石显示
		} else {
			_evoInfo.transform.Find ("stoneNum").GetComponent<UILabel> ().text = "[ff0000]" + ownerNum + "[-]" + "/" + needNum;  //需要的进阶石显示
		}
//		_evoInfo.transform.Find ("goldNum").GetComponent<UILabel> ().text = "X" + nextPet.CostGold;
 
		UITexture icon2 = _evoInfo.transform.Find ("stone").GetComponent<UITexture> ();
		ItemTemplate item = ItemManager.GetInstance ().GetTemplateByTempId (PetManager.Instance.MaxPet.EvoCostItem);
 
		DealTexture.Instance.setTextureToIcon (icon2, item, false);
	}
	
	//显示隐藏上线箭头
	private void HiddenArrow ()
	{
		if (PetManager.Instance.PreviewLadder <= 1) {
			_downArrow.SetActive (true);
			_upArrow.SetActive (false);
		} else if (PetManager.Instance.PreviewLadder >= PetManager.Instance.DictionaryPet.Count) {
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
			this._timeObj.gameObject.SetActive (true);
			this._timeObj.transform.FindChild ("info").GetComponent<UILabel> ().text = LanguageManager.GetText ("pet_time_control");
		} else {
			this._timeObj.gameObject.SetActive (false);
			this._isAutoTime = false;
		} //为0则隐藏
		
		
		if (isLimit) {	
			if (_isAutoTime == false) {
				this._isAutoTime = true;
				
				EndTime = new DateTime (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
				EndTime = EndTime.AddDays (1);
				TimeSpan dt = EndTime - DateTime.Now;
				System.Text.StringBuilder sb = new System.Text.StringBuilder ();
				this._time.text = sb.Append (dt.Hours).Append ("时").Append (dt.Minutes).Append ("分").Append (dt.Seconds).Append ("秒").ToString ();
			}
		} else {  //无限制的时候关闭协同
			this._time.text = "无限制";
		}
		
	}
	
 
 
	
	//自动进阶
	IEnumerator AutoEvolution ()
	{
		while (true) {
			PetManager.Instance.BeginEvolute (true);
			yield return new WaitForSeconds (0.2f);
		}	
	}
 
	void Update ()
	{
 
		if (_isAutoTime) {
			countTime += Time.deltaTime;
			if (countTime > 1) {
				TimeSpan dt = EndTime - DateTime.Now;
				
				if (dt.Ticks <= 0) {
					this._timeObj.gameObject.SetActive (false);
					this._isAutoTime = false;
					return;
				}
				StringBuilder sb = new StringBuilder ();
				this._time.text = sb.Append (dt.Hours).Append ("时").Append (dt.Minutes).Append ("分").Append (dt.Seconds).Append ("秒").ToString ();
				countTime -= 1;
	 
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

    #region 切页逻辑
	/// <summary>
	/// Switchs the tab.
	/// </summary>
	/// <param name='tab'>
	/// Tab.
	/// </param>
	public void SwitchTab (PetTabEnum tab)
	{
		
		switch (tab) {
		case PetTabEnum.Attribute:
			this._right.gameObject.SetActive (true);
			this._rightBag.gameObject.SetActive (false);
			this._checkBoxTab1.isChecked = true;
			
			break;
		case PetTabEnum.Equip:
			this._right.gameObject.SetActive (false);
			this._rightBag.gameObject.SetActive (true);
			this._checkBoxTab2.isChecked = true;
			this._petEquipScrollView.ResetPosition ();
			this._hiddenSale.text = "";
			break;
			
		default:
			break;
		}
		TipsManager.Instance.CloseAllTipsUI (); //销毁tips界面
	}
	
	/// <summary>
	/// Shows the pet equip.
	/// </summary>
	/// <param name="itemList">Item list.</param>
	public void ShowPetEquip (IList<ItemInfo> itemList)
	{
		this._petItemInfoList = itemList;
		List<ItemInfo> showList = new List<ItemInfo> ();
		for (int i = 0,max = itemList.Count>pageSize? pageSize: itemList.Count; i < max; i++) {
			showList.Add (itemList [i]);
		}
		RepeatItem (showList, this._itemTemp);
	}
	
	/// <summary>
	/// Repeats the item.
	/// </summary>
	/// <param name='itemList'>
	/// 需要重复的数据
	/// </param>
	/// <param name='temp'>
	/// 模板
	/// </param>
	public  void RepeatItem (IList<ItemInfo> itemList, Transform  temp)
	{
		Transform container = temp.parent.Find ("itemContainer");
		if (container != null) {
			Destroy (container.gameObject);
		}//先删除容器
		
		
		GameObject gameobj = new GameObject ();     //创建容器
		gameobj.name = "itemContainer";
		gameobj.layer = LayerMask.NameToLayer ("TopTopUI");
		gameobj.transform.parent = temp.parent;
		gameobj.transform.localPosition = Vector3.one;
		gameobj.transform.localScale = Vector3.one;
		
		this._itemContainer = gameobj.transform;
		
		for (int i = 0; i < itemList.Count; i++) {
			AddRow (itemList, i, gameobj, temp);
		}
	}
	
	void AddRow (IList<ItemInfo> itemList, int i, GameObject parent, Transform temp)
	{
		GameObject itemTemp = NGUITools.AddChild (parent, temp.gameObject);
		//赋值模板 
		itemTemp.SetActive (true);
		itemTemp.name = i.ToString ();
		Transform itemTrans = itemTemp.transform;
		Transform itemIcon = itemTrans.Find ("item");
		itemIcon.GetComponent<BtnTipsMsg> ().InstanceId = itemList [i].InstanceId;
		//为tips设置实例id
		DealTexture.Instance.setTextureToIcon (itemIcon.GetComponent<UITexture> (), itemList [i].Item, false);
		//设置图片
		itemTrans.Find ("itembg").GetComponent<UISprite> ().spriteName = BagManager.Instance.getItemBgByType (itemList [i].Item.quality, false);
		//设置装备背景
		itemTrans.Find ("itemlevelnum").GetComponent<UILabel> ().text = itemList [i].Item.usedLevel.ToString () + "阶";
		//需要的等级
		itemTrans.localPosition = new Vector3 (0, i * this.Height + temp.localPosition.y, 0);
		//位置
		UILabel itemName = itemTrans.Find ("itemname").GetComponent<UILabel> ();
		itemName.text = itemList [i].Item.name;
		//道具名字
		itemTrans.Find ("itemcareername").GetComponent<UILabel> ().text = BagManager.Instance.GetItemCareerString (itemList [i].Item.career);
		//道具可用名字
		itemTrans.Find ("Toggle/instanceid").GetComponent<UILabel> ().text = itemList [i].InstanceId.ToString ();
		#region 设置道具数量信息
		Transform itemNum = itemTrans.Find ("itemnum");
		//道具数量
		Transform battle = itemTrans.Find ("battle");
		//战斗力数值信息
		 
		itemNum.gameObject.SetActive (false);
		battle.gameObject.SetActive (true); 
		battle.GetComponent<UILabel> ().text = BagManager.Instance.PowerCompare (itemList [i]);
		//战斗力比较
		#endregion
		
		 
	}
	/// <summary>
	/// Updates the bag number.
	/// </summary>
	/// <param name='current'>
	/// Current.
	/// </param>
	/// <param name='maxBagNum'>
	/// Max bag number.
	/// </param>
	public void UpdateBagNum (int current)
	{
		this._petEquipInfo.text = current.ToString ();
	}

	/// <summary>
	/// 上下拖拽的回调事件
	/// </summary>
	public void OnChangePetEquipItemList ()
	{
		ViewHelper.TouchUpAddingData (ref this.currentPagePetEquip, this.pageSize, this.Height, this._petItemInfoList.Count, this._petEquipItemList, (i) => {
			Destroy (this._itemContainer.FindChild (i.ToString ()).gameObject);
		}, (i) => {
			this.AddRow (this._petItemInfoList, i, this._itemContainer.gameObject, this._itemTemp);
		});
		
		
		ViewHelper.TouchDownAddingData (ref this.currentPagePetEquip, this.pageSize, this.Height, this._petItemInfoList.Count, this._petEquipItemList, (i) => {
			Transform delTrans = this._itemContainer.FindChild (i.ToString ());
			if (delTrans != null) {
				Destroy (delTrans.gameObject);
			}
		}, (i) => {
			this.AddRow (this._petItemInfoList, i, this._itemContainer.gameObject, this._itemTemp);
		});
	}


	/// <summary>
	/// Shows the equip.
	/// </summary>
	/// <param name='equipData'>
	/// Equip data.
	/// </param>
	public void ShowEquip (Dictionary<eEquipPart, EquipmentStruct> equipData)
	{
		#region 为装备图赋值

		SetEquipIconAndBg (equipData, eEquipPart.eTooth, this._toothItem, this._toothBg);
		SetEquipIconAndBg (equipData, eEquipPart.eClaw, this._clawItem, this._clawBg);
		SetEquipIconAndBg (equipData, eEquipPart.eEye, this._eyeItem, this._eyeBg);
		SetEquipIconAndBg (equipData, eEquipPart.eJewelry, this._jewelryItem, this._jewelryBg);
		
		#endregion

	}

	/// <summary>
	/// Sets the equip icon and background.
	/// </summary>
	/// <param name='equipData'>
	/// Equip data.
	/// </param>
	/// <param name='part'>
	/// Part.
	/// </param>
	/// <param name='item'>
	/// Item.
	/// </param>
	/// <param name='itemBg'>
	/// Item background.
	/// </param>
	public void SetEquipIconAndBg (Dictionary<eEquipPart, EquipmentStruct> equipData, eEquipPart part, UITexture item, UISprite itemBg)
	{  
		if (equipData.ContainsKey (part)) {
			var equipStr = equipData [part];
			 
			item.gameObject.GetComponent<BtnTipsMsg> ().InstanceId = equipStr.instanceId; //为tips设置实例id
			ItemTemplate temp = ItemManager.GetInstance ().GetTemplateByTempId (equipStr.templateId);
			DealTexture.Instance.setTextureToIcon (item, temp);
			itemBg.spriteName = BagManager.Instance.getItemBgByType (temp.quality, true);
		}
	}
    #endregion


	public void ShowSkill (PetVo pet)
	{ 
		this._skillImg.mainTexture = SourceManager.Instance.getTextByIconName (pet.PetSkillVO.Icon, PathConst.PET_SKILL_PATH);
		this._skillName.text = pet.PetSkillVO.Name;
		this._skillEffect.text = pet.EffectDesc;
		this._skillUse.text = pet.UsedDesc;

	}

	public void ShowSkillPanel (PetVo pet)
	{
		this._skillPanel.SetActive (true);
		this._petSkillName.text = pet.PetSkillVO.Name;
		this._skillImgBg.mainTexture = SourceManager.Instance.getTextByIconName (pet.PetSkillVO.Icon, PathConst.PET_SKILL_PATH);
		this._petOnlyName.text = LanguageManager.GetText ("pet_only_pet");
        Debug.Log(pet.PetName);        
		this._petContent.text = pet.PetName;
		this._petEffectName.text = LanguageManager.GetText ("pet_effect");
		this._petEffectContent.text = pet.PetSkillVO.SzDesc;
		this._petDescName.text = LanguageManager.GetText ("pet_desc");
		this._petDescConent.text = LanguageManager.GetText ("pet_desc_content"); 
	}

	public void CloseSkillPanel ()
	{
		this._skillPanel.SetActive (false);
	}
}
