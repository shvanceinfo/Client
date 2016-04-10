using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;
using System.Text;

public class NewFunctionView : MonoBehaviour
{
	bool _isPlay = false;
	Transform _trans;
	Transform _functionTemp;
	GameObject _newFun;
	Transform _newFunTrans;

	Transform _topLocation;
	Transform _bottomLocation;
	Transform _NguiCam;

	void Awake ()
	{
		this._trans = this.transform;
		this._functionTemp = this._trans.FindChild ("function");  //模板
										 

		 
	}

	void Start(){
		if (Global.inCityMap ()) {
			this._topLocation = this._trans.parent.FindChild ("ui_main/top_right/showFunc");  //右上位置
			this._bottomLocation = this._trans.parent.FindChild ("ui_main/bottom/menu");     //右下位置
		} else {																			//战斗场景
			this._topLocation = this._trans.parent.FindChild ("ui_fight/Top_Right/Chat_Function/Button_Chat");  //右上位置
			this._bottomLocation = this._trans.parent.FindChild ("ui_fight/bottom/menu");     //右下位置
		}




		_NguiCam = UICamera.currentCamera.transform;	 //ngui 摄像机位置
	}

	void Update ()
	{
		if (this._isPlay == false && FastOpenManager.Instance.FunctionEffectList.Count > 0) {
			this._isPlay = true;
			var model = FastOpenManager.Instance.FunctionEffectList [0];

			this._newFun = NGUITools.AddChild (this.gameObject, this._functionTemp.gameObject);
			this._newFunTrans = this._newFun.transform;
			this._newFunTrans.FindChild ("bg/img").GetComponent<UISprite> ().spriteName = model.FunctionIcon;  //设置图片

			switch (model.Location) {  //根据位置设置飞的方向
			case LocationType.RightTop:
				this._newFunTrans.FindChild ("bg").GetComponent<TweenPosition>().to = this._NguiCam.InverseTransformPoint(this._topLocation.position);
				break;
			case LocationType.Bottom:
				this._newFunTrans.FindChild ("bg").GetComponent<TweenPosition>().to = this._NguiCam.InverseTransformPoint(this._bottomLocation.position);
				break;
			default:
				break;
			}

			this._newFun.SetActive (true);
			FastOpenManager.Instance.FunctionEffectList.RemoveAt (0);
		}
 		 
	}

	public void DestroyFunObj ()
	{
		Destroy (this._newFun);
		this._isPlay = false;
		if (FastOpenManager.Instance.FunctionEffectList.Count == 0) {
			FastOpenManager.Instance.CloseNewFunctionWindow ();
		}
	}


}
