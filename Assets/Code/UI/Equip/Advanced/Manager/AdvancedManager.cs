using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using helper;
using NetGame;

namespace manager
{
	public class AdvancedManager
	{
		static AdvancedManager _instance;
		private EquipmentVo _selectVo;             //当前选中的物品

		private Table _selectTable;                 //当前选中标签

		private GCAskIntensifyEquip _ask15;         //进阶装备协议

        public int selectItem;              //选择的item
		private AdvancedManager ()
		{
			_ask15 = new GCAskIntensifyEquip ();
			_selectVo = null;
		}

		public void Initial ()
		{
			LuckStoneManager.Instance.SetSelectStone (0);
			_selectTable = Table.None;
			_selectVo = null;
			StrengThenManager.Instance.AddEquipData ();
			StrengThenManager.Instance.AddBagData ();
			Gate.instance.sendNotification (MsgConstant.MSG_ADVANCED_DISPLAY_LIST_TABLE, Table.Table1);
		}
        public void UpdateView()
        {
            StrengThenManager.Instance.AddEquipData();
            StrengThenManager.Instance.AddBagData();
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_ENFORCE_DISPLAY_LIST, _selectTable);
        }

		public void SelectAdvancedItem (int id)
		{
			switch (_selectTable) {
			case Table.Table1:
				_selectVo = StrengThenManager.Instance.GetVoByInstanceIdInEquip (id);
				break;
			case Table.Table2:
				_selectVo = StrengThenManager.Instance.GetVoByInstanceIdInBag (id);
				break;
			default:
				Debug.LogError ("Fuck");
				break;
			}
			LuckStoneManager.Instance.SetSelectStone (0);
			Gate.instance.sendNotification (MsgConstant.MSG_ADVANCED_DISPLAT_INFO);
		}

		public void LuckStoneCallback ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_ADVANCED_DISPLAT_INFO);
		}

		//发送进阶装备
		public void AskAdvancedItem ()
		{
			if (_selectVo.NextAdvancedItem.usedLevel > CharacterPlayer.character_property.getLevel ()) {
				ViewHelper.DisplayMessageLanguage ("msg_advanced_level_error");
				return;
			}
			BetterList<TypeStruct> item = _selectVo.EquipData.EquipmentUpItem;
			foreach (TypeStruct ts in item) {
				if (ts.Type == ConsumeType.Item) {
                    if (!FeebManager.Instance.CheckIsHave((uint)ts.Id, ts.Value))
						return;
                    
				} else {
					if (!ViewHelper.CheckIsHava ((eGoldType)ts.Id, ts.Value))
						return;
				}
			}

			if (LuckStoneManager.Instance.SelectedStone != null) {
				TypeStruct ts = LuckStoneManager.Instance.SelectedStoneConsume;
				if (ts.Type == ConsumeType.Item) {
					if (!ViewHelper.CheckIsHave (ts.Id, ts.Value))
						return;
				} else {
					if (!ViewHelper.CheckIsHave (ts.Id, ts.Value))
						return;
				}
			}

			_ask15.u16bagPos = (ushort)_selectVo.InstanceId;
			if (LuckStoneManager.Instance.SelectedStone != null) {
				_ask15.u32LuckStoneId = (uint)LuckStoneManager.Instance.SelectedStone.Id;
			}
			_ask15.option = 0x01;       //强化装备
			NetBase.GetInstance ().Send (_ask15.ToBytes ());
		}

		//进阶返回消息
		public void AdvancedCallBack ()
		{
            this.RefreshRightInfo();
			ViewHelper.DisplayMessageLanguage ("msg_advanced_succ");
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_EFFECT_INFO);
		}
		/// <summary>
		/// 进阶失败
		/// </summary>
		public void AdvancedResultLose ()
		{
			this.RefreshRightInfo();
			ViewHelper.DisplayMessageLanguage ("msg_advanced_error");
		}	
		
		/// <summary>
		/// 刷新右边界面
		/// </summary>
		public void RefreshRightInfo ()
		{
			StrengThenManager.Instance.AddEquipData ();
			StrengThenManager.Instance.AddBagData ();
			SelectAdvancedItem (_selectVo.InstanceId);
			Gate.instance.sendNotification (MsgConstant.MSG_ADVANCED_ENFORCE_DISPLAY_LIST, _selectTable);
		}
		
        #region Attribute
		public Table SelectTable {
			get { return _selectTable; }
			set { _selectTable = value; }
		}

		public EquipmentVo SelectVo {
			get { return _selectVo; }
			set { _selectVo = value; }
		}

		public static AdvancedManager Instance {
			get {
				if (_instance == null) {
					_instance = new AdvancedManager ();
				}
				return AdvancedManager._instance;
			}
		}

		public BetterList<EquipmentVo> Bags {
			get { return StrengThenManager.Instance.Bags; }
		}

		public BetterList<EquipmentVo> Equips {
			get { return StrengThenManager.Instance.Equips; }
		}
        #endregion
        
	}
}
