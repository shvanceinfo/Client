/**该文件实现的基本功能等
function: 背包的vo
author:zyl
date:2014-4-8
**/
using manager;
using System;
using System.Collections.Generic;

public class BagVo   {
 	
	private int _bagNum;				//已经使用的背包格子数量
	private List<ItemInfo> _itemList;
	
	public BagVo(){
		this._itemList = new List<ItemInfo>();
	}

	public int BagNum {
		get {
			return this._itemList.Count;
		}
	}	
	
	public List<ItemInfo> ItemList {
		get {
			return this._itemList;
		}
		set {
			_itemList = value;
		}
	}
}


public class ItemInfo{
	private uint _id;
	private int _instanceId;
	private uint _num;
	private ItemTemplate _item;
//	private bool _isNewItem;
	 
	
	public ItemInfo(uint id,int instanceId,uint num){
		this._id = id;
		this._instanceId = instanceId;
		this._num = num;
		_item = ItemManager.GetInstance ().GetTemplateByTempId (id);
	}
	
//	public ItemInfo(uint id,int instanceId,uint num,bool isNew){
//		this._id = id;
//		this._instanceId = instanceId;
//		this._num = num;
//		_item = ItemManager.GetInstance ().GetTemplateByTempId (id);
//		this._isNewItem = isNew;
//	}
	
	
	public uint Id {
		get {
			return this._id;
		}
		
	}

	public int InstanceId {
		get {
			return this._instanceId;
		}
		set {
			_instanceId = value;
		}
	}
	public uint Num {
		get {
			return this._num;
		}
		
	}

	public ItemTemplate Item {
		get {
			return this._item;
		}
	}

//	public bool IsNewItem {
//		get {
//			return this._isNewItem;
//		}
//		set {
//			_isNewItem = value;
//		}
//	}
}