using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using model;
using NetGame;
using MVC.entrance.gate;
using helper;

namespace manager
{
    public class ChannelManager
    {
        private Hashtable _channelHash;     //频道配置

        private List<ChannelLineVo> _lines;     //当前频道集合

        
        private ChannelLineVo _curLine;         //当前线路

        private GCAskChnnelList _askList;
        private GCAskChnnelChange _askChange;
        private int _changeLine;

        public ChannelLineVo WaitChange
        {
            get {
                return Lines[_changeLine];
            }
        }
        private ChannelManager()
        {
            _channelHash = new Hashtable();
            _lines = new List<ChannelLineVo>();
            _curLine = new ChannelLineVo();
            _curLine.Id = 1;
            _askList = new GCAskChnnelList();
            _askChange = new GCAskChnnelChange();
            _changeLine = 0;
        }
        #region 单例
        private static ChannelManager _instance;
        public static ChannelManager Instance
        {
            get
            {
                if (_instance == null) _instance = new ChannelManager();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 更新当前线路
        /// </summary>
        /// <param name="line"></param>
        public void UpdateCurLine(int line)
        {
            PlayerManager.Instance.destroyAllPlayerOther();
            CurLine.Id = line;
            Gate.instance.sendNotification(MsgConstant.MSG_MIAN_UPDATE_CHANNEL);
        }

        public void UpdateChannel(int line, int people)
        {
            Lines.Add(new ChannelLineVo { Id = line, CurPeople = people });
        }

        /// <summary>
        /// 请求线路列表
        /// </summary>
        public void AskLines()
        {
            NetBase.GetInstance().Send(_askList.ToBytes());
        }

        /// <summary>
        /// 发送换线
        /// </summary>
        public void AskChangeChannle()
        {
            if (WaitChange.Id==CurLine.Id)
            {
                ViewHelper.DisplayMessageLanguage("channel_error");
                return;
            }
            _askChange.channel = (byte)WaitChange.Id;
            NetBase.GetInstance().Send(_askChange.ToBytes());
            ViewHelper.DisplayMessageLanguage("channel_success");
        }
        //设置换线
        public void SetChangeLine(int line)
        {
            _changeLine = line;
        }

        public void OpenWindow()
        {
            UIManager.Instance.openWindow(UiNameConst.ui_channel);
        }

        public void CloseWindow()
        {
            UIManager.Instance.closeWindow(UiNameConst.ui_channel);
        }

        public ChannelVo FindVoByMapId(int mapId)
        {
            foreach (ChannelVo vo in ChannelHash.Values)
            {
                if (vo.MapId==mapId)
                {
                    return vo;
                }
            }
#if UNITY_EDITOR
            Debug.LogError("Can't Find Chaneel Map Data");
#endif
            return null;
        }

        /// <summary>
        /// 频道人数判定数据
        /// </summary>
        public Hashtable ChannelHash
        {
            get { return _channelHash; }
            set { _channelHash = value; }
        }

        public List<ChannelLineVo> Lines
        {
            get { return _lines; }
            set { _lines = value; }
        }
        public ChannelLineVo CurLine
        {
            get { return _curLine; }
            set { _curLine = value; }
        }
    }
}
