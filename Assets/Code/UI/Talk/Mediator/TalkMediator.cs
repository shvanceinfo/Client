using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;
using model;

namespace mediator
{
    public class TalkMediator:ViewMediator
    {
        public TalkMediator(TalkView view, uint id = MediatorName.TALK_MEDIATOR):base(id,view)
        {

        }

        public override IList<uint> listReferNotification()
        {
            return new List<uint> { 
                MsgConstant.MSG_TALK_SWTING_TABLE,
                MsgConstant.MSG_TALK_UPDATE_TEXT,
                MsgConstant.MSG_TALK_DISPLAY_CHANNEL,
                MsgConstant.MSG_TALK_DISPALY_CHANNELLIST,
                MsgConstant.MSG_TALK_SELECT_CHANNEL,
                MsgConstant.MSG_TALK_SEND_MSG,
                MsgConstant.MSG_TALK_CLICK_URL,
                MsgConstant.MSG_TALK_HIDDEN_PLAYER_TIP,
                MsgConstant.MSG_TALK_DISPLAY_FRIEND_INPUT,
                MsgConstant.MSG_TALK_ENTER_FRIEDN_NAME,
                MsgConstant.MSG_TALK_DISPLAY_FRIEND_LIST,
                MsgConstant.MSG_TALK_WHISPER_PLAYER,
                MsgConstant.MSG_TALK_FAST_ADD_FRIEND
            };
        }
        public override void handleNotification(MVC.interfaces.INotification notification)
        {
            if (View!=null)
            {
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_TALK_SWTING_TABLE:
                        TalkManager.Instance.SelectTable = (Table)notification.body;
                        View.Display(TalkManager.Instance.SelectTable);
                        break;
                    case MsgConstant.MSG_TALK_UPDATE_TEXT:
                        View.UpdateCurTable((TalkVo)notification.body);
                        break;
                    case MsgConstant.MSG_TALK_DISPLAY_CHANNEL:
                        View.DisplayCurChannel();
                        break;
                    case MsgConstant.MSG_TALK_DISPALY_CHANNELLIST:
                        View.DisplayChannelList((bool)notification.body);
                        break;
                    case MsgConstant.MSG_TALK_SELECT_CHANNEL:
                        TalkManager.Instance.WhisperName = "";
                        TalkManager.Instance.SelectChannel((TalkType)notification.body);
                        break;
                    case MsgConstant.MSG_TALK_SEND_MSG:
                        string msg = View.GetInputValue();
                        TalkManager.Instance.SendMsg(msg);
                        View.ClearInput();
                        break;
                    case MsgConstant.MSG_TALK_CLICK_URL:
                        string url = View.GetURLLinkString();
                        if (string.IsNullOrEmpty(url))
                        {
                            View.DisplayPlayerTip(false);
                        }
                        else
                        { 
                            LinkType type = TalkManager.Instance.GetURLLink(url);
                            switch (type)
                            {
                                case LinkType.NameLink:
                                    View.DisplayPlayerTip(true);
                                    break;
                                case LinkType.ItemLink:
                                    break;
                                case LinkType.FunctionLink:
                                    break;
                                default:
                                    break;
                            } 
                        }
                        break;
                    case MsgConstant.MSG_TALK_HIDDEN_PLAYER_TIP:
                        View.DisplayPlayerTip(false);
                        break;
                    case MsgConstant.MSG_TALK_DISPLAY_FRIEND_INPUT:
                        View.DisplayWhisperFunc((bool)notification.body);
                        break;
                    case MsgConstant.MSG_TALK_ENTER_FRIEDN_NAME:
                        string name=View.GetFriendInputValue();
                        TalkManager.Instance.SetWhisperName(name);

                        View.SetFriendInputValue("");
                        break;
                    case MsgConstant.MSG_TALK_DISPLAY_FRIEND_LIST:
                        View.DisplayFriendList((bool)notification.body);
                        break;
                    case MsgConstant.MSG_TALK_WHISPER_PLAYER:
                        TalkManager.Instance.SetWhisperName(TalkManager.Instance.SelectURL.Sender);
                        break;
                    case MsgConstant.MSG_TALK_FAST_ADD_FRIEND:
                        FastOpenManager.Instance.FastAddFriend(TalkManager.Instance.SelectURL.Sender);
                        Gate.instance.sendNotification(MsgConstant.MSG_TALK_HIDDEN_PLAYER_TIP);
                        break;
                    default:
                        break;
                }
            }
        }

        public TalkView View
        {
            get
            {
                if (base._viewComponent != null && base._viewComponent is TalkView)
                {
                    return base._viewComponent as TalkView;
                }
                return null;
            }
        }
    }
}
