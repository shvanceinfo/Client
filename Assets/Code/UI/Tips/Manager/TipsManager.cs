/**该文件实现的基本功能等
function: 实现tips的管理
author:zyl
date:2014-04-14
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;
using model;

public enum TipsCommand
{
	None,		//没有
	PerfectNone,//连出售按钮都没有的none
	Compound,	//合并
	Open,		//打开
	CurtAndIntensify, //强化与洗练		
	EquipAndSale      //装备与出售
}

namespace manager
{
	public class TipsManager
	{
		private static TipsManager _instance;
		private ItemInfo _itemInfo;				//道具详细信息
		private EquipmentStruct _equipInfo;		//武器详细信息
		private Transform _iconTrans;			//icon对象
		
		private TipsManager ()
		{
			
		}
		#region 属性信息
		public ItemInfo Iteminfo {
			get {
				return this._itemInfo;
			}
		}

		public EquipmentStruct EquipInfo {
			get {
				return this._equipInfo;
			}
			private set {
				_equipInfo = value;
			}
		}

		public Transform IconTrans {
			get {
				return this._iconTrans;
			}
			set {
				_iconTrans = value;
			}
		}
		#endregion
		

		public static TipsManager Instance {
			get {
				if (_instance == null) {
					_instance = new TipsManager ();
				}
				return _instance;
			}
		}
		
		/// <summary>
		/// 根据模板ID得到数据
		/// </summary>
		/// <param name='tempId'>
		/// Temp identifier.
		/// </param>
		/// <param name='trans'>
		/// Trans.
		/// </param>
		public void ShowCommonInfo (int instanceId, Transform trans)
		{
			if (instanceId == 0) {
				Debug.LogWarning ("实例id为0");
				return;
			}
			
			if (instanceId < BagManager.EQUIP_BEGIN) {   //判断是否是装备数据 ,50000开始是装备数据
				var vItem = ItemManager.GetInstance ().items [instanceId];
				this._itemInfo = new ItemInfo (vItem.tempId, instanceId, vItem.num);
			} else {
				var vItem = EquipmentManager.GetInstance ().equipments [(uint)instanceId];
				this._itemInfo = new ItemInfo (vItem.templateId, instanceId, 1);
			}
			this._iconTrans = trans;
			ShowUI ();
		}
		
		public void ShowCommonInfo (ItemInfo iteminfo, Transform trans)
		{
			if (iteminfo.Id == 0) {
				Debug.LogWarning ("模板id为0");
				return ;
			}
			this._itemInfo = iteminfo;
			this._iconTrans = trans;
			ShowUI ();
		}
		
		/// <summary>
		/// 确实是否还要右边按钮
		/// </summary>
		public TipsCommand CheckIsShowTipsButton (TipsCommand cmd)
		{
			if (this.Iteminfo.Num == 0) {
				cmd = TipsCommand.PerfectNone;
			}
			return cmd;
		}
		
		
		/// <summary>
		/// 显示UI，并且根据UI判断，显示什么按钮
		/// </summary>
		public void ShowUI ()
		{

			TipsCommand cmd = TipsCommand.None;
			
			switch (this._itemInfo.Item.itemType) {
			case eItemType.eEquip:
				this.CloseCommonTipsUI ();
				
				if (this._itemInfo.InstanceId == 0) {  //如果实例ID为0，则表示是模板示例
					var equip = EquipmentManager.GetInstance ().GetTemplateByTempId (this._itemInfo.Id);
					EquipmentStruct e = new EquipmentStruct ();
					e.instanceId = 0;
					e.templateId = this._itemInfo.Id;
					e.intensifyLevel = this._itemInfo.Item.usedLevel;
					e.equipPart = equip.part;
   	 
					AddProperty (ref e, equip.bStateType1, equip.bStateValue1);
					AddProperty (ref e, equip.bStateType2, equip.bStateValue2);
					
					this._equipInfo = e;
				} else {
					this._equipInfo = EquipmentManager.GetInstance ().GetDataByItemId ((uint)this._itemInfo.InstanceId).Clone() as EquipmentStruct; //设置装备信息
					
					if (this._equipInfo.templateId != this._itemInfo.Id) { //如果不相同则表示是进阶的tips显示,需要显示进阶后的基本属性
						var equip = EquipmentManager.GetInstance ().GetTemplateByTempId (this._itemInfo.Id);
						AddProperty (ref this._equipInfo, equip.bStateType1, equip.bStateValue1);//替换现在装备的基本属性信息
						AddProperty (ref this._equipInfo, equip.bStateType2, equip.bStateValue2);//替换现在装备的基本属性信息
					}
				}
 
				UIManager.Instance.openWindow (UiNameConst.ui_equip_tips);
				
				if (this._itemInfo.InstanceId < BagManager.EQUIP_BEGIN) {  //验证是背包装备，还是装备在身上的装备
					cmd = TipsCommand.EquipAndSale;
				} else {
					cmd = TipsCommand.CurtAndIntensify;
				}
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_TIPS_SHOWINFO, cmd);//显示装备信息
				break;
			case eItemType.eExpend:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.None;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;
			case eItemType.eGem:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.Compound;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;
			case eItemType.eMaterial:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.Compound;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;	
			case eItemType.ePacks:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.Open;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;	
			case eItemType.eProduce:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.None;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;	
			case eItemType.ePet:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.PerfectNone;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;
			case eItemType.eTask:
				this.CloseEquipTipsUI ();
				UIManager.Instance.openWindow (UiNameConst.ui_common_tips);
				cmd = TipsCommand.PerfectNone;
				cmd = this.CheckIsShowTipsButton (cmd); //验证是否显示按钮
				Gate.instance.sendNotification (MsgConstant.MSG_COMMON_TIPS_SHOWINFO, cmd);//显示信息
				break;
			case eItemType.ePetEquip:
				this.CloseCommonTipsUI ();
 				
				if (this._itemInfo.InstanceId == 0) {  //如果实例ID为0，则表示是模板示例
					var equip = EquipmentManager.GetInstance ().GetTemplateByTempId (this._itemInfo.Id);
					EquipmentStruct e = new EquipmentStruct ();
					e.instanceId = 0;
					e.templateId = this._itemInfo.Id;
					e.intensifyLevel = this._itemInfo.Item.usedLevel;
					e.equipPart = equip.part;
					
					AddProperty (ref e, equip.bStateType1, equip.bStateValue1);
					AddProperty (ref e, equip.bStateType2, equip.bStateValue2);
					
					this._equipInfo = e;
				} else {
					this._equipInfo = EquipmentManager.GetInstance ().GetDataByItemId ((uint)this._itemInfo.InstanceId); //设置装备信息
				}
				
				UIManager.Instance.openWindow (UiNameConst.ui_pet_equip_tips);
				
				if (this._itemInfo.InstanceId >= BagManager.PET_EQUIP_BEGIN) {  //验证是背包装备，还是装备在身上的装备
					cmd = TipsCommand.PerfectNone;
				} else {
					cmd = TipsCommand.EquipAndSale;
				}
				Gate.instance.sendNotification (MsgConstant.MSG_PET_EQUIP_TIPS_SHOWINFO, cmd);//显示宠物装备信息
				break;	
			default:
				break;
			}
 
		}

		public static void AddProperty (ref EquipmentStruct e, eFighintPropertyCate key, int val)
		{
			if (key != eFighintPropertyCate.eFPC_None) {
				#region 实例化附加属性
				AttributeValue prop = new AttributeValue ();
				prop.Type = key;
				prop.Value = val;
				#endregion
				
				for (int i = 0; i <  e.BaseAtrb.size; i++) {
					if (e.BaseAtrb[i].Type == key) {
						e.BaseAtrb[i] = prop;
						return ;
					}
				}
				
				e.BaseAtrb.Add(prop);
			}
		}
		
		
		
		
		/// <summary>
		/// Closes the common tips U.
		/// </summary>
		public void CloseCommonTipsUI ()
		{
			//if (UIManager.Instance.isWindowOpen (UiNameConst.ui_common_tips)) {
			UIManager.Instance.closeWindow (UiNameConst.ui_common_tips, true, true);
			//}
		}
		
		public void CloseEquipTipsUI ()
		{
			//if (UIManager.Instance.isWindowOpen (UiNameConst.ui_equip_tips)) {
			UIManager.Instance.closeWindow (UiNameConst.ui_equip_tips, true, true);
			//}
		}

        public void CloseMedalTipsUI() 
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_medal,true,true);
        }

		public void ClosePetEquipTipsUI(){
			UIManager.Instance.closeWindow (UiNameConst.ui_pet_equip_tips, true, true);
		}

		public void CloseAllTipsUI ()
		{
			this.CloseCommonTipsUI ();
			this.CloseEquipTipsUI ();
            this.CloseMedalTipsUI();
			this.ClosePetEquipTipsUI ();
		}
		
		
		#region 网络通信
		/// <summary>
		/// 换装备
		/// </summary>
		/// <param name='instanceId'>
		/// Instance identifier.
		/// </param>
		public void NetPutEquip ()
		{


			if (TipsManager.Instance.Iteminfo.Item.packType == ePackageNavType.PetEquip) {    //如果是宠物装备

				if (PetManager.Instance.MaxLadder < this.Iteminfo.Item.usedLevel) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_pet_equip_not_enough_level"),
					                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return ;
				}//验证宠物的阶数


				PetManager.Instance.GCAskEquipPet((ushort)TipsManager.Instance.Iteminfo.InstanceId);
			} else {

				if (TipsManager.Instance.Iteminfo.Item.packType == ePackageNavType.Weapon) {
					if (CharacterPlayer.character_property.career != this.Iteminfo.Item.career) {
						FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_equip_not_equal_career"),
						                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
						return ;
					}
				}//验证职业
				
				
				if (CharacterPlayer.character_property.level < this.Iteminfo.Item.usedLevel) {
					FloatMessage.GetInstance ().PlayFloatMessage (LanguageManager.GetText ("msg_equip_not_enough_level"),
					                                              UIManager.Instance.getRootTrans (), Vector3.zero, Vector3.zero);
					return ;
				}//验证等级

				ItemManager.GetInstance ().NetPutEquip ((ushort)TipsManager.Instance.Iteminfo.InstanceId);
			}

		}
		
		 
		
		#endregion
		
	}
}