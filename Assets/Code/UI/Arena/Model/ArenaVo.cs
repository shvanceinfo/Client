using helper;
/**该文件实现的基本功能等
function: 竞技场的vo
author:ljx
date:2013-11-09
**/
using manager;
using MVC.entrance.gate;
using System;
using System.Collections.Generic;
using System.Timers;

namespace model
{
	public enum eHonorItemType
	{
		noItem = 0,
		equip = 1,
		tools = 2,
		diamond = 3,
		other = 4
	}
	
	public class ArenaVo
	{
		private ArenaInfo _arenaInfo;
		private BetterList<HeroListInfo> _heroList;
		private BetterList<ChallengeSigle> _challengerList;
		private BetterList<ResultInfo> _resultList;
		private BetterList<HonorItem> _honorItemList;
		
		public ArenaVo()
		{
			_heroList = new BetterList<HeroListInfo>();
			_challengerList = new BetterList<ChallengeSigle>();
			_resultList = new BetterList<ResultInfo>();
			_honorItemList = new BetterList<HonorItem>();
			_arenaInfo = new ArenaInfo();
		}
		
		//getter and setter
		public ArenaInfo ArenaInfo
		{
			get { return _arenaInfo; }
		}
		
		
		public BetterList<HeroListInfo> HeroList
		{
			get { return _heroList; }
		}
		
		public BetterList<ChallengeSigle> ChallengerList
		{
			get { return _challengerList; }
		}
		
		public BetterList<ResultInfo> ResultList
		{
			get { return _resultList; }
		}
		
		public BetterList<HonorItem> HonorItemList
		{
			get { return _honorItemList; }
		}
	}
	
	//竞技场信息
	public class ArenaInfo
	{
		public uint currentRank; //当前竞技场排名
		public uint continuousWin; //连胜场数
		public uint currentHonor;	//当前荣誉值
		public uint totalHonor;	//总荣誉值
		public uint honorLevel;	//荣誉等级
		public uint remainChallengeNum; //剩余挑战次数
		public uint buyChallengNum; //购买挑战次数
		public uint remainTime;	//挑战冷却时间(秒)
        public int lessReceiveTime; //领取奖励剩余时间(秒)
        private bool _isReceiveed;    //真:已经领取奖励,假未领取奖励

        public bool isReceiveed
        {
            get {
                Gate.instance.sendNotification(MsgConstant.MSG_ARENA_AWARD_BTN, !_isReceiveed);
                return _isReceiveed;
            }
            set { 
                _isReceiveed = value;
                Gate.instance.sendNotification(MsgConstant.MSG_ARENA_AWARD_BTN, !_isReceiveed);
            }
        }
	}
	
	//英雄榜单个英雄
	public class HeroListInfo
	{
		public string challengerName; //角色名
    	public uint level;		//等级
    	public uint powerStrength;	//战斗力
    	public int trend;			//趋势(0:无变化)
		public CHARACTER_CAREER vocation;		//角色职业
	}
	
	//可挑战对象
	public class ChallengeSigle
	{
		public uint rank; //排名
    	public string roleName; //角色名
    	public uint level; //等级
    	public bool gender; //性别(0:女;1:男)
    	public uint suitID; //服装类型ID
    	public uint weaponID; //巨剑类型ID
        //public uint rowID; //弓类型ID
        //public uint staffID; //双剑类型ID
    	public uint wingID; //翅膀类型ID
    	public int vocation; //职业ID
    	public uint power; //战斗力
    	
    	//比较可挑战对象
    	public static int compare(ChallengeSigle single1, ChallengeSigle single2)
    	{
    		if(single1.rank > single2.rank)
    			return -1;
    		else if(single1.rank == single2.rank)
    			return 0;
    		else
    			return 1;
    	}
	}
	
	//战报信息
	public class ResultInfo
	{
		public string roleName; //对方角色名
    	public bool beFight; //战斗类型(0:挑战,1:被挑战)
    	public bool fightResult; //战斗结果(0:失败,1:胜利)
    	public int varyRank; //排名变化数
    	public uint newRank; //新排名
    	public DateTime Time; //挑战的时间

        /// <summary>
        /// 返回时间段
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            TimeSpan ts = DateTime.Now - Time;
            int year = DateTime.Now.Year - Time.Year;
            if (year != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_year", Math.Max(year, 0));
            }
            int month = DateTime.Now.Month - Time.Month;
            if (month != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_month", Math.Max(month, 0));
            }
            int day = DateTime.Now.Day - Time.Day;
            if (day != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_day", Math.Max(day, 0));
            }
            int hour = DateTime.Now.Hour - Time.Hour;
            if (hour != 0)
            {
                return ViewHelper.FormatLanguage("guild_time_hour", Math.Max(hour, 0));
            }
            int minute = DateTime.Now.Minute - Time.Minute;
            if (minute != 0)
            {

                return ViewHelper.FormatLanguage("guild_time_minute", Math.Max(minute, 0));
            }
            return ViewHelper.FormatLanguage("guild_time_minute", 1);
        }
	}
	
	//荣誉商店物品
	public class HonorItem
	{
		public uint id; //商品编号
		public string itemName; //商品名称
		public eHonorItemType type; //商品分类
		public int sortPlace;		//商品显示序号
		public uint itemID;			//商品类型ID
		public uint hornorLvlLimit;	//购买荣誉等级限制
		public uint itemPrice;		//荣誉价格
		public int limitNum;        //荣誉商城每日商品兑换上限
	}
	
	//竞技场显示相应奖励信息
	public class AwardMsg
	{
		public uint awardID; 	//竞技场奖励编号 ABCD
		public string name; 	//玩家排名名称
		public int minRank; 	//竞技场名次左区间
		public int maxRank; 	//竞技场名次右区间
        public List<TypeStruct> award;
        public AwardMsg()
        {
            award = new List<TypeStruct>();
        }
	}
	
	//荣誉等级信息
	public class HonorLevel
	{
		public uint id;
		public int level;
		public string name;
		public uint needHonor;
	}
	
//	//挑战者的基本信息
//	public struct challengerInfo
//	{
//		string roleName =  Encoding.UTF8.GetChars(memRead.ReadBytes(20)); //角色名
//        uint vocationID = (uint)memRead.ReadInt32(); //职业ID
//        uint level = (uint)memRead.ReadInt32(); //等级
//        uint suitID = (uint)memRead.ReadUInt32(); //衣服物品类型ID
//        uint legID = (uint)memRead.ReadUInt32(); //护腿物品类型ID
//        uint shoeID = (uint)memRead.ReadUInt32(); //鞋物品类型ID
//        uint necklaceID = (uint)memRead.ReadUInt32(); //项链物品类型ID
//        uint ringID = (uint)memRead.ReadUInt32(); //指环物品类型ID
//        uint swordID = (uint)memRead.ReadUInt32(); //巨剑物品类型ID
//        uint bowID = (uint)memRead.ReadUInt32(); //弓物品类型ID
//        uint staffID = (uint)memRead.ReadUInt32(); //双剑物品类型ID
//        int length = (int)eFighintPropertyCate.eFpc_End; //战斗属性
//	}

	//竞技场奖励配置文件解析
	public class DataReadArenaAward : DataReadBase
	{		
	    public override string getRootNodeName()
	    {
	        return "RECORDS";
	    }
	
		public override void appendAttribute(int key, string name, string value)
		{	
			AwardMsg awardMsg;
			if(ArenaManager.Instance.AwardHash.Contains(key))
				awardMsg = (AwardMsg)ArenaManager.Instance.AwardHash[key];
			else
			{
				awardMsg = new AwardMsg();
				ArenaManager.Instance.AwardHash.Add(key, awardMsg);
			}
	        string[] splits = null;
	        char[] charSeparators = new char[] {','};
	        switch (name)
	        {
	            case "ID":
	        		awardMsg.awardID = uint.Parse(value);
	                break;
	            case "name":
	                awardMsg.name = value;
	                break;
	            case "rankLeft":
	                awardMsg.minRank = int.Parse(value);
	                break;
                case "rankRight":
	                awardMsg.maxRank = int.Parse(value);
	                break;
                case "RewardResource":
                    if (!value.Equals("0"))
                    {
                        splits = value.Split(charSeparators);
                        for (int i = 0; i < splits.Length; i+=2)
                        {
                            awardMsg.award.Add(
                                new TypeStruct(int.Parse(splits[i]),ConsumeType.Gold,int.Parse(splits[i+1])));
                        }
                    }
	                break;
	             case "Item":
                    if (!value.Equals("0"))
                    {
                        splits = value.Split(charSeparators);
                        for (int i = 0; i < splits.Length; i += 2)
                        {
                            awardMsg.award.Add(
                                new TypeStruct(int.Parse(splits[i]), ConsumeType.Item, int.Parse(splits[i + 1])));
                        }
                    }
	                break;
	            default:
	                break;
	        }
	    }
	}
	
	//荣誉商城配置文件解析
	public class DataReadHonorShop : DataReadBase
	{		
	    public override string getRootNodeName()
	    {
	        return "RECORDS";
	    }
	
		public override void appendAttribute(int key, string name, string value)
		{	
			HonorItem honorItem;
			uint hashKey = (uint)key;
			if(ArenaManager.Instance.HonorShopHash.Contains(hashKey))
				honorItem = (HonorItem)ArenaManager.Instance.HonorShopHash[hashKey];
			else
			{
				honorItem = new HonorItem();
				ArenaManager.Instance.HonorShopHash.Add(hashKey, honorItem);
			}
	        switch (name)
	        {
	            case "ID":
	        		honorItem.id = uint.Parse(value);
	                break;
				case "name":
	        		honorItem.itemName = value;
	                break;
                case "sort":
	                honorItem.type = (eHonorItemType)int.Parse(value);
	                break;
                case "dispIdx":
	        		honorItem.sortPlace = int.Parse(value);
	                break;
                case "itemID":
	        		honorItem.itemID = uint.Parse(value);
	        		ArenaManager.Instance.HonorShopHash.Add(honorItem.itemID, honorItem);
	                break;
                case "honorLv":
	        		honorItem.hornorLvlLimit = uint.Parse(value);
	                break;
                case "honorPrice":
	        		honorItem.itemPrice = uint.Parse(value);
	                break;
	            case "limitNum":
	        		honorItem.limitNum = int.Parse(value);
	                break;
	            default:
	                break;
	        }
	    }
	}
	
	//荣誉等级配置文件解析
	public class DataReadHonorLevel : DataReadBase
	{		
	    public override string getRootNodeName()
	    {
	        return "RECORDS";
	    }
	
		public override void appendAttribute(int key, string name, string value)
		{	
			HonorLevel honorLevel;
			int hashKey = key%1000;
			if(ArenaManager.Instance.NeedHonorHash.Contains(hashKey))
				honorLevel = ArenaManager.Instance.NeedHonorHash[hashKey] as HonorLevel;
			else
			{
				honorLevel = new HonorLevel();
				ArenaManager.Instance.NeedHonorHash.Add(hashKey, honorLevel);
			}
	        switch (name)
	        {
	            case "ID":
	        		honorLevel.id = uint.Parse(value);
	        		honorLevel.level = hashKey;
	                break;
				case "name":
	        		honorLevel.name = value;
	                break;
                case "honor":
	                honorLevel.needHonor = uint.Parse(value);
	                break;
	            default:
	                break;
	        }
	    }
	}
}
