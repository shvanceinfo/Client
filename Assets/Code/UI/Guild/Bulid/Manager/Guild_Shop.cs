using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using NetGame;
using System;

namespace manager
{
    /// <summary>
    /// 公会商城
    /// </summary>
    public partial class GuildManager
    {

        private SellShopType _shopSelectTable;  //当前选择的table

        private GuildShopVo _shopSelectItem;         //当前选择的物品


        private int _shopSelectItemBuyCount;    //当前选择的物品购买数量

        private GCAskShopTemp _shopAksShopInfo; //请求商城数据
        private GCAskBuyShopItem _shopAskBuyItem;

        void InitialShop()
        {
            _shopAskBuyItem = new GCAskBuyShopItem();
            _shopAksShopInfo = new GCAskShopTemp();
            _shopSelectTable = SellShopType.HotSell;

            _xmlGuildShop.Add(SellShopType.HotSell, new BetterList<GuildShopVo>());
        }


        private bool ShopIsExitsItemInTables(GuildShopVo vo)
        {
            BetterList<GuildShopVo> items = _xmlGuildShop[vo.ShopType] as BetterList<GuildShopVo>;
            for (int i = 0; i < items.size; i++)
            {
                if (items[i].Id == vo.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private GuildShopVo ShopFindShopVo(int id)
        {
            BetterList<GuildShopVo> gsv = _xmlGuildShop[_shopSelectTable] as BetterList<GuildShopVo>;
            for (int i = 0; i < gsv.size; i++)
            {
                if (gsv[i].Id == id)
                {
                    return gsv[i];
                }
            }
            return null;
        }

        public void ShopTabelSwting(SellShopType table)
        {
            _shopSelectTable = table;
            Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SHOP_DISPLAYTABLE, _shopSelectTable);
        }


        //设置当前要购买的物品
        public void ShopSetBuyItem(int id)
        {
            _shopSelectItem = ShopFindShopVo(id);
            _shopSelectItemBuyCount = 1;
            Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_DISPLAY_BUYINFO);
        }
        //设置当前购买数量
        public void ShopSetBuyCount(int count)
        {
            if (count <= 0)
                count = 1;
            if (count >= 100)
                count = 99;
            _shopSelectItemBuyCount = count;
            Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYCOUNT);
        }
        

        public void ShopBuyItemOption(bool isBuy)
        {
            if (isBuy)
            {
                bool isHave = helper.ViewHelper.CheckIsHava(_shopSelectItem.SellMoneyType, _shopSelectItemBuyCount * _shopSelectItem.SellPrice);
                if (isHave || _shopSelectItem.ResourceNum > 0)
                {
                    _shopAskBuyItem.m_unShopItemID = (uint)_shopSelectItem.Id;
                    _shopAskBuyItem.u16ShopNum = (ushort)_shopSelectItemBuyCount;
                    NetBase.GetInstance().Send(_shopAskBuyItem.ToBytes());
                    helper.ViewHelper.DisplayMessage(LanguageManager.GetText("msg_buy_item_success"));
                    Gate.instance.sendNotification(MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYPANEL_OPTION, false);
                }

            }
        }

        //打开商城界面
        public void ShopOpenShopWindow()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_shop);
            _shopSelectTable = SellShopType.HotSell;
            ShopTabelSwting(_shopSelectTable);
        }


        public void ShopAddRMB()
        {

        }

        public BetterList<GuildShopVo> ShopGetTableLvl(SellShopType table)
        {
            BetterList<GuildShopVo> bs = XmlGuildShop[table] as BetterList<GuildShopVo>;
            bs.Sort(ShopSortId);
            return bs;
        }

        private int ShopSortId(GuildShopVo v1, GuildShopVo v2)
        {
            if (v1.DisplayId > v2.DisplayId)
            {
                return 1;
            }
            return -1;
        }


        public GuildShopVo ShopFindVoByShopId(int id)
        {
            return _xmlGuildShop[id] as GuildShopVo;
        }

        public GuildShopVo ShopSelectItem
        {
            get { return _shopSelectItem; }
        }
        public int ShopSelectItemBuyCount
        {
            get { return _shopSelectItemBuyCount; }
        }
    }
}
