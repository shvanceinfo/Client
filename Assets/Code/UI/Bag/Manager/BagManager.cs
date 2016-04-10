//#define Test

/**该文件实现的基本功能等
function: 实现背包以及物品的管理
author:ljx
date:2014-04-03
**/
using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using NetGame;
using System.Linq;

namespace manager
{
	public class BagManager
	{
		public  const int EQUIP_BEGIN = 50000; 	//身上装备实例ID从50000开始计数
		public  const int PET_EQUIP_BEGIN = 55000; 	//身上装备实例ID从55000开始计数
		private static BagManager _instance;
		private string _modelName;				//模型名称
		private ModelPos _modelPos;				//模型位置
		private DataModelPos _dataModelPos;		//模型位置表数据信息
		private Dictionary<eEquipPart, EquipmentStruct> _equipData;
		private BagVo _bagVo;//背包拥有的道具
		
		private IList<ItemInfo> _allItem;		//排序后的所有物品
		private IList<ItemInfo> _equipItem;		//排序后的装备
		private IList<ItemInfo> _normalItem;	//排序后的普通道具
		private IList<ItemInfo> _petEquipItem;	//排序后的宠物装备

		private bool _hasValuable = false; 		//默认没有贵重物品
		private IList<ItemStruct> _saleItemList = new  List<ItemStruct> ();   //被销售的物品队列集合
		private bool _isChange = true;			//如果itemmanager 改变过数据则需要重新初始化
        private int _autoFindItem = 0;         //自动定位物品,(新手引导)

        
		private BagManager ()
		{
			_dataModelPos = ConfigDataManager.GetInstance ().getModelPos ();
			_equipData = new Dictionary<eEquipPart, EquipmentStruct> ();
			this._bagVo = new BagVo ();
#if Test
			this._bagVo.ItemList.Add (new ItemInfo (1010240, 50001, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1010620, 50002, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200520, 50003, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200630, 50004, 1));
			this._bagVo.ItemList.Add (new ItemInfo (3001205, 50005, 10));
			this._bagVo.ItemList.Add (new ItemInfo (4010232, 50006, 99));
			this._bagVo.ItemList.Add (new ItemInfo (3000109, 50007, 40));
			this._bagVo.ItemList.Add (new ItemInfo (3001005, 50008, 5));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50009, 999));
			this._bagVo.ItemList.Add (new ItemInfo (6200640, 50010, 10));
			this._bagVo.ItemList.Add (new ItemInfo (6200750, 50011, 20));
			this._bagVo.ItemList.Add (new ItemInfo (6300140, 50012, 90));
			this._bagVo.ItemList.Add (new ItemInfo (6300340, 50013, 100));
			this._bagVo.ItemList.Add (new ItemInfo (6300540, 50014, 1));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50015, 99));
			this._bagVo.ItemList.Add (new ItemInfo (1010110, 50016, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1010240, 50017, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1010620, 50018, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200520, 50019, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200630, 50020, 1));
			this._bagVo.ItemList.Add (new ItemInfo (3001205, 50021, 10));
			this._bagVo.ItemList.Add (new ItemInfo (4010232, 50022, 99));
			this._bagVo.ItemList.Add (new ItemInfo (3000109, 50023, 40));
			this._bagVo.ItemList.Add (new ItemInfo (3001005, 50024, 5));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50025, 999));
			this._bagVo.ItemList.Add (new ItemInfo (6200640, 50026, 10));
			this._bagVo.ItemList.Add (new ItemInfo (6200750, 50027, 20));
			this._bagVo.ItemList.Add (new ItemInfo (6300140, 50028, 90));
			this._bagVo.ItemList.Add (new ItemInfo (6300340, 50029, 100));
			this._bagVo.ItemList.Add (new ItemInfo (6300540, 50030, 1));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50031, 99));
			this._bagVo.ItemList.Add (new ItemInfo (1010110, 50032, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1010240, 50033, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1010620, 50034, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200520, 50035, 1));
			this._bagVo.ItemList.Add (new ItemInfo (1200630, 50036, 1));
			this._bagVo.ItemList.Add (new ItemInfo (3001205, 50037, 10));
			this._bagVo.ItemList.Add (new ItemInfo (4010232, 50038, 99));
			this._bagVo.ItemList.Add (new ItemInfo (3000109, 50039, 40));
			this._bagVo.ItemList.Add (new ItemInfo (3001005, 50040, 5));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50041, 999));
			this._bagVo.ItemList.Add (new ItemInfo (6200640, 50042, 10));
			this._bagVo.ItemList.Add (new ItemInfo (6200750, 50043, 20));
			this._bagVo.ItemList.Add (new ItemInfo (6300140, 50044, 90));
			this._bagVo.ItemList.Add (new ItemInfo (6300340, 50045, 100));
			this._bagVo.ItemList.Add (new ItemInfo (6300540, 50046, 1));
			this._bagVo.ItemList.Add (new ItemInfo (4000102, 50047, 99));
			 
#endif
			
		}
		
		#region 属性信息
		public string ModelName {
			get {
				return this._modelName;
			}
			set {
				_modelName = value;
			}
		}

		public ModelPos ModelPos {
			get {
				return this._modelPos;
			}
			set {
				_modelPos = value;
			}
		}

		public DataModelPos DataModelPos {
			get {
				return this._dataModelPos;
			}
		}

		public Dictionary<eEquipPart, EquipmentStruct> EquipData {
			get {
				return this._equipData;
			}
		}

		public BagVo BagVo {
			get {
				return this._bagVo;
			}
			set {
				_bagVo = value;
			}
		}
 
		public IList<ItemInfo> AllItem {
			get {
				return this._allItem;
			}
			set {
				_allItem = value;
			}
		}

		public IList<ItemInfo> EquipItem {
			get {
				return this._equipItem;
			}
			set {
				_equipItem = value;
			}
		}

		public IList<ItemInfo> NormalItem {
			get {
				return this._normalItem;
			}
			set {
				_normalItem = value;
			}
		}

		public bool HasValuable {
			get {
				return this._hasValuable;
			}
			set {
				_hasValuable = value;
			}
		}

		public IList<ItemStruct> SaleItemList {
			get {
				return this._saleItemList;
			}
		}

		public bool IsChange {
			get {
				return this._isChange;
			}
			private set {
				_isChange = value;
			}
		}

		public IList<ItemInfo> PetEquipItem {
			get {
				return _petEquipItem;
			}
			private set{
				this._petEquipItem = value;
			}
		}
 
		#endregion

		public void Reset(){
			this._modelName = null;			//模型名称
			this._hasValuable = false; 		//默认没有贵重物品
			this._isChange = true;			//如果itemmanager 改变过数据则需要重新初始化
			_equipData.Clear ();			//清装备数据
			this._bagVo = new BagVo ();
		}
	
		//跟进物品品质获取物品的背景, isBorder是否获取有花纹得
		public string getItemBgByType (eItemQuality quality, bool hasBorder)
		{
			string spName = null;  //跟进品质获取相应的spriteName
			switch (quality) {
			case eItemQuality.eWhite:
				if (hasBorder)
					spName = "common_baise";
				else
					spName = "common_baise_wu";
				break;
			case eItemQuality.eGreen:
				if (hasBorder)
					spName = "common_lvse";
				else
					spName = "common_lvse_wu";
				break;
			case eItemQuality.eBlue:
				if (hasBorder)
					spName = "common_lanse";
				else
					spName = "common_lanse_wu";
				break;
			case eItemQuality.ePurple:
				if (hasBorder)
					spName = "common_zise";
				else
					spName = "common_zise_wu";
				break;
			case eItemQuality.eOrange:
				if (hasBorder)
					spName = "common_chengse";
				else
					spName = "common_chengse_wu";
				break;
			default:
				break;
			}
			return spName;
		}
		
		/// <summary>
		/// 得到装备对应的职业字符串
		/// </summary>
		/// <returns>
		/// The item career string.
		/// </returns>
		/// <param name='itemCareer'>
		/// Item career.
		/// </param>
		public string GetItemCareerString (CHARACTER_CAREER itemCareer)
		{
			string itemCareerStr = string.Empty;
			switch (itemCareer) {
			case CHARACTER_CAREER.CC_BEGIN: 			//任何人都能使用
				itemCareerStr = LanguageManager.GetText ("vocation0");
				break;
			case CHARACTER_CAREER.CC_ARCHER:
				itemCareerStr = LanguageManager.GetText ("vocation1");
				break;
			case CHARACTER_CAREER.CC_SWORD:
				itemCareerStr = LanguageManager.GetText ("vocation2");
				break;
			case CHARACTER_CAREER.CC_MAGICIAN:
				itemCareerStr = LanguageManager.GetText ("vocation3");
				break;	
				
			default:
				break;
			}
			return  itemCareerStr;
		}
		
		/// <summary>
		/// Gets the equip part string.
		/// </summary>
		/// <returns>
		/// The equip part string.
		/// </returns>
		/// <param name='part'>
		/// Part.
		/// </param>
		public string GetEquipPartString (eEquipPart part)
		{
			string partStr = string.Empty;
			switch (part) {
			case eEquipPart.eArcher:
				partStr = "role_weapon";
				break;
			case eEquipPart.eDoublePole:
				partStr = "role_weapon";
				break;
			case eEquipPart.eGreatSword:
				partStr = "role_weapon";
				break;
			case eEquipPart.eLeggings:
				partStr = "role_leggings";
				break;
			case eEquipPart.eNecklace:
				partStr = "role_necklace";
				break;
			case eEquipPart.eRing:
				partStr = "role_ring";
				break;
			case eEquipPart.eShoes:
				partStr = "role_shoes";
				break;
			case eEquipPart.eSuit:
				partStr = "role_suit";
				break;
			case eEquipPart.eTooth:
				partStr ="role_tooth";
				break;
			case eEquipPart.eClaw:
				partStr ="role_claw";
				break;
			case eEquipPart.eEye:
				partStr ="role_eye";
				break;
			case eEquipPart.eJewelry:
				partStr ="role_jewelry";
				break;
			default:
				break;
			}
			return LanguageManager.GetText (partStr);
		}
		
		/// <summary>
		/// 初始化模型信息 
		/// </summary>
		public void InitCareerModel ()
		{
			if (string.IsNullOrEmpty (this.ModelName)) {//判断当前模型名称是否为空
				switch (CharacterPlayer.character_property.career) {
				case CHARACTER_CAREER.CC_SWORD:
					this.ModelName = Constant.SWORD_UI;
					break;
				case CHARACTER_CAREER.CC_ARCHER:
					this.ModelName = Constant.BOW_UI;
					break;
				case CHARACTER_CAREER.CC_MAGICIAN:
					this.ModelName = Constant.RABBI_UI;
					break;
				default:
					break;
				}
			}
			
			this.ModelPos = this.DataModelPos.getModelInfo ((int)CharacterPlayer.character_property.career);
		}
		
		/// <summary>
		/// 初始化装备数据
		/// </summary>
		public void InitEquipData (eEquipPart part, EquipmentStruct equip)
		{
			if (equip.instanceId >= EQUIP_BEGIN&&equip.instanceId <PET_EQUIP_BEGIN) {
				_equipData [part] = equip;//得到装备信息
				this.ShowEquipedData ();  //跟新图片
			}else if (equip.instanceId >= PET_EQUIP_BEGIN) {
				_equipData [part] = equip;//得到装备信息
				PetManager.Instance.UpdatePreviewAttr(); 
				PetManager.Instance.ShowEquipedData();  //更新宠物界面
			}
		}
		
		/// <summary>
		/// Updates the equip data.
		/// </summary>
		/// <param name='part'>
		/// Part.
		/// </param>
		/// <param name='equip'>
		/// Equip.
		/// </param>
		/// <param name='isRefreshUI'>
		/// Is refresh U.
		/// </param>
		public void UpdateEquipData (eEquipPart part, EquipmentStruct equip)
		{
			if (equip.instanceId >= EQUIP_BEGIN&&equip.instanceId <PET_EQUIP_BEGIN) {
				_equipData [part] = equip;//得到装备信息
				this.ShowEquipedData ();  //跟新图片
				
				#region 更新武器模型
				if (part == eEquipPart.eArcher || part == eEquipPart.eDoublePole || part == eEquipPart.eGreatSword) {
					CharacterPlayer.character_property.setWeapon ((int)equip.templateId);
					CharacterPlayer.sPlayerMe.equipItem (CharacterPlayer.character_property.getWeapon ());
					this.UpdateModelWeapon ();
				}
				#endregion
				
				#region 更新衣服模型
				if (part == eEquipPart.eSuit) {
					CharacterPlayer.character_property.setArmor ((int)equip.templateId);
					CharacterPlayer.sPlayerMe.equipItem (CharacterPlayer.character_property.armor);
					this.UpdateModelArmor ();
				}
				
				#endregion
			}else if (equip.instanceId >= PET_EQUIP_BEGIN) {
				_equipData [part] = equip;//得到装备信息
				PetManager.Instance.UpdatePreviewAttr();
				PetManager.Instance.ShowEquipedData();  //更新宠物界面
			}




		}
		
		/// <summary>
		/// Inits the bag item list.
		/// </summary>
		public void InitBagItemList ()
		{
			this.ResetSaleStatus (); 
#if   !Test	
			this.InitBag ();
#endif
		}

		public void InitBag ()
		{
			this.BagVo.ItemList.Clear ();
			foreach (var item in ItemManager.GetInstance().items) {
				this._bagVo.ItemList.Add (new ItemInfo (item.Value.tempId, item.Key, item.Value.num));
			}
//			this.IsChange = false;
		}
		
		
		
		
		/// <summary>
		/// 打开背包
		/// </summary>
		public void OpenBag (int autoFind=0)
		{
            _autoFindItem = autoFind;
			UIManager.Instance.openWindow (UiNameConst.ui_bag);
			#region 重新包装背包数据
			this.InitBagItemList ();
			#endregion
			 
			#region 左边数据
			this.InitCareerModel ();
			//this.InitEquipData ();
			this.ShowCareerModel ();
			this.ShowEquipedData ();
			#endregion
			
			#region 右边数据
			this.ShowAllItem ();
			#endregion
			
		}
		
		/// <summary>
		/// Shows the equiped data.
		/// </summary>
		public void ShowEquipedData ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SHOWEQUIPDATA);//显示装备数据
		}
		
		
		
		#region 回调的比较方法
 
		
		/// <summary>
		/// 比较所有道具或者普通道具
		/// </summary>
		/// <returns>
		/// The all item.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		public static int CompareAllItem (ItemInfo x, ItemInfo y)
		{
			if (x.Item.typeRank < y.Item.typeRank) {
				return -1;
			} else if (x.Item.typeRank == y.Item.typeRank) {
				if (x.Item.quality > y.Item.quality) {
					return -1;
				} else if (x.Item.quality == y.Item.quality) {
					if (x.Item.usedLevel > y.Item.usedLevel) {
						return -1;
					} else if (x.Item.usedLevel == y.Item.usedLevel) {
						if (x.Id < y.Id) {
							return -1;
						} else if (x.Id == y.Id) {
							return 0;
						} else {
							return 1;
						}
					} else {
						return 1;
					}
				} else {
					return 1;
				}
			} else {
				return 1;
			}
		}
		
		/// <summary>
		/// 比较装备道具
		/// </summary>
		/// <returns>
		/// The equip item.
		/// </returns>
		/// <param name='x'>
		/// X.
		/// </param>
		/// <param name='y'>
		/// Y.
		/// </param>
		public static int CompareEquipItem (ItemInfo x, ItemInfo y)
		{
			if (x.Item.quality > y.Item.quality) {
				return -1;
			} else if (x.Item.quality == y.Item.quality) {
				if (x.Item.usedLevel > y.Item.usedLevel) {
					return -1;
				} else if (x.Item.usedLevel == y.Item.usedLevel) {
					if (x.Id < y.Id) {
						return -1;
					} else if (x.Id == y.Id) {
						return 0;
					} else {
						return 1;
					}
				} else {
					return 1;
				}
			} else {
				return 1;
			}
		}
		#endregion
		
		
		/// <summary>
		/// Shows all item.
		/// </summary>
		public void ShowAllItem ()
		{ 
//			var sortItem = this.BagVo.ItemList.OrderBy (b => b.Item.typeRank)
//						.ThenByDescending (b => b.Item.quality)
//						.ThenByDescending (b => b.Item.usedLevel)
//						.ThenBy (b => b.Id);//根据类型，道具质量，使用等级，ID来进行排序
			this.AllItem = GetAllItem ();
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SHOWALLITEM);//显示所有道具数据
		}
		
		/// <summary>
		/// Gets all item.
		/// </summary>
		/// <returns>
		/// The all item.
		/// </returns>
		public IList<ItemInfo> GetAllItem ()
		{
			if (this.IsChange) {
				this.InitBag ();
			}
			this.BagVo.ItemList.Sort (CompareAllItem);			
			return this.BagVo.ItemList; //为所有道具赋值
		}
		
		/// <summary>
		/// Shows the equip item.
		/// </summary>
		public void ShowEquipItem ()
		{
			this.EquipItem = GetEquipItem ();
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SHOWEQUIPITEM);//显示所有装备	
		}
		
		/// <summary>
		/// Gets the equip item.
		/// </summary>
		/// <returns>
		/// The equip item.
		/// </returns>
		public IList<ItemInfo> GetEquipItem ()
		{
			if (this.IsChange) {
				this.InitBag ();
			}
			var sortItem = (from item in this.BagVo.ItemList
			                			where item.Item.packType == ePackageNavType.Equip||item.Item.packType == ePackageNavType.Weapon
										select item).ToList ();
			//			sortItem = sortItem.OrderByDescending (b => b.Item.quality)
			//					.ThenByDescending (b => b.Item.usedLevel)
			//					.ThenBy (b => b.Id);  //按照质量，使用等级，id来排序
						
			sortItem.Sort (CompareEquipItem);
			return sortItem;
		}
 
		/// <summary>
		/// Shows the normal item.
		/// </summary>
		public void ShowNormalItem ()
		{
			
			this.NormalItem = GetNormalItem ();
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SHOWNORMALITEM);//显示所有非装备的道具
		}
		
		/// <summary>
		/// Gets the normal item.
		/// </summary>
		/// <returns>
		/// The normal item.
		/// </returns>
		public IList<ItemInfo> GetNormalItem ()
		{
			if (this.IsChange) {
				this.InitBag ();
			}
			var sortItem = (from item in this.BagVo.ItemList
									where item.Item.packType == ePackageNavType.Other
							select item).ToList ();
//			sortItem = sortItem.OrderByDescending (b => b.Item.quality)
//					.ThenByDescending (b => b.Item.usedLevel)
//					.ThenBy (b => b.Id);  //按照质量，使用等级，id来排序
			
			sortItem.Sort (CompareAllItem);
			return sortItem;
		}

		/// <summary>
		/// Shows the pet equip item.
		/// </summary>
		public void ShowPetEquipItem(){
			this.PetEquipItem = this.GetPetEquipItem ();
		}

		/// <summary>
		/// Gets the pet equip item.
		/// </summary>
		/// <returns>The pet equip item.</returns>
		public IList<ItemInfo> GetPetEquipItem(){
			if (this.IsChange) {
				this.InitBag ();
			}
			var sortItem = (from item in this.BagVo.ItemList
			                		where item.Item.packType == ePackageNavType.PetEquip
			                select item).ToList ();

			sortItem.Sort (CompareEquipItem);
			return sortItem;
		}




		public int GetPowerCompareValue (ItemInfo itemInfo)
		{
			ItemStruct itemData = ItemManager.GetInstance ().GetItemByItemId (itemInfo.InstanceId); //得到物品数据
				 
			///装备评分对比
			EquipmentStruct equipData = EquipmentManager.GetInstance ().GetDataByItemId (itemData.instanceId);	//得到背包的装备数据
			int value = 0;
			if (this.EquipData.ContainsKey (equipData.equipPart)) {
				EquipmentStruct equipedData = this.EquipData [equipData.equipPart];			//得到已经装备的装备数据
				
				value = equipData.score - equipedData.score; //当前道具装备的战斗力减去使用中装备的战斗力
			} else {
				value = equipData.score;
			}

			return value;
		}

       /// <summary>
       /// 装备评分对比
       /// </summary>
        /// <param name="InstanceId">实例ID</param>
       /// <returns></returns>
        public int GetPowerCompareValue(int InstanceId)
        {
            ItemStruct itemData = ItemManager.GetInstance().GetItemByItemId(InstanceId); //得到物品数据
            if (itemData == null) return 0;
            ///装备评分对比
            EquipmentStruct equipData = EquipmentManager.GetInstance().GetDataByItemId(itemData.instanceId);	//得到背包的装备数据
            int value = 0;
            if (this.EquipData.ContainsKey(equipData.equipPart))
            {
                EquipmentStruct equipedData = this.EquipData[equipData.equipPart];			//得到已经装备的装备数据

                value = equipData.score - equipedData.score; //当前道具装备的战斗力减去使用中装备的战斗力
            }
            else
            {
                value = equipData.score;
            }

            return value;
        }
 
		
		/// <summary>
		/// 战斗力比较
		/// </summary>
		/// <returns>
		/// 比较的结果
		/// </returns>
		/// <param name='itemInfo'>
		/// 被比较的装备
		/// </param>
		public string PowerCompare (ItemInfo itemInfo)
		{
			int value = GetPowerCompareValue (itemInfo);
			if (value >= 0) {
				return  Global.FormatStrimg (LanguageManager.GetText ("lbl_item_func_cmp_up"), Mathf.Abs (value));
			} else {
				return  Global.FormatStrimg (LanguageManager.GetText ("lbl_item_func_cmp_down"), Mathf.Abs (value));
			}
		}
		
		/// <summary>
		/// Gets the intensity level.
		/// </summary>
		/// <returns>
		/// The intensity level.
		/// </returns>
		/// <param name='instanceId'>
		/// Instance identifier.
		/// </param>
		public uint GetIntensityLevel (int instanceId)
		{
			EquipmentStruct equipData = EquipmentManager.GetInstance ().GetDataByItemId ((uint)instanceId);	//得到装备数据
			return equipData.intensifyLevel;
		}
		
		
		/// <summary>
		/// Switchs the tab.
		/// </summary>
		/// <param name='tab'>
		/// tab类型
		/// </param>
		public void SwitchTab (BagView.Tab tab)
		{
			switch (tab) {
			case BagView.Tab.all:
				this.ShowAllItem ();
				break;
			case BagView.Tab.equip:
				this.ShowEquipItem ();
				break;
			case BagView.Tab.item:
				this.ShowNormalItem ();
				break;
			default:
				break;
			}	
		} 
		
		
		/// <summary>
		/// 卖物品
		/// </summary>
		/// <param name='itemsString'>
		/// 物品字符串列表，以逗号分隔
		/// </param>
		public void SaleItems (string itemsString)
		{
			var itemsArray = itemsString.Remove (itemsString.Length - 1, 1).Split (',');
 
			foreach (var item in itemsArray) {
				int key = int.Parse (item);
				var itemInfo = ItemManager.GetInstance ().items [key];
				if (ItemManager.GetInstance ().GetTemplateByTempId (itemInfo.tempId).saleProtect == 1) { //判断是否有出售保护
					this.HasValuable = true; //如果有出售保护则设置标志位
				} 
			 	 
				ItemStruct copyObj = (ItemStruct)itemInfo.Clone ();
				copyObj.num = 0; 	//0表示出售全部物品
				this._saleItemList.Add (copyObj); //添加入被销售物品的集合中
			}//批量贩卖道具
			
			if (this.HasValuable) {
//				this.HideCareerModel ();
				UIManager.Instance.ShowDialog (eDialogSureType.eSellItem, LanguageManager.GetText ("item_sell_valuable_msg"));
			} else {
				SaleItemListAndCountSaleNum ();    //销售物品并且计数
			}
		}
		
		/// <summary>
		/// Sales the item list and count sale number.
		/// </summary>
		public void SaleItemListAndCountSaleNum ()
		{
			SaleManager.Instance.SaleOrOpenItem (this.SaleItemList, UseGoodsStatus.Sale); 
		}
		
		/// <summary>
		/// 重置出售状态
		/// </summary>
		public  void ResetSaleStatus ()
		{
			this.HasValuable = false;    //重置是否有贵重品状态
			this.SaleItemList.Clear (); 	 //重置被卖数据队列
		}
		
		#region 事件注册
		public void RegisterEvent ()
		{
			DataChangeNotifyer.GetInstance ().EventChangeMoney += ShowIncreasedDiamond;	 //钻石变化的事件
			DataChangeNotifyer.GetInstance ().EventChangeCurrency += ShowIncreasedGold;  //金币变化的事件
			ItemEvent.GetInstance ().EventItemchange += ChangeItem;						 //如果发生变化，则需要重置是否改变的状态
			ItemEvent.GetInstance ().EventItemchangeRefUI += RefreshItems;					 //道具变化的事件
			EventDispatcher.GetInstance ().DialogSure += DialogSure;					 //弹出对话框的事件
			EventDispatcher.GetInstance ().DialogCancel += DialogCancel;				 //弹出对话框的事件
		}

		public void CancelEvent ()
		{
			DataChangeNotifyer.GetInstance ().EventChangeMoney -= ShowIncreasedDiamond;
			DataChangeNotifyer.GetInstance ().EventChangeCurrency -= ShowIncreasedGold;
			ItemEvent.GetInstance ().EventItemchange -= ChangeItem;						 
			ItemEvent.GetInstance ().EventItemchangeRefUI -= RefreshItems;
			EventDispatcher.GetInstance ().DialogSure -= DialogSure;
			EventDispatcher.GetInstance ().DialogCancel -= DialogCancel;
		}
		
		#endregion
		
		#region 事件注册对应的方法
		/// <summary>
		/// 显示增加的钻石数量
		/// </summary>
		/// <param name='dmd'>
		/// Dmd.
		/// </param>
		public void ShowIncreasedDiamond (int dmd)
		{
            //Gate.instance.sendNotification(MsgConstant.MSG_BAG_UPDATE_DIAMOND_GOLD);
            //FloatMessage.GetInstance().PlayFloatMessage(string.Format(LanguageManager.GetText("item_diamond_success_msg"), dmd),
            //                                           UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
		}
		
		/// <summary>
		/// 显示增加的金币数量
		/// </summary>
		/// <param name='gold'>
		/// Gold.
		/// </param>
		public void ShowIncreasedGold (int gold)
        {
            //Gate.instance.sendNotification(MsgConstant.MSG_BAG_UPDATE_DIAMOND_GOLD);
            //FloatMessage.GetInstance().PlayFloatMessage(string.Format(LanguageManager.GetText("item_sell_success_msg"), gold),
            //                                           UIManager.Instance.getRootTrans(), Vector3.zero, Vector3.zero);
		}

        
		/// <summary>
		/// 刷新道具列表
		/// </summary>
		/// <param name='itemId'>
		/// Item identifier.
		/// </param>
		/// <param name='type'>
		/// Type.
		/// </param>
		public void RefreshItems (int itemId, IsRefresh isRef)
		{
			if (isRef == IsRefresh.Refresh) {
				InitBagItemList ();						//初始化背包列表
				Gate.instance.sendNotification (MsgConstant.MSG_BAG_SWITCHTAB, BagView.CurrentTab);//显示当前tab
				PetManager.Instance.RefreshEquip();
			}
		}
		
		/// <summary>
		///	确认弹出框
		/// </summary>
		/// <param name='type'>
		/// 弹出的类型
		/// </param>
		public void DialogSure (eDialogSureType type)
		{
			if (type == eDialogSureType.eSellItem) {
				this.SaleItemListAndCountSaleNum (); 	//循环销售物品并且计数
			}
		}
		
		/// <summary>
		/// 取消弹出框
		/// </summary>
		/// <param name='type'>
		/// 弹出的类型
		/// </param>
		public void DialogCancel (eDialogSureType type)
		{
			if (type == eDialogSureType.eSellItem) {
				this.ResetSaleStatus ();
			}
		}
		
		public void ChangeItem (int itemId, ItemEvent.eItemChangeType type)
		{
			if (this.IsChange == false) {
				this.IsChange = true;
			}
		}
		
		#endregion

		/// <summary>
		/// Shows the career model.
		/// </summary>

		public void ShowCareerModel ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_SHOWCAREERMODEL);//显示模型
		}
		
		/// <summary>
		/// Hides the career model.
		/// </summary>
		public void HideCareerModel ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_HIDECAREERMODEL);//隐藏模型
		}
 		
		/// <summary>
		/// Updates the model weapon.
		/// </summary>
		public void UpdateModelWeapon ()
		{
			
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_UPDATE_MODEL_WEAPON);//更新武器模型
		}
		
		/// <summary>
		/// Updates the model armor.
		/// </summary>
		public void UpdateModelArmor ()
		{
			Gate.instance.sendNotification (MsgConstant.MSG_BAG_UPDATE_MODEL_ARMOR);//更新衣服模型
		}
		
		//getter and setter
		public static BagManager Instance {
			get { 
				if (_instance == null)
					_instance = new BagManager ();
				return _instance; 
			}
		}

		public EquipmentStruct GetEquipByPart (eEquipPart part)
		{
			return _equipData [part];
		}

        public int AutoFindItem
        {
            get { return _autoFindItem; }
            set { _autoFindItem = value; }
        }
		 
	}
}