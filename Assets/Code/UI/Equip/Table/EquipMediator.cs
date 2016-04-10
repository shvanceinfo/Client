using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;

namespace mediator
{
    public class EquipMediator:ViewMediator
    {
        public EquipMediator(EquipView view, uint id = MediatorName.EQUIP_MEDIATOR):base(id,view)
        {

        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_EQUIP_SWITCHING_TABLE,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_EQUIP_SWITCHING_TABLE:
                        View.SwitchingTable((Table)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }

        public EquipView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is EquipView)
                {
                    return base._viewComponent as EquipView;
                }
                return null;
            }
        }
    }
}