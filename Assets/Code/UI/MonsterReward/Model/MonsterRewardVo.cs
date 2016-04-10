/**该文件实现的基本功能等
function: 魔物悬赏界面的数据
author:zyl
date:2014-06-04
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;

namespace model
{
	public enum MonsterRewardType
	{
		None=0,
		Level=1,
		Vip =2,
		Raid=3
	}
	
	public enum MonsterRewardNumType
	{
		Item=1,
		Resource=2
	}
	
	public class MonsterRewardVo
	{
		private int _id;
		private int _order;
		private MonsterRewardType _type;
		private uint _typeValue;
		private uint _itemId;
		private List<int> _numList;
		private eGoldType _resourceType;
		private List<int> _resourceNumList;
		private string _des;
		private uint _mapId;
		private MapDataItem _map;
		private int _currentClearNum;

		public MonsterRewardVo ()
		{
			this._numList = new List<int> ();
			this._resourceNumList = new List<int> ();
		}
		
		public int this [MonsterRewardNumType type] {
			get {
				int num = 0;
				switch (type) {
				case MonsterRewardNumType.Item:
                    if (this._currentClearNum < this._numList.Count)
					    num = this._numList [this._currentClearNum];
                    else
                        num = this._numList[this._currentClearNum-1];
					break;
				case MonsterRewardNumType.Resource:
                    if (this._currentClearNum < this._resourceNumList.Count)
                        num = this._resourceNumList[this._currentClearNum];
                    else
                        num = this._resourceNumList[this._currentClearNum-1];
					break;	
				default:
					break;
				}
				
				return num;
			}
		}
		
		//根据打的次数得到消耗的数值
		public int this [MonsterRewardNumType type, int index] {
			get {
				int num = 0;
				switch (type) {
				case MonsterRewardNumType.Item:
					if (index < 0) {
						index = 0;
					}
					if (index >= this._numList.Count) {
						index = this._numList.Count - 1;
					}
					num = this._numList [index];
					break;
				case MonsterRewardNumType.Resource:
					if (index < 0) {
						index = 0;
					}
					if (index >= this._resourceNumList.Count) {
						index = this._resourceNumList.Count - 1;
					}
					num = this._resourceNumList [index];
					break;	
				default:
					break;
				}
				
				return num;
			}
		}
		/// <summary>
		/// 当前通关的次数
		/// </summary>
		/// <value>
		/// The current number.
		/// </value>
		public int CurrentClearNum {
			get {
				return this._currentClearNum;
			}
			set {
				_currentClearNum = value;
			}
		}

		public string Des {
			get {
				return this._des;
			}
			set {
				_des = value;
			}
		}

		public int Id {
			get {
				return this._id;
			}
			set {
				_id = value;
			}
		}

		public uint ItemId {
			get {
				return this._itemId;
			}
			set {
				_itemId = value;
			}
		}

		public uint MapId {
			get {
				return this._mapId;
			}
			set {
				_mapId = value;
			}
		}

		public List<int> NumList {
			get {
				return this._numList;
			}
			set {
				_numList = value;
			}
		}

		public int Order {
			get {
				return this._order;
			}
			set {
				_order = value;
			}
		}

		public List<int> ResourceNumList {
			get {
				return this._resourceNumList;
			}
			set {
				_resourceNumList = value;
			}
		}

		public eGoldType ResourceType {
			get {
				return this._resourceType;
			}
			set {
				_resourceType = value;
			}
		}

		public MonsterRewardType Type {
			get {
				return this._type;
			}
			set {
				_type = value;
			}
		}

		public uint TypeValue {
			get {
				return this._typeValue;
			}
			set {
				_typeValue = value;
			}
		}
		
		public ItemTemplate UseItem {
			get {
				return  ItemManager.GetInstance ().GetTemplateByTempId (this._itemId);
				;
			}
		}
		
		public MapDataItem Map {
			get {
				if (this._map == null) {
					this._map = ConfigDataManager.GetInstance ().getMapConfig ().getMapData ((int)this._mapId);
				}
				return this._map;
			}
		}
		
		public int MaxClearNum {
			get {
				return VipManager.Instance.MonsterRewardCount;
			}
		}
		/// <summary>
		/// 是否开启了快速追缉
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is can quick; otherwise, <c>false</c>.
		/// </value>
		public bool IsCanQuick{
			get{
				return this.HasPass&&VipManager.Instance.MonsterRewardQuick;
			}
		}
		/// <summary>
		/// 是否开启了一键追缉
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is can one clear; otherwise, <c>false</c>.
		/// </value>
		public bool IsCanOneClear{
			get{
				return this.HasPass&&VipManager.Instance.MonsterRewardOneClear;
			}
		}

		public bool HasPass {
			set;get;
		}		
		
		/// <summary>
		/// 判断条件是否满足
		/// </summary>
		/// <returns>
		/// The condition.
		/// </returns>
		/// <param name='monster'>
		/// If set to <c>true</c> monster.
		/// </param>
		public bool CheckCondition {
			get {
				bool isTrue = false; //判断是否满足条件
				switch (this.Type) {
				case MonsterRewardType.None:
					isTrue = true;
					break;
				case MonsterRewardType.Level:
					if (CharacterPlayer.character_property.getLevel () >= this.TypeValue) {
						isTrue = true;
					}
					break;
				case MonsterRewardType.Vip:
					if (VipManager.Instance.VipLevel >= this.TypeValue) {
						isTrue = true;
					}
					break;
				case MonsterRewardType.Raid:
				
					uint mapId = this.TypeValue / 10;
					uint easyOrHard = this.TypeValue % 10; //0是普通 1是精英
					if (RaidManager.Instance.PassMapHash.ContainsKey (mapId)) {
						sPassMap passmap = (sPassMap)RaidManager.Instance.PassMapHash [mapId]; //得到随机通关地图的信息
						if (easyOrHard == 0) {
							if (passmap.easy >= 3) {//通关
								isTrue = true;
							} 
						} else if (easyOrHard == 1) {
							if (passmap.normal >= 3) { //精英通关
								isTrue = true;
							}
						}
					} 
				
					break;
			
				default:
					break;
				}
				return isTrue;
			}
			
		}
	}
	
}
