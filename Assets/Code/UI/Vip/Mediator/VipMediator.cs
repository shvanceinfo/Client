using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
namespace mediator
{
    public class VipMediator:ViewMediator
    {
        private VipView _view;
        public VipMediator(VipView view, uint id = MediatorName.VIP_MEDIATOR):base(id,view)
        {
            _view = view;
        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_VIP_DISPLAY_TABLE,
                MsgConstant.MSG_VIP_SWTING_VIP_SHOW,
                MsgConstant.MSG_VIP_SWTING_TABLES,
                MsgConstant.MSG_VIP_RECEIVE_AWARD,
                MsgConstant.MSG_VIP_SHOW_TIP,
                MsgConstant.MSG_VIP_HID_TIP,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (_view!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_VIP_SWTING_TABLES:
                        VipManager.Instance.SwtingTable((Table)notification.body);
                        break;
                    case MsgConstant.MSG_VIP_DISPLAY_TABLE:
                        _view.DisplayTable((Table)notification.body);
                        break;
                    case MsgConstant.MSG_VIP_SWTING_VIP_SHOW:
                        if (VipManager.Instance.SetShowVip((int)notification.body))
                        {
                            _view.DisplayInfo(true);    //显示table1
                        }
                        break;
                    case MsgConstant.MSG_VIP_RECEIVE_AWARD:
                        VipManager.Instance.ReveiveAward();
                        break;
                    case MsgConstant.MSG_VIP_SHOW_TIP:
                        _view.DisplayTip((int)notification.body);
                        break;
                    case MsgConstant.MSG_VIP_HID_TIP:
                        _view.HiddenAllTip();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
