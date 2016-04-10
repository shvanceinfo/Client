using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace model
{
    public class PowerVo
    {
        private int _powerAttack;


        /// <summary>
        /// 攻击力
        /// </summary>
        public int PowerAttack
        {
            get { return _powerAttack; }
            set {
                _powerAttack = value;
            
            }
        }

        private Queue<PowerNum> _powerList;


        private Queue<PowerAttribute> _attibuteList;

        /// <summary>
        /// 队列属性集合
        /// </summary>
        public Queue<PowerNum> PowerList
        {
            get { return _powerList; }
            set { _powerList = value; }
        }

        /// <summary>
        /// 文字属性集合
        /// </summary>
        public Queue<PowerAttribute> AttibuteList
        {
            get { return _attibuteList; }
            set { _attibuteList = value; }
        }

        public PowerVo()
        {
            _powerAttack = 0;
            _powerList = new Queue<PowerNum>();
            _attibuteList = new Queue<PowerAttribute>();
        }
    }

    

    /// <summary>
    /// 战斗力数字
    /// </summary>
    public class PowerNum
    {
        public int OldPower { get; set; }

        public int CurPower { get; set; }

        public bool IsAddition { get; set; }
    }
    /// <summary>
    /// 战斗力属性文字
    /// </summary>
    public class PowerAttribute
    {
        /// <summary>
        /// 属性/资产
        /// </summary>
        public bool IsAttribute { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public eFighintPropertyCate Attribute { get; set; }

        public eGoldType Gold { get; set; }

        public int Num { get; set; }

        public bool IsAddition { get; set; }
    }
}

