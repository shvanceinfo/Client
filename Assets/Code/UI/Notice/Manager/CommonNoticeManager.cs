//#define Test

/**该文件实现的基本功能等
function: 实现公共公告的管理
author:zyl
date:2014-5-5
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;

namespace manager
{
	public class CommonNoticeManager
	{
		private bool _isInit;
		private bool _isShowNotice;
		private BetterList<NoticeMsgVo> _msgList;
		private static CommonNoticeManager _instance;
		
		private CommonNoticeManager ()
		{
			_msgList = new BetterList<NoticeMsgVo> ();
			this._isShowNotice = false;
#if Test
			this.Init ();
#endif
			
		}

		#region 属性

		public bool IsInit {
			get {
				return this._isInit;
			}
			set {
				_isInit = value;
			}
		}
		
		public BetterList<NoticeMsgVo> MsgList {
			get {
				return this._msgList;
			}
			private	set {
				_msgList = value;
			}
		}
		
		public bool IsShowNotice {
			get {
				return this._isShowNotice;
			}
			set {
				_isShowNotice = value;
			}
		}
		
		public static CommonNoticeManager Instance {
			get {
				if (_instance == null) {
					_instance = new CommonNoticeManager ();
				}
				return _instance;
			}
		}
		#endregion
		
#if Test
		public void Init ()
		{
			NoticeMsgVo msg = new NoticeMsgVo ();
			msg.Message = "恭喜凶残苹果皮获得火龙剑+10，神力大增";
			msg.Num = 1;
			this.MsgList.Add (msg);
			
			NoticeMsgVo msg2 = new NoticeMsgVo ();
			msg2.Message = "恭喜凶残菠萝皮获得火龙剑+20，神力大增";
			msg2.Num = 2;
			this.MsgList.Add (msg2);
			
			
			NoticeMsgVo msg3 = new NoticeMsgVo ();
			msg3.Message = "恭喜凶残香蕉皮获得火龙剑+20，神力大增";
			msg3.Num = 3;
			this.MsgList.Add (msg3);
			
		}
#endif
		
		
		public void ShowInfo ()
		{
			if (!this.IsInit) {
				UIManager.Instance.openWindow (UiNameConst.ui_common_notice);  //激活面板
				this.IsInit = true;
			}
		}
		
		public void StopTween(){
			Gate.instance.sendNotification (MsgConstant.MSG_COMMON_NOTICE_STOPTWEEN);
		}
		
		public void ResumeTween(){
			Gate.instance.sendNotification (MsgConstant.MSG_COMMON_NOTICE_RESUMETWEEN);
		}
		
		public void HideUI ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_COMMON_NOTICE_HIDEUI);
		}
 
		 
		
	}
}
