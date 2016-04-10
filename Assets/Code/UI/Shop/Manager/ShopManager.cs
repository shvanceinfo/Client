using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using NetGame;
using System;

namespace manager
{
    public class ShopManager
    {

        static ShopManager _instance;
        private Hashtable _shopHash;        //商城表的所有字段(暂时没用)

        private Hashtable _shopTypeHash;   //商品分类表,根据服务器发来的物品进行显示

        private SellShopType _selectTable;  //当前选择的table

        private ShopVo _selectItem;         //当前选择的物品

       
        private int _selectItemBuyCount;    //当前选择的物品购买数量

        private GCAskShopTemp _aksShopInfo; //请求商城数据
        private GCAskBuyShopItem _askBuyItem;
        private ShopManager()
        {
            _askBuyItem = new GCAskBuyShopItem();
            _aksShopInfo = new GCAskShopTemp();
            _selectTable = SellShopType.HotSell;
            _shopHash = new Hashtable();
            _shopTypeHash = new Hashtable();

            int start=(int)SellShopType.HotSell;
            int end=(int)SellShopType.Shuijing;
            for (int i = start; i <= end; i++)
            {
                _shopTypeHash.Add((SellShopType)i,new BetterList<ShopVo>());
            }
        }

        private bool IsExitsItemInTables(ShopVo vo)
        {
            BetterList<ShopVo> items = _shopTypeHash[vo.Table] as BetterList<ShopVo>;
            for (int i = 0; i < items.size; i++)
            {
                if (items[i].Id==vo.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private ShopVo FindShopVo(int id)
        {
            BetterList<ShopVo> sps = _shopTypeHash[_selectTable] as BetterList<ShopVo>;
            for (int i = 0; i < sps.size; i++)
            {
                if (sps[i].Id == id)
                {
                    return sps[i];
                }
            }
            return null;
        }

        public void TabelSwting(SellShopType table)
        {
            _selectTable = table;
            Gate.instance.sendNotification(MsgConstant.MSG_SHOP_DISPLAY_TABLE, _selectTable);
        }

        /// <summary>
        /// 添加服务器过来的数据
        /// </summary>
        /// <param name="vo"></param>
        public void AddShopItem(ShopVo vo)
        {
            _shopHash.Add(vo.Id, vo);

            BetterList<ShopVo> items = _shopTypeHash[vo.Table] as BetterList<ShopVo>;

            if (IsExitsItemInTables(vo))
            {
                for (int i = 0; i < items.size; i++)
                {
                    if (items[i].Id == vo.Id)
                    {
                        items[i] = vo;
                    }
                }
            }
            else {
                items.Add(vo);
            }
        }

        //设置当前要购买的物品
        public void SetBuyItem(int id)
        {
            _selectItem= FindShopVo(id);
            _selectItemBuyCount = 1;
            Gate.instance.sendNotification(MsgConstant.MSG_SHOP_DISPLAY_BUY_INFO);
        }
        //设置当前购买数量
        public void SetBuyCount(int count)
        {
            if (count <= 0)
                count = 1;
            if (count >= 100)
                count = 99;
            _selectItemBuyCount = count;
            Gate.instance.sendNotification(MsgConstant.MSG_SHOP_DISPLAY_BUY_COUNT);
        }

        public void BuyItemOption(bool isBuy)
        {

            if (isBuy)
            {
                bool isHave=helper.ViewHelper.CheckIsHava(_selectItem.SellMoneyType, _selectItemBuyCount * _selectItem.SellPrice);
                if (isHave||_selectItem.RmbPrice>0)
                {
                    _askBuyItem.m_unShopItemID = (uint)_selectItem.Id;
                    _askBuyItem.u16ShopNum = (ushort)_selectItemBuyCount;
                    NetBase.GetInstance().Send(_askBuyItem.ToBytes());
                    //helper.ViewHelper.DisplayMessage(LanguageManager.GetText("msg_buy_item_success"));
					Gate.instance.sendNotification(MsgConstant.MSG_SHOP_BUY_PANEL_OPTION, false);
                }

            }
        }

        //向服务器请求数据
        public void AskShopData(SellShopType table = SellShopType.HotSell)
        {
            if ((_shopTypeHash[_selectTable] as BetterList<ShopVo>).size <= 0)
            {
                NetBase.GetInstance().Send(_aksShopInfo.ToBytes(), true);
            }
            else {
                OpenShopWindow(table);
            }
            
        }
        
        //打开商城界面
        public void OpenShopWindow(SellShopType table = SellShopType.HotSell)
        {
            UIManager.Instance.openWindow(UiNameConst.ui_shop);
            _selectTable = table;
            TabelSwting(_selectTable);
        }



        public void AddRMB()
        { 
            
        }

        public BetterList<ShopVo> GetTableLvl(SellShopType table)
        {
             BetterList<ShopVo> bs=ShopTypeHash[table] as BetterList<ShopVo>;
             bs.Sort(SortId);
             return bs;
        }

        private int SortId(ShopVo v1, ShopVo v2)
        {
            if (v1.DisplayId>v2.DisplayId)
            {
                return 1;
            }
            return -1;
        }


        public ShopVo FindVoByShopId(int id)
        {
            return _shopHash[id] as ShopVo;
        }



















        public static ShopManager Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new ShopManager();
                }
                return ShopManager._instance;
            }
        }
        public Hashtable ShopTypeHash
        {
            get { return _shopTypeHash; }
        }
        public Hashtable ShopHash
        {
            get { return _shopHash; }
        }
        public ShopVo SelectItem
        {
            get { return _selectItem; }
        }
        public int SelectItemBuyCount
        {
            get { return _selectItemBuyCount; }
        }
    }
}
