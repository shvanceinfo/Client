using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using model;

namespace mediator
{
    public class GuildShopMediator : ViewMediator
    {
        public GuildShopView View { get; set; }
        public GuildShopMediator(GuildShopView view, uint id = MediatorName.GUILD_SHOP_MEDIATOR)
            : base(id, view)
        {
            this.View = view;
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_COMMON_NOTICE_SHOP,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_DISPLAYTABLE,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_SETBUYITEM,
                MsgConstant.MSG_COMMON_NOTICE_DISPLAY_BUYINFO,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYCOUNT,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYPANEL_OPTION,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_DISPLAY_MONEY,
                MsgConstant.MSG_COMMON_NOTICE_SHOP_ADD_RMB
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP_SETBUYITEM:
                        GuildManager.Instance.ShopSetBuyItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_DISPLAY_BUYINFO:
                        View.ShopDisplayBuyPanel();             //显示购买详细信息
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYCOUNT:
                        View.ShopDisplayBuyPanelCount();        //显示购买数量
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP_BUYPANEL_OPTION: //操作选项
                        bool isBuy = (bool)notification.body;
                        if (!isBuy)
                        {
                            View.ShopHideBuyPanel();
                        }
                        GuildManager.Instance.ShopBuyItemOption(isBuy);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP_DISPLAY_MONEY:
                        View.ShopDisplayMoney();
                        break;
                    case MsgConstant.MSG_SHOP_ADD_RMB:
                        GuildManager.Instance.ShopAddRMB();
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP:
                        //View.OpenWindow();
                        GuildManager.Instance.ShopTabelSwting((SellShopType)notification.body);
                        break;
                    case MsgConstant.MSG_COMMON_NOTICE_SHOP_DISPLAYTABLE:
                        View.ShopDisplayTable((SellShopType)notification.body);
                        break;
                    default:
                        break;

                }
            }
        }
    }
}
