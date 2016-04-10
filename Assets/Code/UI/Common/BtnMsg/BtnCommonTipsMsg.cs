/**该文件实现的基本功能等
function: 实现按钮点击的消息传送
author:zyl
date:2014-4-14
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;
using model;

public class BtnCommonTipsMsg : MonoBehaviour
{
 	
	public const string CLOSE_TIPS = "close_tips";
	public const string SALE = "sale";
	public const string OPEN = "open";
	public const string COMPOUND = "compound";
	public const string CURT = "curt";
	public const string EQUIP = "equip";
	public const string INTENSIFY = "intensify";
	
	void OnClick ()
	{
		switch (gameObject.name) {
		case CLOSE_TIPS:
			TipsManager.Instance.CloseAllTipsUI ();
			break;
		case SALE:
			SaleManager.Instance.ShowSaleInfo ();
			break;	
		case OPEN:
			SaleManager.Instance.ShowOpenInfo ();
			break;
		case COMPOUND:
			if (!FastOpenManager.Instance.CheckFunctionIsOpen(FunctionName.Merge)) {
				return;
			}
			NPCManager.Instance.createCamera (false); //消除3D相机
			FastOpenManager.Instance.OpenWindow(FunctionName.Merge);
			break;	
		case EQUIP:
			if (UIManager.Instance.getUIFromMemory(UiNameConst.ui_pet_equip_tips)!=null && UIManager.Instance.getUIFromMemory(UiNameConst.ui_bag)!=null) {
				UIManager.Instance.closeAllUI ();
				NPCManager.Instance.createCamera (false); //消除3D相机
				PetManager.Instance.OpenWindow(PetTabEnum.Equip);
				return;
			}

			TipsManager.Instance.NetPutEquip();
			break;
		case INTENSIFY:
			if (!FastOpenManager.Instance.CheckFunctionIsOpen(FunctionName.Strengthen)) {
				return;
			}

			UIManager.Instance.closeAllUI ();
			NPCManager.Instance.createCamera (false); //消除3D相机
			UIManager.Instance.openWindow(UiNameConst.ui_equip);
			UIManager.Instance.getUIFromMemory(UiNameConst.ui_equip).transform.FindChild("Table/Table1").GetComponent<UICheckBoxColor>().isChecked = true;
			Gate.instance.sendNotification(MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table1);
			break;
		case CURT:
			if (!FastOpenManager.Instance.CheckFunctionIsOpen(FunctionName.Refine)) {
				return;
			}


			UIManager.Instance.closeAllUI ();
			NPCManager.Instance.createCamera (false); //消除3D相机
			UIManager.Instance.openWindow(UiNameConst.ui_equip);
			UIManager.Instance.getUIFromMemory(UiNameConst.ui_equip).transform.FindChild("Table/Table3").GetComponent<UICheckBoxColor>().isChecked = true;
			Gate.instance.sendNotification(MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table3);
			break; 	
		default:
			break;
			
			
		}
		 
	}
}
