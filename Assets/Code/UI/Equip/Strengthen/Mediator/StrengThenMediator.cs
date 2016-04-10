using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class StrengThenMediator:ViewMediator
    {
        public StrengThenMediator(StrengThenView view,uint id=MediatorName.STRENGTHEN_MEDIATOR):base(id,view)
        {

        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_STRENGTHEN_DISPLAY_LIST_TABLE,
                MsgConstant.MSG_STRENGTHEN_SELECT_ITEM,
                MsgConstant.MSG_STRENGTHEN_DISPLAT_INFO,
                MsgConstant.MSG_STRENGTHEN_SELECT_INDEX,
                MsgConstant.MSG_STRENGTHEN_LUCKSTONE_CALLBACK,
                MsgConstant.MSG_STRENGTHEN_ASK_ST,
                MsgConstant.MSG_STRENGTHEN_ASK_CALLBACK_RESULT,
                MsgConstant.MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST,
                MsgConstant.MSG_STRENGTHEN_ENFORCE_EFFECT,
            };
        }

        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View != null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_STRENGTHEN_DISPLAY_LIST_TABLE:
                        View.DisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_STRENGTHEN_SELECT_ITEM:
                        StrengThenManager.Instance.SelectStrengThenItem((int)notification.body);
                        //View.DisplayEffect((int)notification.body);
                        break;
                    case MsgConstant.MSG_STRENGTHEN_DISPLAT_INFO:
                        View.DisplayInfo();
                        break;
                    case MsgConstant.MSG_STRENGTHEN_SELECT_INDEX:
                        View.SelectIndexItem();
                        break;
                    case MsgConstant.MSG_STRENGTHEN_LUCKSTONE_CALLBACK:
                        StrengThenManager.Instance.LuckStoneCallback();
                        break;
                    case MsgConstant.MSG_STRENGTHEN_ASK_ST:
                        StrengThenManager.Instance.AskStrengthenItem();
                        break;
                    case MsgConstant.MSG_STRENGTHEN_ASK_CALLBACK_RESULT:
                        if ((bool)notification.body)
                        {
                            StrengThenManager.Instance.StrengthemResultCallBack();
                        }
                        else {
                            StrengThenManager.Instance.StrengthemResultLose();
                        }
                        
                        break;
                    case MsgConstant.MSG_STRENGTHEN_ENFORCE_DISPLAY_LIST:
                        View.EnforceDisplayList((Table)notification.body);
                        break;
                    case MsgConstant.MSG_STRENGTHEN_ENFORCE_EFFECT:
                        View.DisplayEffect();
                        break;
                    default:
                        break;
                }
            }
        }

        public StrengThenView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is StrengThenView)
                {
                    return base._viewComponent as StrengThenView;
                }
                return null;
            }
        }
    }
}
