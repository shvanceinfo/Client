using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using NetGame;
using System.IO;
using manager;
using System.Text;
using model;
using MVC.entrance.gate;


#region 好友操作返回结果

public enum FriendOptType
{
    Add = 1,
    Cancel = 2,
    Agree = 3,
    Delete = 4,
    Send = 5,
    Receive = 6

}

public enum FriendOptRet
{
    /// <summary>
    /// 成功
    /// </summary>
    Success = 0,
    /// <summary>
    /// 失败
    /// </summary>
    Fail =-1,

    /// <summary>
    /// 不在线
    /// </summary>
    NotOnline=-2,
}

/// <summary>
/// 好友操作返回结果,73
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NotifyAddFriendMessage : NetHead
{
    int strLenth = 20;

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        FriendOptType optType = (FriendOptType)memRead.ReadInt32();
        //   int FriendMessageId = (int)memRead.ReadInt32();
        FriendOptRet optRet = (FriendOptRet)memRead.ReadInt32(); 

        string name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(strLenth)));
        FriendManager.Instance.ResultMsg(optType, optRet, name);
    }

}
#endregion



#region 返回好友列表


/// <summary>
/// 返回好友列表,74
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NotifyFriendInfo : NetHead
{

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        int receive = (int)memRead.ReadByte();          //接收次数
        int send = (int)memRead.ReadByte();             //发送次数

        int nFriendListType = (int)memRead.ReadInt32(); //消息类型
        string[] str;
        string[] spl;
        int count = 0;
        int infoLen=0;
        string info;
        FriendVo vo;
        switch (nFriendListType)
        {
            case 1:   //好友列表信息
                infoLen = (int)memRead.ReadInt32();
                info = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(infoLen)));

                str = info.Split(';');
                count = str.Length;
                
                for (int i = 0; i < count; i++)
                {
                    if (str[i].Length == 0||string.IsNullOrEmpty(str[i]))
                    {
                        continue;
                    }
                    spl= str[i].Split(',');
                    vo = new FriendVo() { 
                    Name=spl[0],
                    Level=int.Parse(spl[1]),
                    VipLevel = int.Parse(spl[2]),
                    Fighter = int.Parse(spl[3]),
                    Career = (CHARACTER_CAREER)int.Parse(spl[4]),
                     IsOnline=Convert.ToBoolean(int.Parse( spl[5]))
                    };
                    FriendManager.Instance.AddFriendList(vo);
                }
                FriendManager.Instance.SortFriendList();
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                break;


            case 2:         //申请列表信息
                infoLen = (int)memRead.ReadInt32();
                info = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(infoLen)));
                FriendManager.Instance.ClearRecord();
                str = info.Split(';');
                count = str.Length;
                for (int i = 0; i < count; i++)
                {
                    if (str[i].Length == 0||string.IsNullOrEmpty(str[i]))
                    {
                        continue;
                    }
                    spl= str[i].Split(',');
                    vo = new FriendVo() { 
                    Name=spl[0],
                    Level=int.Parse(spl[1]),
                    VipLevel = int.Parse(spl[2]),
                    Fighter = int.Parse(spl[3]),
                    Career = (CHARACTER_CAREER)int.Parse(spl[4]),
                     IsOnline=Convert.ToBoolean(int.Parse( spl[5]))
                    };
                    FriendManager.Instance.AddRecordList(vo);
                }
                FriendManager.Instance.ExDisplayRecordList();

                break;


            case 3:           //收到的体力赠送列表
               infoLen = (int)memRead.ReadInt32();
                info = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(infoLen)));

                str = info.Split(';');
                count = str.Length;
                FriendManager.Instance.ClearFriendReceive();
                for (int i = 0; i < count; i++)
                {
                    if (str[i].Length == 0||string.IsNullOrEmpty(str[i]))
                    {
                        continue;
                    }
                    spl= str[i].Split(',');

                    FriendManager.Instance.UpdateFriendReceive(spl[0]);
                }
                Gate.instance.sendNotification(MsgConstant.MSG_FRIEND_DISPLAY_MAIN);
                break;

            case 4:          //赠送的体力赠送列表
                infoLen = (int)memRead.ReadInt32();
                info = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(infoLen)));

                str = info.Split(';');
                count = str.Length;
                
                for (int i = 0; i < count; i++)
                {
                    if (str[i].Length == 0||string.IsNullOrEmpty(str[i]))
                    {
                        continue;
                    }
                    spl= str[i].Split(',');
                    FriendManager.Instance.UpdateFriendSend(spl[0]);
                }
                FriendManager.Instance.FriendInfoReset(receive, send);
                if (!FastOpenManager.Instance.IsFastAddFriend)
                {
                    FriendManager.Instance.ReceivedData();
                }
                else {
                    FastOpenManager.Instance.FriendDataReceiveed();//置标志位归为
                }
                
                break;
            default:
                break;
        }

        
    }
}

#endregion



#region 通知好友操作的另一方      


public enum FriendSendType
{
    ReceiveAdd = 1,
    ReceiveCancel = 2,
    ReceiveAgree = 3,
    ReceiveSend = 4,
    Delete=5,
}


/// <summary>
/// 通知好友操作的另一方,75
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class NotifyAddFriendList : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        FriendSendType optType = (FriendSendType)memRead.ReadByte();
        bool isReceive = Convert.ToBoolean(memRead.ReadByte());
        short len = memRead.ReadInt16();
        string name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(len)));
        string[] spl = name.Split(',');

        FriendVo vo = new FriendVo()
        {
            Name = spl[0],
            Level = int.Parse(spl[1]),
            VipLevel = int.Parse(spl[2]),
            Fighter = int.Parse(spl[3]),
            Career = (CHARACTER_CAREER)int.Parse(spl[4]),
            IsOnline = Convert.ToBoolean(int.Parse(spl[5])),
            IsSend = isReceive
        };
        FriendManager.Instance.FriendEventOpt(optType,vo);

    }
}
#endregion