using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using System;

namespace manager
{
    public class PowerManager
    {
        private PowerVo _vo;
        private PowerManager()
        {
            _vo = new PowerVo();
            _isFristGet = false;
        }


        /// <summary>
        /// 数据源
        /// </summary>
        public PowerVo Vo
        {
            get { return _vo; }
            set { _vo = value; }
        }

        private bool _isShowPower;  //是否显示战斗力

        private bool _isFristGet;

        public bool IsFristGet
        {
            get { return _isFristGet; }
            set { _isFristGet = value; }
        }
        public bool IsShowPower
        {
            get { return _isShowPower; }
            set { _isShowPower = value; }
        }

        public void OpenWindow()
        {
            int power = CharacterPlayer.character_property.getFightPower();

            _isShowPower = Vo.PowerAttack != power;

            if ((Vo.PowerAttack != power && Vo.AttibuteList.Count!=0)
                || Vo.AttibuteList.Count != 0)
            {
                if (_isShowPower)
                lock (Vo.PowerList)
                {
                    Vo.PowerList.Enqueue(new PowerNum
                    {
                        OldPower = Vo.PowerAttack,
                        CurPower = power,
                        IsAddition = Vo.PowerAttack > power ? false : true
                    });
                }
                if (!UIManager.Instance.isWindowOpen(UiNameConst.ui_power))
                {
                    UIManager.Instance.openWindow(UiNameConst.ui_power);   
                }
                Gate.instance.sendNotification(MsgConstant.MSG_POWER_SHOW);
            }
            Vo.PowerAttack = power;
        }

        public void OpenResourceWindow()
        {
            if (CheckIsHaveAttribute())
            {
                _isShowPower = true;
            }
            else {
                _isShowPower = false;
            }
            if (Vo.AttibuteList.Count != 0)
            {

                if (!UIManager.Instance.isWindowOpen(UiNameConst.ui_power))
                {
                    UIManager.Instance.openWindow(UiNameConst.ui_power);
                }
                Gate.instance.sendNotification(MsgConstant.MSG_POWER_SHOW);
            }
        }

        public bool CheckIsHaveAttribute()
        {
            foreach (PowerAttribute pow in Vo.AttibuteList)
            {
                if (pow.IsAttribute)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加属性显示
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void PushAttribute( eFighintPropertyCate type,int value)
        {
            if (value==0)
            {
                return;
            }
            lock (Vo.AttibuteList)
            {
                Vo.AttibuteList.Enqueue(new PowerAttribute() {
                 Attribute=type,
                  Num=value,
                 IsAddition=value<0?false:true,
                  IsAttribute=true
                });
            }
            _isShowPower = true;
        }
        /// <summary>
        /// 添加资源显示
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void PushResources(eGoldType type, int value)
        {
            if (value == 0)
            {
                return;
            }
            lock (Vo.AttibuteList)
            {
                Vo.AttibuteList.Enqueue(new PowerAttribute()
                {
                    Gold = type,
                    Num = value,
                    IsAddition = value < 0 ? false : true,
                    IsAttribute = false
                });
            }
        }

        public void CloseWindow()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_power);
        }

        #region 单例
        private static PowerManager _instance;
        public static PowerManager Instance
        {
            get
            {
                if (_instance == null) _instance = new PowerManager();
                return _instance;
            }
        }
        #endregion


        public string ChangeInfoData(eFighintPropertyCate vo, int value)
        {
            float info = 0.0f;
            float dataInfo = ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(9001010).type7Data * 100;
            switch (vo)
            {
                case eFighintPropertyCate.eFPC_Precise:
                case eFighintPropertyCate.eFPC_Dodge:
                case eFighintPropertyCate.eFPC_BlastAttack:
                case eFighintPropertyCate.eFPC_Tenacity:
                case eFighintPropertyCate.eFPC_AntiFightBreak:
                case eFighintPropertyCate.eFPC_FightBreak:
                case eFighintPropertyCate.eFPC_BlastAttackAdd:
                case eFighintPropertyCate.eFPC_BlastAttackReduce:
                    info = value * dataInfo;
                    string s = string.Format("{0:F2}", Convert.ToDouble(info));
                    return s+"%";
                    break;
                default:
                    return value.ToString();
                    break;
            }
        }
    }
}

