/**该文件实现的基本功能等
function: 实现扫荡的管理
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetGame;
using System;
using model;
using MVC.entrance.gate;

namespace manager
{	
	public class SweepManager
	{		
		const int NEXT_RESULT_PADDING = 10; //两次结果的间隔
		public int sweepTotalNum;		//扫荡的总次数
		
		private static SweepManager _instance;
		private Hashtable _energyHash;		//体力的哈希表		
		private TimeSpan _countDownSpan;
		private MapDataItem _currentMap; 		//当前点开的数据
		private float _lastCloseTime;		//上次关闭
		private List<SweepResultVO> _resultList;
		private int _currentSweepNum; 	//当前扫荡的次数
		private bool _isSweeping;			//是否在扫荡中
		private bool _isAccelerate; 		//是否在加速
		private bool _isShowResult;			//是否在显示结果的状态中

		private SweepManager()
		{
			this._isShowResult = false;
			_energyHash = new Hashtable();
			_isSweeping = false;
			_currentMap = null;
			_lastCloseTime = 0f;
			_currentSweepNum = 0;
			sweepTotalNum = 0;
			_isAccelerate = false;
			_resultList = new List<SweepResultVO>();
		}
		
		//开始跟服务器通信，请求开始，结束，加速扫荡
		public void askServerSweep(uint askFlag)
		{
			if(askFlag == MsgConstant.MSG_SWEEP_START)
			{  
				_isSweeping = true;
				_isAccelerate = false;
				_resultList.Clear(); //清除原来的结果
				_lastCloseTime = 0f;
				_currentSweepNum = 1; //请求扫荡都是第一次扫荡
				int needSeconds = VipManager.Instance.SweepJiaSu * sweepTotalNum;
				DateTime allTime = DateTime.Now.AddSeconds(needSeconds);
				_countDownSpan = allTime.Subtract(DateTime.Now);
				GCAskBeginSweep ask = new GCAskBeginSweep();
				NetBase.GetInstance().Send(ask.ToBytes((uint)_currentMap.id), false);
			}
			else if(askFlag == MsgConstant.MSG_SWEEP_STOP)
			{
				GCAskStopSweep ask = new GCAskStopSweep();
				NetBase.GetInstance().Send(ask.ToBytes(), false);
			}
			else if(askFlag == MsgConstant.MSG_SWEEP_ACCELERATE)
			{
				_isAccelerate = true;
				GCAskAccelerateSweep ask = new GCAskAccelerateSweep();
				NetBase.GetInstance().Send(ask.ToBytes(sweepTotalNum - _currentSweepNum + 1), false);
			}
		}
		
		//设置服务器返回的扫荡结果
		public void setSweepResult(bool isExp, int num, uint itemID=0)
		{
			if(_currentSweepNum == 0)
				return;

			#region 初始化结果对象
			SweepResultVO result;
			if(_currentSweepNum > _resultList.Count)
			{
				result = new SweepResultVO();
				_resultList.Add(result);
			}
			else
				result = _resultList[_currentSweepNum-1];
			#endregion

			#region 设置经验和获得的物品数量
			if(itemID == 0) //就是添加经验跟金钱   ,来自GSNotifyAssetChange  GSNotifyExpChange 类 
			{
				if(isExp)
					result.expNum = num;
				else
					result.goldNum = num;
			}
			else
			{
				ItemTemplate itemTemp = ItemManager.GetInstance().GetTemplateByTempId(itemID);
				if(result.itemHash.Contains(itemTemp.name))
				{
					int oldNum = (int)result.itemHash[itemTemp.name];
					result.itemHash[itemTemp.name] = oldNum + num;
				}
				else
					result.itemHash.Add(itemTemp.name, num);
			}
			#endregion

		}
		
		//设置当前的关卡
		public void setCurrentMap(int raidID)
		{
			_currentMap = ConfigDataManager.GetInstance().getMapConfig().getMapData(raidID);
		}
		
		//设置下次扫荡次数
		public void setNextSweep()
		{
			if(_currentSweepNum >= sweepTotalNum)  //扫荡结束
			{
				Gate.instance.sendNotification(MsgConstant.MSG_SWEEP_SHOW_FINAL_RESULT,_currentSweepNum);
				_currentSweepNum = 0;
			}
			else
			{
				this._isShowResult = true;
				Gate.instance.sendNotification(MsgConstant.MSG_SWEEP_SHOW_RESULT,_currentSweepNum);
				_currentSweepNum++;
				if(!_isAccelerate) //不是加速状态才申请下一次
				{
					GCAskBeginSweep ask = new GCAskBeginSweep(); //请求第二次扫荡
					NetBase.GetInstance().Send(ask.ToBytes((uint)_currentMap.id), false);
				}
			}
		}
		
		//结束扫荡时清楚扫荡信息(异常结束会调用)
		public void clearSweepInfo()
		{
			_isSweeping = false;
			_isAccelerate = false;
			_lastCloseTime = 0f;
			_currentSweepNum = 0;
			sweepTotalNum = 0;
			_resultList.Clear();
		}

		/// <summary>
		/// 非异常结束清空结果
		/// </summary>
		public void ClearSweepInfoNoException(){
			_isSweeping = false;
			_lastCloseTime = 0f;
			_currentSweepNum = 0;
			sweepTotalNum = 0;
			_resultList.Clear();
		}


		//getter and setter
		public static SweepManager Instance
		{
			get 
			{ 
				if(_instance == null)
					_instance = new SweepManager();
				return _instance; 
			}
		}

		public bool IsAccelerate {
			get {
				return _isAccelerate;
			}
			set {
				_isAccelerate = value;
			}
		}

		public bool IsShowResult {
			get {
				return _isShowResult;
			}
			set {
				_isShowResult = value;
			}
		}
		
		public Hashtable EnergyHash
		{
			get { return _energyHash; }
		}
		
		public TimeSpan CountDownSpan
		{
			get { return _countDownSpan; }
			set { _countDownSpan = value; }
		}
		
		public MapDataItem CurrentMap
		{
			get { return _currentMap; }
		}
		
		public float LastCloseTime
		{
			get {return _lastCloseTime;}
			set { _lastCloseTime = value;}
		}
		
		public int CurrentSweepNum
		{
			get { return _currentSweepNum; }
			set {
				this._currentSweepNum = value;
			}
		}
		
		public List<SweepResultVO> ResultList
		{
			get { return _resultList; }
		}
		
		public bool IsSweeping 
		{
			get { return _isSweeping; }
		}

		public bool CheckIsCanSweep{
			get{
				TimeSpan countDownSpan = this.CountDownSpan;
				int addMinute = 1;
				if((int)countDownSpan.TotalSeconds == 0)
					addMinute = 0;
				int diamond = (int)countDownSpan.TotalMinutes + addMinute;
				if (CharacterPlayer.character_asset.diamond<diamond) {
					return false;
				}
				return true;
			}
		}

	}
}
