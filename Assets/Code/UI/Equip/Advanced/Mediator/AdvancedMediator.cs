using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;


namespace mediator
{
    public class AdvancedMediator:ViewMediator
    {
        public AdvancedMediator(AdvancedView view, uint id = MediatorName.ADVANCED_MEDIATOR):base(id,view)
        {

        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_ADVANCED_DISPLAY_LIST_TABLE,
                MsgConstant.MSG_ADVANCED_SELECT_ITEM,
                MsgConstant.MSG_ADVANCED_DISPLAT_INFO,
                MsgConstant.MSG_ADVANCED_SELECT_INDEX,
                MsgConstant.MSG_ADVANCED_LUCKSTONE_CALLBACK,
                MsgConstant.MSG_ADVANCED_ASK_ST,
                MsgConstant.MSG_ADVANCED_ASK_CALLBACK_RESULT,
                MsgConstant.MSG_ADVANCED_ENFORCE_DISPLAY_LIST,
                MsgConstant.MSG_ADVANCED_EFFECT_INFO,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_ADVANCED_DISPLAY_LIST_TABLE:
                        View.DisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_ADVANCED_SELECT_ITEM:
                        AdvancedManager.Instance.SelectAdvancedItem((int)notification.body);
                        break;
                    case MsgConstant.MSG_ADVANCED_DISPLAT_INFO:
                        View.DisplayInfo();
                        break;
                    case MsgConstant.MSG_ADVANCED_SELECT_INDEX:
                        View.SelectIndexItem();
                        break;
                    case MsgConstant.MSG_ADVANCED_LUCKSTONE_CALLBACK:
                        AdvancedManager.Instance.LuckStoneCallback();
                        break;
                    case MsgConstant.MSG_ADVANCED_ASK_ST:
                        AdvancedManager.Instance.AskAdvancedItem();
                        break;
                    case MsgConstant.MSG_ADVANCED_ASK_CALLBACK_RESULT:
                        if ((bool)notification.body)
                        {
                            AdvancedManager.Instance.AdvancedCallBack();
                        }
                        else { 
                            AdvancedManager.Instance.AdvancedResultLose();
                        }
                        
                        break;
                    case MsgConstant.MSG_ADVANCED_ENFORCE_DISPLAY_LIST:
                        View.EnforceDisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_ADVANCED_EFFECT_INFO:
                        View.DisplayEffect();
                        break;
                    default:
                        break;
                }
            }
        }

        public AdvancedView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is AdvancedView)
                {
                    return base._viewComponent as AdvancedView;
                }
                return null;
            }
        }
    }
}
