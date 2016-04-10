/**该文件实现的基本功能等
function: 实现装备tips界面
author:zyl
date:2014-7-9
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;
using System.Linq;

public class PetEquipTipsView : TipsView {

	const int HEIGHT = -30;
	const int EXTMAX = 6;
	//	Transform _trans;
	UILabel _title;  		//标题
	UILabel _partinfo; 		//使用部位
	
	UILabel _levelnum;		//使用等级
	UILabel _goldnum;		//价格
	
	
	Transform _basicattrTemp; 	//基本属性模板
	 
	
	Transform _curt;
	Transform _equip;
	Transform _intensify;
	Transform _sale;
	
	void Awake ()
	{
		base.Awake ();
		this._title = this._trans.Find ("info/title").GetComponent<UILabel> ();
		this._partinfo = this._trans.Find ("info/part/partinfo").GetComponent<UILabel> ();
		this._levelnum = this._trans.Find ("info/level/levelnum").GetComponent<UILabel> ();
		
		
		this._goldnum = this._trans.Find ("info/gold/goldnum").GetComponent<UILabel> ();
		
		
		
		this._basicattrTemp = this._trans.Find ("info/basicattr/attr");
		 
		
	 
		this._equip = this._trans.Find ("btn/equip");
	 
		this._sale = this._trans.Find ("btn/sale");
	 
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new PetEquipTipsMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.PET_EQUIP_TIPS_MEDIATOR);
	}
	
	#region 控制显示

	
	public void ShowEquipAndSale ()
	{
		this._equip.gameObject.SetActive (true);
		this._sale.gameObject.SetActive (true);
	}
	
	public void ShowPerfectNone(){
		this._equip.gameObject.SetActive (false);
		this._sale.gameObject.SetActive (false);
	}
	
	#endregion
	
	
	public 	void ShowInfo (EquipmentStruct equipInfo, ItemInfo itemInfo, TipsCommand cmd)
	{
		if (itemInfo.InstanceId!=0) {    //如果是背包装备则需要加上强化等级
			this._title.text = itemInfo.Item.name + (equipInfo.intensifyLevel!=0?"    +"+equipInfo.intensifyLevel:"");
		}else{
			this._title.text = itemInfo.Item.name;
		}
		this._goldnum.text = itemInfo.Item.silivePrice.ToString ();
		this._levelnum.text = itemInfo.Item.usedLevel.ToString ()+"阶";
		this._partinfo.text = BagManager.Instance.GetEquipPartString (equipInfo.equipPart);
		
		 
		BasicAttrInit (ref equipInfo);

		#region 根据tips状态显示按钮
		switch (cmd) {
		case TipsCommand.PerfectNone:
			this.ShowPerfectNone();
			break;
		case TipsCommand.EquipAndSale:
			this.ShowEquipAndSale ();
			break;
		default:
			break;
		}
		#endregion
		
		this.SetPosition ();
		
	}
	
	/// <summary>
	/// 基本属性
	/// </summary>
	/// <param name='equipInfo'>
	/// Equip info.
	/// </param>
	public void BasicAttrInit (ref EquipmentStruct equipInfo)
	{
		var attrlist = this._trans.Find ("info/basicattr/attrlist");
		if (attrlist != null) {
			Destroy (attrlist.gameObject);
		} //先销毁原始数据
		
		GameObject obj = new GameObject ();				//创建空对象
		obj.name = "attrlist";
		obj.transform.parent = _basicattrTemp.parent;
		obj.transform.localPosition = Vector3.one;
		obj.transform.localScale = Vector3.one;
		obj.layer = LayerMask.NameToLayer("UI");
		
		
		this._basicattrTemp.gameObject.SetActive (true);
		int count = 0;
		
		foreach (var item in equipInfo.BaseAtrb) {
			
			GameObject itemTemp = NGUITools.AddChild(_basicattrTemp.parent.gameObject, this._basicattrTemp.gameObject);//赋值模板 
			Transform basicTrans = itemTemp.transform;
			basicTrans.localPosition = new Vector3 (0, count * HEIGHT, 0);  //设置位置
			string name = EquipmentManager.GetEquipAttributeName (item.Type);	 //基本名字
			basicTrans.Find ("attname").GetComponent<UILabel> ().text = name;
            string svalue = PowerManager.Instance.ChangeInfoData(item.Type,item.Value);
            basicTrans.Find("attvalue").GetComponent<UILabel>().text = svalue;
			
			
			 
			
			count++;
		}
		
		this._basicattrTemp.gameObject.SetActive (false);
	}
	
	 
	
	
	
	void Update ()
	{
		this.CheckShowOrHide ();
	}

}
