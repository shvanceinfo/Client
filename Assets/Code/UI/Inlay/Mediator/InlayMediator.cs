using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class InlayMediator:ViewMediator
    {
        public InlayMediator(InlayView view, uint id = MediatorName.INLAY_MEDIATOR):base(id,view)
        {

        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_INLAY_DISPLAY_EQUIPLIST,
                MsgConstant.MSG_INLAY_SELECT_ITEM,
                MsgConstant.MSG_INLAY_DISPLAY_INFO,
                MsgConstant.MSG_INLAY_SELECT_GEM,
                MsgConstant.MSG_INLAY_REMOVE_GEM,
                MsgConstant.MSG_INLAY_GEM_INLAY,
                MsgConstant.MSG_INLAY_EFFECT_INFO,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_INLAY_DISPLAY_EQUIPLIST:
                        View.DisplayEquipList();
                        break;
                    case MsgConstant.MSG_INLAY_SELECT_ITEM:
                        InlayManager.Instance.SelectItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_INLAY_DISPLAY_INFO:
                        InlayManager.Instance.ResetData();
                        View.DisplayInfo();
                        break;
                    case MsgConstant.MSG_INLAY_SELECT_GEM:
                        InlayManager.Instance.SelectGemItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_INLAY_REMOVE_GEM:
                        InlayManager.Instance.RemoveGem((int)notification.body);
                        break;
                    case MsgConstant.MSG_INLAY_GEM_INLAY:
                        InlayManager.Instance.InlayGem();
                        break;
                    case MsgConstant.MSG_INLAY_EFFECT_INFO:
                        View.DisplayEffect((int)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }

        public InlayView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is InlayView)
                {
                    return base._viewComponent as InlayView;
                }
                return null;
            }
        }
    }
}
