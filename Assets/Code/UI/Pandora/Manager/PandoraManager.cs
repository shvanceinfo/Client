/**该文件实现的基本功能等
function: 潘多拉的数据存储管理
author:zyl
date:2014-06-10
**/
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;
using UnityEngine;

namespace manager
{
	public class PandoraManager
	{
		private List<PandoraVo> _pandoraList;
		private Dictionary<int,PandoraVo> _dicPandora; 
		private Dictionary<int,PandoraNumVo> _dicPandoraNum;
		private bool _isInit;
		private DataReadItem _item;

		private GCChallengePandora _challengePandora;
		private GCResetPandoraNum _resetPandoraNum;
		private GCChallengeAllPandora _challengeAllPandora;
		private GCOpenPandora _openPandora;

		private PandoraVo _currentPandora;




		/// <summary>
		/// 潘多拉副本
		/// </summary>
		private static PandoraManager _instance;
	 	
		private PandoraManager(){
			this._pandoraList = new List<PandoraVo> ();
			this._dicPandora = new Dictionary<int, PandoraVo> ();
			this._dicPandoraNum = new Dictionary<int, PandoraNumVo> ();
			this._item = ConfigDataManager.GetInstance ().getItemTemplate (); //得到道具的配置信息
			this._challengePandora = new GCChallengePandora ();
			this._resetPandoraNum = new GCResetPandoraNum ();
			this._challengeAllPandora = new GCChallengeAllPandora ();
			this._openPandora = new GCOpenPandora ();

		}

		#region 属性

		private GCOpenPandora OpenPandora {
			get {
				return _openPandora;
			}
		}

		private GCChallengeAllPandora ChallengeAllPandora {
			get {
				return _challengeAllPandora;
			}
		}

		private GCResetPandoraNum ResetPandoraNum {
			get {
				return _resetPandoraNum;
			}
		}

		private GCChallengePandora ChallengePandora {
			get {
				return _challengePandora;
			}
		}

		public DataReadItem Item {
			get {
				return _item;
			}
		}

		public bool IsInit {
			get {
				return _isInit;
			}
			set {
				_isInit = value;
			}
		}

		public PandoraVo CurrentPandora {
			get {
				return _currentPandora;
			}
			private set {
				_currentPandora = value;
			}
		}

		public Dictionary<int, PandoraVo> DicPandora {
			get {
				return _dicPandora;
			}
		}

		public List<PandoraVo> PandoraList {
			get {
				return _pandoraList;
			}
		}

		public Dictionary<int, PandoraNumVo> DicPandoraNum {
			get {
				return _dicPandoraNum;
			}
		}

		public static PandoraManager Instance {
			get { 
				if (_instance == null)
					_instance = new PandoraManager ();
				return _instance; 
			}
		}
		#endregion

		#region 打开窗口
		public void OpenWindow(){
			if (this.PandoraList.Count==0) {
				FloatMessage.GetInstance().PlayFloatMessage("没有副本数据",
				                                         UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
				return;
			}

			this.Init ();
			UIManager.Instance.openWindow (UiNameConst.ui_pandora);
			this.Show ();
		}
		public void CloseWindow ()
		{
			UIManager.Instance.closeAllUI ();
		}
		
		#endregion

		#region 控制器代码

		public void Show(){
			Gate.instance.sendNotification (MsgConstant.MSG_PANDORA_SHOW);
		}



		#endregion

		#region 方法
		private void Init(){
			this.CurrentPandora = this.PandoraList [0];
		}

		public void Reset(){

		}

		#endregion

 
 

		#region 网络通信
		/// <summary>
		/// 前往挑战潘多拉
		/// </summary>
		public void AskChallengePandora ()
		{
			NetBase.GetInstance ().Send (this.ChallengePandora.ToBytes(), false);
		}

		/// <summary>
		/// 重置潘多拉挑战次数
		/// </summary>
		/// <param name="num">重置次数</param>
		public void AskResetPandoraNum(ushort num){
			NetBase.GetInstance ().Send (this.ResetPandoraNum.ToBytes(num), false);
		}

		/// <summary>
		/// 全部挑战
		/// </summary>
		/// <param name="num">全部挑战的次数</param>
		public void AskChallengeAllPandora(ushort num){
			NetBase.GetInstance ().Send (this.ChallengeAllPandora.ToBytes(num), false);
		}

		/// <summary>
		/// 开启潘多拉宝盒
		/// </summary>
		public void AskOpenPandora(){
			NetBase.GetInstance ().Send (this.OpenPandora.ToBytes(), false);
		}

		#endregion

	}
}
