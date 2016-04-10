using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class GuideMediator : ViewMediator
    {
        private GuideView _view;
        public GuideMediator(GuideView view, uint id = MediatorName.GUIDE_MEDIATOR):base(id,view)
        {
            _view = view;
        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> 
            {
                MsgConstant.MSG_GUIDE_SEND_TRIGGER,
                MsgConstant.MSG_GUIDE_DISPLAY,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (_view!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_GUIDE_SEND_TRIGGER:
                        GuideManager.Instance.PushTrigger((Trigger)notification.body);
                        break;
                    case MsgConstant.MSG_GUIDE_DISPLAY:

                        break;
                    default:
                        break;
                }
            }
        }
    }

}
