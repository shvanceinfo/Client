using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class BasicView : MonoBehaviour {

	protected Transform _trans;
	protected UILabel _name;
	protected UILabel _num;
	
	protected virtual void Awake ()
	{
		this._trans = this.transform;
		this._name = this._trans.Find ("Item/Name").GetComponent<UILabel> ();
		this._num = this._trans.Find ("Count/InputCount/Label").GetComponent<UILabel> ();
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new SaleMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.SALE_MEDIATOR);
	}
	
	public virtual void ShowSaleInfo (ItemTemplate itemTemp, int num)
	{
		this._name.text = itemTemp.name;
		this._num.text = num.ToString ();
	}
	
	public virtual void UpdateSaleInfo (ItemTemplate itemTemp, int num)
	{
		this._num.text = num.ToString ();
	}
}
