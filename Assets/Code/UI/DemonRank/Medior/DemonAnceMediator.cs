using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using System.Collections.Generic;
namespace mediator
{
    public class DemonAnceMediator:ViewMediator
    {
        public DemonAnceMediator(DemonAnceView view, uint id = MediatorName.DEMON_ANCE_MEDIATOR)
            : base(id, view)
        { 
            
        }


        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
            MsgConstant.MSG_DEMON_ARCE_DISPLAY_PAGE
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_DEMON_ARCE_DISPLAY_PAGE:
                        View.DisplaySwtingPage((DemonAnceView.DemonArceType)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }


        public DemonAnceView View
        {
            get {
                if (base._viewComponent!=null&&base._viewComponent is DemonAnceView)
                {
                    return base._viewComponent as DemonAnceView;
                }
                return null;
            }
        }
    }
}