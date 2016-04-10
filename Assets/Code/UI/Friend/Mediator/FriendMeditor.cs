using UnityEngine;
using System.Collections;
using MVC.interfaces;
using MVC.entrance.gate;
using System.Collections.Generic;
using manager;

namespace mediator
{
	public class FriendMeditor:ViewMediator
	{
		private  FriendView _view;
		public FriendMeditor(FriendView view,uint id=MediatorName.FRIEND_MEDIATOR):base(id,view)
		{
			_view=view;
		}
	
		//注册列表
		public override IList<uint> listReferNotification ()
		{
			return new List<uint>
			{
			MsgConstant.MSG_FRIEND_DISPLAY_MAIN
            ,
            MsgConstant.MSG_FRIEND_RECORD_LIST
            ,
            MsgConstant.MSG_FRIEND_DISPLAY_ADDFRIEND
            ,
            MsgConstant.MSG_FRIEND_ADD_TRUE
            ,
            MsgConstant.MSG_FRIEND_RECORD_SHOW
            ,
            MsgConstant.MSG_FRIEND_RECORD_CLOSE
            ,
            MsgConstant.MSG_FRIEND_DISPLAY_LIST_CLOSE
            ,
            MsgConstant.MSG_FRIEND_DELETE
            ,
            MsgConstant.MSG_FRIEND_DELETE_RUFUSE
            ,
            MsgConstant.MSG_FRIEND_TABEL_CHANGE
            ,
            MsgConstant.MSG_FRIEND_DELETE_TRUE
            ,
            MsgConstant.MSG_FRIEND_TABEL_RECORD
            ,
            MsgConstant.MSG_FRIEND_TABEL_RESULT
            ,
            MsgConstant.MSG_FRIEND_RECORD_ADD_FRIENT
            ,
            MsgConstant.MSG_FRIEND_RECORD_RESULT_FRIENT
            ,
            MsgConstant.MSG_FRIEND_RESULT_LIST
            ,
            MsgConstant.MSG_FRIEND_DISPLAY_INFO
            ,
            MsgConstant.MSG_FRIEND_MESSAGE_CLOSE
            ,
            MsgConstant.MSG_FRIEND_ALL_AGREE
            ,
            MsgConstant.MSG_FRIEND_ALL_REFUSE
            ,
            MsgConstant.MSG_FRIEND_SEND_TILI
            ,
            MsgConstant.MSG_FRIEND_RECEIVE_TILI
            ,
            MsgConstant.MSG_FRIEND_ALL_SEND_TILI
            ,
            MsgConstant.MSG_FRIEND_ALL_RECEIVE_TILI
            ,
            MsgConstant.MSG_FRIEND_EX_INFO,

            MsgConstant.MSG_FRIEND_OPT,
            MsgConstant.MSG_FRIEND_WHISPER,
            MsgConstant.MSG_FRIEND_NOTIFY
			};
		} 
		
		//转发消息
		public override void handleNotification (MVC.interfaces.INotification notification)
		{
			if (_view!=null)
			{
                switch (notification.notifyId)
                {
                    case MsgConstant.MSG_FRIEND_DISPLAY_MAIN:
                        _view.DisplayMain();
                        break;
                    case MsgConstant.MSG_FRIEND_DISPLAY_ADDFRIEND:
                         _view.DisplayAddFriendPanle((bool)notification.body);
                        break;

                    case MsgConstant.MSG_FRIEND_ADD_TRUE:
                        FriendManager.Instance.SendAddFriendMsg(_view.GetAddFriendInputValue());
                        break;

                    case MsgConstant.MSG_FRIEND_RECORD_SHOW:
                        _view.DisplayRecord((Table)notification.body);
                        break;
                    case MsgConstant.MSG_FRIEND_RECORD_CLOSE:
                        _view.CloseRecord();
                        break;
                    case MsgConstant.MSG_FRIEND_RECORD_ADD_FRIENT:      //同意
                        FriendManager.Instance.FriendAgree((int)notification.body);
                        break;
                    case MsgConstant.MSG_FRIEND_RECORD_RESULT_FRIENT:   //拒绝
                        FriendManager.Instance.FriendRefuse((int)notification.body);
                        break;
                    case MsgConstant.MSG_FRIEND_DELETE:     //打开删除面板
                        //为null 则为信息列表的删除
                        if (notification.body != null)
                        {
                            FriendManager.Instance.SetFriendWaitforOpt((int)notification.body);
                        }
                        _view.DisplayDeletePanle(true);
                        break;
                    case MsgConstant.MSG_FRIEND_DELETE_RUFUSE://取消删除
                        _view.DisplayDeletePanle(false);
                        FriendManager.Instance.WaitforOpt = null;
                        break;
                    case MsgConstant.MSG_FRIEND_DELETE_TRUE:
                        FriendManager.Instance.DeleteFriend();
                        break;

                    case MsgConstant.MSG_FRIEND_SEND_TILI:
                        if (notification.body == null)
                        {
                            FriendManager.Instance.SendAwardInfo();
                        }
                        else {
                            FriendManager.Instance.SendAward((int)notification.body);
                        }
                        break;
                    case MsgConstant.MSG_FRIEND_RECEIVE_TILI:
                        FriendManager.Instance.Receive((int)notification.body);
                        break;
                    case MsgConstant.MSG_FRIEND_DISPLAY_INFO:
                        FriendManager.Instance.SetFriendWaitforOpt((int)notification.body);
                        _view.DisplayInfo(true);
                        break;
                    case MsgConstant.MSG_FRIEND_MESSAGE_CLOSE:
                        _view.DisplayInfo(false);
                        break;
                    case MsgConstant.MSG_FRIEND_EX_INFO:
                        _view.DisplayExinfo();
                        break;
                    case MsgConstant.MSG_FRIEND_OPT:
                        FriendManager.Instance.VipOption((int)notification.body);
                        break;
                    case MsgConstant.MSG_FRIEND_WHISPER:
                        if (notification.body == null)
                        {
                            FriendManager.Instance.SetWhisper();
                        }
                        else {
                            FriendManager.Instance.SetWhisper((int)notification.body);
                        }
                        break;
                    case MsgConstant.MSG_FRIEND_NOTIFY:
                        _view.UpdateNotfiy((bool)notification.body);
                        break;
                    default:
                        break;
                }
			}
		}
		
		
	
		
		
		
	}
}