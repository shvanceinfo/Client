/**该文件实现的基本功能等
function: 翅膀的数据存储管理
author:ljx
date:2013-11-09
**/
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;

namespace manager
{
	public class WingManager
	{
		public uint previewLadder; //当前预览的阶数
//		private uint _currentExp;  //当前的经验值
		private uint _currentLuckNum; //当前幸运值
//		private uint _currentStar; //当前星星的数目
		private uint _currentLevel; //当前的阶数
		
		private static WingManager _instance;
		private Hashtable _wingHash;
		private WingVO _currentWing;
		private float _currentTime; //当前时间
		
		//net message
//		private GCAskWingCulture _askCulture;
		private GCAskWingEvolution _askEvolution;
		private GCAskWingInfo _askInfo;
		private bool _openWingAsk;
				
		private WingManager ()
		{
            _wingHash = new Hashtable();
            _askInfo = new GCAskWingInfo();
//			_askCulture = new GCAskWingCulture();
            _askEvolution = new GCAskWingEvolution();
			init();
		}

	    public void init()
	    {
            previewLadder = 0;
            _currentLuckNum = 0; //当前幸运值
            _currentLevel = 0; //当前的阶数           
            _currentWing = null;
            _currentTime = 0f; //当前时间
            _openWingAsk = false;
	    }
		
		//初始化设置翅膀ID
		public void setCurrentWing (uint wingID)
		{
			_currentWing = _wingHash [wingID] as WingVO;
		}
		
		//请求翅膀初始数据
		public void askWingMsg ()
		{
			NetBase.GetInstance ().Send (_askInfo.ToBytes (), true);
			_openWingAsk = true;
		}
		
		//初始化翅膀的界面
		public void initWing (uint wingID, uint exp, uint luckNum, int doubleNum)
		{
			bool evoSuccess = false; //默认非进阶
			if (this._currentWing==null) {				//如有翅膀则需要初始化翅膀，并且为自己模型绑定翅膀
				this.setCurrentWing(wingID);
				CharacterPlayer.sPlayerMe.character_avatar.installWing();
				return;
			}
			if (!_openWingAsk && wingID != _currentWing.id) //进阶成功
				evoSuccess = true;

//			uint expDelta = 0;
//			if(exp > _currentExp)
//				expDelta = exp - _currentExp;
//			else if(!_openWingAsk)
//				expDelta = _currentWing.wingLvUpExp - _currentExp;
			_currentWing = _wingHash [wingID] as WingVO;	 //得到翅膀数据
			uint luckDelta = 0;
			if (luckNum > _currentLuckNum)
				luckDelta = luckNum - _currentLuckNum;
//			_currentExp = exp;
			_currentLuckNum = luckNum;
//			_currentStar = wingID % 1000;
			_currentLevel = wingID / 1000;
//			bool showCulture = true; 
			if (_openWingAsk) { //初始化界面需要重置模型以及阶数
				_openWingAsk = false;
				Gate.instance.sendNotification (MsgConstant.MSG_WING_UPDATE_MODEL_LADDER, new List<string>{_currentWing.wingName, getLadder (_currentLevel), _currentWing.wingModle});
//				if(_currentExp == _currentWing.wingLvUpExp && _currentStar == 10) //显示进阶界面	
//					showCulture = false;
			} else {
				if (evoSuccess) { //进阶成功换翅膀模型跟名字
					Gate.instance.sendNotification (MsgConstant.MSG_WING_UPDATE_MODEL_LADDER, new List<string>{_currentWing.wingName, getLadder (_currentLevel), _currentWing.wingModle});
					Gate.instance.sendNotification (MsgConstant.MSG_WING_STOP_AUTO); //进阶成功停止自动进阶
					Gate.instance.sendNotification (MsgConstant.MSG_WING_SHOW_EFFECT, new List<string>{"1", LanguageManager.GetText ("wing_evo_success"), _currentWing.playEffect});
                    Gate.instance.sendNotification(MsgConstant.MSG_WING_SHOW_SUCCESS);
                }
//				else if(_currentExp == _currentWing.wingLvUpExp && _currentStar == 10) //进阶返回
//				{
//					if(_currentLevel <= 10) //不满十阶不发生变化，直接提升
//					{
////						showCulture = false;	
////						if(expDelta > 0)//最后一次培养大于零，以后就是进阶，培养成功停止自动培养
////							Gate.instance.sendNotification(MsgConstant.MSG_WING_STOP_AUTO); 
//						if(luckDelta > 0)
//							Gate.instance.sendNotification(MsgConstant.MSG_WING_SHOW_EFFECT, new List<string>{"1", LanguageManager.GetText("wing_evo_per_time")+luckDelta, _currentWing.playEffect});				
//					}
//				}
//				else if(expDelta > 0)
//					Gate.instance.sendNotification(MsgConstant.MSG_WING_SHOW_EFFECT, new List<string>{doubleNum.ToString(), 
//					                               	LanguageManager.GetText("wing_culture_per_time")+expDelta, _currentWing.playEffect});
				else if (luckDelta > 0) { //不满十阶不发生变化，直接提升
						Gate.instance.sendNotification (MsgConstant.MSG_WING_SHOW_EFFECT, new List<string>{"1", LanguageManager.GetText ("wing_evo_per_time") + luckDelta, _currentWing.playEffect});				
				}						
			}
			Gate.instance.sendNotification (MsgConstant.MSG_WING_INIT, false);
//			if(showCulture)
//			{
//				List<uint> lists = new List<uint>{_currentExp, _currentWing.wingLvUpExp, _currentStar};
//				Gate.instance.sendNotification(MsgConstant.MSG_WING_UPDATE_EXP, lists);
//			}
//			else
			calSuccessRate ();
			
			ShowTime ();
		}
		
//		//开始培养, 是否自动培养
//		public void beginCulture(bool isAuto=false)
//		{
//			if(canCulture())
//				NetBase.GetInstance().Send(_askCulture.ToBytes(), false); //Gate.instance.sendNotification(MsgConstant.MSG_WING_NOT_ENOUGH_MSG, "aaaaaaaa");
//			else if(isAuto) //如果是自动，要停止相应线程
//				Gate.instance.sendNotification(MsgConstant.MSG_WING_STOP_AUTO);
//		}
		
		//获取阶数
		public string getLadder (uint ladder)
		{
			string ladderName = "wing_ladder_" + ladder;
			return LanguageManager.GetText (ladderName);
		}
		
		//一次培养结束
		public void finishCulture ()
		{
			
		}
		
		//开始进阶，是否自动进阶
		public void beginEvolute (bool isAuto=false)
		{
			if (canEvolute ())
				NetBase.GetInstance ().Send (_askEvolution.ToBytes (), false);
			else if (isAuto) //如果是自动，要停止相应线程
				Gate.instance.sendNotification (MsgConstant.MSG_WING_STOP_AUTO);			
		}
		
		//一次进阶结束
		public void finishEvolute ()
		{
			
		}
		
		//计算成功率
		private void calSuccessRate ()
		{
			string successText = LanguageManager.GetText ("wing_success_lower");
			float rate = ((int)_currentLuckNum - (int)_currentWing.lowLimit) * _currentWing.sucRateAdd;
			if (rate < 0.2f)
				successText = LanguageManager.GetText ("wing_success_lower"); 	//极低
			else if (rate < 0.4f)
				successText = LanguageManager.GetText ("wing_success_low");	//低
			else if (rate < 0.6f)
				successText = LanguageManager.GetText ("wing_success_middle");	//中
			else if (rate < 0.8f)
				successText = LanguageManager.GetText ("wing_success_high");	//高
			else 
				successText = LanguageManager.GetText ("wing_success_higher");	//极高
			Gate.instance.sendNotification (MsgConstant.MSG_WING_UPDATE_LUCK, new ArrayList{_currentLuckNum, _currentWing.highLimit, successText});
		}
		
		//能否进行培养操作
//		private bool canCulture()
//		{
//			if(_currentLevel == 10 && _currentStar == 10) //满十阶不发生变化，并且使能按钮
//			{
//				Gate.instance.sendNotification(MsgConstant.MSG_WING_NOT_ENOUGH_MSG, "wing_full_ladder"); //满星都停止自动培养
//				return false;
//			}
//			else if(ItemManager.GetInstance().GetItemNumById(_currentWing.cultureItemID) >= _currentWing.cultureItemNum)
//				return true;
//			else
//			{
//				Gate.instance.sendNotification(MsgConstant.MSG_WING_NOT_ENOUGH_MSG, "wing_not_enough_culture");
//				return false;
//			}		
//		}
		
		//能否进行进阶操作
		private bool canEvolute ()
		{
            if (!FeebManager.Instance.CheckIsHave((uint)_currentWing.evoCostItem, (int)_currentWing.evoNum))
            {
                Gate.instance.sendNotification(MsgConstant.MSG_WING_SET_CAMERA,false);
                return false;
            }
            else { 
                
            }
            //if (ItemManager.GetInstance ().GetItemNumById (_currentWing.evoCostItem) < _currentWing.evoNum) {
            //    Gate.instance.sendNotification (MsgConstant.MSG_WING_NOT_ENOUGH_MSG, "wing_not_enough_evolution");
            //    return false;
            //} else 
            if (CharacterPlayer.character_asset.gold < _currentWing.costGold) {
				Gate.instance.sendNotification (MsgConstant.MSG_WING_NOT_ENOUGH_MSG, "wing_not_enough_money");
				return false;
			} else			
				return true;
		}

		public void ShowTime ()
		{
			ArrayList al = new ArrayList();
			al.Add(_currentLuckNum);
			al.Add(_currentWing.isLimit);
			Gate.instance.sendNotification (MsgConstant.MSG_WING_SHOW_TIME, al);
		}
		
		//getter and setter
		public WingVO CurrentWing {
			get { return _currentWing; }
		}
		
		public Hashtable WingHash {
			get { return _wingHash; }
		}
		
		public uint CurrentLevel {
			get { return _currentLevel; }
		}
		
		public static WingManager Instance {
			get { 
				if (_instance == null)
					_instance = new WingManager ();
				return _instance; 
			}
		}
	}
}
