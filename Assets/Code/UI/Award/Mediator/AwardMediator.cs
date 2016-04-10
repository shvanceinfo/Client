using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MVC.entrance.gate;
using manager;


namespace mediator
{

    public class AwardMediator:ViewMediator
    {


        public AwardMediator(AwardView view,uint id=MediatorName.AWARD_MEDIATOR):base(id,view)
        { }

        
        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_AWARD_INITIAL_DATA,
                MsgConstant.MSG_AWARD_GO_HOME,
                MsgConstant.MSG_AWARD_DISPALY_INFO,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_AWARD_INITIAL_DATA:
                        AwardManager.Instance.InitialAwardInfo();
                        break;
                    case MsgConstant.MSG_AWARD_GO_HOME:
                        AwardManager.Instance.SendGoHome();
                        break;
                    case MsgConstant.MSG_AWARD_DISPALY_INFO:
                        View.StartDisplayInfo();
                        break;
                    default:
                        break;
                }
            }
        }


        public AwardView View
        {
            get {
                if (base._viewComponent!=null&&base._viewComponent is AwardView)
                {
                    return _viewComponent as AwardView;
                }
                return null;
            }
        }
    }

}