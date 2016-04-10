using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using helper;
using NetGame;
namespace manager
{
    public class MergeManager
    {
        public const int MAX_CONSUME_ITEM = 5;   //最大合成格子
        const float SUC = 10;
        const int MINCOUNT = 1;
        const int MAXCOUNT = 99;
        static MergeManager _instance;
        private Hashtable _gemHash;

        private BetterList<GemVo> _gems;

        private GemVo _selectVo;            //当前选中的宝石
        private int _selectCount;           //合成数量


        private GCAskCombineGem _ask27;     //请求合成宝石
        

        public MergeManager()
        {
            _selectCount = 1;
            _selectVo = null;
            _gems = new BetterList<GemVo>();
            _gemHash = new Hashtable();

            _ask27 = new GCAskCombineGem();
        }


        public void Initial()
        {
            LuckStoneManager.Instance.SetSelectStone(0);
            _selectVo = null;
            _selectCount = 1;
            SortGems();            
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_LIST);
            SelectItem(0);
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SELECT_INDEX_ITEM);
        }


        private void SortGems()
        {
            _gems.Clear();
            List<int> ids = new List<int>();

            foreach (int key in _gemHash.Keys)
            {
                ids.Add(key);
            }
            ids.Sort();
            foreach (int key in ids)
            {
                GemVo vo=_gemHash[key] as GemVo;

                if (vo.IsMergeGoing)    //继续合成
                {
                    //添加可以合成出的数量
                   int num= (int)ItemManager.GetInstance().GetItemNumById((uint)vo.Id);
                   if (num == 0)
                   {
                       vo.CanMergeCount = 0;
                   }else{
                       vo.CanMergeCount = (int)(num / (float)vo.MergeNum);
                       _gems.Add(vo);
                   }
                }
            }
            //根据可合成的数量，进行排序
            _gems.Sort((GemVo v1,GemVo v2) => {
                if (v1.CanMergeCount<v2.CanMergeCount)
                {
                    return 1;
                }
                return -1;
            });
        }


        /// <summary>
        /// 设置当前选中的物品
        /// </summary>
        /// <param name="id"></param>
        public void SelectItem(int id)
        {
            if (id == 0)
            {
                _selectVo = null;
            }
            else {
                _selectVo = FindItemById(id);
                if (!_selectVo.IsUseLuckStone)  //如果不能增加幸运值。赋值null
                {
                    LuckStoneManager.Instance.SetSelectStone(0);
                } 
            }
            //发送刷新详细信息
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO);
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
            else if(num>=MAXCOUNT) {
                _selectCount = MAXCOUNT;
            }
            //显示选择数量消耗
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO);
        }
        /// <summary>
        /// 发送合成
        /// </summary>
        public void SendMergeInfo()
        {
            if (!FeebManager.Instance.CheckIsHave((uint)_selectVo.Id, _selectVo.MergeNum *_selectCount)) 
            //if (!CheckIndexHave())
            {
              //  ViewHelper.DisplayMessageLanguage("gem_notenght");
                return;
            }
            if (!CheckTowHave())
            {
                ViewHelper.DisplayMessageLanguage("msg_money_not_enough");
                return;
            }
            if (LuckStoneManager.Instance.SelectedStone!=null)
            {
                if (!CheckThreeHave())
                {
                    ViewHelper.DisplayMessageLanguage("gem_luckstone_nohave");
                    return;
                } 
            }
            _ask27._un32Id = (uint)_selectVo.MergeNextId;
            _ask27._un32LuckStoneId = LuckStoneManager.Instance.SelectedStone == null ? 0 : (uint)LuckStoneManager.Instance.SelectedStone.Id;
            _ask27._un16MergeCount = (ushort)_selectCount;
            _ask27._un8Option = 0x00;   //合成宝石
            NetBase.GetInstance().Send(_ask27.ToBytes());
        }

        /// <summary>
        /// 宝石合成，成功信息
        /// </summary>
        public void MergeComplate(bool isSuccess)
        {
            SortGems();
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_LIST);
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_SELECT_INDEX_ITEM);
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO);

            if (isSuccess)
            {
                ViewHelper.DisplayMessageLanguage("gem_coplate");
            }
            else
            {
                ViewHelper.DisplayMessageLanguage("gem_fail");
            }
            
        }
        public void UpdateGem()
        {
            SortGems();
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_LIST);
            Gate.instance.sendNotification(MsgConstant.MSG_MERGE_DISPLAY_GEM_INFO);
        }


        /// <summary>
        /// 获取成功率字符串
        /// </summary>
        /// <returns></returns>
        public string GetSucess()
        {
            string str = string.Format("{0}%", (int)(_selectVo.Successrate / SUC));
            if (LuckStoneManager.Instance.SelectedStone!=null)
            {
                str += string.Format("({0}%)", (int)(LuckStoneManager.Instance.SelectedStone.Successrate / SUC));
            }
            return str;
        }

        public int GetSelectItemCount()
        {
            return GetNumById(_selectVo.Id);
        }
        /// <summary>
        /// 根据物品id查找数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetNumById(int id)
        {
            return (int)ItemManager.GetInstance().GetItemNumById((uint)id);
        }

        private GemVo FindItemById(int id)
        {
            foreach (GemVo vo in _gems)
            {
                if (vo.Id==id)
                {
                    return vo;
                }
            }
            return null;
        }



        /// <summary>
        /// 检查第一个消耗是否满足
        /// </summary>
        /// <returns></returns>
        public bool CheckIndexHave()
        {
            int needCount = _selectVo.MergeNum * _selectCount;  //需要的符石个数
            int haveCount = GetSelectItemCount();               //当前已有的数量
            if (haveCount>=needCount)
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
            int needCount = _selectVo.MergeGold * _selectCount;  //需要的符石个数
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
            int needCount = LuckStoneManager.Instance.SelectedStone.ConsumeItem[0].Value* _selectCount;                        //需要的符石个数
            int haveCount = LuckStoneManager.Instance.GetSelectCount();               //当前已有的数量
            if (haveCount >= needCount)
            {
                return true;
            }
            else { 
                //符石不足，计算钻石
                needCount = LuckStoneManager.Instance.SelectedStone.ConsumeDiamond*_selectCount;
                haveCount = CharacterPlayer.character_asset.diamond;
                if (haveCount >= needCount)
                {
                    return true;
                }
            }
            return false;
        }



        public GemVo GetGemVoById(int id)
        {
            return _gemHash[id] as GemVo;
        }



        public static MergeManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new MergeManager();
                }
                return MergeManager._instance;
            }
        }

        public int CanMergeCount
        {
            get {
                if (_selectVo.CanMergeCount<=MINCOUNT)
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

        /// <summary>
        /// 选择的数量
        /// </summary>
        public int SelectCount
        {
            get { return _selectCount; }
            set { _selectCount = value; }
        }

        /// <summary>
        /// 宝石hash表
        /// </summary>
        public Hashtable GemHash
        {
            get { return _gemHash; }
            set { _gemHash = value; }
        }

        /// <summary>
        /// 排序之后的宝石
        /// </summary>
        public BetterList<GemVo> Gems
        {
            get { return _gems; }
            set { _gems = value; }
        }

        public GemVo SelectVo
        {
            get { return _selectVo; }
            set { _selectVo = value; }
        }



        /// <summary>
        /// 根据宝石ID，获取物品ID
        /// </summary>
        /// <param name="gemId"></param>
        /// <returns></returns>
        public static ItemTemplate FindItem(int gemId)
        {
            return ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(gemId);
        }
        /// <summary>
        /// 根据宝石ID获取品质边框
        /// </summary>
        /// <param name="gemId"></param>
        /// <returns></returns>
        public static string FindQualityByGemId(int gemId)
        {
            return BagManager.Instance.getItemBgByType(FindItem(gemId).quality, true);
        }
    }
}