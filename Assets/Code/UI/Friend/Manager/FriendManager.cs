using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using MVC.entrance.gate;
using System;
using NetGame;
using helper;


namespace manager
{
    public class FriendManager
    {
        private List<FriendVo> _myFriendList;   //我的好友列表

        /// <summary>
        /// 好友列表
        /// </summary>
        public List<FriendVo> MyFriendList
        {
            get { return _myFriendList; }
            set { _myFriendList = value; }
        }
        private List<FriendVo> _recordFriendList;//好友申请列表

        public List<FriendVo> RecordFriendList
        {
            get { return _recordFriendList; }
            set { _recordFriendList = value; }
        }
        private List<string> _logList;           //好友日志列表

        /// <summary>
        /// 日志信息
        /// </summary>
        public List<string> LogList
        {
            get { return _logList; }
            set { _logList = value; }
        }
        private FriendMyInfoVo _myInfo;          //个人信息

        /// <summary>
        /// 个人信息
        /// </summary>
        public FriendMyInfoVo MyInfo
        {
            get { return _myInfo; }
            set { _myInfo = value; }
        }
        private FriendVo _waitforOpt;

        public FriendVo WaitforOpt
        {
            get { return _waitforOpt; }
            set { _waitforOpt = value; }
        }
        private GCAskFriendRecord _ask110;
        private GCAskAddFriend _ask108;
        private GCVipFriend _ask109;
        private bool _recordUIIsOpen;

        /// <summary>
        /// 申请列表是否打开
        /// </summary>
        public bool RecordUIIsOpen
        {
            get { return _recordUIIsOpen; }
            set { _recordUIIsOpen = value; }
        }
        private FriendManager()
        {
            _myFriendList = new List<FriendVo>();
            _recordFriendList = new List<FriendVo>();
            _logList = new List<string>();
            _myInfo = new FriendMyInfoVo();
            _ask110 = new GCAskFriendRecord();
            _recordUIIsOpen = false;
        }


        /// <summary>
        /// 更新接收信息
        /// </summary>
        /// <param name="name"></param>
        public void UpdateFriendReceive(string name)
        {
            FriendVo vo = FindFriend(name);
            if (vo!=null)
            {
                vo.IsCanReceive = true;
            }
        }

        public void ClearFriendReceive()
        {
            _myFriendList.ForEach(v => v.IsCanReceive = false);
        }

        /// <summary>
        /// 更新发送信息
        /// </summary>
        /// <param name="name"></param>
        public void UpdateFriendSend(string name)
        {
            FriendVo vo = FindFriend(name);
            if (vo != null)
            {
                vo.IsSend = true;
            }
        }

        private FriendVo FindFriend(string name)
        {
            for (int i = 0; i < _myFriendList.Count; i++)
            {
                if (_myFriendList[i].Name.Equals(name))
               {
                   return _myFriendList[i];

               }
            }
            return null;
        }

        /// <summary>
        /// 服务器添加好友列表
        /// </summary>
        /// <param name="vo"></param>
        public void AddFriendList(FriendVo vo)
        {
           for (int i = 0; i < _myFriendList.Count; i++)
           {
               if (_myFriendList[i].Name.Equals(vo.Name))
               {
                   _myFriendList[i] = vo;
                   return;
               }
           }
           _myFriendList.Add(vo);
           _myInfo.CurFriendCount++;
        }

        public void SortFriendList()
        {
            List<FriendVo> temp = new List<FriendVo>();
            foreach (FriendVo vo in _myFriendList)
            {
                if (vo.IsOnline)
                {
                    temp.Insert(0,vo);
                }
                else {
                    temp.Add(vo);
                }
            }
            _myFriendList.Clear();
            _myFriendList.AddRange(temp.ToArray());
        }

        /// <summary>
        /// 添加申请记录
        /// </summary>
        /// <param name="vo"></param>
        public void AddRecordList(FriendVo vo)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY,
                _recordFriendList.Count == 0 ? false : true
                );
            for (int i = 0; i < _recordFriendList.Count; i++)
            {
                if (_recordFriendList[i].Name.Equals(vo.Name))
                {
                    _recordFriendList[i] = vo;
                    return;
                }
            }
                _recordFriendList.Add(vo);
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY,
                    _recordFriendList.Count == 0 ? false : true
                    );
        }
        public void ClearRecord()
        {
            _recordFriendList.Clear();
        }

        /// <summary>
        /// 添加LOG
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(string log)
        {
            string fv = _logList.Find(v => v.Equals(log));
            if (string.IsNullOrEmpty(fv))
            {
                _logList.Add(log);
                SaveLog();
            }
        }

        //添加好友
        public void FormatLogFriend(string name)
        {
            AddLog(string.Format("[{0}]{1}[-][{2}]与您结为好友，快互赠体力加深友谊吧。[-]",ColorConst.Color_Green,name,ColorConst.Color_HeSe));
        }

        //删除好友
        public void FormatLogDelete(string name)
        {
            AddLog(string.Format("[{0}]你与[{1}]{2}[-]断绝好友关系。[-]", ColorConst.Color_HeSe, ColorConst.Color_Green,name));
        }

        //拒绝添加好友
        public void FormatLogToOtherNot(string name)
        {
            AddLog(string.Format("[{0}]你拒绝与[{1}]{2}[-]结为好友。[-]", ColorConst.Color_HeSe, ColorConst.Color_Green, name));
        }

        public void FormatLogOtherToNot(string name)
        {
            AddLog(string.Format("[{0}][{1}]{2}[-]不同意与你结为好友。[-]", ColorConst.Color_HeSe, ColorConst.Color_Green, name));
        }


        public void DeleteFriend(string name)
        {
            for (int i = 0; i < _myFriendList.Count; i++)
            {
                if (_myFriendList[i].Name.Equals(name))
                {
                    _myFriendList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 消息总计
        /// </summary>
        /// <param name="receive"></param>
        /// <param name="send"></param>
        public void FriendInfoReset(int receive,int send)
        {
            _myInfo.CurFriendCount = _myFriendList.Count;
            _myInfo.CurReceive = receive;
            _myInfo.CurSendAward = send;
        }


        public void OpenWindow()
        {
            
            ReadLog();
            NetBase.GetInstance().Send(_ask110.ToBytes());
        }

        public void CloseWindow()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_friend);
            SaveLog();
        }

        //读取本地日志
        public void ReadLog()
        {
            _logList.Clear();
            foreach (string str in CacheManager.GetInstance().GetCacheInfo().FriendLog)
            {
                _logList.Add(str);
            }
        }

        //保存本地日志
        public void SaveLog()
        {
            CacheManager.GetInstance().GetCacheInfo().FriendLog.Clear();
            foreach (string str in _logList)
            {
                CacheManager.GetInstance().GetCacheInfo().FriendLog.Add(str);
            }
            CacheManager.GetInstance().SaveCache();
        }

        /// <summary>
        /// 标示位，只请求一次数据
        /// </summary>
        public void ReceivedData()
        {
            OpenUI();
        }

        private void OpenUI()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_friend);
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY,
                _recordFriendList.Count == 0 ? false : true
                );
        }

        //发送添加好友
        public void SendAddFriendMsg(string name)
        {
            if (_myInfo.CurFriendCount>=_myInfo.MaxFriendCount)
            {
                ViewHelper.DisplayMessageLanguage("friend_error_max_friendcount");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                ViewHelper.DisplayMessageLanguage("friend_add_name_isnull");
                return;
            }
            if (name.Equals(CharacterPlayer.character_property.getNickName()))
            {
                ViewHelper.DisplayMessageLanguage("friend_error_is_own");
                return;
            }

            _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.AddFriend, name);
            NetBase.GetInstance().Send(_ask108.ToBytes());
            //关闭好友输入
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_ADDFRIEND, false);
        }

        public void AddFriendEx(string name)
        {
            if (name.Equals(CharacterPlayer.character_property.getNickName()))
            {
                ViewHelper.DisplayMessageLanguage("friend_error_is_own");
                return;
            }
            if (_myFriendList.Exists(v => v.Name.Equals(name)))
            {
                ViewHelper.DisplayMessageLanguage("friend_error_ready_is_friend");
                return;
            }
            SendAddFriendMsg(name);
        }

        public void ResultMsg(FriendOptType type, FriendOptRet result, string name)
        {
            switch (type)
            {
                case FriendOptType.Add:
                    switch (result)
                    {
                        case FriendOptRet.Success:
                            ViewHelper.DisplayMessageLanguage("friend_add_name_add_ok");
                            break;
                        case FriendOptRet.Fail:
                            ViewHelper.DisplayMessageLanguage("friend_add_name_isnothave");
                            break;
                        case FriendOptRet.NotOnline:
                            ViewHelper.DisplayMessageLanguage("friend_add_name_isnotonline");
                            break;
                        default:
                            break;
                    }
                    break;
                case FriendOptType.Cancel:      //拒绝结为好友
                    Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY,
                _recordFriendList.Count == 0 ? false : true
                );
                    FormatLogToOtherNot(name);
                    break;
                case FriendOptType.Agree:       //同意结为好友
                    switch (result)
                    {
                        case FriendOptRet.Success:
                            FormatLogFriend(name);
                            _recordFriendList.Remove(_waitforOpt);
                            _myFriendList.Insert(0,_waitforOpt);
                            _myInfo.CurFriendCount = _myFriendList.Count;
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table1);
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY, 
                _recordFriendList.Count==0?false:true
                );
                            break;
                        case FriendOptRet.Fail:
                            ViewHelper.DisplayMessage("好友未知错误");
                            break;
                        case FriendOptRet.NotOnline:
                            ViewHelper.DisplayMessageLanguage("friend_is_not_online");
                            break;
                        default:
                            break;
                    }
                    break;
                case FriendOptType.Delete:
                    switch (result)
                    {
                        case FriendOptRet.Success:
                            FormatLogDelete(name);
                            _myInfo.CurFriendCount--;
                            _myFriendList.Remove(_waitforOpt);
                            ViewHelper.DisplayMessageLanguage("friend_delete_ok");
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DELETE_RUFUSE);
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_MESSAGE_CLOSE);
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                            break;
                        case FriendOptRet.Fail:
                            break;
                        case FriendOptRet.NotOnline:
                            break;
                        default:
                            break;
                    }
                    break;
                case FriendOptType.Send:
                    switch (result)
                    {
                        case FriendOptRet.Success:
                            _waitforOpt.IsSend = true;
                            _myInfo.CurSendAward++;
                            ViewHelper.DisplayMessageLanguage("friend_send_ok");
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_EX_INFO);
                            break;
                        case FriendOptRet.Fail:
                            break;
                        case FriendOptRet.NotOnline:
                            ViewHelper.DisplayMessageLanguage("friend_is_not_online");
                            break;
                        default:
                            break;
                    }
                    break;
                case FriendOptType.Receive:
                    switch (result)
                    {
                        case FriendOptRet.Success:
                            _myInfo.CurReceive++;
                            ViewHelper.DisplayMessageLanguage("friend_receive_ok");
                            _waitforOpt.IsCanReceive = false;
                            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                            break;
                        case FriendOptRet.Fail:
                            break;
                        case FriendOptRet.NotOnline:
                            ViewHelper.DisplayMessageLanguage("friend_is_not_online");
                            break;
                        default:
                            break;
                    }
                   
                    break;
                default:
                    break;
            }
        }

        //强制刷新一键领取
        public void ExDisplayRecordList()
        {
            if (_recordUIIsOpen)
            {
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table1);
            }
        }

        public void FriendEventOpt(FriendSendType opt, FriendVo vo)
        {
            switch (opt)
            {
                case FriendSendType.ReceiveAdd://被申请好友
                    Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_NOTIFY, true);
                    AddRecordList(vo);
                    if (_recordUIIsOpen)
                    {
                        Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table1);
                    }
                    break;
                case FriendSendType.ReceiveCancel://被拒绝还有
                    FormatLogOtherToNot(vo.Name);
                    break;
                case FriendSendType.ReceiveAgree://被添加好友
                    
                    FormatLogFriend(vo.Name);
                    ViewHelper.DisplayMessageLanguage("friend_add_friend_ok",vo.Name);
                    AddFriendList(vo);
                    SortFriendList();
                    Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                    break;
                case FriendSendType.ReceiveSend:    //被赠送体力
                    UpdateFriendReceive(vo.Name);
                    Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                    break;
                case FriendSendType.Delete:
                    _myInfo.CurFriendCount--;
                    DeleteFriend(vo.Name);
                    Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                    break;
                default:
                    break;
            }
        }

        //同意添加好友
        public void FriendAgree(int index)
        {
            _waitforOpt = _recordFriendList[index];
            _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.Ok, _waitforOpt.Name);
            NetBase.GetInstance().Send(_ask108.ToBytes());
        }

        //拒绝好友
        public void FriendRefuse(int index)
        {
            FriendVo vo = _recordFriendList[index];
            _recordFriendList.RemoveAt(index);
            _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.Not, vo.Name);
            NetBase.GetInstance().Send(_ask108.ToBytes(),false);
            Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_RECORD_SHOW, Table.Table1);
        }

        public void SetFriendWaitforOpt(int index)
        {
            _waitforOpt = _myFriendList[index];
        }

        //删除好友
        public void DeleteFriend()
        {
            _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.DeleteFriend, _waitforOpt.Name);
            NetBase.GetInstance().Send(_ask108.ToBytes(), false);
        }


        public void Receive(int index)
        {
            if (_myInfo.CurReceive >= _myInfo.MaxReceive)
            {
                ViewHelper.DisplayMessageLanguage("friend_error_max_receive");
            }
            else {
                _waitforOpt = _myFriendList[index];
                _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.ReceiveAward, _waitforOpt.Name);
                NetBase.GetInstance().Send(_ask108.ToBytes(), false);
            }
        }

        //赠送体力
        public void SendAward(int index)
        {
            if (_myInfo.CurSendAward >= _myInfo.MaxSendAward)
            {
                ViewHelper.DisplayMessageLanguage("friend_error_max_send");
            }
            else {
                _waitforOpt = _myFriendList[index];
                _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.SendAward, _waitforOpt.Name);
                NetBase.GetInstance().Send(_ask108.ToBytes(), false); 
            }
            
        }

        //详细信息赠送体力
        public void SendAwardInfo()
        {
            if (_myInfo.CurSendAward >= _myInfo.MaxSendAward)
            {
                ViewHelper.DisplayMessageLanguage("friend_error_max_send");
            }
            else {
                _ask108 = new GCAskAddFriend(GCAskAddFriend.FriendSendType.SendAward, _waitforOpt.Name);
                NetBase.GetInstance().Send(_ask108.ToBytes(), false);
 
            }
            
        }

        public void SetWhisper(int index)
        {
            if (_myFriendList[index].IsOnline)
            {
                FastOpenManager.Instance.OpenWhisper(_myFriendList[index].Name);
            }
            else {
                ViewHelper.DisplayMessageLanguage("friend_is_not_online");
            }
        }
        public void SetWhisper()
        {
            if (_waitforOpt.IsOnline)
            {
                FastOpenManager.Instance.OpenWhisper(_waitforOpt.Name);
            }
            else
            {
                ViewHelper.DisplayMessageLanguage("friend_is_not_online");
            }
        }

        public void VipOption(int opt)
        {
            
            switch (opt)
            {
                case 0: AllSend();
                    break;
                case 1: AllReceive();
                    break;
                case 2: AllArgee();
                    break;
                case 3: AllRefush();
                    break;
                default:
                    break;
            }
        }
        private void AllSend()         //全部赠送
        {
            if (_myFriendList.Count != 0)
            {
                if (_myInfo.CurSendAward>=_myInfo.MaxSendAward)
                {
                    ViewHelper.DisplayMessageLanguage("friend_error_send_max");
                    return;
                }
                bool iscanSend = false;
                foreach (FriendVo vo in _myFriendList)
                {
                    if (!vo.IsSend)
                    {
                        iscanSend = true;
                        break;
                    }
                }
                if (!iscanSend)
                {
                    ViewHelper.DisplayMessageLanguage("friend_error_not_send_friend");
                    return;
                }
                else {

                    _ask109 = new GCVipFriend(1);
                    NetBase.GetInstance().Send(_ask109.ToBytes(), false);
                    ViewHelper.DisplayMessageLanguage("friend_send_ok");
                }

            }
            else {
                ViewHelper.DisplayMessageLanguage("friend_error_not_have_friend_to_edit");
            }
        }

        private void AllReceive()       //全部接受
        {
            if (_myFriendList.Count!=0)
            {
                if (_myInfo.CurReceive >= _myInfo.MaxReceive)
                {
                    ViewHelper.DisplayMessageLanguage("friend_error_receive_max");
                    return;
                }
                bool iscanreceive = false;
                foreach (FriendVo vo in _myFriendList)
                {
                    if (vo.IsCanReceive)
                    {
                        iscanreceive = true;
                        break;
                    }
                }
                if (!iscanreceive)
                {
                    ViewHelper.DisplayMessageLanguage("friend_error_not_receive_friend");
                    return;
                }

                _ask109 = new GCVipFriend(2);
                NetBase.GetInstance().Send(_ask109.ToBytes(), false);
                ViewHelper.DisplayMessageLanguage("friend_receive_ok");
            }
            else
            {
                ViewHelper.DisplayMessageLanguage("friend_error_not_have_friend_to_edit");
            }
            
        }
        private void AllArgee()         //全部同意
        {
            if (_recordFriendList.Count != 0)
            {
                if (_myInfo.CurFriendCount>=_myInfo.MaxFriendCount)
                {
                    ViewHelper.DisplayMessageLanguage("friend_error_max_friendcount");
                    return;
                }
                ViewHelper.DisplayMessageLanguage("friend_all_argee_ok");
                _ask109 = new GCVipFriend(3);
                NetBase.GetInstance().Send(_ask109.ToBytes(), false);
            }
            else
            {
                ViewHelper.DisplayMessageLanguage("friend_error_not_have_friend_to_edit");
            }
        }
        private void AllRefush()        //全部拒绝
        {
            if (_recordFriendList.Count!=0)
            {
                ViewHelper.DisplayMessageLanguage("friend_all_no");
                _ask109 = new GCVipFriend(4);
                NetBase.GetInstance().Send(_ask109.ToBytes(), false);
            }
            else
            {
                ViewHelper.DisplayMessageLanguage("friend_error_not_have_friend_to_edit");
            }
        }

        #region 单例
        private static FriendManager _instance;
        public static  FriendManager Instance
        {
            get
            {
                if (_instance == null) _instance = new FriendManager();
                return _instance;
            }
        }
        #endregion
        #region Static
        public static string RECEIVE = LanguageManager.GetText("friend_receive");
        public static string SEND = LanguageManager.GetText("friend_send");
        public static string FRIEND = LanguageManager.GetText("friend_friend");
        public static string FormatTitle(string funcname, int value, int max)
        {
            return string.Format("[{0}]{1}:{2}/{3}[-]", ColorConst.Color_Qianlan,
                funcname, value, max);
        }
        #endregion
    }
}
