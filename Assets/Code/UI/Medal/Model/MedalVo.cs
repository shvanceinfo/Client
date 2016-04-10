using UnityEngine;
using System.Collections;

namespace model{
	public class MedalVo{
		public int ID{get;set;}
		public string Name{get;set;}
		public int Level{get;set;}
		public string Icon{get;set;}
		public BetterList<GoldValue> Consumes{get;set;}
		public int LvUp_item{get;set;}
		public BetterList<AttributeValue> Attributes{get;set;}
		public string ChengHao{get;set;}
		public string ChengHaoReward_resource{get;set;}
		public string ChengHaoReward_item{get;set;}
		public MedalVo ()
		{
			Attributes=new BetterList<model.AttributeValue>();
			Consumes=new BetterList<model.GoldValue>();
		}
	}
	public class GoldValue
	{
		public eGoldType Type {
			get;
			set;
		}
		public int Value {
			get;
			set;
		}
	}
}
