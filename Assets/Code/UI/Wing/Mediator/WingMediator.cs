/**该文件实现的基本功能等
function: 实现翅膀培养的View控制
author:ljx
date:2013-11-09
**/
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using NetGame;

namespace mediator
{	
	public class WingMediator : ViewMediator
	{	
		private bool _autoCulture;  //是否正在进行自动培养
		private bool _autoEvolution; //是否正在自动进阶
	
		public WingMediator(WingView view, uint id = MediatorName.WING_MEDIATOR) : base(id, view)
		{
			_autoCulture = false;
			_autoEvolution = false;
		}
		
		public override IList<uint> listReferNotification()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_WING_INIT,
				MsgConstant.MSG_WING_AUTO_CULTURE,
				MsgConstant.MSG_WING_AUTO_EVOLUTION,
				MsgConstant.MSG_WING_CULTURE,
				MsgConstant.MSG_WING_EVOLUTION,
				MsgConstant.MSG_SHOW_CULTURE,
				MsgConstant.MSG_SHOW_EVOLUTION,	//显示进阶界面
				MsgConstant.MSG_WING_UPDATE_EXP,	  //经验条的变化
				MsgConstant.MSG_WING_UPDATE_MONEY,  //金钱钻石的变化
				MsgConstant.MSG_WING_UPDATE_LUCK,	  //成功率，幸运点的变化
				MsgConstant.MSG_WING_UPDATE_MODEL_LADDER,  //更新阶数的时候同时更新模型
				MsgConstant.MSG_WING_SHOW_EFFECT,  //每次培养进阶的时候显示文字以及特效
				MsgConstant.MSG_WING_NOT_ENOUGH_MSG,	  //弹出材料不足的信息
				MsgConstant.MSG_WING_STOP_AUTO,	  //退出自动培养，自动进阶
				MsgConstant.MSG_WING_DRAG_NEXT,
				MsgConstant.MSG_WING_DRAG_PREV,
                MsgConstant.MSG_WING_SET_CAMERA,
				MsgConstant.MSG_WING_SHOW_TIME,
                MsgConstant.MSG_WING_SHOW_RETURN,
                MsgConstant.MSG_WING_SHOW_SUCCESS,
			};
		}
		
		//处理相关的消息
		public override void handleNotification(INotification notification)
		{
			WingView wingView = view;
			if(wingView != null)
			{				
				switch (notification.notifyId) 
				{
					case MsgConstant.MSG_WING_INIT:
						bool showCulture = (bool)notification.body;
						wingView.showView(showCulture);
						break;
					case MsgConstant.MSG_CLOSE_UI:
						UIManager.Instance.closeAllUI();
						_autoCulture = false;
						_autoEvolution = false;
//						funcMgr.isChildOpen = false; //还原子菜单为关闭状态
						wingView.StopAllCoroutines();
						break;
//					case MsgConstant.MSG_SHOW_CULTURE:
//						wingView.showView(true);
//						break;
					case MsgConstant.MSG_SHOW_EVOLUTION:
						wingView.showView(false);
						break;
//					case MsgConstant.MSG_WING_AUTO_CULTURE:
//						if(_autoCulture)
//							Gate.instance.sendNotification(MsgConstant.MSG_WING_STOP_AUTO);
//						else
//						{
//							_autoCulture = true;
//							wingView.autoCultureEvo(true);
//						}
//						break;	
					case MsgConstant.MSG_WING_AUTO_EVOLUTION:
						if(_autoEvolution)
							Gate.instance.sendNotification(MsgConstant.MSG_WING_STOP_AUTO);
						else
						{
							_autoEvolution = true;
							wingView.autoCultureEvo(false);
						}
						break;
//					case MsgConstant.MSG_WING_CULTURE:
//						if(_autoCulture)
//							wingView.showErrMsg("wing_auto_culture_msg");
//						else
//							WingManager.Instance.beginCulture();
//						break;
					case MsgConstant.MSG_WING_EVOLUTION:
						if(_autoEvolution)
							wingView.showErrMsg("wing_auto_evolution_msg");
						else
							WingManager.Instance.beginEvolute();
						break;	
//					case MsgConstant.MSG_WING_UPDATE_EXP:
//						List<uint> lists = notification.body as List<uint>;
//						uint exp = lists[0];
//						uint maxExp = lists[1];
//						uint starNum = lists[2];
//						wingView.updateExp(exp, maxExp, starNum);
//						break;
					case MsgConstant.MSG_WING_UPDATE_LUCK:
						ArrayList lucks = notification.body as ArrayList;
						if(lucks != null && lucks.Count >=2)
							wingView.updateLuckPoint((uint)lucks[0], (uint)lucks[1], (string)lucks[2]);
						break;
					case MsgConstant.MSG_WING_UPDATE_MONEY:
						wingView.updateMoney();
						break;		
					case MsgConstant.MSG_WING_UPDATE_MODEL_LADDER:
						List<string> models = notification.body as List<string>;
						if(models != null)
						{
                            wingView.updateLadder(models[0], models[1], models[2], WingManager.Instance.CurrentWing.modelPos, WingManager.Instance.CurrentWing.modelScale);
							wingView.updateLevelUpLadder(models[0], models[1], models[2]);
							CharacterPlayer.sPlayerMe.character_avatar.installWing();
						}
						break;
					case MsgConstant.MSG_WING_SHOW_EFFECT:
						List<string> effects = notification.body as List<string>;
						if(effects != null)
							wingView.showEffect(effects[0], effects[1], effects[2]);
						break;
                    case MsgConstant.MSG_WING_SHOW_SUCCESS:
                        wingView.SuccessPlayAnim();
                        break;
					case MsgConstant.MSG_WING_NOT_ENOUGH_MSG:
						string errMsg = (string)notification.body;
						wingView.showErrMsg(errMsg);
						break;
					case MsgConstant.MSG_WING_STOP_AUTO:
						wingView.changeBtn(_autoCulture, false); //切换按钮
						_autoCulture = false;
						_autoEvolution = false;
						wingView.StopAllCoroutines();
						break;
					case MsgConstant.MSG_WING_DRAG_NEXT: //预览下一个
						if(WingManager.Instance.previewLadder < 10)
						{
							WingManager.Instance.previewLadder++;
							previewWing(wingView);
						}
						break;
					case MsgConstant.MSG_WING_DRAG_PREV:
						if(WingManager.Instance.previewLadder > 1)
						{
							WingManager.Instance.previewLadder--;
							previewWing(wingView);
						}
						break;
                    case MsgConstant.MSG_WING_SET_CAMERA:
                        view.SetCamera((bool)notification.body);
                        break;
					case MsgConstant.MSG_WING_SHOW_TIME:
						ArrayList al = (ArrayList)notification.body;
						uint num =  (uint)al[0];
						bool isLimit = (bool)al[1];
 
						wingView.ShowTime(num,isLimit);
						break;
                    case MsgConstant.MSG_WING_SHOW_RETURN:
                        WingManager.Instance.previewLadder = WingManager.Instance.CurrentLevel;
                        previewWing(wingView);
                        break;
					default:					
						break;
				}
			}
		}
		
		//预览翅膀
		private void previewWing(WingView wingView)
		{
			Gate.instance.sendNotification(MsgConstant.MSG_WING_STOP_AUTO);
			wingView.enableButton(); //使能按钮
			uint wingID = WingManager.Instance.previewLadder*1000 + 1;
			WingVO vo = WingManager.Instance.WingHash[wingID] as WingVO; 
			wingView.updateLadder(vo.wingName, WingManager.Instance.getLadder(WingManager.Instance.previewLadder), vo.wingModle,vo.modelPos,vo.modelScale);
			wingView.updateMaterial(vo);
			wingView.updateAttr(vo.attrTypes, vo.attrValues);
		}
		
		//getter and setter
		public WingView view
		{
			get 
			{
				if(_viewComponent != null && _viewComponent is WingView)
					return _viewComponent as WingView;
				return  null;					
			}
		}
	}
}
