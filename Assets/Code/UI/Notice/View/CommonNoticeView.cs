/**该文件实现的基本功能等
function: 实现公共公告界面
author:zyl
date:2014-5-5
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;
using model;
using System.Collections.Generic;

public class CommonNoticeView : MonoBehaviour
{
	
	private const int DURATION = 10;
	Transform _trans;
	Transform _info;
	Transform _bg;
	
	NoticeMsgVo msgVo;
	
	void Awake ()
	{
		this._trans = this.transform;
		this._info = this._trans.Find ("info");
		this._bg = this._trans.Find ("bg");
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new CommonNoticeMediator (this));
	}
	
	void OnDisable ()
	{	 
		Gate.instance.removeMediator (MediatorName.COMMON_NOTICE_MEDIATOR);
	}
	
	public  void ShowInfo ()
	{
		if (msgVo.Num == 0) {												//如果播放完成,则移除
			CommonNoticeManager.Instance.MsgList.Remove (msgVo);
			CommonNoticeManager.Instance.IsShowNotice = false;
			return;
		}
		
		#region 显示的文字
		if (msgVo.From == "1") {
			this._info.GetComponent<UILabel> ().text = LanguageManager.GetText ("msg_common_notice_from") + msgVo.Message;
		} else {
			this._info.GetComponent<UILabel> ().text = msgVo.Message;
		}
		#endregion
		
		
		var tweenPos = UITweener.Begin<TweenPosition> (this._info.gameObject, DURATION);
		 
		tweenPos.SetOnFinished (new EventDelegate (() => {
			msgVo.Num--;
			this.ShowInfo ();
		}));														   //播放完成递归此方法
		CommonNoticeManager.Instance.IsShowNotice = true;			   //正在显示公告的状态
	}
	
	void Update ()
	{
		if (CommonNoticeManager.Instance.IsShowNotice == false && CommonNoticeManager.Instance.MsgList.size > 0) {  //没有在播放公告，并且公告数据条数大于0,则开始播放公告
			msgVo = CommonNoticeManager.Instance.MsgList [0];    //得到第一条公告
			this.ShowUI ();
			this.ShowInfo ();
		}
		
		if (CommonNoticeManager.Instance.MsgList.size == 0) {
			this.HideUI ();
		}
	}
	
	public  void StopTween ()
	{
		this._info.GetComponent<TweenPosition> ().enabled = false;
	}
	
	public void ResumeTween ()
	{
		this._info.GetComponent<TweenPosition> ().enabled = true;
	}
	
	public void HideUI ()
	{
		this._bg.gameObject.SetActive (false);
		this._info.gameObject.SetActive (false);
	}
	
	public void ShowUI ()
	{
		if (this._bg.gameObject.activeSelf == false) {
			this._bg.gameObject.SetActive (true);
		}
		if (this._info.gameObject.activeSelf == false) {
			this._info.gameObject.SetActive (true);
		}
	}
	
	
}
