/**该文件实现的基本功能等
function: 实现宠物培养的View控制
author:zyl
date:2014-06-03
**/
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;
using NetGame;
using System.Diagnostics;
using UnityEngine;

namespace mediator
{	
	public class PetMediator : ViewMediator
	{
		private bool _autoEvolution; //是否正在自动进阶
		
		public PetMediator (PetView view, uint id = MediatorName.PET_MEDIATOR) : base(id, view)
		{
			_autoEvolution = false;
		}
		
		public override IList<uint> listReferNotification ()
		{
			return new List<uint> 
			{
				MsgConstant.MSG_CLOSE_PET_UI,
				MsgConstant.MSG_PET_INIT,
				MsgConstant.MSG_PET_AUTO_EVOLUTION,
				MsgConstant.MSG_PET_EVOLUTION,
				MsgConstant.MSG_PET_SHOW_EVOLUTION,	//显示进阶界面
				MsgConstant.MSG_PET_UPDATE_MONEY,  //金钱钻石的变化
				MsgConstant.MSG_PET_UPDATE_LUCK,	  //成功率，幸运点的变化
				MsgConstant.MSG_PET_UPDATE_MODEL_LADDER,  //更新阶数的时候同时更新模型
				MsgConstant.MSG_PET_SHOW_EFFECT,  //每次培养进阶的时候显示文字以及特效
				MsgConstant.MSG_PET_NOT_ENOUGH_MSG,	  //弹出材料不足的信息
				MsgConstant.MSG_PET_STOP_AUTO,	  //退出自动培养，自动进阶
				MsgConstant.MSG_PET_DRAG_NEXT,
				MsgConstant.MSG_PET_DRAG_PREV,
                MsgConstant.MSG_PET_SET_CAMERA,
				MsgConstant.MSG_PET_SHOW_TIME,
				MsgConstant.MSG_PET_RET,
				MsgConstant.MSG_PET_FOLLOW,
                MsgConstant.MSG_PET_SUCCESS,
				MsgConstant.MSG_PET_SWITCH_TAB,
				MsgConstant.MSG_PET_SHOWPETEQUIP,
				MsgConstant.MSG_PET_SHOWPETEQUIPITEM,
				MsgConstant.MSG_BAG_SALE,
				MsgConstant.MSG_PET_OPEN_PET_SKILL,
				MsgConstant.MSG_PET_CLOSE_PET_SKILL,
				MsgConstant.MSG_PET_UPDATE_PREVIEW_ATTR,
			};
		}
		
		//处理相关的消息
		public override void handleNotification (INotification notification)
		{
			if (this.view != null) {				
				switch (notification.notifyId) {
				case MsgConstant.MSG_CLOSE_PET_UI:
					UIManager.Instance.closeAllUI ();
					_autoEvolution = false;
					this.view.StopAllCoroutines ();
                    SkillTalentManager.Instance.ClearEffect();
					break;
				case MsgConstant.MSG_PET_INIT:
					this.view.ShowView ();
					break;
				case MsgConstant.MSG_PET_AUTO_EVOLUTION:
					if (_autoEvolution)
						PetManager.Instance.StopAuto ();
					else {
						_autoEvolution = true;
						this.view.AutoCultureEvo ();
					}
					break;
				case MsgConstant.MSG_PET_EVOLUTION:
					if (_autoEvolution)
						this.view.ShowErrMsg ("pet_auto_evolution_msg");
					else
						PetManager.Instance.BeginEvolute ();
					break;
				case MsgConstant.MSG_PET_SHOW_EVOLUTION:
					this.ShowMaxPet ();
					break;
 
				case MsgConstant.MSG_PET_UPDATE_MONEY:
					this.view.UpdateMoney ();
					break;	
					
				case MsgConstant.MSG_PET_UPDATE_LUCK:
					ArrayList lucks = notification.body as ArrayList;
					if (lucks != null && lucks.Count >= 2)
						this.view.UpdateLuckPoint ((uint)lucks [0], (uint)lucks [1]);
					break;
					
				case MsgConstant.MSG_PET_SUCCESS:
					this.view.SuccessPlayAnim ();
					break;

				case MsgConstant.MSG_PET_UPDATE_MODEL_LADDER:
					List<string> models = notification.body as List<string>;
					if (models != null) {
                        PetManager.Instance.PreviewLadder = PetManager.Instance.CurrentLevel;
						this.view.UpdateLadder (models [0], models [1], PetManager.Instance.CurrentPet.ModelPos, PetManager.Instance.CurrentPet.RotateXYZ, PetManager.Instance.CurrentPet.ModelScale);
						this.view.UpdateLevelUpLadder (models [0]);
 
					}
					break;
				case MsgConstant.MSG_PET_SHOW_EFFECT:
					List<string> effects = notification.body as List<string>;
					if (effects != null)
						this.view.ShowEffect (effects [0], effects [1]);
					break;
				case MsgConstant.MSG_PET_NOT_ENOUGH_MSG:
					string errMsg = (string)notification.body;
					this.view.ShowErrMsg (errMsg);
					break;
				case MsgConstant.MSG_PET_STOP_AUTO:
					this.view.ChangeBtn (false); //切换按钮
					_autoEvolution = false;
					this.view.StopAllCoroutines ();
					break;
				case MsgConstant.MSG_PET_DRAG_NEXT:
					if (PetManager.Instance.PreviewLadder < PetManager.Instance.DictionaryPet.Count) {
						PetManager.Instance.PreviewLadder++;
						PreviewPet ();
					}
					break;
				case MsgConstant.MSG_PET_DRAG_PREV:
					if (PetManager.Instance.PreviewLadder > 1) {
						PetManager.Instance.PreviewLadder--;
						PreviewPet ();
					}
					break;
				case MsgConstant.MSG_PET_SET_CAMERA:
					this.view.SetCamera ((bool)notification.body);
					break;
				case MsgConstant.MSG_PET_SHOW_TIME:
					ArrayList al = (ArrayList)notification.body;
					uint num = (uint)al [0];
					bool isLimit = (bool)al [1];
 
					this.view.ShowTime (num, isLimit);
					break;
				case MsgConstant.MSG_PET_RET:		//回到当前最高阶宠物 
					this.ShowMaxPet ();
					PetVo vo = PetManager.Instance.MaxPet;
					this.view.UpdateLadder (vo.PetName, vo.PetModle, vo.ModelPos, vo.RotateXYZ, vo.ModelScale);
					this.view.ShowSkill (vo);
					break;
				case MsgConstant.MSG_PET_FOLLOW:
					PetManager.Instance.SetFollowPet ();
					break;
				case MsgConstant.MSG_PET_SWITCH_TAB:
					this.view.SwitchTab (PetManager.Instance.CurrentTab);
					break;
				case MsgConstant.MSG_PET_SHOWPETEQUIP:
					this.view.ShowPetEquip (BagManager.Instance.PetEquipItem);
					this.view.UpdateBagNum (BagManager.Instance.PetEquipItem.Count);
					break;
				case MsgConstant.MSG_PET_SHOWPETEQUIPITEM:
					this.view.ShowEquip (BagManager.Instance.EquipData);
					break;
				case MsgConstant.MSG_BAG_SALE:
					BagManager.Instance.SaleItems (notification.body.ToString ());
					break;
				case MsgConstant.MSG_PET_OPEN_PET_SKILL:
				{
					uint petId = PetManager.Instance.PreviewLadder * 1000 + 1;
					PetVo model = PetManager.Instance.DictionaryPet [petId];
                    UnityEngine.Debug.LogError("petId---" + petId);
					this.view.ShowSkillPanel(model);
					break;
				}
				case MsgConstant.MSG_PET_CLOSE_PET_SKILL:
					this.view.CloseSkillPanel();
					break;
				case MsgConstant.MSG_PET_UPDATE_PREVIEW_ATTR:
					this.UpdatePreviewAttr();
					break;		  
				default:					
					break;
				}
			}
		}
		
		//预览宠物
		private void PreviewPet ()
		{
			PetManager.Instance.StopAuto ();
			this.view.EnableButton (); //使能按钮
			uint petId = PetManager.Instance.PreviewLadder * 1000 + 1;
			PetVo vo = PetManager.Instance.DictionaryPet [petId];
			this.view.UpdateLadder (vo.PetName, vo.PetModle, vo.ModelPos, vo.RotateXYZ, vo.ModelScale);
			this.view.UpdateMaterial (vo);
			this.view.UpdateAttr (vo.AttrTypes, vo.AttrValues);
			this.view.SetFollowPet ();
			this.view.ShowSkill (vo);
		}

		private void ShowMaxPet ()
		{
			PetManager.Instance.PreviewLadder = PetManager.Instance.MaxLadder;
			this.view.EnableButton (); //使能按钮
			PetVo vo = PetManager.Instance.MaxPet;
			this.view.UpdateMaterial (vo);
			this.view.UpdateAttr (vo.AttrTypes, vo.AttrValues);
			this.view.SetFollowPet ();
			this.view.ShowSkill (vo);
		}

		private void UpdatePreviewAttr(){
			uint petId = PetManager.Instance.PreviewLadder * 1000 + 1;
			PetVo vo = PetManager.Instance.DictionaryPet [petId];
			this.view.UpdateAttr (vo.AttrTypes, vo.AttrValues);
		}

		
		
		//getter and setter
		public PetView view {
			get {
				if (_viewComponent != null && _viewComponent is PetView)
					return _viewComponent as PetView;
				return  null;					
			}
		}
	}
}