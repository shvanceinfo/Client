/**该文件实现的基本功能等
function: 哥布林的数据存储管理
author:zyl
date:2014-3-12
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;

namespace manager
{
	public class GoblinManager
	{
	
		private static GoblinManager _instance;
		
		private uint _remainTimes; //当前剩余的次数
		private uint _todayBuy;    //今天已经购买的次数
		private uint _canBuyTimes; //今天剩下的购买次数
		private uint _price;       //当前购买一次需要的钻石数量
		
		private DataReadVIP _vip;
		private DataReadGBL _gbl;
		
		public static GoblinManager Instance {
			get {
				if (_instance == null) {
					_instance = new GoblinManager ();
				}
				return _instance;
			}
		}
	
		private GoblinManager ()
		{
			_vip = ConfigDataManager.GetInstance().getVIPConfig();
			_gbl = ConfigDataManager.GetInstance().getGBLConfig();
		}

		public uint Price {
			get {
				return this._price;
			}
		}		
		
		
		//初始化
		public void InitGoblin (uint remainTimes, uint todayBuy)
		{
			this._remainTimes = remainTimes;
			this._todayBuy = todayBuy;
			
			//显示哥布林模型
			Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_INIT);
			//更新剩余次数
			Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_UPDATE_ENTER_TIMES,new List<uint>(){remainTimes,todayBuy}); 
			this.UpdateGoblinCanBuyNum(todayBuy);
			this.UpdateGoblinBuyPrice(todayBuy);
		}
 		
		//更新哥布林信息
		public void UpdateGoblinInfo(uint remainTimes, uint todayBuy){
			this._remainTimes = remainTimes;
			this._todayBuy = todayBuy;
			//更新剩余次数
			Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_UPDATE_ENTER_TIMES,new List<uint>(){remainTimes,todayBuy}); 
			this.UpdateGoblinCanBuyNum(todayBuy);
			this.UpdateGoblinBuyPrice(todayBuy);
		}
		
		
		//是否能进入哥布林副本
		public bool IsCanEnterGoblin(){
			if (this._remainTimes>0) {
				return true;
			}else
				return false;
		}
		
		
	 	//哥布林副本可以购买的次数
		public void UpdateGoblinCanBuyNum(uint todayBuy){


            this._canBuyTimes = VipManager.Instance.GblBuyCount > todayBuy ? (uint)(VipManager.Instance.GblBuyCount - todayBuy) : 0; 
			Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_CAN_BUY_NUM,this._canBuyTimes);
		}
		
		//哥布林副本购买的价格
		public void UpdateGoblinBuyPrice(uint todayBuy){
			todayBuy++;//得到实际第几次购买. todayBuy为0,则是第1次购买
			var gbl = this._gbl.getGBLData((int)todayBuy);
			if (gbl != null) {
				this._price = gbl.dia_price;
				Gate.instance.sendNotification(MsgConstant.MSG_GOBLIN_BUY_PRICE,this._price);
			}else{  //购买溢出的情况
				Gate.instance.sendNotification(MsgConstant.MSG_CLEAR_GOBLIN_BUY_PRICE);
			}
		}
		
		//是否还能购买打副本次数
		public bool IsCanBuyTimes(){
			if (this._canBuyTimes>0) {
				return true;
			}else
				return false;
		}
		
		public bool IsDiamondEnough(){
			if (CharacterPlayer.character_asset.diamond >=this._price) {
				return true;
			}else
				return false;
		}
		
		
	}
	
}