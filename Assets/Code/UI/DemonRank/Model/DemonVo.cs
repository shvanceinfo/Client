using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace model
{
    /// <summary>
    /// 玩家通关信息表
    /// </summary>
    public class DemonInfoVo
    {

        /// <summary>
        /// 当前的恶魔洞窟契约数量
        /// </summary>
        public Hashtable TickCount { get; set; }

        /// <summary>
        /// 每个等级消耗的契约数量 ,0,1,31,61
        /// </summary>
        public Hashtable ConsumeTickCount { get; set; }

        /// <summary>
        /// 今日最大层次
        /// </summary>
        public Hashtable CurGate { get; set; }
        /// <summary>
        /// 历史最大层次
        /// </summary>
        public Hashtable MaxGate {get;set;}

        /// <summary>
        /// 今日排名
        /// </summary>
        public Hashtable CurRank { get; set; }

        /// <summary>
        /// 昨日排名
        /// </summary>
        public Hashtable HistoryRank { get; set; }
        public DemonInfoVo()
        {
            ConsumeTickCount = new Hashtable();
            TickCount = new Hashtable();
            HistoryRank = new Hashtable();
            CurRank = new Hashtable();
            CurGate = new Hashtable();
            MaxGate = new Hashtable();
            int begin = (int)DemonDiffEnum.Begin;
            int end = (int)DemonDiffEnum.End;
            for (int i = begin; i < end; i++)
            {
                CurGate.Add((DemonDiffEnum)i, 0);
                MaxGate.Add((DemonDiffEnum)i, 0);
                CurRank.Add((DemonDiffEnum)i, 0);
                HistoryRank.Add((DemonDiffEnum)i, new HistoryRankVo());
                TickCount.Add((DemonDiffEnum)i, 0);
                ConsumeTickCount.Add((DemonDiffEnum)i, new BetterList<DemonVoItem>());
            }
        }
        
    }
    public class DemonVo
    {
        public DemonVo()
        {
            IsComplate = false;
            IsReceive = false;
            ConsumeItems = new BetterList<DemonVoItem>();
            RankRewards = new BetterList<DemonVoItem>();
            ConsumeGolds = new BetterList<DemonVoItem>();
        }

        public int Id { get; set; }

        public TowerDataItem.ETowerType eTowerType { get; set; }

        /// <summary>
        /// 难度
        /// </summary>
        public int Diff {
            get {
                return Id / 10000;
            }
        }

        public int Level
        {
            get { return Id % 100 == 0 ? 100 : Id % 100; }
        }

        /// <summary>
        /// 爬塔预制件
        /// </summary>
        public string BattlePrefab { get; set; }

        /// <summary>
        /// 当前关卡需要的等级
        /// </summary>
        public int UnLockLevel { get; set; }

        /// <summary>
        /// 解锁需求层数
        /// </summary>
        public int UnLockId { get; set; }

        /// <summary>
        /// 提取UnLockId的LVL
        /// </summary>
        public int UnLockId_Level {
            get {
                return UnLockId % 100 == 0 ? 100 : UnLockId % 100; ;
            }
        }

        /// <summary>
        /// 消耗物品列表，满足任意一个条件即可进入关卡
        /// </summary>
        public BetterList<DemonVoItem> ConsumeItems { get; set; }

        /// <summary>
        /// 消耗货币
        /// </summary>
        public BetterList<DemonVoItem> ConsumeGolds { get; set; }

        /// <summary>
        /// 掉落宝箱预制件
        /// </summary>
        public string BoxType { get; set; }

        /// <summary>
        /// 掉落宝箱ID
        /// </summary>
        public int DropOutBoxId { get; set; }

        /// <summary>
        /// 爬塔奖励
        /// </summary>
        public BetterList<DemonVoItem> RankRewards { get; set; }

        /// <summary>
        /// 是否以领取 ture=已经领取
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsComplate { get; set; }
    }
    /// <summary>
    /// DemonVo,奖励列表，id对应奖励数量
    /// </summary>
    public struct DemonVoItem
    {
        public int Id { get; set; }
        public int Nums { get; set; }
    }


    /// <summary>
    /// 排名奖励表
    /// </summary>
    public class RankXmlVo
    {
        public RankXmlVo()
        {
            Items = new BetterList<DemonVoItem>();
        }

        public int Id { get; set; }
        public int Diff {
            get { return Id / 1000; }
        }
        public string Name { get; set; }

        public int MinRank { get; set; }    //排名区间

        public int MaxRank { get; set; }    //排名区间 

        public BetterList<DemonVoItem> Items { get; set; }  //奖励物品
    }

    public class RankVo
    {
        /// <summary>
        /// 排名名次
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 哪个难度的
        /// </summary>
        public int Diff { get; set; }

        /// <summary>
        /// 玩家等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public CHARACTER_CAREER Career { get; set; }

        /// <summary>
        /// 今日层数
        /// </summary>
        public int RankTower { get; set; }

        /// <summary>
        /// 奖励物品图片
        /// </summary>
        public string ItemIcon { get; set; }

        /// <summary>
        /// 奖励物品个数
        /// </summary>
        public int ItemCount { get; set; }
    }

    /// <summary>
    /// 昨日排名
    /// </summary>
    public class HistoryRankVo
    {
        /// <summary>
        /// 昨日排名
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public int Gate { get; set; }

        /// <summary>
        /// 奖励物品图标
        /// </summary>
        public string AwardIcon { get; set; }

        /// <summary>
        /// 奖励个数
        /// </summary>
        public int AwardCount { get; set; }

        /// <summary>
        /// 是否领取
        /// </summary>
        public bool IsReceive { get; set; }

        /// <summary>
        /// 是否有奖励
        /// </summary>
        public bool IsAward { get; set; }
    }
}
