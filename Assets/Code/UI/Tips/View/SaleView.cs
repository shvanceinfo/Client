/**该文件实现的基本功能等
function: 实现出售界面
author:zyl
date:2014-4-15
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class SaleView : BasicView
{
	 
	UILabel _price;
 
	protected override void  Awake ()
	{
		base.Awake();
		this._price = this._trans.Find ("Count/sumPrice").GetComponent<UILabel> ();
	}
	
	 
	public override void ShowSaleInfo (ItemTemplate itemTemp, int num)
	{
		base.ShowSaleInfo(itemTemp,num);
		this._price.text = (itemTemp.silivePrice * num).ToString ();
	}
	
	public override void UpdateSaleInfo (ItemTemplate itemTemp, int num)
	{
		base.UpdateSaleInfo(itemTemp,num);
		this._price.text = (itemTemp.silivePrice * num).ToString ();
	}
}
