
using UnityEngine;
using System.Collections;

namespace model
{
    public class FightVo
    {
        private float _time;    //普通副本时间正计时
        private float _reTime;         //哥布林倒计时
        private bool _isReTime;        //是否是倒计时

        public bool IsReTime
        {
            get { return _isReTime; }
            set { _isReTime = value; }
        }
        
        

        private MonsterProperty _boss;
        private int _curHpMpItemCount;   //当前拥有血瓶个数
        private int _curBuyHpMpItemCount;//当前已经购买的血瓶个数
        private int _curExp;             //当前EXP
        private int _nextExp;            //下一级所需经验

        public int CurExp
        {
            get { return _curExp; }
            set { _curExp = value; }
        }
        public int NextExp
        {
            get { return _nextExp; }
            set { _nextExp = value; }
        }

        public int CurHpMpItemCount
        {
            get { return _curHpMpItemCount; }
            set { _curHpMpItemCount = value; }
        }
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public float ReTime
        {
            get { return _reTime; }
            set { _reTime = value; }
        }
        public int CurBuyHpMpItemCount
        {
            get { return _curBuyHpMpItemCount; }
            set { _curBuyHpMpItemCount = value; }
        }
        public MonsterProperty Boss
        {
            get { return _boss; }
            set { _boss = value; }
        }
    }
}
