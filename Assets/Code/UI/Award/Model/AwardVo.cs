using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace model
{

    public class AwardVo
    {
        private string _mapName;

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }
        private bool _playerIsDead;

        public bool PlayerIsDead                //玩家是否死亡
        {
            get { return _playerIsDead; }
            set { _playerIsDead = value; }
        }

        private bool _isRevive;

        private int _revivalCount;

        /// <summary>
        /// 复活次数
        /// </summary>
        public int RevivalCount
        {
            get { return _revivalCount; }
            set { _revivalCount = value; }
        }

        public bool IsUseRevive            //是否使用复活币
        {
            get { return _isRevive; }
            set { _isRevive = value; }
        }

        private float _enterTime;     //进入副本钱，记录一下一下信息

        public float EnterTime
        {
            get { return _enterTime; }
            set { _enterTime = value; }
        }
        private int _enterMoney;

        public int EnterMoney
        {
            get { return _enterMoney; }
            set { _enterMoney = value; }
        }
        private int _enterExp;

        public int EnterExp
        {
            get { return _enterExp; }
            set { _enterExp = value; }
        }

        private int _getTime;

        public int GetTime              //花费时间
        {
            get { return _getTime; }
            set { _getTime = value; }
        }
        private int _getMoney;          //得到金钱

        public int GetMoney
        {
            get { return _getMoney; }
            set { _getMoney = value; }
        }   
        private int _getExp;            //得到经验

        public int GetExp
        {
            get { return _getExp; }
            set { _getExp = value; }
        }

        private BetterList<ItemStruct> _curAwardItemList;
        private BetterList<ItemStruct> _curDisplayAwardList;




        public AwardView.AssessLevel AssessLvl { get; set; }

        /// <summary>
        /// 当前奖励物品
        /// </summary>
        public BetterList<ItemStruct> CurAwardItemList
        {
            get { return _curAwardItemList; }
            set { _curAwardItemList = value; }
        }

        /// <summary>
        /// 当前用来显示的奖励物品列表
        /// </summary>
        public BetterList<ItemStruct> CurDisplayAwardList
        {
            get { return _curDisplayAwardList; }
            set { _curDisplayAwardList = value; }
        }
        
        public AwardVo()
        {
            _curAwardItemList = new BetterList<ItemStruct>();
            _curDisplayAwardList = new BetterList<ItemStruct>();
            
            
        }
    }
}