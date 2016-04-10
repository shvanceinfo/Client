using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class PowerMediator:ViewMediator
    {
        public PowerMediator(PowerView view,uint id=MediatorName.POWER_MEDIATOR):base(id,view)
        {

        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
            MsgConstant.MSG_POWER_SHOW,
            MsgConstant.MSG_POWER_CLOSE,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_POWER_SHOW:
                        View.DisplayPower();
                        break;
                    case MsgConstant.MSG_POWER_CLOSE:
                        PowerManager.Instance.CloseWindow();
                        break;

                    default:
                        break;
                }
            }
        }



        public PowerView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is PowerView)
                {
                    return base._viewComponent as PowerView;
                }
                return null;
            }
        }

    }

}
