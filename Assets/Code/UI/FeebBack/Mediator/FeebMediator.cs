using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class FeebMediator:ViewMediator
    {
        public FeebMediator(FeebView view, uint id = MediatorName.FEEB_MEDIATOR):base(id,view)
        {

        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_FEEB_DISPLAY_TABLE,
                MsgConstant.MSG_FEEB_CLOSE,
                MsgConstant.MSG_FEEB_SHOW_SUM_PRICE,
                MsgConstant.MSG_FEEB_SET_COUNT,
                MsgConstant.MSG_FEEB_SHOW_TABLE,
                MsgConstant.MSG_FEEB_BUY_ITEM,
                MsgConstant.MSG_FEEB_GO_SHOP,
                MsgConstant.MSG_FEEB_FAST_OPEN,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_FEEB_DISPLAY_TABLE:
                        View.Display((FeedBack)notification.body);
                        break;
                    case MsgConstant.MSG_FEEB_CLOSE:
                        FeebManager.Instance.CloseWindow();
                        break;
                    case MsgConstant.MSG_FEEB_SHOW_SUM_PRICE:
                        View.QuickBuyCount();
                        break;
                    case MsgConstant.MSG_FEEB_SET_COUNT:
                        FeebManager.Instance.SetBuyCount((int)notification.body);
                        break;
                    case MsgConstant.MSG_FEEB_SHOW_TABLE:
                        View.DisplayTable((Table)notification.body);
                        break;
                    case MsgConstant.MSG_FEEB_BUY_ITEM:
                        FeebManager.Instance.SendBuyItem();
                        break;
                    case MsgConstant.MSG_FEEB_GO_SHOP:
                        break;
                    case MsgConstant.MSG_FEEB_FAST_OPEN:
                        FeebManager.Instance.FastOpen((int)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }

        public FeebView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is FeebView)
                {
                    return base._viewComponent as FeebView;
                }
                return null;
            }
        }
    }
}

	

