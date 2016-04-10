/**该文件实现的基本功能等
function: 弹出对话框的数据存储管理
author:zyl
date:2014-4-5
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using NetGame;



public enum eDialogSureType
{
	eNone = 0,
	eApplicationExit = 1,
	eSellItem = 2,
	eExitFight =3,
	eReturnLogin, 	//返回游戏登录界面
	eExitGame, 		//退出游戏
	eBuyItem,		//购买物品
	eRecoverHPMP,	//主城回复血法包
	ePurchaseGoldenGoblinTicket,        // 购买黄金哥布林入场券
	eSureClearChallengeCD, 	//确认清除挑战CD
	eSureBuyChallengeNum,	//确认购买挑战次数
	eSureBuyHonorItem,		//确认购买商城物品
	eSureBuyEngery,			//确认购买体力
	eSureBuyRevive,			//确认购买复活
    eSureComsumeCry,        //确认消耗水晶进入恶魔洞窟
	eDownloadAsset,			//下载场景资源
	eNetworkUnReachable,	//网络不可用
    eRefine_Reset,          //确认重置属性
	eSureBackCity	,		//确认是否回城
	eDisconnect,		// disconnect
    eChannel,
}



namespace manager
{
	public class DialogManager
	{
		
		private eDialogSureType sureType;
		
		private static DialogManager _instance;
		
		public delegate void SetDialogMessageEventHandler(string msg);
		public SetDialogMessageEventHandler OnSetDialogMsg;
	 	
		public static DialogManager Instance {
			get {
				if (_instance == null) {
					_instance = new DialogManager ();
				}
				return _instance;
			}
		}
		//设置弹出框的类型
		public eDialogSureType SureType {
			get {
				return this.sureType;
			}
			set {
				sureType = value;
			}
		}
		
		
		//显示文字
	public void Show (eDialogSureType type, string msg)
	{
		DialogManager.Instance.SureType = type; //设置属性的类型
		 
		Gate.instance.sendNotification (MsgConstant.MSG_DIALOG_SHOW,msg);
	}
		
	public void SetDialogMsg(string msg)
	{
		if (OnSetDialogMsg != null)
		{
			OnSetDialogMsg(msg);		
		}
	}
 
		//关闭窗口
		public void Close ()
		{
			UIManager.Instance.closeWaitting ();
			string uiName = null;
			if (SceneManager.Instance.currentScene == SCENE_POS.IN_CITY) {
                uiName = UiNameConst.ui_dialog;
			} else {
                uiName = UiNameConst.ui_dialog_fight;
			}
			if (sureType == eDialogSureType.eNone) {
				UIManager.Instance.closeWindow (uiName);
			} else {
                UIManager.Instance.closeWindow(uiName);
			}
		}
		 
		
		//执行关闭弹出窗口的事件
		public void  Cancel ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_ENABLE_MODEL_CAMERA);
			EventDispatcher.GetInstance ().OnDialogCancel (sureType);	
			Close (); //关闭窗口
		}


		
		//执行确认按钮 
		public void Sure ()
        {
            HideEnemyBlood();
            HideGoblinIcon();
            Debug.Log(sureType);
			EventDispatcher.GetInstance ().OnDialogSure (sureType);
			Gate.instance.sendNotification (MsgConstant.MSG_SURE_DIALOG, sureType);
			if (sureType != eDialogSureType.eApplicationExit) {
				Close ();
				Gate.instance.sendNotification (MsgConstant.MSG_ENABLE_MODEL_CAMERA);
			}
            if (sureType == eDialogSureType.eExitFight)
            {
                MessageManager.Instance.sendMessageReturnCity();
				UIManager.Instance.showWaitting (true); //回主城强制弹出对话框
				Global.m_bAutoFightSaved = Global.m_bAutoFight;
				Global.m_bAutoFight = false;
			}
            if (sureType==eDialogSureType.eSureComsumeCry)  //确认消耗水晶进入
            {
                Close();
                Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DIALOG_SURE);
            }
            switch (sureType)
            {
                case eDialogSureType.eNone:
                    break;
                case eDialogSureType.eApplicationExit:
                    break;
                case eDialogSureType.eSellItem:
                    break;
                case eDialogSureType.eExitFight:
                    break;
                case eDialogSureType.eReturnLogin:
                    SettingManager.Instance.ReLogin();
                    break;
                case eDialogSureType.eExitGame:
                    break;
                case eDialogSureType.eBuyItem:
                    break;
                case eDialogSureType.eRecoverHPMP:
                    break;
                case eDialogSureType.ePurchaseGoldenGoblinTicket:
                    break;
                case eDialogSureType.eSureClearChallengeCD:
                    break;
                case eDialogSureType.eSureBuyChallengeNum:
                    break;
                case eDialogSureType.eSureBuyHonorItem:
                    break;
                case eDialogSureType.eSureBuyEngery:
                    break;
                case eDialogSureType.eSureBuyRevive:
                    break;
                case eDialogSureType.eSureComsumeCry:
                    break;
                case eDialogSureType.eDownloadAsset:
                    break;
                case eDialogSureType.eNetworkUnReachable:
                    break;
				case eDialogSureType.eDisconnect:
					EasyTouchJoyStickProperty.ShowJoyTouch(false);
					break;
                case eDialogSureType.eRefine_Reset:
                    Close();
                    Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DIALOG_CALLBACK);
                    break;
                default:
                    break;
            }
		}

        private void HideEnemyBlood()
        {
            Transform trans = GameObject.Find("ui_root").transform.FindChild("Camera");
            for (int i = 0; i < trans.childCount; i++)
            {
				for(int j=0;j<10;j++)
				{
                    if (trans.GetChild(i).name.Length > 1 && trans.GetChild(i).name.Substring(0, 1).Equals(j.ToString()))
                    {
                        GameObject.Destroy(trans.GetChild(i).gameObject);
                    }
				}
            }
        }

        private void HideGoblinIcon()
        {
            if (CharacterPlayer.sPlayerMe.GetComponent<HUD>().insObj != null)
            {
                CharacterPlayer.sPlayerMe.GetComponent<HUD>().insObj.SetActive(false);
            }
        }
		
		 
	}
}
