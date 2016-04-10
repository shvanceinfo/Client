/**该文件实现的基本功能等
function: 潘多拉界面的数据
author:zyl
date:2014-06-10
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using System;

namespace model
{
	public class PandoraVo
	{
		private int _id;
		private string _name;
		private uint _mapId;
		private MapDataItem _map;
		private string _monsterName;
		private List<SActivityTime> _schedule;
		private string _ruleDesc;
		private string _desc;

		public PandoraVo ()
		{
			_schedule = new List<SActivityTime> ();
		}
	 
		public int ID {
			get {
				return _id;
			}
			set {
				_id = value;
			}
		}

		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		public uint MapId {
			get {
				return _mapId;
			}
			set {
				_mapId = value;
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


		public string MonsterName {
			get {
				return _monsterName;
			}
			set {
				_monsterName = value;
			}
		}

		public List<SActivityTime> Schedule {
			get {
				return _schedule;
			}
			set {
				_schedule = value;
			}
		}

		public string RuleDesc {
			get {
				return _ruleDesc;
			}
			set {
				_ruleDesc = value;
			}
		}

		public string Desc {
			get {
				return _desc;
			}
			set {
				_desc = value;
			}
		}

		public List<ItemTemplate> DropItem{
			get{
				var dropItem = new List<ItemTemplate>();
				if (this.Map!=null) {
					string[] itemList = this.Map.dropItem.Split(',');
					for (int i = 0; i < itemList.Length; i++) {
						if (!string.IsNullOrEmpty(itemList[i])) {
							var itemTemp = PandoraManager.Instance.Item.getTemplateData(Convert.ToInt32(itemList[i]));
							dropItem.Add(itemTemp);
						}
					}
				}

				return dropItem;
			}
		}
	}

	public class PandoraNumVo{
		private int _id;
		private eGoldType _tiaoZhanRes = eGoldType.none;
		private int _tiaoZhanResNum;
		private uint _tiaoZhanItemId;
		private int _tiaoZhanItemNum;
		private eGoldType _resetRes = eGoldType.none;
		private int _resetResNum;
		private uint _resetItemId;
		private int _resetItemNum;
		private eGoldType _pandoraRes = eGoldType.none;
		private int _pandoraResNum;
		private uint _pandoraItemId;
		private int _pandoraItemNum;



		public int PandoraItemNum {
			get {
				return _pandoraItemNum;
			}
			set {
				_pandoraItemNum = value;
			}
		}


		public uint PandoraItemId {
			get {
				return _pandoraItemId;
			}
			set {
				_pandoraItemId = value;
			}
		}

		public int PandoraResNum {
			get {
				return _pandoraResNum;
			}
			set {
				_pandoraResNum = value;
			}
		}

		public eGoldType PandoraRes {
			get {
				return _pandoraRes;
			}
			set {
				_pandoraRes = value;
			}
		}

		public int ResetItemNum {
			get {
				return _resetItemNum;
			}
			set {
				_resetItemNum = value;
			}
		}

		public uint ResetItemId {
			get {
				return _resetItemId;
			}
			set {
				_resetItemId = value;
			}
		}


		public int ResetResNum {
			get {
				return _resetResNum;
			}
			set {
				_resetResNum = value;
			}
		}

		public eGoldType ResetRes {
			get {
				return _resetRes;
			}
			set {
				_resetRes = value;
			}
		}

		public int TiaoZhanItemNum {
			get {
				return _tiaoZhanItemNum;
			}
			set {
				_tiaoZhanItemNum = value;
			}
		}

		public uint TiaoZhanItemId {
			get {
				return _tiaoZhanItemId;
			}
			set {
				_tiaoZhanItemId = value;
			}
		}

		public int ID {
			get {
				return _id;
			}
			set {
				_id = value;
			}
		}

		public eGoldType TiaoZhanRes {
			get {
				return _tiaoZhanRes;
			}
			set {
				_tiaoZhanRes = value;
			}
		}

		public int TiaoZhanResNum {
			get {
				return _tiaoZhanResNum;
			}
			set {
				_tiaoZhanResNum = value;
			}
		}
	}


}
