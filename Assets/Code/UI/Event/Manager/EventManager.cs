/**该文件实现的基本功能等
function: 副本的数据存储管理
author:zyl
date:2014-5-12
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;

namespace manager
{
	public class EventManager
	{
		private bool _isInit = false;
		private List<int> _keyList; 					//主键队列
		private Dictionary<int,EventVo> _dictionaryEvent;//主键对应的键值对
		private GCAskJoinActivity _askJoinActivity;
		private static  EventManager _instance;

        public EventVo curActiveVo;

		private EventManager ()
		{
			this._keyList = new List<int> ();
			this._dictionaryEvent = new Dictionary<int, EventVo> ();
			this._askJoinActivity = new GCAskJoinActivity ();
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

		public List<int> KeyList {
			get {
				return this._keyList;
			}
			set {
				_keyList = value;
			}
		}

		public Dictionary<int, EventVo> DictionaryEvent {
			get {
				return this._dictionaryEvent;
			}
			set {
				_dictionaryEvent = value;
			}
		}
		 
		public static  EventManager Instance {
			get {
				if (_instance == null) {
					_instance = new EventManager ();
				}
				return _instance;
			}
		}
		#endregion
		
		#region 窗口打开关闭
		public void OpenWindow ()
		{
			if (this.IsInit == false) {
				this.KeyList.Sort ();
				this.IsInit = true;
			}
			UIManager.Instance.openWindow (UiNameConst.ui_event);
			this.EventShow ();
 
		}
		
		public void CloseUI ()
		{
			UIManager.Instance.closeWindow (UiNameConst.ui_event);
		}
		#endregion
		
		
		#region 触发的控制器事件
		public void EventShow ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_EVENT_SHOW);
		}
		
		public void EventUpdate (EventVo vo)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_EVENT_UPDATEINFO, vo);
		}
		#endregion
		
		
		
		
		#region 搜索
		public EventVo GetEventVoFromDictionary (int key)
		{
			return this.DictionaryEvent [key];
		}
		#endregion
		
		#region 网络通信
		/// <summary>
		/// 请求加入活动
		/// </summary>
		/// <param name='key'>
		/// Key.
		/// </param>
		public void	AskJoinActivity (int key)
		{
			if (this.DictionaryEvent.ContainsKey (key)) {
				var eventVo = this.GetEventVoFromDictionary (key);

				if (eventVo.EventStates == EventState.NotJoin) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_event_not_join"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				}else if(eventVo.EventStates == EventState.Finish){
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_event_finish"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
				}else{
                    curActiveVo = eventVo;

					NetBase.GetInstance ().Send (this._askJoinActivity.ToBytes (eventVo.InstanceId), true);
				}
 
			}else{
				FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_event_not_info"),
			                                           UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
			}
 
		}
		
		
		#endregion
		
		
	}
	
}