using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using helper;

namespace mediator
{
    public class ChannelMediator:ViewMediator
    {
        private ChannelView _view;
        public ChannelMediator(ChannelView view,uint id=MediatorName.CHANNEL_MEDIATOR):base(id,view)
        {
            _view = view;
        }
        public override IList<uint> listReferNotification()
        {
            return new List<uint> 
            {
                MsgConstant.MSG_CHANNEL_CHANGE_LINE,
                MsgConstant.MSG_CHANNEL_CHANGE_SUBMIT,
                MsgConstant.MSG_SURE_DIALOG,
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            switch (notification.notifyId)
            {
                case MsgConstant.MSG_CHANNEL_CHANGE_LINE:
                    ChannelManager.Instance.SetChangeLine((int)notification.body);
                    break;
                case MsgConstant.MSG_CHANNEL_CHANGE_SUBMIT:
                    if (ChannelManager.Instance.WaitChange.Type == model.ChannelType.Max)
                    {
                        UIManager.Instance.ShowDialog(eDialogSureType.eChannel, ViewHelper.FormatLanguage("channel_msg",ChannelManager.Instance.WaitChange.Id));
                    }
                    else {
                        ChannelManager.Instance.AskChangeChannle();
                        ChannelManager.Instance.CloseWindow();
                    }
                    break;
                case MsgConstant.MSG_SURE_DIALOG:
                    if (DialogManager.Instance.SureType==eDialogSureType.eChannel)
                    {
                        ChannelManager.Instance.AskChangeChannle();
                        ChannelManager.Instance.CloseWindow();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}