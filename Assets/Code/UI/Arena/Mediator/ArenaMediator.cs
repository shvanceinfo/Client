/**该文件实现的基本功能等
function: 竞技场的视图控制器
author:ljx
date:2013-11-09
**/
using System.Collections.Generic;
using model;
using manager;
using MVC.entrance.gate;
using MVC.interfaces;
using NetGame;
using helper;

namespace mediator
{
	public class ArenaMediator : ViewMediator
	{			
		public ArenaMediator(ArenaView view, uint id = MediatorName.ARENA_MEDIATOR) : base(id, view)
		{
		}
		
		public override IList<uint> listReferNotification()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_OPEN_ARENA,
				MsgConstant.MSG_OPEN_HERO_BOARD,
				MsgConstant.MSG_OPEN_HONOR_SHOP,
				MsgConstant.MSG_ARENA_CHALLENGE,
				MsgConstant.MSG_ARENA_CLEAR_CD,
				MsgConstant.MSG_ARENA_BUY_NUM,	  //购买竞技场挑战次数
				MsgConstant.MSG_ARENA_RESULT_INFO,
				MsgConstant.MSG_ARENA_SET_CHALLENGE_NUM,
				MsgConstant.MSG_ARENA_COUNT_DOWN,
				MsgConstant.MSG_SURE_DIALOG, //弹出确认对话框
				MsgConstant.MSG_ENABLE_MODEL_CAMERA, //激活模型相机
				MsgConstant.MSG_DISABLE_MODEL_CAMERA, //禁止模型相机
				MsgConstant.MSG_DESTROY_MODEL_CAMERA, //清除模型相机
                MsgConstant.MSG_ARENA_OPEN_AWARD,
                MsgConstant.MSG_ARENA_SEND_RECEIVE_AWARD,
                MsgConstant.MSG_ARENA_AWARD_BTN,
                MsgConstant.MSG_ARENA_AWARD_REFRESH,
			};
		}
		
		public override void handleNotification(INotification notification)
		{
			if(view != null)
			{
				ArenaView arenaView = view;
				switch (notification.notifyId) 
				{
					case MsgConstant.MSG_OPEN_ARENA:
						AwardMsg msg = notification.body as AwardMsg;
						arenaView.setAwardInfo(msg);
						arenaView.setArenaInfo();
						arenaView.setHonorAreaInfo();
						arenaView.setCheallengeInfo();
 
						break;
					case MsgConstant.MSG_OPEN_HERO_BOARD:
				        UIManager.Instance.openWindow(UiNameConst.ui_hero_list);
						break;
					case MsgConstant.MSG_OPEN_HONOR_SHOP:
				        UIManager.Instance.openWindow(UiNameConst.ui_honor_shop);
						break;	
					case MsgConstant.MSG_ARENA_CHALLENGE:
						if(ArenaManager.Instance.challengerRank == 0) //保证当前只被挑战一次
						{
							ArenaManager.Instance.challengerRank = (uint)notification.body;
							ArenaManager.Instance.challengerName = notification.type;
							ArenaManager.Instance.askChallenge(); //先请求挑战
						}
						break;
					case MsgConstant.MSG_ARENA_CLEAR_CD:
						ArenaManager.Instance.showClickDialog(notification.notifyId);
						break;
					case MsgConstant.MSG_ARENA_BUY_NUM:
						ArenaManager.Instance.showClickDialog(notification.notifyId);
						break;	
					case MsgConstant.MSG_ARENA_RESULT_INFO:
						UIManager.Instance.openWindow(UiNameConst.ui_battle_log);
						break;
					case MsgConstant.MSG_ARENA_SET_CHALLENGE_NUM:
						uint challengeNum = (uint)notification.body;
						arenaView.setChallengeNum(challengeNum);
						break;
					case MsgConstant.MSG_SURE_DIALOG:
						eDialogSureType sureType = (eDialogSureType)notification.body;
						if(sureType == eDialogSureType.eSureBuyChallengeNum)
						{
							GCAskBuyChallengeNum buyNum = new GCAskBuyChallengeNum();
							NetBase.GetInstance().Send(buyNum.ToBytes(), true);
						}
						else if(sureType == eDialogSureType.eSureClearChallengeCD)
						{
							GCAskClearChallengeCD clearCD = new GCAskClearChallengeCD();
							NetBase.GetInstance().Send(clearCD.ToBytes(), true);
						}
						break;
					case MsgConstant.MSG_ENABLE_MODEL_CAMERA:
						ArenaManager.Instance.controlCamera(true);
						break;
					case MsgConstant.MSG_DISABLE_MODEL_CAMERA:
						ArenaManager.Instance.controlCamera(false);
						break;
					case MsgConstant.MSG_DESTROY_MODEL_CAMERA:
						ArenaManager.Instance.controlCamera(true, true);
						break;
//					case MsgConstant.MSG_ARENA_COUNT_DOWN:
//						List<string> countDownTimes = notification.body as List<string>;
//						arenaView.setTimeInfo(countDownTimes[0], countDownTimes[1]);
//						break;	
                    case MsgConstant.MSG_ARENA_OPEN_AWARD:

                        bool isOpen = (bool)notification.body;
                        if (isOpen)
                        {
                            if (ArenaManager.Instance.ArenaVo.ArenaInfo.isReceiveed)
                            {
                                ViewHelper.DisplayMessageLanguage("arena_receiveed");
                                return;
                            }
                            view.DisplayAwardPanel(ArenaManager.Instance.CurAward);
                            ArenaManager.Instance.controlCamera(false);
                        }
                        else {
                            ArenaManager.Instance.controlCamera(true);
                            view.SetAwardPanel(false);
                        }
                        
                        break;
                    case MsgConstant.MSG_ARENA_SEND_RECEIVE_AWARD:
                        ArenaManager.Instance.SendReceiveAward();
                        break;
                    case MsgConstant.MSG_ARENA_AWARD_BTN:
                        view.SetAwardBtn((bool)notification.body);
                        break;
                    case MsgConstant.MSG_ARENA_AWARD_REFRESH:
                        view.RefreshHonor((string)notification.body);
                        break;
					default:					
						break;
				}
			}
		}
		
		public ArenaView view
		{
			get 
			{
				if(_viewComponent != null && _viewComponent is ArenaView)
					return _viewComponent as ArenaView;
				return  null;					
			}
		}
	}
}

