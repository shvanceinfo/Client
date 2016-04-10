using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using NetGame;
using helper;

namespace manager
{
	public class MedalManager
	{
		
		private Hashtable _medalHash;
		private int _curID;        //当前勋章ID
		private int _nextID;        //下一级勋章ID
		private int _curLevel;	//当前勋章等级
		private int _nextLevel;
		private int _maxLevel;  //最大等级
		public bool isMedalLevelUp;//判断勋章是否升级

		private bool isRequestData;

		public bool IsRequestData {
			get { return isRequestData; }
		}

		private MedalManager ()
		{
			isRequestData = false;
			_curLevel = 1;
			_medalHash = new Hashtable ();
		}
		 

		/// <summary>
		/// 勋章页签被激活
		/// </summary>
		public void OnTableClick ()
		{
			AskInfo ();
			Gate.instance.sendNotification (MsgConstant.MSG_MEDAL_DISPLAY_VIEW);
		}

        #region 单利
		static MedalManager _instance;

		public static MedalManager Instance {
			get {
				if (_instance == null)
					_instance = new MedalManager ();
				return _instance;
			}
		}
        #endregion
       
		#region 字段
		public MedalVo FindVoByID (int id)
		{
			foreach (MedalVo vo in _medalHash.Values) {
				if (vo.ID == id) {
					return vo;
				}
			}
			return null;
		}
        #endregion

		
        #region 字段
		public MedalVo FindVoByLevel (int lvl)
		{
			foreach (MedalVo vo in _medalHash.Values) {
				if (vo.Level == lvl) {
					return vo;
				}
			}
			return null;
		}

        #endregion

		/// <summary>
		/// 数据表
		/// </summary>
		public Hashtable MedalHash {
			get { return _medalHash; }
		}

		/// <summary>
		/// 当前勋章ID
		/// </summary>
		public int CurID {
			get { return _curID; }
		}


		/// <summary>
		/// 当前勋章等级
		/// </summary>
		public int CurLevel {
			get { return _curLevel; }
		}


		/// <summary>
		/// 下一级勋章等级，如果为满级，则为当前
		/// </summary>
		public int NextLevel {
			get { return (_curLevel + 1) > MaxLevel ? MaxLevel : (_curLevel + 1); }
		}

        

		/// <summary>
		/// 获取勋章最大等级
		/// </summary>
		public int MaxLevel {
			get { return _maxLevel; }
			set {
				if (value > _maxLevel) {
					_maxLevel = value;
				}
			}
		}

		/// <summary>
		/// 返回当前VO
		/// </summary>
		public MedalVo CurVo {
			get {
				return FindVoByLevel (_curLevel);
			}
		}



		/// <summary>
		/// 返回下一级VO
		/// </summary>
		public MedalVo NextVo {
			get {
				return FindVoByLevel (NextLevel);
			}
		}

		public void AskInfo ()
		{
			if (!isRequestData) {
				GCAskMedalInfo ask = new GCAskMedalInfo ();
				NetBase.GetInstance ().Send (ask.ToBytes ());
			} else {
				GetServerData (_curID);
			}
                
            
		}

		public void MedalLevelUp ()
		{
			if (ViewHelper.CheckIsHava (CurVo.Consumes [0].Type, CurVo.Consumes [0].Value)) {
				GCAskMedalLevelUp ask = new GCAskMedalLevelUp ();
				NetBase.GetInstance ().Send (ask.ToBytes ());
			}
		}

		public void GetServerData (int id)
		{
			isRequestData = true;
			_curID = id;
			var medal = FindVoByID (_curID);
			if (medal == null) {
				return;
			}
			_curLevel = medal.Level;
			//if ( !isMedalLevelUp )
			Gate.instance.sendNotification (MsgConstant.MSG_MEDAL_DISPLAY_VIEW);

			if (CharacterPlayer.sPlayerMe != null) {
				CharacterPlayer.sPlayerMe.GetComponent<HUD> ().GenerateHeadUI (CharacterPlayer.character_property.nick_name, 1, HUD.HUD_CHARACTER_TYPE.HCT_PLAYER, false);
			}
		}

	}
}
