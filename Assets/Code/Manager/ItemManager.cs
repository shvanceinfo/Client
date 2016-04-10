using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using manager;
using UnityEngine;
/// <summary>
/// 左侧导航
/// </summary>
public enum ePackageNavType
{
	Weapon = 1,
	Equip = 2,
	Other = 3,
	PetEquip =4,
}
/// <summary>
/// 底部功能
/// </summary>
public enum ePackageFuncType
{
	eNone = 0,      //所有
	eRef = 1,       ///强化
	eCurt = 2,      ///洗练
	eAdvanced = 3, ///进阶功能
	eInlay = 4, ///镶嵌
	eAnalysis = 5, ///分解
	eCmp=6, ///对比
	eDetail = 7,
	eComp = 8,      //合成
}

/// <summary>
/// 物品管理事件定义
/// </summary>
public class ItemEvent
{
	public enum eItemChangeType
	{
		Add = 1,
		Delete = 2,
		Edit = 3
	};

	private static ItemEvent instance;

	private ItemEvent ()
	{
	}
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	public static ItemEvent GetInstance ()
	{
		if (instance == null) {
			instance = new ItemEvent ();
		}
		return instance;
	}

   
	/// <summary>
	/// 物品变更事件
	/// </summary>
	/// <param name="itemId"></param>
	/// <param name="type"></param>
	public delegate void HandleItemChange (int itemId,eItemChangeType type);

	public event HandleItemChange EventItemchange;

	public void OnItemChange (int itemId, eItemChangeType type)
	{
		if (EventItemchange != null) {
			EventItemchange (itemId, type);
		}
	}
	
	/// <summary>
	///  物品变更事件,根据参数决定是否刷新ui
	/// </summary>
	public delegate void HandleItemChangeRefUI (int itemId,IsRefresh isRef);

	public event HandleItemChangeRefUI EventItemchangeRefUI;

	public void OnItemChange (int itemId, IsRefresh isRef)
	{
		if (EventItemchangeRefUI != null) {
			EventItemchangeRefUI (itemId, isRef);
		}
	}
	
	/// <summary>
	/// 物品改变的原因
	/// </summary>
	public delegate void HandleItemChangeReason (int itemId,ItemChangeReason changeReason);
	
	public event HandleItemChangeReason EventItemChangeReason;
	
	public void OnItemChange (int itemId, ItemChangeReason changeReason)
	{
		if (EventItemChangeReason != null) {
			EventItemChangeReason (itemId, changeReason);
		}
	}
	
	public delegate void HandleNewItemChange (ItemInfo newItem,IsRefresh isRef);

	public event HandleNewItemChange EventNewItemChange;
	
	public void OnItemChange (ItemInfo newItem, IsRefresh isRef)
	{	
		if (newItem.Num !=0) {
			MainManager.Instance.NewItemList.Add(newItem);
		}
		if (EventNewItemChange != null) {
			EventNewItemChange (newItem, isRef);
		}
	}
	
	
	
	
	
	/// <summary>
	/// 选中物品
	/// </summary>
	/// <param name="itemInstanceId"></param>
	public delegate void HandleSelectItem (int itemInstanceId);

	public event HandleSelectItem EventSelectItem;

	public void OnSelectItem (int itemInstanceId)
	{
		if (EventSelectItem != null) {
			EventSelectItem (itemInstanceId);
		}
	}
	/// <summary>
	/// 物品过滤
	/// </summary>
	/// <param name="navType"></param>
	public delegate void HandleSelectNav (ePackageNavType navType);

	public HandleSelectNav EventSelectNav;

	public void OnSelectNav (ePackageNavType navType)
	{
		if (EventSelectNav != null) {
			EventSelectNav (navType);
		}
	}
	/// <summary>
	/// 底部功能
	/// </summary>
	/// <param name="funcType"></param>
	/// <param name="open"></param>
	public delegate void HandleItemFuncNav (ePackageFuncType funcType,bool open);

	public HandleItemFuncNav EventItemFuncNav;

	public void OnItemFuncNav (ePackageFuncType funcType, bool open)
	{
		if (EventItemFuncNav != null) {
			EventItemFuncNav (funcType, open);
		}
	}

	/// <summary>
	/// 关卡掉落物品
	/// </summary>
	/// HandleItemChange(int itemId, eItemChangeType type
	public delegate void HandleAwardItemChange (uint itemId,uint num,eItemChangeType type);

	public event HandleAwardItemChange EventAwardItemChange;

	public void OnAddAwardItemChange (uint itemId, uint num, eItemChangeType type)
	{
		if (EventAwardItemChange != null) {
			EventAwardItemChange (itemId, num, type);
		}
	}
	/// <summary>
	/// 物品数量变更
	/// </summary>
	public delegate void HandleChangeItemNum ();

	public event HandleChangeItemNum EventChangeItemNum;

	public void OnChangeItemNum ()
	{
		if (EventChangeItemNum != null) {
			EventChangeItemNum ();
		}
	}
	/// <summary>
	/// 物品操作状态
	/// </summary>
	/// <param name="type"></param>
	/// <param name="status"></param>
	public delegate void HandleNotifyItemFuncStatus (ePackageFuncType type,int status);

	public event HandleNotifyItemFuncStatus EventNotifyItemFuncStatus;

	public void OnNotifyItemFuncStatus (ePackageFuncType type, int status)
	{
		if (EventNotifyItemFuncStatus != null) {
			EventNotifyItemFuncStatus (type, status);
		}
	}
}

public class ItemManager
{
	public const int MAX_REF_LEVEL = 15;
	public enum ePackType
	{
		ePackage,
		eTempPackage,
		eAward
	};

	public bool showAwardItems = false;
	public bool[] hasNewItem = { false, false, false };
	/// <summary>
	/// 临时包裹，开始编号
	/// </summary>
    private const uint tempItemStartId = 30000;
    private const uint tempEquipStartId = 50000;
	public uint equipNum = 0;
	public uint weaponNum = 0;
	public uint oherNum = 0;
	public uint petEquipNum = 0;
	private static ItemManager instance;
	public Dictionary<int, ItemStruct> items;
	public Dictionary<int, ItemStruct> equippedItems;
	public Dictionary<int, ItemStruct> awardItems;
    public List<int> showAwardList = null;
	public bool canShwoAwardPanel;
	public int lastSelectItem;
	private bool _isInitFinish; //初始化item结束
	private ItemManager ()
	{
		Init ();
	}

	public void Init ()
	{
		equipNum = 0;
		weaponNum = 0;
		oherNum = 0;
		petEquipNum = 0;
		_isInitFinish = false;
		canShwoAwardPanel = false;
		lastSelectItem = 0;
		items = new Dictionary<int, ItemStruct> ();
		equippedItems = new Dictionary<int, ItemStruct> ();
		awardItems = new Dictionary<int, ItemStruct> ();
		showAwardList = new List<int>();
	}

	public static ItemManager GetInstance ()
	{
		if (instance == null) {
			instance = new ItemManager ();
		}
		return instance;
	}

	/// <summary>
	/// 添加物品
	/// </summary>
	/// <param name="item">物品实例</param>
	private bool Add (ItemStruct item)
	{
		if (items.ContainsKey (item.instanceId)) {
			return false;
		}
		if (EquipmentManager.GetInstance ().equipments.ContainsKey (item.instanceId)) {
			item.equipSource = EquipmentManager.GetInstance ().equipments [item.instanceId].score;
		}
		items.Add (item.instanceId, item);
		ChangeEquipedItem (item);
		ChangeNum (item.tempId, true);
		if (_isInitFinish)
			item.isNewItem = true;
		else
			item.isNewItem = false;
        GuideInfoManager.Instance.AddGuideTrigger(item);
		return true;
	}
    

	void ChangeNum (uint tempId, bool add)
	{
		ItemTemplate temp = GetTemplateByTempId (tempId);
		if (temp.packType == ePackageNavType.Equip) {
			if (add) {
				equipNum++;
				hasNewItem [0] = true;
			} else {
				equipNum--;
			}
		} else if (temp.packType == ePackageNavType.Weapon) {
			if (add) {
				weaponNum++;
				hasNewItem [1] = true;
			} else {
				weaponNum--;
			}
		} else if (temp.packType == ePackageNavType.Other){
			if (add) {
				oherNum++;
				hasNewItem [2] = true;
			} else {
				oherNum--;
			}
		}else if (temp.packType == ePackageNavType.PetEquip){
			if (add) {
				petEquipNum++;
//				hasNewItem [2] = true;
			} else {
				petEquipNum--;
			}
		}
		ItemEvent.GetInstance ().OnChangeItemNum ();
	}

	/// <summary>
	/// 删除物品
	/// </summary>
	/// <param name="itemId">物品id</param>
	/// <returns></returns>
	public bool Delete (int itemId, IsRefresh isRefresh)
	{
		bool deleted = false;
		uint tempId = 0;
		if (items.ContainsKey (itemId)) {
			tempId = GetItemByItemId (itemId).tempId;
			ChangeEquipedItem (items [itemId]);
			deleted = items.Remove (itemId);
            
			//OnItemChangeNum(type, items.Count);                
		}
		if (deleted) {
//			ItemEvent.GetInstance ().OnItemChange (new ItemInfo (0, 0, 0), isRefresh);
			ItemEvent.GetInstance ().OnItemChange (itemId, isRefresh);
			ItemEvent.GetInstance ().OnItemChange (itemId, ItemEvent.eItemChangeType.Delete);
			ChangeNum (tempId, false);
		}
        
		return deleted;
	}
	/// <summary>
	/// 物品变更操作
	/// </summary>
	/// <param name="item"></param>
	/// <param name="type"></param>
	public void Change (ItemStruct item, IsRefresh isRefresh)
	{
        if (item.instanceId < tempItemStartId)
        {
			if (items.ContainsKey (item.instanceId)) {
				if ((item.tempId == 0) || (item.num == 0)) {
//					Debug.Log(item.instanceId + " " + item.num + " "+ isRefresh);
					Delete (item.instanceId, isRefresh);
					EquipmentManager.GetInstance ().Delete (item.instanceId);
				} else {
					if (EquipmentManager.GetInstance ().equipments.ContainsKey (item.instanceId)) {
						item.equipSource = EquipmentManager.GetInstance ().equipments [item.instanceId].score; //设置战斗力
					}

					// 保存起始的模板id 用在对装备的武器进阶 需要及时显示模型
					bool bJinjieEquip = false;
					if (items [item.instanceId].tempId != item.tempId) {
						// 是在进阶状态
						bJinjieEquip = true;
					}
//					Debug.Log(item.instanceId + " " + item.num + " "+ isRefresh);
					if (_isInitFinish) {
						if (item.num > items [item.instanceId].num) { //如果大于原始的值,说明是添加，需要触发事件
//							ItemEvent.GetInstance ().OnItemChange (new ItemInfo (item.tempId, item.instanceId, (item.num - items [item.instanceId].num)), isRefresh);
						}
					}
					items [item.instanceId] = item;
					ChangeEquipedItem (item, bJinjieEquip);
					ItemEvent.GetInstance ().OnItemChange (item.instanceId, item.changeReason);
					ItemEvent.GetInstance ().OnItemChange (item.instanceId, isRefresh);
					ItemEvent.GetInstance ().OnItemChange (item.instanceId, ItemEvent.eItemChangeType.Edit);
				}
			} else if ((item.tempId != 0) && (item.num > 0)) {
//				Debug.Log(item.instanceId + " " + item.num + " "+ isRefresh);
				Add (item);
				if (this._isInitFinish) {
//					ItemEvent.GetInstance ().OnItemChange (new ItemInfo (item.tempId, item.instanceId, item.num), isRefresh);
				}
				ItemEvent.GetInstance ().OnItemChange (item.instanceId, item.changeReason);
				ItemEvent.GetInstance ().OnItemChange (item.instanceId, isRefresh);
				ItemEvent.GetInstance ().OnItemChange (item.instanceId, ItemEvent.eItemChangeType.Add);

				//BattleEmodongku.GetInstance().ShowTowerAward(item.tempId);
			}  
		} else if(item.instanceId >= tempItemStartId && item.instanceId < tempEquipStartId) {
			ChangeAwardItem (item);
			AwardManager.Instance.AddAwardItem (item);
		}
	}
	/// <summary>
	/// 奖励物品
	/// </summary>
	/// <param name="item"></param>
	private void ChangeAwardItem (ItemStruct item)
	{
		if (awardItems.ContainsKey (item.instanceId)) {
            
			if ((item.tempId == 0) || (item.num == 0)) {
				bool deleted = false;
				if (awardItems.ContainsKey (item.instanceId)) {
					deleted = awardItems.Remove (item.instanceId);          
				}
				if (deleted) {
					ItemEvent.GetInstance ().OnAddAwardItemChange (item.instanceId, item.num, ItemEvent.eItemChangeType.Delete);
				}
			} else {
				awardItems [item.instanceId] = item;
//				ItemEvent.GetInstance ().OnItemChange(item.instanceId,isRefresh);
				ItemEvent.GetInstance ().OnItemChange (item.instanceId, ItemEvent.eItemChangeType.Edit);
			}
		} else if ((item.tempId != 0) && (item.num > 0)) {
			showAwardList.Add (item.instanceId);
			awardItems.Add (item.instanceId, item);
			ItemEvent.GetInstance ().OnAddAwardItemChange (item.instanceId, item.num, ItemEvent.eItemChangeType.Add);
		}  
	}

	/// <summary>
	/// 包裹排序
	/// </summary>
	/// <param name="type"></param>
	public void Sort (ePackType type)
	{
		//已装备、模板id排序
		//items = (from entry in items orderby entry.Value.equipPos descending, entry.Value.tempId select entry).ToDictionary(pair => pair.Key, pair => pair.Value);           
		items = SortDictionaryBySlot (items); 
	}
	/// <summary>
	/// 排序
	/// </summary>
	/// <param name="members"></param>
	/// <returns></returns>
	private Dictionary<int, ItemStruct> SortDictionaryBySlot (Dictionary<int, ItemStruct> members)
	{
		List<KeyValuePair<int, ItemStruct>> list = members.ToList ();
		list.Sort ((firstPair, nextPair) => {
			return SortCompare (firstPair.Value, nextPair.Value); });
		return list.ToDictionary ((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
	}
	/// <summary>
	/// 排序处理
	/// </summary>
	/// <param name="first"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	private static int SortCompare (ItemStruct first, ItemStruct next)
	{
		uint x = (uint)first.equipPos;
		uint y = (uint)next.equipPos;
		///是否已经装备
		int cmp = y.CompareTo (x);
		if (cmp == 0) {
			///位装备，对比装备评分
			int cmpSource = next.equipSource.CompareTo (first.equipSource);
			if (cmpSource == 0) {
				///评分一样，对比模版id
				return first.tempId.CompareTo (next.tempId);
			} else {
				return cmpSource;
			}
		} else {
			return cmp;
		}
	}
	/// <summary>
	/// 通过装备评分排序物品
	/// </summary>
	/// <param name="first"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	private static int SortCompareBySource (ItemStruct first, ItemStruct next)
	{
		int x = first.equipSource;
		int y = next.equipSource;
		int cmp = y.CompareTo (x);
		if (cmp == 0) {
			return first.tempId.CompareTo (next.tempId);
		} else {
			return cmp;
		}
	}

	/// <summary>
	/// 已装备的数据
	/// </summary>
	/// <param name="item"></param>
	void ChangeEquipedItem (ItemStruct item, bool bInJinjie = false)
	{        
		if (!equippedItems.ContainsKey (item.instanceId)) {
			if (item.equipPos != 0) {
				equippedItems.Add (item.instanceId, item);
				if (CharacterPlayer.sPlayerMe)
					CharacterPlayer.sPlayerMe.equipItem ((int)item.tempId);
				if (CharacterPlayerUI.sPlayerMeUI)
					CharacterPlayerUI.sPlayerMeUI.equipItem ((int)item.tempId);
			}
		} else {
			if (equippedItems.ContainsKey (item.instanceId)) {
				// 对装备的武器进阶处理
				if (bInJinjie) {
					// 先移调
					equippedItems.Remove (item.instanceId);
					// 再装上
					CharacterPlayer.sPlayerMe.equipItem ((int)item.tempId);
				}

				if ((item.tempId == 0) || (item.num == 0) || (item.equipPos == 0)) {
					equippedItems.Remove (item.instanceId);
				} else {
					if (EquipmentManager.GetInstance ().equipments.ContainsKey (item.instanceId)) {
						item.equipSource = EquipmentManager.GetInstance ().equipments [item.instanceId].score;
					}
					equippedItems [item.instanceId] = item;
				}
			}
		}
	}
	/// <summary>
	/// 通过装备位置查找已经装备的物品
	/// </summary>
	/// <param name="part"></param>
	/// <returns></returns>
	public ItemStruct FindEquipedItemByPart (eEquipPart part)
	{
		ItemStruct item = new ItemStruct ();
		foreach (KeyValuePair<int, ItemStruct> keyItem in equippedItems) {
			//EquipmentTemplate temp = ConfigDataManager.GetInstance().getEquipmentConfig().getEquipData((int)keyItem.Value.tempId);
			EquipmentTemplate temp = EquipmentManager.GetInstance ().GetTemplateByTempId (keyItem.Value.tempId);
			if ((temp.id != 0) && (temp.part == part)) {
				item = keyItem.Value;
				break;
			}
		}
		return item;
	}
	/// <summary>
	/// 通过物品模板ID获取模板信息
	/// </summary>
	/// <param name="instanceId"></param>
	/// <returns></returns>
	public ItemTemplate GetTemplateByTempId (uint templateId)
	{
		ItemTemplate temp = ConfigDataManager.GetInstance ().getItemTemplate ().getTemplateData ((int)templateId);
		return temp;
	}
	/// <summary>
	/// 通过模板id查找物品实例
	/// </summary>
	/// <param name="tempId">物品模板ID</param>
	/// <returns>ArrayList</returns>
	public ArrayList FindItemByTempId (uint tempId)
	{
		ArrayList findItemId = new ArrayList ();
		foreach (KeyValuePair<int, ItemStruct> item in items) {
			if (item.Value.tempId == tempId) {
				findItemId.Add (item.Value.instanceId);
			}            
		}
		return findItemId;
	}
    
	/// <summary>
	/// 通关模板ID查找物品的数量
	/// </summary>
	public uint GetItemNumById (uint tempId)
	{
		uint itemNum = 0;
		foreach (KeyValuePair<int, ItemStruct> item in items) {
			if (item.Value.tempId == tempId) {
				itemNum += item.Value.num;
			}            
		}
		return itemNum;
	}

    public ItemStruct FindItemInBagByTempId(int tempId)
    {
        foreach (KeyValuePair<int, ItemStruct> item in items)
        {
            if (item.Value.tempId == tempId)
            {
                return item.Value;
            }
        }
        return null;
    }
    
	/// <param name="itemId"></param>
	/// <summary>
	/// 请求换装备
	/// </summary>
	/// <param name="itemId"></param>
	public void NetPutEquip (ushort instanceId)
	{
		GCAskEquipGoods equip = new GCAskEquipGoods (instanceId);
		MainLogic.SendMesg (equip.ToBytes ());
	}
	/// <summary>
	/// 提取奖励物品
	/// </summary>
	/// <param name="awardItemId"></param>
	public void NetMoveGoods (uint awardItemId)
	{
		GCAskMoveGoods net = new GCAskMoveGoods (awardItemId, 0);
		MainLogic.SendMesg (net.ToBytes ());
	}

	/// <summary>
	/// 通关物品ID提取物品实例
	/// </summary>
	/// <param name="itemId"></param>
	/// <returns></returns>
	public ItemStruct GetItemByItemId (int itemId)
	{
		if (items.ContainsKey (itemId)) {
			return items [itemId];
		}
		return null;
	}
	/// <summary>
	/// 奖励物品
	/// </summary>
	/// <param name="itemId"></param>
	/// <returns></returns>
	public ItemStruct GetAwardItemById (int itemId)
	{
		if (awardItems.ContainsKey (itemId)) {
			return awardItems [itemId];
		}
		return null;
	}

	/// <summary>
	/// 获取已经装备数据，同过part进行索引
	/// </summary>
	/// <returns></returns>
	public Dictionary<eEquipPart, ItemStruct> GetEquipData ()
	{
		Dictionary<eEquipPart, ItemStruct> equip = new Dictionary<eEquipPart, ItemStruct> ();
		foreach (KeyValuePair<int, ItemStruct> item in equippedItems) {
			EquipmentTemplate temp = EquipmentManager.GetInstance ().GetTemplateByTempId (item.Value.tempId);
			if (!equip.ContainsKey (temp.part)) {
				equip.Add (temp.part, item.Value);
			}
		}
		return equip;
	}
	/// <summary>
	/// 提取宝石模版
	/// </summary>
	/// <param name="tempId"></param>
	/// <returns></returns>
	[Obsolete()]
	public GemTemplate GetGemTempByTempId (uint tempId)
	{
		return ConfigDataManager.GetInstance ().getGemTemplate ().getTemplateData ((int)tempId);        
	}

	public void SetLoginItem ()
	{
		hasNewItem [0] = false;
		hasNewItem [1] = false;
		hasNewItem [2] = false;
		_isInitFinish = true;
	}
    
//	public bool judgeNewItems (uint id, out uint index)
//	{
//		ItemTemplate template = GetTemplateByTempId (id);
//		ePackageNavType type = template.packType;
//		switch (type) {
//		case ePackageNavType.Equip:
//			index = 0;
//			break;
//		case ePackageNavType.Weapon:
//			index = 1;
//			break;
//		case ePackageNavType.Other:
//			index = 2;
//			break;
//		default:
//			index = 0;
//			break;
//		}
//		foreach (KeyValuePair<int, ItemStruct> keyItem in items) {
//			//EquipmentTemplate temp = ConfigDataManager.GetInstance().getEquipmentConfig().getEquipData((int)keyItem.Value.tempId);
//			ItemTemplate temp = GetTemplateByTempId (keyItem.Value.tempId);
//			if (temp.packType == type) {
//				if (keyItem.Value.isNewItem)
//					return false;
//			}
//		}
//		return true;
//	}
    
	//获取相应的颜色值
	public float getItemColor (eItemQuality quality)
	{
		float color = DealTexture.COLOR_NONE;
		switch (quality) {
		case eItemQuality.eWhite:
			color = DealTexture.COLOR_NONE;
			break;
		case eItemQuality.eGreen:
			color = DealTexture.COLOR_GREEN;
			break;
		case eItemQuality.eBlue:
			color = DealTexture.COLOR_BLUE;
			break;
		case eItemQuality.ePurple:
			color = DealTexture.COLOR_PURPLE;
			break;
		case eItemQuality.eOrange:
			color = DealTexture.COLOR_ORANGE;
			break;
		default:
			break;
		}
		return color;
	}
}