using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;
using helper;
using System.Collections.Generic;
using NetGame;
using System;

namespace manager
{
    public class TalkManager
    {
        public const int MAX_MSG_LEN = 40;              //最大发送字符数

        const uint SENDDELAY = 5;                       //发送时间间隔

        private uint _lastSendTime;

        static string ErrorFast;                        //发送太快信息

        private BetterList<TalkVo> _contents;           //聊天信息

        private Table _selectTable;                     //选择页签

        private TalkType _sendType;                     //发送频道

        private string _whisperName;                    //私聊名称

        private TalkVo _selectURL;                       //选中URL玩家名称

        private  int MAXTALK;            //最大聊天记录

        public bool m_bOpen = false;

        private GCAskChat _ask35;
        private GCAskSelectPlayer _ask112;              //请求查询玩家
        private TalkManager()
        {
            MAXTALK = ViewHelper.FindPublicById(1018001).type2Data;
            ErrorFast = string.Format(LanguageManager.GetText("chat_time_out_error"), SENDDELAY);
            _lastSendTime = 0;
            _contents = new BetterList<TalkVo>();
            _ask112 = new GCAskSelectPlayer();
        }

        public void OpenWindow()
        {
            _selectTable = Table.Table1;
            _sendType = TalkType.World;
            if (!UIManager.Instance.isWindowOpen(UiNameConst.ui_chat))
            {
                UIManager.Instance.openWindow(UiNameConst.ui_chat);
            }
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_SWTING_TABLE, _selectTable);
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_CHANNEL);

            m_bOpen = true;
        }

        
        public void CloseWindow()
        {
            m_bOpen = false;
            UIManager.Instance.closeWindow(UiNameConst.ui_chat);
        }

        //添加消息内容
        public void AddContent(TalkType type,string sender,string content)
        {
            content=LeachDirtyManager.Instance.FilterInfo(content);

            TalkVo vo = new TalkVo { Type = type, Sender = sender, Content = content };
            if (_contents.size>MAXTALK)
            {
                _contents.RemoveAt(0);
            }
            _contents.Add(vo);
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_UPDATE_TEXT, vo);   //更新聊天界面的消息
            Gate.instance.sendNotification(MsgConstant.MSG_MAIN_UPDATA_TALK);       //更新主界面的消息
            Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_TALK_MSG); //更新战斗界面的消息
        }

        //选择频道
        public void SelectChannel(TalkType channel)
        {
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPALY_CHANNELLIST, false);
            switch (channel)
            {
                case TalkType.World:
                    _sendType = channel;
                    break;
                case TalkType.Guild:
                    ViewHelper.DisplayMessage("您没有公会");
                    _sendType = TalkType.World;
                    break;
                case TalkType.Whisper:
                    _sendType = channel;
                    Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_FRIEND_INPUT, true);
                    break;
                default:
                    break;
            }
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_CHANNEL);
        }       

        //发送聊天信息
        public void SendMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            if (SendGMCommand(msg))
            {
                return;
            }
            if (CheckTime())
            {
                msg=NGUIText.StripSymbols(msg);
                _ask35 = new GCAskChat(_sendType, msg);
                if (_sendType == TalkType.Whisper)
                {
                    _ask35.SetWhisperName(WhisperName);
                }
                NetBase.GetInstance().Send(_ask35.ToBytes(),false);
            }

        }

        //获取当前点击的URL是什么类型的链接
        public LinkType GetURLLink(string url)
        {
            for (int i = 0; i < _contents.size; i++)
            {
                if (!string.IsNullOrEmpty(_contents[i].Sender))
                {
                    if (_contents[i].Sender.Equals(url))
                    {
                        //查找选择的玩家名称  
                        _selectURL = _contents[i];
                    }
                }
                
            }
            //TODE.. 链接功能分析代码
            return LinkType.NameLink;
        }

        //设置私聊状态
        public void SetWhisperResult(bool isOnline)
        {
            if (isOnline)
            {
                _sendType = TalkType.Whisper;   //在线
            }
            else {
                _sendType = TalkType.World; //玩家不在线
                ViewHelper.DisplayMessageLanguage("talk_msg_player_not_online");
            }
            
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_CHANNEL);
        }
        
        //设置私聊玩家名称
        public void SetWhisperName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ViewHelper.DisplayMessageLanguage("talk_msg_inputName");
                return;
            }
            if (name.Equals(CharacterPlayer.character_property.getNickName()))
            {
                return;
            }
            _whisperName = name; 
            //TODE: 查找私聊对象是否存在
            _ask112.SetName(name);

            NetBase.GetInstance().Send(_ask112.ToBytes());

            //关闭好友列表
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_FRIEND_LIST, false);
            //关闭好友输入界面
            Gate.instance.sendNotification(MsgConstant.MSG_TALK_DISPLAY_FRIEND_INPUT, false);      
        }



        private bool SendGMCommand(string msg)
        {
            // 判断是否客户端GM命令
            string gmmsg;
            if (msg.StartsWith("/gm "))
            {
                gmmsg = msg.Substring(4);

                string[] myTmp = gmmsg.Split(new char[] { ' ' });

                List<int> param = new List<int>();

                string name = "";
                int index = 0;

                foreach (string i in myTmp)
                {
                    if (index == 0)
                    {
                        name = myTmp[0];
                    }
                    else if (name.ToLower() == "talk")
                    {
                        NetBase.GetInstance().Send((new GC_GM_SendAnnouncement(i).ToBytes()));
                        return true;
                    }
                    else
                    {
                        param.Add(int.Parse(i));
                    }
                    index++;
                }

                ClientGMCommands.GMCommand(name, param);

                return true;
            }
            return false;
        }

        //检查时间是否可以发送
        private bool CheckTime()
        {
            uint now = Global.ToUnixTimeStamp(DateTime.Now);
            if ((now - _lastSendTime) < SENDDELAY)
            {
                AddContent(TalkType.Error, null, ErrorFast);
                return false;
            }
            _lastSendTime = now;
            return true;
        } 
        
        #region 单例
        private static TalkManager _instance;
        public static TalkManager Instance
        {
            get
            {
                if (_instance == null) _instance = new TalkManager();
                return _instance;
            }
        }
        #endregion

        #region Attribute
        public TalkVo SelectURL
        {
            get { return _selectURL; }
            set { _selectURL = value; }
        }
        /// <summary>
        /// 当前选择的标签
        /// </summary>
        public Table SelectTable
        {
            get { return _selectTable; }
            set { _selectTable = value; }
        }
        /// <summary>
        /// 聊天消息内容
        /// </summary>
        public BetterList<TalkVo> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// 发送频道
        /// </summary>
        public TalkType SendType
        {
            get { return _sendType; }
            set { _sendType = value; }
        }
        /// <summary>
        /// 私聊对象
        /// </summary>
        public string WhisperName
        {
            get { return _whisperName; }
            set { _whisperName = value; }
        }
        #endregion

        #region Static

        static string WORLD = string.Format("[{0}]【世界】[-]", ColorConst.Color_Juhuang);
        static string GRILD = string.Format("[{0}]【公会】[-]", ColorConst.Color_Green);
        static string WHISPER = string.Format("[{0}]【私聊】[-]", ColorConst.Color_Pink);
        static string SYSTEM = string.Format("[{0}]【系统】[-]", ColorConst.Color_Red);

        public static string WORLD_CHANNEL = ColorConst.Format(ColorConst.Color_Juhuang, "世界频道");
        public static string GRILD_CHANNEL = ColorConst.Format(ColorConst.Color_Green, "公会频道");
        public static string WHISPER_CHANNEL = ColorConst.Format(ColorConst.Color_Pink, "私聊频道");

        const string NAME_FMT = "[url={0}][{1}][u]{2}[/u][-][/url]";
        const string CONTENT_FMT = "[{0}]:{1}[-]";
        //我对XX
        const string ME_TO_WHISPER = "[{0}]我[-][{1}]对[-][url={2}][{3}][u]{4}[/u][-][/url]";
        /// <summary>
        /// 格式化私聊名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatWhipserName(string name)
        {
            return ColorConst.Format(ColorConst.Color_Pink, name);
        }

        public static string FormatWorldText(TalkVo vo)
        {
            return WORLD + string.Format(NAME_FMT, vo.Sender, ColorConst.Color_Blue, vo.Sender) +
                string.Format(CONTENT_FMT, ColorConst.Color_Juhuang, vo.Content);
        }

        public static string FormatGrildText(TalkVo vo)
        {
            return GRILD + string.Format(NAME_FMT, vo.Sender, ColorConst.Color_Blue, vo.Sender) +
                string.Format(CONTENT_FMT, ColorConst.Color_Green, vo.Content);
        }

        public static string FormatWhisper(TalkVo vo)
        {
            if (vo.Sender.Equals(CharacterPlayer.character_property.getNickName()))
            {
                return FormatMeWhisper(TalkManager.Instance.WhisperName, vo.Content);
            }
            else {
                return WHISPER + string.Format(NAME_FMT, vo.Sender, ColorConst.Color_Blue, vo.Sender) +
                string.Format(CONTENT_FMT, ColorConst.Color_Pink, vo.Content);
            }
        }

        //我对别人说
        private static string FormatMeWhisper(string sender,string content)
        { 
            return WHISPER+string.Format(ME_TO_WHISPER,ColorConst.Color_Blue,
                ColorConst.Color_Pink,sender,ColorConst.Color_Blue,sender)+
                string.Format(CONTENT_FMT, ColorConst.Color_Pink, content);
        }

        public static string FormatSystem(TalkVo vo)
        { 
            return SYSTEM +
                string.Format(CONTENT_FMT, ColorConst.Color_Red, vo.Content);
        }

        public static string FormatShotMsg(TalkVo vo)
        {
            switch (vo.Type)
            {
                case TalkType.Error:
                    return ColorConst.Format(ColorConst.Color_Red, vo.Content);
                case TalkType.World:
                    return ColorConst.Format(ColorConst.Color_Juhuang, vo.Content);
                case TalkType.Guild:
                    return ColorConst.Format(ColorConst.Color_Green, vo.Content);
                case TalkType.Whisper:
                    return ColorConst.Format(ColorConst.Color_Pink, vo.Content);
                case TalkType.System:
                    return ColorConst.Format(ColorConst.Color_Red, vo.Content);
                case TalkType.SystemAndPost:
                    return ColorConst.Format(ColorConst.Color_Red, vo.Content);
                default:
                    return "";
            }
        }
        #endregion
    }
}
	
