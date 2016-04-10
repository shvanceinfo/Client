using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class RefineMediator:ViewMediator
    {
        public RefineMediator(RefineView view,uint id=MediatorName.REFINE_MEDIATOR):base(id,view)
        {

        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> 
            {
                MsgConstant.MSG_REFINE_DISPLAY_LIST_TABLE,
                MsgConstant.MSG_REFINE_SELECT_ITEM,
                MsgConstant.MSG_REFINE_DISPLAT_INFO,
                MsgConstant.MSG_REFINE_SEND_REFINE,
                MsgConstant.MSG_REFINE_RESET,
                MsgConstant.MSG_REFINE_SELECT_RESET_ITEM,
                MsgConstant.MSG_REFINE_DISPLAY_SELECT_VO,
                MsgConstant.MSG_REFINE_RESET_OK,
                MsgConstant.MSG_REFINE_DIALOG_CALLBACK,
                MsgConstant.MSG_REFINE_ENFORCE_DISPLAY_LIST,
                MsgConstant.MSG_REFINE_DISPLAY_RESET_LIST_CONSUME,
                MsgConstant.MSG_REFINE_EFFECT_INFO,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_REFINE_DISPLAY_LIST_TABLE:
                        View.DisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_REFINE_SELECT_ITEM:
                        RefineManager.Instance.SelectItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_REFINE_DISPLAT_INFO:
                        View.DisplayInfo();
                        break;
                    case MsgConstant.MSG_REFINE_SEND_REFINE:
                        RefineManager.Instance.SendRefine((int)notification.body);
                        break;
                    case MsgConstant.MSG_REFINE_RESET:
                        bool b = (bool)notification.body;
                        if (b) RefineManager.Instance.ClaerResetList();
                        View.DisplayReset(b);
                        break;
                    case MsgConstant.MSG_REFINE_SELECT_RESET_ITEM:
                        RefineManager.Instance.SelectResetItem((int)notification.body);
                        View.DisplayResetConsume();
                        break;
                    case MsgConstant.MSG_REFINE_DISPLAY_SELECT_VO:
                        RefineManager.Instance.UpdateSelectVoInfo((bool)notification.body);
                        break;
                    case MsgConstant.MSG_REFINE_RESET_OK:
                        RefineManager.Instance.SendResetRefine();
                        break;
                    case MsgConstant.MSG_REFINE_DIALOG_CALLBACK:
                        RefineManager.Instance.SendResetNet();
                        break;
                    case MsgConstant.MSG_REFINE_ENFORCE_DISPLAY_LIST:
                        View.EnforceDisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_REFINE_DISPLAY_RESET_LIST_CONSUME:
                        View.DisplayResetConsume();
                        break;
                    case MsgConstant.MSG_REFINE_EFFECT_INFO:
                        View.DisplayEffect((int)notification.body);
                        break;
                    default:
                        break;
                }
            }
        }
        public RefineView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is RefineView)
                {
                    return base._viewComponent as RefineView;
                }
                return null;
            }
        }
    }

}