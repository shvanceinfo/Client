using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using MVC.interfaces;
using manager;
using model;

namespace mediator
{
    public class ShopMediator:ViewMediator
    {
        public ShopMediator(ShopView view, uint id = MediatorName.SHOP_MEDIATOR)
            : base(id, view)
        { 
            
        }

        public override System.Collections.Generic.IList<uint> listReferNotification()
        {
            return new System.Collections.Generic.List<uint> { 
            MsgConstant.MSG_SHOP_SET_BUY_ITEM,
            MsgConstant.MSG_SHOP_DISPLAY_BUY_INFO,
            MsgConstant.MSG_SHOP_DISPLAY_BUY_COUNT,
            MsgConstant.MSG_SHOP_SET_BUY_COUNT,
            MsgConstant.MSG_SHOP_BUY_PANEL_OPTION,
            MsgConstant.MSG_SHOP_TABLE_SWTING,
            MsgConstant.MSG_SHOP_DISPLAY_TABLE,
            MsgConstant.MSG_SHOP_DISPLAY_MONEY,
            MsgConstant.MSG_SHOP_ADD_RMB
            };
        }

        public override void handleNotification(INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_SHOP_SET_BUY_ITEM:
                        ShopManager.Instance.SetBuyItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_SHOP_DISPLAY_BUY_INFO:
                        View.DisplayBuyPanel();             //显示购买详细信息
                        break;
                    case MsgConstant.MSG_SHOP_DISPLAY_BUY_COUNT:
                        View.DisplayBuyPanelCount();        //显示购买数量
                        break;
                    case MsgConstant.MSG_SHOP_SET_BUY_COUNT:
                        ShopManager.Instance.SetBuyCount((int)notification.body);
                        break;
                    case MsgConstant.MSG_SHOP_BUY_PANEL_OPTION: //操作选项
                        bool isBuy = (bool)notification.body;
                        if (!isBuy)
                        {
                            View.HideBuyPanel();
                        }
                        ShopManager.Instance.BuyItemOption(isBuy);
                        break;
                    case MsgConstant.MSG_SHOP_TABLE_SWTING:
                        ShopManager.Instance.TabelSwting((SellShopType)notification.body);
                        break;
                    case MsgConstant.MSG_SHOP_DISPLAY_TABLE:
                        View.DisplayTable((SellShopType)notification.body);
                        break;
                    case MsgConstant.MSG_SHOP_DISPLAY_MONEY:
                        View.DisplayMoney();
                        break;
                    case  MsgConstant.MSG_SHOP_ADD_RMB:
                        ShopManager.Instance.AddRMB();
                        break;
                    default:
                        break;
                }
            }   
        }


        public ShopView View
        {
            get {
                if (base._viewComponent!=null&&base._viewComponent is ShopView)
                {
                    return base._viewComponent as ShopView;
                }
                return null;
            }
        }
    }

}