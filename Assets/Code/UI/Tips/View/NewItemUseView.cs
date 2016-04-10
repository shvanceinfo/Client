/**该文件实现的基本功能等
function: 实现新物品使用界面
author:zyl
date:2014-5-16
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class NewItemUseView : MonoBehaviour
{
	Transform _trans;
	Transform _btnEquip;
	Transform _btnUse;
	Transform _battle;
	Transform _info;
	Transform _num;
	UILabel _name;
	UISprite _bg2;
	UITexture _item;
	
	
	
	void Awake ()
	{
		this._trans = this.transform;
		this._btnEquip = this._trans.FindChild ("btnEquip");
		this._btnUse = this._trans.FindChild ("btnUse");
		this._battle = this._trans.FindChild ("item/battle");
		this._info = this._trans.FindChild ("item/info");
		this._num = this._trans.FindChild ("item/num");
		this._name = this._trans.FindChild ("item/name").GetComponent<UILabel> ();
		this._bg2 = this._trans.FindChild("item/bg2").GetComponent<UISprite>();
		this._item = this._trans.FindChild("item/item").GetComponent<UITexture>();
		
		
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new NewItemUseMediator (this));
	}
	
	void OnDisable ()
	{	 
		Gate.instance.removeMediator (MediatorName.NEWITEM_USE_MEDIATOR);
	}
	
	public void ShowFirst (ItemInfo item)
	{
		
		this._name.text = item.Item.name;
		this._bg2.spriteName = BagManager.Instance.getItemBgByType (item.Item.quality, false);
		DealTexture.Instance.setTextureToIcon (this._item, item.Item);
		
		if (item.Item.packType == ePackageNavType.Other) {
			this._btnUse.gameObject.SetActive (true);
			this._btnEquip.gameObject.SetActive (false);
			this._battle.gameObject.SetActive (false);
			this._info.gameObject.SetActive (true);
			this._num.gameObject.SetActive (true);
			
			this._info.GetComponent<UILabel>().text = item.Item.discription;
			this._num.GetComponent<UILabel>().text = item.Num.ToString();
		} else {
			this._btnEquip.gameObject.SetActive (true);
			this._btnUse.gameObject.SetActive (false);
			this._battle.gameObject.SetActive (true);
			this._info.gameObject.SetActive (false);
			this._num.gameObject.SetActive (false);
			
			this._battle.FindChild("battle").GetComponent<UILabel>().text = BagManager.Instance.GetPowerCompareValue(item).ToString();
 

		}
	}
}
