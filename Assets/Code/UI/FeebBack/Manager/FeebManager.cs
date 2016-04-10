using UnityEngine;
using System.Collections;
using helper;
using model;
using NetGame;
using MVC.entrance.gate;

namespace manager
{

    public class FeebManager
    {
        bool _request;

        
        ItemTemplate _checkItem;
        ShopVo _itemVo;
        int _buyCount;
        FeedBack _waitOpenType;
        GCAskBuyShopItem _askBuyItem;
        BetterList<FastOpenVo> _fastList;       //快捷提示

        
        public FeebManager()
        {
            _askBuyItem = new GCAskBuyShopItem();
            _fastList = new BetterList<FastOpenVo>();
            _buyCount = 1;
            _request = false;
        }
        /// <summary>
        /// 检查物品是否足够，如果不足够返回false，并开启提示功能
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="needCount">个数</param>
        /// <returns></returns>
        public bool CheckIsHave(uint id,int needCount)
        {
            _buyCount = 1;
            _checkItem = ItemManager.GetInstance().GetTemplateByTempId(id);
            uint haveCount=ItemManager.GetInstance().GetItemNumById(id);
            if (haveCount>=needCount)
            {
                return true;
            }
            switch (_checkItem.feedType)
            {
                case FeedBack.None:
                    ViewHelper.DisplayMessage("物品表，提示类型错误");
                    break;
                case FeedBack.Text:
                    ViewHelper.DisplayMessage(string.Format("{0} [ff0000]不足![-]", _checkItem.name));
                    break;
                case FeedBack.DoubleDialog:
                    _waitOpenType = FeedBack.DoubleDialog;
                    FindFastDesList(_checkItem);
                    CheckShopDataAndOpen();
                    break;
                case FeedBack.QuickBuy:
                    _waitOpenType = FeedBack.QuickBuy;
                    CheckShopDataAndOpen();
                    break;
                case FeedBack.FindInfo:
                    _waitOpenType = FeedBack.FindInfo;
                    FindFastDesList(_checkItem);
                    OpenWindow();
                    break;
                default:
                    break;
            }
            return false;
        }

        //查找当前物品的快捷提示
        public void FindFastDesList(ItemTemplate item)
        {
            _fastList.Clear();
            FastOpenVo vo;
            for (int i = 0; i < item.feedBackFunctionId.size; i++)
            {
                int id = item.feedBackFunctionId[i];
                vo= FastOpenManager.Instance.FastOpenHash[id] as FastOpenVo;
                _fastList.Add(vo);
            }
        }

        public void CheckShopDataAndOpen()
        {
            if (ShopManager.Instance.ShopHash.Count <= 0)
            {
                _request = true;
                NetBase.GetInstance().Send(new GCAskShopTemp().ToBytes(), true);
            }
            else {
                OpenWindow();
            }
        }

        public void OpenWindow()
        {
            _itemVo = ShopManager.Instance.FindVoByShopId((int)_checkItem.feedBackShopId);
            _request = false;
            UIManager.Instance.openWindow(UiNameConst.ui_quickbuy);
            Gate.instance.sendNotification(MsgConstant.MSG_FEEB_DISPLAY_TABLE, _waitOpenType);
        }
       

        public void CloseWindow()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_quickbuy);
            ResultCallback();
        }

        /// <summary>
        /// 窗口关闭回调
        /// </summary>
        public void ResultCallback()
        {
            Gate.instance.sendNotification(MsgConstant.MSG_STRENGTHEN_DISPLAT_INFO);
            Gate.instance.sendNotification(MsgConstant.MSG_ADVANCED_DISPLAT_INFO);
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAT_INFO);
            Gate.instance.sendNotification(MsgConstant.MSG_SHOW_EVOLUTION);
            Gate.instance.sendNotification(MsgConstant.MSG_WING_SET_CAMERA, true);
			Gate.instance.sendNotification(MsgConstant.MSG_PET_SHOW_EVOLUTION);
			Gate.instance.sendNotification(MsgConstant.MSG_PET_SET_CAMERA, true);
			MonsterRewardManager.Instance.MonsterRewardShow ();
            MergeManager.Instance.UpdateGem();
            FormulaManager.Instance.UpdateGem();
            Gate.instance.sendNotification(MsgConstant.MSG_REFINE_DISPLAY_RESET_LIST_CONSUME);
            Gate.instance.sendNotification(MsgConstant.MSG_SKILL_CALLBACK_DISPLAY_INFO);
        }

        //设置购买个数
        public void SetBuyCount(int count)
        {
            if (count <= 0)
                count = 1;
            if (count > 99)
                count = 99;
            _buyCount = count;
            Gate.instance.sendNotification(MsgConstant.MSG_FEEB_SHOW_SUM_PRICE);
        }

        //购买物品
        public void SendBuyItem()
        {
            int needPrice = _buyCount * _itemVo.SellPrice;
            if (!ViewHelper.CheckIsHava(_itemVo.SellMoneyType,needPrice))
            {
                return;
            }
            _askBuyItem.m_unShopItemID = (uint)_itemVo.Id;
            _askBuyItem.u16ShopNum = (ushort)_buyCount;
            NetBase.GetInstance().Send(_askBuyItem.ToBytes());
        }


        public void FastOpen(int id)
        {
            if (id<FastList.size)
            {
                FastOpenManager.Instance.OpenWindow(FastList[id].Id, true);
            }
        }
        
        #region 单例
        private static FeebManager _instance;
        public static FeebManager Instance
        {
            get
            {
                if (_instance == null) _instance = new FeebManager();
                return _instance;
            }
        }
        #endregion

        public ShopVo ItemVo
        {
            get { return _itemVo; }
            set { _itemVo = value; }
        }
        public ItemTemplate CheckItem
        {
            get { return _checkItem; }
            set { _checkItem = value; }
        }

        public int BuyCount
        {
            get { return _buyCount; }
            set { _buyCount = value; }
        }
        public bool Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public BetterList<FastOpenVo> FastList
        {
            get { return _fastList; }
            set { _fastList = value; }
        }
    }
}
