//自动计算位置
#define AutoCal    

/**该文件实现的基本功能等
function: 实现tips界面
author:zyl
date:2014-4-12
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class CommonTipsView : TipsView
{
	const int offsetY = 0;	//y轴的偏移
	const int offsetX = 0;	//x轴的偏移
	
	
//	Transform _trans;
	UILabel _title;  		//标题
	UILabel _levelnum;		//使用等级
	UILabel _description;	//描述
	UILabel _source;		//来源
	UILabel _goldnum;		//价格
	 
	Transform _compound;	//合成
	Transform _open;		//打开
	Transform _sale;		//出售
	
	
	void Awake ()
	{
		base.Awake ();
//		this._trans = this.transform;
		this._title = this._trans.Find ("info/title").GetComponent<UILabel> ();
		this._levelnum = this._trans.Find ("info/levelnum").GetComponent<UILabel> ();
		this._description = this._trans.Find ("info/des/description").GetComponent<UILabel> ();
        this._source = this._trans.Find("info/sources/source").GetComponent<UILabel>();
		this._goldnum = this._trans.Find ("info/goldnum").GetComponent<UILabel> ();
		 
		this._compound = this._trans.Find ("btn/compound");
		this._open = this._trans.Find ("btn/open");
		this._sale = this._trans.Find ("btn/sale");
		
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new CommonTipsMediator (this));
	}
	
	void OnDisable ()
	{	 
		Gate.instance.removeMediator (MediatorName.COMMON_TIPS_MEDIATOR);
	}
	/// <summary>
	/// Shows the compound.
	/// </summary>
	public void ShowCompound ()
	{
		this._compound.gameObject.SetActive (true);
        this._open.gameObject.SetActive(false);
        this._sale.localPosition = new Vector3(50, -110, 0);
	}
	/// <summary>
	/// Shows the open.
	/// </summary>
	public void ShowOpen ()
	{
		this._open.gameObject.SetActive (true);
        this._compound.gameObject.SetActive(false);
        this._sale.localPosition = new Vector3(50, -110, 0);
	}
	
	public void ShowNone ()
	{
		this._compound.gameObject.SetActive (false);
		this._open.gameObject.SetActive (false);
        this._sale.localPosition = new Vector3(-40,-110,0);
	}
	
	public void ShowPerfectNone ()
	{
		this._compound.gameObject.SetActive (false);
		this._open.gameObject.SetActive (false);
		this._sale.gameObject.SetActive (false);
	}
	
	/// <summary>
	/// Shows the info.
	/// </summary>
	/// <param name='itemTemp'>
	/// Item temp.
	/// </param>
	public void ShowInfo (ItemTemplate itemTemp, TipsCommand cmd)
	{
		this._title.text = itemTemp.name;
		this._levelnum.text = itemTemp.usedLevel.ToString ();
		this._description.text = itemTemp.discription;
		this._source.text = itemTemp.itemSource;
		this._goldnum.text = itemTemp.silivePrice.ToString ();
		
		switch (cmd) {
		case TipsCommand.None:
			this.ShowNone ();
			break;	
		case TipsCommand.PerfectNone:
			this.ShowPerfectNone ();
			break;
		case TipsCommand.Compound:
			this.ShowCompound ();
			break;
		case TipsCommand.Open:
			this.ShowOpen ();
			break;
		default:
			break;
		}
		
#if AutoCal
		this.SetPosition ();
#endif		
		
	}
	

	
	
	
	void Update ()
	{
		this.CheckShowOrHide ();
	}
	
  
	
}
