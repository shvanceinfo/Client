/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-4-14
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;


public class BtnTipsMsg : MonoBehaviour
{
	private int _instanceId;
	private ItemInfo _iteminfo;
	private bool _isShow;
	
	public int InstanceId {
		get {
			return this._instanceId;
		}
		set {
			_instanceId = value;
		}
	}
	public ItemInfo Iteminfo {
		get {
			return this._iteminfo;
		}
		set {
			_iteminfo = value;
		}
	}
	
	void OnClick ()
	{
		if (this.Iteminfo!=null) {
			TipsManager.Instance.ShowCommonInfo(this.Iteminfo,this.transform);
		}else{
			TipsManager.Instance.ShowCommonInfo(this._instanceId,this.transform);
		}
        EasyTouchJoyStickProperty.ShowJoyTouch(false); 
 		 
	}
	
	
 
	
}
