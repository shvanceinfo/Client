/**该文件实现的基本功能等
function: 实现背包界面
author:zyl
date:2014-4-8
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using System.Collections.Generic;
using helper;

public class BagView : ModelView
{
	public enum Tab
	{
		all,
		equip,
		item
	}
	
	public float Height = -105f;
	Transform _trans; 	//当前对象的transform组件信息
	UISprite _weaponBg;
	UISprite _weaponBg1;
	UITexture _weaponItem;
	UILabel _weaponPlus;
	UISprite _clothBg;
	UISprite _clothBg1;
	UITexture _clothItem;
	UILabel _clothPlus;
	UISprite _pantsBg;
	UISprite _pantsBg1;
	UITexture _pantsItem;
	UILabel _pantsPlus;
	UISprite _shoeBg;
	UISprite _shoeBg1;
	UITexture _shoeItem;
	UILabel _shoePlus;
	UISprite _necklaceBg;
	UISprite _necklaceBg1;
	UITexture _necklaceItem;
	UILabel _necklacePlus;
	UISprite _ringBg;
	UISprite _ringBg1;
	UITexture _ringItem;
	UILabel _ringPlus;
	UILabel _name;
	Transform _all;
	Transform _equip;
	Transform _item;
	Transform _allItemTemp;
	Transform _equipItemTemp;
	Transform _normalItemTemp;
	
//	bool _hasShowAll = false;  //是否已经加载过全部道具的数据了
//	bool _hasShowEquip = false;//是否已经加载过所有装备的数据了
//	bool _hasShowItem =false;  //是否已经加载过所有道具的数据了
	
	UILabel _allHiddenSale;    //全部道具的隐藏域值
	UILabel _equipHiddenSale;  //装备的隐藏域值
	UILabel _normalHiddenSale; //普通道具的隐藏域值
	
	UILabel _allInfo;			//已用背包格数的信息
	UILabel _equipInfo;			//已用背包格数的信息
	UILabel _itemInfo;			//已用背包格数的信息

	UILabel VipLevel;//vip等级显示
	UILabel FightTxt;//战斗力数值
	
	public static Tab CurrentTab; //当前的tab

	Vector3 _modelRootPos;
	UIPanel _allItemList;
	UIScrollView _allScrollView;
	UIPanel _equipItemList;
	UIScrollView _equipScrollView;
	UIPanel _itemItemList;
	UIScrollView _itemScrollView;

	IList<ItemInfo> _allItemInfoList;
	IList<ItemInfo> _equipItemInfoList;
	IList<ItemInfo> _normalItemInfoList;
	public int pageSize = 8;
	int currentPageAll = 1;
	int currentPageEquip = 1;
	int currentPageItem = 1;

	Transform itemContainer;

	void Awake ()
	{
		CurrentTab = Tab.all;	//初始值为显示所有
		
		this._trans = this.transform.FindChild ("top_ui");
		#region 左边
		this._weaponBg = this._trans.Find ("userinfo/weapon/bg").GetComponent<UISprite> ();
		this._weaponBg1 = this._trans.Find ("userinfo/weapon/bg1").GetComponent<UISprite> ();
		this._weaponItem = this._trans.Find ("userinfo/weapon/item").GetComponent<UITexture> ();
		this._weaponPlus = this._trans.Find ("userinfo/weapon/plus").GetComponent<UILabel> ();
		this._clothBg = this._trans.Find ("userinfo/cloth/bg").GetComponent<UISprite> ();
		this._clothBg1 = this._trans.Find ("userinfo/cloth/bg1").GetComponent<UISprite> ();
		this._clothItem = this._trans.Find ("userinfo/cloth/item").GetComponent<UITexture> ();
		this._clothPlus = this._trans.Find ("userinfo/cloth/plus").GetComponent<UILabel> ();
		this._pantsBg = this._trans.Find ("userinfo/pants/bg").GetComponent<UISprite> ();
		this._pantsBg1 = this._trans.Find ("userinfo/pants/bg1").GetComponent<UISprite> ();
		this._pantsItem = this._trans.Find ("userinfo/pants/item").GetComponent<UITexture> ();
		this._pantsPlus = this._trans.Find ("userinfo/pants/plus").GetComponent<UILabel> ();
		this._shoeBg = this._trans.Find ("userinfo/shoe/bg").GetComponent<UISprite> ();
		this._shoeBg1 = this._trans.Find ("userinfo/shoe/bg1").GetComponent<UISprite> ();
		this._shoeItem = this._trans.Find ("userinfo/shoe/item").GetComponent<UITexture> ();
		this._shoePlus = this._trans.Find ("userinfo/shoe/plus").GetComponent<UILabel> ();
		this._necklaceBg = this._trans.Find ("userinfo/necklace/bg").GetComponent<UISprite> ();
		this._necklaceBg1 = this._trans.Find ("userinfo/necklace/bg1").GetComponent<UISprite> ();
		this._necklaceItem = this._trans.Find ("userinfo/necklace/item").GetComponent<UITexture> ();
		this._necklacePlus = this._trans.Find ("userinfo/necklace/plus").GetComponent<UILabel> ();
		this._ringBg = this._trans.Find ("userinfo/ring/bg").GetComponent<UISprite> ();
		this._ringBg1 = this._trans.Find ("userinfo/ring/bg1").GetComponent<UISprite> ();
		this._ringItem = this._trans.Find ("userinfo/ring/item").GetComponent<UITexture> ();
		this._ringPlus = this._trans.Find ("userinfo/ring/plus").GetComponent<UILabel> ();
		#endregion
 
		this._name = this._trans.Find ("userinfo/name").GetComponent<UILabel> ();
		_modelRootPos = transform.FindChild ("bg/fazhen").position;

		this._all = this._trans.Find ("allitem/all");    //tab1
		this._equip = this._trans.Find ("allitem/equip");//tab2
		this._item = this._trans.Find ("allitem/item");	//tab3
		this._allItemTemp = this._trans.Find ("allitem/all/content/itemlist/gird/itemTemp");	//所有道具的模板
		this._equipItemTemp = this._trans.Find ("allitem/equip/content/itemlist/gird/itemTemp");//装备的模板
		this._normalItemTemp = this._trans.Find ("allitem/item/content/itemlist/gird/itemTemp");	//道具的模板
		
		this._allHiddenSale = this._trans.Find ("allitem/all/content/itemlist/gird/hiddenSale").GetComponent<UILabel> ();
		this._equipHiddenSale = this._trans.Find ("allitem/equip/content/itemlist/gird/hiddenSale").GetComponent<UILabel> ();
		this._normalHiddenSale = this._trans.Find ("allitem/item/content/itemlist/gird/hiddenSale").GetComponent<UILabel> ();
		
		this._allInfo = this._trans.Find ("allitem/all/content/btn/info").GetComponent<UILabel> ();
		this._equipInfo = this._trans.Find ("allitem/equip/content/btn/info").GetComponent<UILabel> ();
		this._itemInfo = this._trans.Find ("allitem/item/content/btn/info").GetComponent<UILabel> ();

		this._allItemList = this._trans.FindChild ("allitem/all/content/itemlist").GetComponent<UIPanel> ();
		this._allScrollView = this._trans.FindChild ("allitem/all/content/itemlist").GetComponent<UIScrollView> ();
		this._equipItemList = this._trans.FindChild ("allitem/equip/content/itemlist").GetComponent<UIPanel> ();
		this._equipScrollView = this._trans.FindChild ("allitem/equip/content/itemlist").GetComponent<UIScrollView> ();
		this._itemItemList = this._trans.FindChild ("allitem/item/content/itemlist").GetComponent<UIPanel> ();
		this._itemScrollView = this._trans.FindChild ("allitem/item/content/itemlist").GetComponent<UIScrollView> ();


		VipLevel = _trans.FindChild ("roleVip/VipLabel").GetComponent<UILabel> ();
		FightTxt = _trans.FindChild ("roleVip/FightLabel").GetComponent<UILabel> ();

	}
	
	void OnEnable ()
	{
		BagManager.Instance.RegisterEvent ();
		Gate.instance.registerMediator (new BagMediator (this));
	}
	
	void OnDisable ()
	{
		BagManager.Instance.CancelEvent ();
		Gate.instance.removeMediator (MediatorName.BAG_MEDIATOR);
	}

	public void ShowDiamondGold (List<string> s)
	{
		StartCoroutine (ShowDiamondAndGold (s));
	}

	private List<string> showInfo = new List<string> ();

	IEnumerator ShowDiamondAndGold (List<string> s)
	{
		yield return null;
		showInfo.Clear ();
		showInfo.Add (s [0]);
		showInfo.Add (s [1]);
		if (s [0] != "0") {
			FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText ("item_diamond_success_msg"), showInfo [0]),
                                                UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
		}
		if (s [1] != "0") {
			FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText ("item_sell_success_msg"), showInfo [1]),
                                                   UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
		} else if (s [0] != "0" && s [1] != "0") {

			FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText ("item_diamond_success_msg"), showInfo [0]),
                                                UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			yield return new WaitForSeconds (0.5f);
			FloatMessage.GetInstance ().PlayFloatMessage (string.Format (LanguageManager.GetText ("item_sell_success_msg"), showInfo [1]),
                                                UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
		}

	}
	 
	
	/// <summary>
	/// 更新武器模型
	/// </summary>
	public void UpdateModelWeapon ()
	{
		CharacterPlayerUI playerUI = this.model.GetComponent<CharacterPlayerUI> (); //加载模型装备信息
		playerUI.equipItem (CharacterPlayer.character_property.weapon,true);
		ToolFunc.SetLayerRecursively (model, LayerMask.NameToLayer ("TopUI")); //设置位置，大小，旋转角度，摄像机和层
	}
	
	/// <summary>
	/// 更新衣服模型
	/// </summary>
	public void UpdateModelArmor ()
	{
		CharacterPlayerUI playerUI = this.model.GetComponent<CharacterPlayerUI> (); //加载模型装备信息
		playerUI.equipItem (CharacterPlayer.character_property.armor,true);
		ToolFunc.SetLayerRecursively (model, LayerMask.NameToLayer ("TopUI")); //设置位置，大小，旋转角度，摄像机和层
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
	public static void SetEquipIconAndBg (Dictionary<eEquipPart, EquipmentStruct> equipData, eEquipPart part, UITexture item, UISprite itemBg, UISprite itemBg1, UILabel itemPlus)
	{
		if (equipData.ContainsKey (part)) {
			var equipStr = equipData [part];
			if (equipStr.intensifyLevel > 0) {
				itemPlus.text = string.Format (LanguageManager.GetText ("item_plus_msg"), equipStr.intensifyLevel.ToString ());
			} else {
				itemPlus.text = string.Empty;
			}
			item.gameObject.GetComponent<BtnTipsMsg> ().InstanceId = equipStr.instanceId; //为tips设置实例id
			ItemTemplate temp = ItemManager.GetInstance ().GetTemplateByTempId (equipStr.templateId);
			DealTexture.Instance.setTextureToIcon (item, temp);
			itemBg1.gameObject.SetActive (true);
			itemBg.gameObject.SetActive (false);
			itemBg1.spriteName = BagManager.Instance.getItemBgByType (temp.quality, true);
		} else {
			
		}
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
		SetEquipIconAndBg (equipData, eEquipPart.eArcher, this._weaponItem, this._weaponBg, this._weaponBg1, this._weaponPlus);
		SetEquipIconAndBg (equipData, eEquipPart.eDoublePole, this._weaponItem, this._weaponBg, this._weaponBg1, this._weaponPlus);
		SetEquipIconAndBg (equipData, eEquipPart.eGreatSword, this._weaponItem, this._weaponBg, this._weaponBg1, this._weaponPlus); 
		SetEquipIconAndBg (equipData, eEquipPart.eLeggings, this._pantsItem, this._pantsBg, this._pantsBg1, this._pantsPlus);
		SetEquipIconAndBg (equipData, eEquipPart.eNecklace, this._necklaceItem, this._necklaceBg, this._necklaceBg1, this._necklacePlus);
		SetEquipIconAndBg (equipData, eEquipPart.eRing, this._ringItem, this._ringBg, this._ringBg1, this._ringPlus);
		SetEquipIconAndBg (equipData, eEquipPart.eShoes, this._shoeItem, this._shoeBg, this._shoeBg1, this._shoePlus);
		SetEquipIconAndBg (equipData, eEquipPart.eSuit, this._clothItem, this._clothBg, this._clothBg1, this._clothPlus);
  
		#endregion
		
		this._name.text = CharacterPlayer.character_property.getNickName (); 
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

		this.itemContainer = gameobj.transform;

		for (int i = 0; i < itemList.Count; i++) {
			AddRow (itemList,i,gameobj, temp);
		}
	}

	void AddRow (IList<ItemInfo> itemList,int i,GameObject parent, Transform temp)
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
		itemTrans.Find ("itemlevelnum").GetComponent<UILabel> ().text = itemList [i].Item.usedLevel.ToString ();
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
		//			Transform intensify = itemTrans.Find ("intensify");//星级
		if (itemList [i].Item.packType == ePackageNavType.Other) {
			itemNum.GetComponent<UILabel> ().text = itemList [i].Num.ToString ();
			itemNum.gameObject.SetActive (true);
			battle.gameObject.SetActive (false);
			//				intensify.gameObject.SetActive (false);
		}
		else if(itemList [i].Item.packType == ePackageNavType.Equip||itemList [i].Item.packType == ePackageNavType.Weapon){
			itemNum.gameObject.SetActive (false);
			battle.gameObject.SetActive (true);
			//				intensify.gameObject.SetActive (true);
			uint intensityLevel = BagManager.Instance.GetIntensityLevel (itemList [i].InstanceId);
			if (intensityLevel != 0) {
				itemName.text = itemName.text + "    +" + intensityLevel;
			}
			battle.GetComponent<UILabel> ().text = BagManager.Instance.PowerCompare (itemList [i]);
			//战斗力比较
		}else if(itemList [i].Item.packType == ePackageNavType.PetEquip){
			itemNum.gameObject.SetActive (false);
			battle.gameObject.SetActive (true);
			//				intensify.gameObject.SetActive (true);
//			uint intensityLevel = BagManager.Instance.GetIntensityLevel (itemList [i].InstanceId);
//			if (intensityLevel != 0) {
//				itemName.text = itemName.text + "    +" + intensityLevel;
//			}
			battle.GetComponent<UILabel> ().text = BagManager.Instance.PowerCompare (itemList [i]);
			//战斗力比较
		}
        #endregion

        TipBind bd = itemIcon.GetComponent<TipBind>();
        if (bd)
            bd.Id = (int)itemList[i].Item.id;
    }
	
	
	/// <summary>
	/// Shows all item.
	/// </summary>
	/// <param name='itemList'>
	/// Item list.
	/// </param>
	public void ShowAllItem (IList<ItemInfo> itemList)
	{
		this._allItemInfoList = itemList;
		List<ItemInfo> showList = new List<ItemInfo> ();

        if (BagManager.Instance.AutoFindItem != 0)
        {
            for (int i = 0; i < _allItemInfoList.Count; i++)
            {
                ItemInfo info=_allItemInfoList[i];
                if (info.Item.id == (uint)BagManager.Instance.AutoFindItem)
                {
                    if (_allItemInfoList.Count <= pageSize)
                    {
                        showList.AddRange(_allItemInfoList);
                    }
                    else if (i > (_allItemInfoList.Count - pageSize))
                    {
                        for (int j = _allItemInfoList.Count - pageSize; j < _allItemInfoList.Count; j++)
                        {
                            showList.Add(_allItemInfoList[j]);
                        }
                    }
                    else
                    {
                        for (int j = i; j < pageSize+i; j++)
                        {
                            showList.Add(_allItemInfoList[j]);
                        }
                    }
                    BagManager.Instance.AutoFindItem = 0;
                    break;
                }
            }
        }
        else {
            for (int i = 0, max = itemList.Count > pageSize ? pageSize : itemList.Count; i < max; i++)
            {
                showList.Add(itemList[i]);
            } 
        }
		RepeatItem (showList, this._allItemTemp); 
	}
	/// <summary>
	/// Shows the equip item.
	/// </summary>
	/// <param name='itemList'>
	/// Item list.
	/// </param>
	public void ShowEquipItem (IList<ItemInfo> itemList)
	{ 
		this._equipItemInfoList = itemList;
		List<ItemInfo> showList = new List<ItemInfo> ();
		for (int i = 0,max = itemList.Count>pageSize? pageSize: itemList.Count; i < max; i++) {
			showList.Add (itemList [i]);
		}
		RepeatItem (showList, this._equipItemTemp);
	}
	/// <summary>
	/// Shows the normal item.
	/// </summary>
	/// <param name='itemList'>
	/// Item list.
	/// </param>
	public void ShowNormalItem (IList<ItemInfo> itemList)
	{
		this._normalItemInfoList = itemList;
		List<ItemInfo> showList = new List<ItemInfo> ();
		for (int i = 0,max = itemList.Count>pageSize? pageSize: itemList.Count; i < max; i++) {
			showList.Add (itemList [i]);
		}
		RepeatItem (showList, this._normalItemTemp);
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
	public void UpdateBagNum (int current, int maxBagNum)
	{
		this._allInfo.text = current + "/" + maxBagNum;
		this._equipInfo.text = current + "/" + maxBagNum;
		this._itemInfo.text = current + "/" + maxBagNum;
	}
	
	/// <summary>
	/// Switchs the tab.
	/// </summary>
	/// <param name='tab'>
	/// Tab.
	/// </param>
	public void SwitchTab (Tab tab)
	{
		
		switch (tab) {
		case Tab.all:
			this._all.gameObject.SetActive (true);
			this._equip.gameObject.SetActive (false);
			this._item.gameObject.SetActive (false);
			CurrentTab = Tab.all;
			this._allHiddenSale.text = "";
			this.currentPageAll = 1;
			this._allScrollView.ResetPosition ();
			break;
		case Tab.equip:
			this._all.gameObject.SetActive (false);
			this._equip.gameObject.SetActive (true);
			this._item.gameObject.SetActive (false);
			CurrentTab = Tab.equip;
			this._equipHiddenSale.text = "";
			this.currentPageEquip = 1;
			this._equipScrollView.ResetPosition();
			break;
		case Tab.item:
			this._all.gameObject.SetActive (false);
			this._equip.gameObject.SetActive (false);
			this._item.gameObject.SetActive (true);
			CurrentTab = Tab.item;
			this._normalHiddenSale.text = "";
			this.currentPageItem = 1;
			this._itemScrollView.ResetPosition();
			break;
		default:
			break;
		}
		TipsManager.Instance.CloseAllTipsUI (); //销毁tips界面
	}

	public void ChangeVipLevel ()
	{
		VipLevel.text = VipManager.Instance.VipLevel.ToString ();
		FightTxt.text = CharacterPlayer.character_property.getFightPower ().ToString ();
	}



	public void OnChangeAllItemList ()
	{

		ViewHelper.TouchUpAddingData (ref this.currentPageAll, this.pageSize, this.Height, this._allItemInfoList.Count, this._allItemList, (i) => {
			Destroy (itemContainer.FindChild (i.ToString ()).gameObject);
		}, (i) => {
			this.AddRow(this._allItemInfoList,i,itemContainer.gameObject,this._allItemTemp);
		});


		ViewHelper.TouchDownAddingData (ref this.currentPageAll, this.pageSize, this.Height, this._allItemInfoList.Count, this._allItemList, (i) => {
			Transform delTrans = itemContainer.FindChild (i.ToString ());
			if (delTrans != null) {
				Destroy (delTrans.gameObject);
			}
		}, (i) => {
			this.AddRow(this._allItemInfoList,i,itemContainer.gameObject,this._allItemTemp);
		});

	}


	public void OnChangeEquipItemList(){
		ViewHelper.TouchUpAddingData (ref this.currentPageEquip, this.pageSize, this.Height, this._equipItemInfoList.Count, this._equipItemList, (i) => {
			Destroy (itemContainer.FindChild (i.ToString ()).gameObject);
		}, (i) => {
			this.AddRow(this._equipItemInfoList,i,itemContainer.gameObject,this._equipItemTemp);
		});
		
		
		ViewHelper.TouchDownAddingData (ref this.currentPageEquip, this.pageSize, this.Height, this._equipItemInfoList.Count, this._equipItemList, (i) => {
			Transform delTrans = itemContainer.FindChild (i.ToString ());
			if (delTrans != null) {
				Destroy (delTrans.gameObject);
			}
		}, (i) => {
			this.AddRow(this._equipItemInfoList,i,itemContainer.gameObject,this._equipItemTemp);
		});
	}


	public void OnChangeItemList(){
		ViewHelper.TouchUpAddingData (ref this.currentPageItem, this.pageSize, this.Height, this._normalItemInfoList.Count, this._itemItemList, (i) => {
			Destroy (itemContainer.FindChild (i.ToString ()).gameObject);
		}, (i) => {
			this.AddRow(this._normalItemInfoList,i,itemContainer.gameObject,this._normalItemTemp);
		});
		
		
		ViewHelper.TouchDownAddingData (ref this.currentPageItem, this.pageSize, this.Height, this._normalItemInfoList.Count, this._itemItemList, (i) => {
			Transform delTrans = itemContainer.FindChild (i.ToString ());
			if (delTrans != null) {
				Destroy (delTrans.gameObject);
			}
		}, (i) => {
			this.AddRow(this._normalItemInfoList,i,itemContainer.gameObject,this._normalItemTemp);
		});
	}
 



}

