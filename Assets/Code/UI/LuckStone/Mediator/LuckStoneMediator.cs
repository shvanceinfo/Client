using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class LuckStoneMediator:ViewMediator
    {

        public LuckStoneMediator(LuckStoneView view, uint id = MediatorName.LUCKSTONE_MEDIATOR)
            : base(id,view)
        { }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_LUCKSTONE_SELECT_STONE,
                MsgConstant.MSG_LUCKSTONE_WINDOW_OPTION
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_LUCKSTONE_SELECT_STONE:
                        LuckStoneManager.Instance.SetSelectStone((int)notification.body);
                        break;
                    case MsgConstant.MSG_LUCKSTONE_WINDOW_OPTION:
                        bool opt = (bool)notification.body;
                        if (opt)
                        {
                            LuckStoneManager.Instance.SureButton();
                        }else{
                            LuckStoneManager.Instance.CloseWindow();
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        public LuckStoneView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is LuckStoneView)
                {
                    return base._viewComponent as LuckStoneView;
                }
                return null;
            }
        }
    }
}
