using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using NetGame;

namespace manager
{

    public class EmailManager
    {
        static EmailManager _instance;

        private BetterList<EmailVo> _emails;

        private EmailVo _selectMail;

        private GCAskEMailList _requestMailList;    //请求邮件列表
        private GCAskReadEMail _requestReadMail;    //请求阅读邮件 id
        private GCAskGetEMailPrize _requestAward;   //请求接收奖励 id
        private GCAskRemoveEMail _requestRemoveMail;//请求删除邮件 id
        private EmailManager()
        {
            _selectMail = null;
            _requestMailList = new GCAskEMailList();
            _requestReadMail = new GCAskReadEMail();
            _requestAward = new GCAskGetEMailPrize();
            _requestRemoveMail = new GCAskRemoveEMail();
            _emails = new BetterList<EmailVo>();
        }

        public void ClearEmail()
        {
			if (_selectMail!=null) {
				_selectMail.AwardItems.Clear();
                _selectMail = null;
			}
            _emails.Clear();
        }

        //请求邮件列表
        public void RequestEmailList()
        {
            NetBase.GetInstance().Send(_requestMailList.ToBytes());
        }

        public void OpenWindow()
        {
            if (!UIManager.Instance.isWindowOpen(UiNameConst.ui_email))
            {
                UIManager.Instance.openWindow(UiNameConst.ui_email);
            }
            
        }

        public void RefreshWindow()
        {
            if (_selectMail != null)
            {
                //显示详细信息
                Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_INFO);
            }
        }



        /// <summary>
        /// 添加邮件数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="itemid">物品ID</param>
        /// <param name="itemnum">物品数量</param>
        /// <param name="isreceive">是否接受过</param>
        /// <param name="isread">是否读过</param>
        public void AddEmailInfo(int id,string title,string content,int itemid,int itemnum,bool isreceive,bool isread)
        {
            EmailVo vo=FindEmailVoById(id);
            if (vo == null)
            {
                vo = new EmailVo();
                _emails.Add(vo);
            }
            vo.Id = id;
            vo.Title = title;
            vo.Content = content;
            vo.AwardItems.Add(new IdStruct(itemid, itemnum));
            if (isread)
            {
                if (isreceive)
                {
                    vo.AwardState = EmailState.Receive;
                }
                else {
                    vo.AwardState = EmailState.NotReceive;
                }
                vo.State = EmailState.Read;
                
            }
            else {
                vo.State = EmailState.NotRead;
            }
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_EMAIL, CheckEmailState());
        }



        private bool CheckEmailState()
        {
            foreach (EmailVo vo in _emails)
            {
                if (vo.State==EmailState.NotRead)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 请求阅读邮件
        /// </summary>
        /// <param name="mailId"></param>
        public void RequestReadEmail(int mailId)
        {
            EmailVo vo = FindEmailVoById(mailId);
            if (vo!=null)
            {
                _selectMail = vo;
                _selectMail.State = EmailState.Read;

                _requestReadMail.m_un32EMailID = (uint)vo.Id;
                NetBase.GetInstance().Send(_requestReadMail.ToBytes());

                Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_INFO);
                Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST);
            } 
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_EMAIL, CheckEmailState());
        }

        public void Request_ReceiveOrDelete()
        {
            if (_selectMail!=null)
            {
                if (_selectMail.IsHaveAward)
                {
                    //接收物品
                    _requestAward.m_un32EMailID = (uint)_selectMail.Id;
                    NetBase.GetInstance().Send(_requestAward.ToBytes());
//                    Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST);
                } 
                else {
                    //删除邮件
                    _requestRemoveMail.m_un32EMailID = (uint)_selectMail.Id;
                    NetBase.GetInstance().Send(_requestRemoveMail.ToBytes(),false);
                    _emails.Remove(_selectMail);
					if(_emails.size==0)
					{
						Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_HIDE_INFO,false);
					}
                    _selectMail = null;
                    Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST);
					Gate.instance.sendNotification(MsgConstant.MSG_SELECT_INDEX_EMAIL);
                    Gate.instance.sendNotification(MsgConstant.MSG_SELECT_EMAIL_COUNT);
                }
            }
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_EMAIL, CheckEmailState());
        }
        public void CloseWindow()
        {
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_EMAIL, CheckEmailState());
            UIManager.Instance.closeWindow(UiNameConst.ui_email);
            //_emails.Clear();
            //_selectMail = null;
        }

		/// <summary>
		/// 成功接收附件的处理函数
		/// </summary>
		public void SuccessReceiveItem(){
            if (this.SelectMail != null)
			    this.SelectMail.AwardState = EmailState.Receive;
			Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_INFO);
			Gate.instance.sendNotification(MsgConstant.MSG_EMAIL_DISPLAY_EMAIL_LIST);
			Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATE_EMAIL, this.CheckEmailState());
		}


        private EmailVo FindEmailVoById(int id)
        {
            for (int i = 0; i < _emails.size; i++)
            {
                if (_emails[i].Id==id)
                {
                    return _emails[i];
                }
            }
            return null;
        }


        /// <summary>
        /// 选中的邮件
        /// </summary>
        public EmailVo SelectMail
        {
            get { return _selectMail; }
            set { _selectMail = value; }
        }
        /// <summary>
        /// 没有读过的邮件数量
        /// </summary>
        public int NotReadCount
        {
            get {
                int n = 0;
                for (int i = 0; i < _emails.size; i++)
                {
                    if (_emails[i].State==EmailState.NotRead)
                    {
                        n++;
                    }
                }

                return n;
            }
        }

        /// <summary>
        /// 邮件数量
        /// </summary>
        public int EmailCount
        {
            get { return _emails.size; }
        }

        public BetterList<EmailVo> Emails
        {
            get { return _emails; }
        }

        public static EmailManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance= new EmailManager();
                }
                return EmailManager._instance; 
            }
        }
           
    }
}
