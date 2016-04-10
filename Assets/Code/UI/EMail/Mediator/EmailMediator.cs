using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
    public class EmailMediator:ViewMediator
    {
        public EmailMediator(EmailView view, uint id = MediatorName.EMAIL_MEDIATOR):base(id,view)
        { 
        }


        public override IList<uint> listReferNotification()
        {
            return new List<uint>
            {
                MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_INFO,
                MsgConstant.MSG_EMAIL_READ_EMAIL,
                MsgConstant.MSG_EMAIL_RECEIVE,
                MsgConstant.MSG_EMAIL_HIDE_INFO,
                MsgConstant.MSG_EMAIL_CLOSE,
                MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST,
                MsgConstant.MSG_SELECT_INDEX_EMAIL,
                MsgConstant.MSG_SELECT_EMAIL_COUNT,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_INFO:
                        View.DisplayEmailCount();       //显示个数
                        View.DisplayEmailInfo();        //显示详细信息
                        break;
                    case MsgConstant.MSG_EMAIL_READ_EMAIL:          //请求阅读邮件
                        EmailManager.Instance.RequestReadEmail((int)notification.body);
                        break;
                    case MsgConstant.MSG_EMAIL_RECEIVE:
                        EmailManager.Instance.Request_ReceiveOrDelete();
                        break;
                    case MsgConstant.MSG_EMAIL_HIDE_INFO:
                        View.SetInfo((bool)notification.body);
                        break;
                    case MsgConstant.MSG_EMAIL_CLOSE:
                        EmailManager.Instance.CloseWindow();
                        break;
                    case MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST:
                        View.DisplayEmailList();
                        break;
                    case MsgConstant.MSG_SELECT_INDEX_EMAIL:
                        View.SelectIndex();
                        break;
                    case MsgConstant.MSG_SELECT_EMAIL_COUNT:
                        View.DisplayEmailCount();
                        break;
                    default:
                        break;
                }
            }
        }


        public EmailView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is EmailView)
                {
                    return base._viewComponent as EmailView;
                }
                return null;
            }
        }
    }
}
