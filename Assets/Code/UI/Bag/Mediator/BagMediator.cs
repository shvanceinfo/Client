/**该文件实现的基本功能等
function: 实现背包的View控制
author:zyl
date:2014-4-8
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using NetGame;

namespace mediator
{
	public class BagMediator : ViewMediator
	{

		public BagMediator (BagView view, uint id = MediatorName.BAG_MEDIATOR) : base(id, view)
		{
			 
		}
			
		public BagView View {
			get {
				if (_viewComponent != null && _viewComponent is BagView)
					return _viewComponent as BagView;
				return  null;		
			}
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_UI,
				MsgConstant.MSG_BAG_SHOWCAREERMODEL,
				MsgConstant.MSG_BAG_SHOWEQUIPDATA,
				MsgConstant.MSG_BAG_SHOWALLITEM,
				MsgConstant.MSG_BAG_SWITCHTAB,
				MsgConstant.MSG_BAG_SHOWEQUIPITEM,
				MsgConstant.MSG_BAG_SHOWNORMALITEM,
				MsgConstant.MSG_BAG_SALE,
				MsgConstant.MSG_BAG_HIDECAREERMODEL,
				MsgConstant.MSG_BAG_UPDATE_MODEL_WEAPON,
				MsgConstant.MSG_BAG_UPDATE_MODEL_ARMOR,
                MsgConstant.MSG_BAG_UPDATE_DIAMOND_GOLD
			};
		}
		
		
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			BagView bagView = this.View;
			if (bagView != null) {
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_UI:
					UIManager.Instance.closeAllUI ();
					break;
				case MsgConstant.MSG_BAG_SHOWCAREERMODEL:
					bagView.ShowCareerModel (BagManager.Instance.ModelName, BagManager.Instance.ModelPos);
                    bagView.ChangeVipLevel();
					break;
				case MsgConstant.MSG_BAG_SHOWEQUIPDATA:
					bagView.ShowEquip (BagManager.Instance.EquipData);
					break;
				case MsgConstant.MSG_BAG_SHOWALLITEM:
					bagView.ShowAllItem (BagManager.Instance.AllItem);
                    bagView.UpdateBagNum(BagManager.Instance.AllItem.Count, VipManager.Instance.BagMaxSize);
					break;	
				case MsgConstant.MSG_BAG_SWITCHTAB:
					BagView.Tab tab = (BagView.Tab)notification.body;
					BagManager.Instance.SwitchTab (tab);
					bagView.SwitchTab (tab);
					break;
				case MsgConstant.MSG_BAG_SHOWEQUIPITEM:
					bagView.ShowEquipItem (BagManager.Instance.EquipItem);
					bagView.UpdateBagNum(BagManager.Instance.AllItem.Count,VipManager.Instance.BagMaxSize);
					break;
				case MsgConstant.MSG_BAG_SHOWNORMALITEM:
					bagView.ShowNormalItem (BagManager.Instance.NormalItem);
                    bagView.UpdateBagNum(BagManager.Instance.AllItem.Count, VipManager.Instance.BagMaxSize);
					break;	
				case MsgConstant.MSG_BAG_SALE:
					BagManager.Instance.SaleItems (notification.body.ToString ());
					break;
				case MsgConstant.MSG_BAG_HIDECAREERMODEL:
					bagView.HideCareerModel();
					break;
				case MsgConstant.MSG_BAG_UPDATE_MODEL_WEAPON:
					bagView.UpdateModelWeapon();
					break;
				case MsgConstant.MSG_BAG_UPDATE_MODEL_ARMOR:
					bagView.UpdateModelArmor();
					break;
				case MsgConstant.MSG_BAG_UPDATE_DIAMOND_GOLD:
                    List<string> s = (List<string>)(notification.body);
                    bagView.ShowDiamondGold(s);
                    break;
				default:
					break;
				}
				
			}
			
		}
	}
}