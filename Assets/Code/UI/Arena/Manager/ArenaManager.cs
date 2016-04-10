/**该文件实现的基本功能等
function: 竞技场管理器
author:ljx
date:2013-11-09
**/
using System;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using model;
using NetGame;
using UnityEngine;
using helper;

namespace manager
{
	public class ArenaManager
	{
		public const float cameraView = 59f;
		public const float posY = -0.44f;
		public const float posZ = 3.55f;
		public const float posX1 = -1.49f;
		public const float posX2 = -0.51f;
		public const float posX3 = 0.43f;
		public const float posX4 = 1.29f;
		public const float posX5 = 2.22f;
		
		public uint buyItemID;			//购买的荣誉商店的ID
		public uint challengerRank;		//被挑战者的排名
		public string challengerName; 	//被挑战者的名称
		public bool playEffect; 		//播放开战动画
		
		private static ArenaManager _instance;
		private CharacterPlayer challenger;
		private ArenaVo _arenaVo;
		private Hashtable _awardHash; //竞技场奖励信息
		private Hashtable _honorShopHash; //荣誉商城信息
		private Hashtable _needHonorHash; //荣誉等级升级的信息
		private TimeSpan _timeSpan; //挑战的cd时间
        private TimeSpan _awardSpan;//奖励时间

        
		
		//服务器请求信息
		private GCAskArenaInfo _askArenaInfo;  		//服务器请求竞技场信息
		private GCAskChallenge _askChallenge;	//服务器请求挑战
		private GCAskHeroRankList _askHeroList;		//请求英雄榜
		private GCAskHonorShopInfo _askHonorShop;	//请求荣誉商店信息
		private GCAskBuyHonorItem _buyHonorItem;	//购买荣誉榜物品
        private GCAskReceiveArenaAward _askAward;   //请求奖励
		private ArenaManager()
		{
			_arenaVo = new ArenaVo();
			_awardHash = new Hashtable();
			_honorShopHash = new Hashtable();
			_needHonorHash = new Hashtable();
			_askArenaInfo = new GCAskArenaInfo();
			_askChallenge = new GCAskChallenge();
			_askHeroList = new GCAskHeroRankList();
			_askHonorShop = new GCAskHonorShopInfo();
			_buyHonorItem = new GCAskBuyHonorItem();
			buyItemID = 0;
			challengerRank = 0;
			playEffect = false;
		}

        public void SendReceiveAward()
        {
            _askAward = new GCAskReceiveArenaAward();
            NetBase.GetInstance().Send(_askAward.ToBytes());
            Gate.instance.sendNotification(MsgConstant.MSG_ARENA_OPEN_AWARD, false);
            ViewHelper.DisplayMessageLanguage("arena_award_sucess");
            _arenaVo.ArenaInfo.isReceiveed = true;
        }
		//请求竞技场相关数据
		public void askArenaInfo()
		{
			NetBase.GetInstance().Send(_askArenaInfo.ToBytes(), true);
		}
		
		//开始倒计时表现
		public void prepareArena()
		{
			if(challengerRank > 0) //有过请求挑战
			{
                BattleArena.GetInstance().PrepareBattleArena(); //开始对战
                UIManager.Instance.closeAllUI(false, true); //清除所有UI
                NPCManager.Instance.createCamera(false); //清除人物相机
                if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
                {
                    BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, PathConst.ARENA_BEGIN_EFFECT,
                        (asset) =>
                        {
                            GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                            obj.transform.localScale = Vector3.one;
                            obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                            obj.transform.localPosition = new Vector3(0f, 1f, 1f);
                            playEffect = true;
                        });
                }
                else
                {
                    GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.ARENA_BEGIN_EFFECT, EBundleType.eBundleUIEffect);
                    GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                    obj.transform.localPosition = new Vector3(0f, 1f, 1f);
                    playEffect = true;
                }          
			}
		}
		
		//请求挑战竞技场
		public void askChallenge()
		{
			string errMsg;
			if(_arenaVo.ArenaInfo.remainChallengeNum <=0)
			{
				errMsg = LanguageManager.GetText("not_enough_challenge_num");
				FloatMessage.GetInstance().PlayFloatMessage(errMsg, UIManager.Instance.getRootTrans(), 
		                                            new Vector3(0f, 150f, -150f), new Vector3(0f, 280f, -150f));
				challengerRank = 0;
			}
			else if(_timeSpan.TotalSeconds > 0)
			{
				errMsg = LanguageManager.GetText("in_arena_cd");
				FloatMessage.GetInstance().PlayFloatMessage(errMsg, UIManager.Instance.getRootTrans(), 
		                                            new Vector3(0f, 150f, -150f), new Vector3(0f, 280f, -150f));
				challengerRank = 0;
			}
			else
				NetBase.GetInstance().Send(_askChallenge.ToBytes(challengerRank, challengerName), false);
		}
		
		//汇报竞技场结果
		public void sendArenaResult()
		{
			GCReportChallengeResult reportArena = new GCReportChallengeResult();
	        NetBase.GetInstance().Send(reportArena.ToBytes(BattleArena.GetInstance().m_bPlayerWin), false);
	        BattleArena.GetInstance().EndFight();
		}
		
		//显示竞技场奖励UI
		public void showAwardInfo()
		{
			UIManager.Instance.openWindow(UiNameConst.ui_arena_result);
	        ArenaResult result = UIManager.Instance.getUIFromMemory(UiNameConst.ui_arena_result).GetComponent<ArenaResult>();
            //uint newRank = 0;
            //if(challengerRank < _arenaVo.ArenaInfo.currentRank)
            //    newRank = challengerRank;
            if (BattleArena.GetInstance().m_bPlayerWin)
            {
                result.showResult(BattleArena.GetInstance().m_bPlayerWin, challengerRank);
            }
            else {
                result.showResult(BattleArena.GetInstance().m_bPlayerWin, _arenaVo.ArenaInfo.currentRank);
            }
	        
		}
		
		//网络信息返回才显示竞技场信息
		public void showArenaUI()
		{
			UIManager.Instance.openWindow(UiNameConst.ui_arena);
			AwardMsg award = getAwardMsg(ArenaVo.ArenaInfo.currentRank);
			Gate.instance.sendNotification(MsgConstant.MSG_OPEN_ARENA, award);
			DateTime nextTime = DateTime.Now.AddSeconds(_arenaVo.ArenaInfo.remainTime);
			_timeSpan = nextTime.Subtract(DateTime.Now);
            _awardSpan = new TimeSpan((new DateTime().AddSeconds(_arenaVo.ArenaInfo.lessReceiveTime)).Ticks);
            Gate.instance.sendNotification(MsgConstant.MSG_ARENA_AWARD_BTN, !ArenaVo.ArenaInfo.isReceiveed);
		}
		
		//显示战斗信息列表
		public void showChallengeResult()
		{
			Gate.instance.sendNotification(MsgConstant.MSG_ARENA_RESULT_INFO);
			controlCamera(false);
		}
		
		//显示英雄榜列表
		public void showHeroList()
		{
			Gate.instance.sendNotification(MsgConstant.MSG_OPEN_HERO_BOARD);
			controlCamera(false);
		}
		
		//显示荣誉商店
		public void showHonorShop()
		{
			Gate.instance.sendNotification(MsgConstant.MSG_OPEN_HONOR_SHOP);
			controlCamera(false);
		}
		
 
		
		//更新CD时间跟挑战次数
		public void setCDAndNum()
		{
			Gate.instance.sendNotification(MsgConstant.MSG_ARENA_SET_CHALLENGE_NUM, _arenaVo.ArenaInfo.remainChallengeNum);
			DateTime nextTime = DateTime.Now.AddSeconds(_arenaVo.ArenaInfo.remainTime);
			_timeSpan = nextTime.Subtract(DateTime.Now);
		}
		
		//点击按钮处理弹出的消息, needHonor 需要的荣誉值
		public void showClickDialog(uint msgType, uint needHonor = 0)
		{		
			uint needDiamond = 0;	//需要的钻石
			string showMsg = "";
			bool showError = false; //是否显示错误的消息
			eDialogSureType type = eDialogSureType.eSureBuyChallengeNum;
			switch (msgType) 
			{
				case MsgConstant.MSG_HONOR_BUY:
					if(needHonor > _arenaVo.ArenaInfo.currentHonor)
					{
						showError = true;
						showMsg = LanguageManager.GetText("buy_honor_item_not_diamond");
					}
					else
					{
						showError = false;
						showMsg = LanguageManager.GetText("buy_honor_item_sure");
						showMsg = showMsg.Replace(Constant.REPLACE_PARAMETER_1, needHonor.ToString());
					}
					type = eDialogSureType.eSureBuyHonorItem;
					break;
				case MsgConstant.MSG_ARENA_BUY_NUM:
					needDiamond = 10 + _arenaVo.ArenaInfo.buyChallengNum*5;
					if(needDiamond > CharacterPlayer.character_asset.diamond)
					{
						showError = true;
						showMsg = LanguageManager.GetText("buy_challenge_num_not_diamond");
					}
					else
					{
						showError = false;
						showMsg = LanguageManager.GetText("buy_challenge_num_sure");
						showMsg = showMsg.Replace(Constant.REPLACE_PARAMETER_1, needDiamond.ToString());
					}
					type = eDialogSureType.eSureBuyChallengeNum;
					break;
				case MsgConstant.MSG_ARENA_CLEAR_CD:
					needDiamond = (uint)((_timeSpan.Minutes + 1) * 2);
					if(needDiamond > CharacterPlayer.character_asset.diamond)
					{
						showError = true;
						showMsg = LanguageManager.GetText("clear_challenge_cd_not_diamond");
					}
					else if(_timeSpan.TotalSeconds <=0)
					{
						showError = true;
						showMsg = LanguageManager.GetText("clear_challenge_cd_not_need");
					}
					else
					{
						showError = false;
						showMsg = LanguageManager.GetText("clear_challenge_cd_sure");
						showMsg = showMsg.Replace(Constant.REPLACE_PARAMETER_1, needDiamond.ToString());
					}
					type = eDialogSureType.eSureClearChallengeCD;
					break;
				default:				
					break;
			}
			if(showError)
				FloatMessage.GetInstance().PlayFloatMessage(showMsg, UIManager.Instance.getRootTrans(), 
					new Vector3(0f, 220f, -50f), new Vector3(0f, 300f, -50f));
			else
			{
				controlCamera(false);
				UIManager.Instance.ShowDialog(type, showMsg);
			}
		}
		
		//控制相应的模型相机
		public void controlCamera(bool enable, bool destroy=false)
		{
			if(destroy)
				NPCManager.Instance.createCamera(false);
			else if(enable)
				NPCManager.Instance.ModelCamera.active = true;
			else
				NPCManager.Instance.ModelCamera.active = false;
		}
		
		//获取相应的奖励信息
		private AwardMsg getAwardMsg(uint rank)
		{
			foreach (AwardMsg msg in _awardHash.Values) 
			{
				if(msg.minRank <= rank && rank <= msg.maxRank)
					return msg;	
			}
			return null;
		}
		
		//getter and setter		
		public static ArenaManager Instance
		{
			get 
			{ 
				if(_instance == null)
					_instance = new ArenaManager();
				return _instance; 
			}
		}
        /// <summary>
        /// 今日奖励
        /// </summary>
        public AwardMsg CurAward
        {
            get {
                return getAwardMsg(ArenaManager.Instance.ArenaVo.ArenaInfo.currentRank);
            }
        }
		
		public ArenaVo ArenaVo
		{
			get { return _arenaVo; }
		}
		
		public Hashtable HonorShopHash
		{
			get { return _honorShopHash; }
		}
		
		public Hashtable AwardHash
		{
			get { return _awardHash; }
		}
		
		public Hashtable NeedHonorHash
		{
			get { return _needHonorHash; }
		}
		
		public TimeSpan TimeSpan
		{
			get { return _timeSpan; }
			set {_timeSpan = value; }
		}

        public TimeSpan AwardSpan
        {
            get { return _awardSpan; }
            set { _awardSpan = value; }
        }
	}
}
