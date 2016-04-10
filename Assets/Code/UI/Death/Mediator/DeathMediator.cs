using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;


namespace mediator
{
    public class DeathMediator:ViewMediator
    {
        public DeathMediator(DeathView view, uint id = MediatorName.DEATH_MEDIATOR)
            : base(id, view)
        { 
        
        }

        public override System.Collections.Generic.IList<uint> listReferNotification()
        {
            return new System.Collections.Generic.List<uint>
            {
                MsgConstant.MSG_DEATH_TO_CITY,
                MsgConstant.MSG_DEATH_REVIVE,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_DEATH_TO_CITY:
                        DeathManager.Instance.ToCity();
                        break;
                    case MsgConstant.MSG_DEATH_REVIVE:
                        DeathManager.Instance.Revival();
                        break;
                    default:
                        break;
                }
            }
        }


        public DeathView View
        {
            get {
                if (base._viewComponent!=null&&base._viewComponent is DeathView)
                {
                    return base._viewComponent as DeathView;
                }
                return null;
            }
        }
    }
}
