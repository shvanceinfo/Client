using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using NetGame;
using helper;
using System.Collections.Generic;

namespace manager
{
    public class FormulaManager
    {
        public const int MAX_CONSUME_ITEM = 5;   //最大合成格子
        const float SUC = 10;
        const int MINCOUNT = 1;
        const int MAXCOUNT = 99;

        static FormulaManager _instance;

        private Hashtable _formulaHash;

        private BetterList<FormulaVo> _sortVo;

        private FormulaVo _selectVo;
        private int _selectCount;

        private GCAskCombineGem _ask27;     //请求合成宝石
        private FormulaManager()
        {
            _ask27 = new GCAskCombineGem();
            _formulaHash = new Hashtable();
            _sortVo = new BetterList<FormulaVo>();
        }


        public void Initial()
        {
            SortData();
            LuckStoneManager.Instance.SetSelectStone(0);
            _selectVo = null;
            _selectCount = 1;
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_LIST);
            SetSelectItem(0);
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_SELECT_INDEX_ITEM);
        }

        private void SortData()
        {
            _sortVo.Clear();
            List<int> ids = new List<int>();

            foreach (int key in _formulaHash.Keys)
            {
                ids.Add(key);
            }
            ids.Sort();
            foreach (int key in ids)
            {
                FormulaVo vo = _formulaHash[key] as FormulaVo;

                    //添加可以合成出的数量
                    int num = (int)ItemManager.GetInstance().GetItemNumById((uint)vo.ConsumeItem[0].Id);
                    if (num == 0)
                    {
                        vo.CanMergeCount = 0;
                    }
                    else
                    {
                        vo.CanMergeCount = (int)(num / (float)vo.ConsumeItem[0].Value);
                    }
                    _sortVo.Add(vo);
                
            }
            ////根据可合成的数量，进行排序
            _sortVo.Sort((FormulaVo vo1, FormulaVo vo2) =>
            {
                if (vo1.CanMergeCount < vo2.CanMergeCount)
                {
                    return 1;
                }
                return -1;
            });
        }



        private FormulaVo FindVoById(int id)
        {
            foreach (FormulaVo vo in _sortVo)
            {
                if (vo.Id==id)
                {
                    return vo;
                }
            }
            return null;
        }
        #region Public 
        
        public string GetSucess()
        {
            string str = string.Format("{0}%", (int)(_selectVo.Successrate / SUC));
            if (LuckStoneManager.Instance.SelectedStone != null)
            {
                str += string.Format("({0}%)", (int)(LuckStoneManager.Instance.SelectedStone.Successrate / SUC));
            }
            return str;
        }

        public void SetSelectItem(int id)
        {
            if (id == 0)
            {
                _selectVo = null;
            }
            else {
                _selectVo = FindVoById(id);
                if (!_selectVo.IsUsedLuckStone)  //如果不能增加幸运值。赋值null
                {
                    LuckStoneManager.Instance.SetSelectStone(0);
                } 
            }
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_INFO);
            SetMergeNum(MINCOUNT);
        }

        /// <summary>
        /// 设置合成数量
        /// </summary>
        /// <param name="num"></param>
        public void SetMergeNum(int num)
        {
            _selectCount = num;
            if (num <= MINCOUNT)
            {
                _selectCount = MINCOUNT;
            }
            else if (num >= MAXCOUNT)
            {
                _selectCount = MAXCOUNT;
            }
            //显示选择数量消耗
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_INFO);
        }

        public void MergeComplate(bool isSuccess)
        {
            SortData();
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_LIST);
            //Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_SELECT_INDEX_ITEM);
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_INFO);
            if (isSuccess)
            {
                ViewHelper.DisplayMessageLanguage("gem_coplate");
            }
            else {
                ViewHelper.DisplayMessageLanguage("gem_fail");
            }
            
        }
        public void UpdateGem()
        {
            SortData();
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_LIST);
            Gate.instance.sendNotification(MsgConstant.MSG_FORMULA_DISPLAY_INFO);
        }

        public void SendMergeInfo()
        {
            if(!FeebManager.Instance.CheckIsHave((uint)_selectVo.ConsumeItem[0].Id,_selectVo.ConsumeItem[0].Value * _selectCount))
            //if (!CheckIndexHave())
            {
                //ViewHelper.DisplayMessageLanguage("gem_notenght");
                return;
            }
            if (!CheckTowHave())
            {
                ViewHelper.DisplayMessageLanguage("msg_money_not_enough");
                return;
            }
            if (!CheckThreeHave())
            {
                ViewHelper.DisplayMessageLanguage("gem_luckstone_nohave");
                return;
            }

            _ask27._un32Id = (uint)_selectVo.MergeNextId;
            _ask27._un32LuckStoneId = LuckStoneManager.Instance.SelectedStone == null ? 0 : (uint)LuckStoneManager.Instance.SelectedStone.Id;
            _ask27._un16MergeCount = (ushort)_selectCount;
            _ask27._un8Option = 0x01;   //合成宝石
            NetBase.GetInstance().Send(_ask27.ToBytes());
        }


        public int GetSelectItemCount()
        {
            return (int)ItemManager.GetInstance().GetItemNumById((uint)_selectVo.ConsumeItem[0].Id);
        }
        /// <summary>
        /// 检查第一个消耗是否满足
        /// </summary>
        /// <returns></returns>
        public bool CheckIndexHave()
        {
            int needCount = _selectVo.ConsumeItem[0].Value * _selectCount;  //需要的符石个数
            int haveCount = GetSelectItemCount();               //当前已有的数量
            if (haveCount >= needCount)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查金币是否足够
        /// </summary>
        /// <returns></returns>
        public bool CheckTowHave()
        {
            int needCount = _selectVo.ConsumeGold * _selectCount;  //需要的符石个数
            int haveCount = CharacterPlayer.character_asset.gold;               //当前已有的数量
            if (haveCount >= needCount)
            {
                return true;
            }
            return false;
        }

        //检查强化石是否足够
        public bool CheckThreeHave()
        {
            if (LuckStoneManager.Instance.SelectedStone==null)
            {
                return true;
            }
            int needCount = LuckStoneManager.Instance.SelectedStone.ConsumeItem[0].Value * _selectCount;                        //需要的符石个数
            int haveCount = LuckStoneManager.Instance.GetSelectCount();               //当前已有的数量
            if (haveCount >= needCount)
            {
                return true;
            }
            else
            {
                //符石不足，计算钻石
                needCount = LuckStoneManager.Instance.SelectedStone.ConsumeDiamond * _selectCount;
                haveCount = CharacterPlayer.character_asset.diamond;
                if (haveCount >= needCount)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


        #region Value
        public BetterList<FormulaVo> SortVo
        {
            get { return _sortVo; }
        }

        public Hashtable FormulaHash
        {
            get { return _formulaHash; }
        }

        public static FormulaManager Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new FormulaManager();
                }
                return FormulaManager._instance;
            }
        }

        public FormulaVo SelectVo
        {
            get { return _selectVo; }
            set { _selectVo = value; }
        }
        public int SelectCount
        {
            get { return _selectCount; }
            set { _selectCount = value; }
        }
        public int CanMergeCount
        {
            get
            {
                if (_selectVo.CanMergeCount <= MINCOUNT)
                {
                    return MINCOUNT;
                }
                else if (_selectVo.CanMergeCount > MAXCOUNT)
                {
                    return MAXCOUNT;
                }
                return _selectVo.CanMergeCount;
            }
        }
        #endregion
        
    }
}
