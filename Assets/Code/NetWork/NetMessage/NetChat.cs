using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;

/// <summary>
/// 发送聊天信息 
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskChat : NetHead
{
	byte chanel;

    [MarshalAs(UnmanagedType.ByValArray,SizeConst=20)]
    byte[] _whipserName;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = TalkManager.MAX_MSG_LEN)]    
	byte[] msg;
	UInt16 len;
    public GCAskChat(TalkType type, string msg)
        : base()
    {
        this.chanel = (byte)type;
        this.msg = StringToByte(msg);
        this.len = (UInt16)this.msg.Length;
        NetHead head = new NetHead();
        int headLength = Marshal.SizeOf(head) - 2;
        this._assistantCmd = (UInt16)eC2GType.C2G_AskChat;
        this._length = (UInt16)(headLength + 1 +20+ 2 + this.len);
        _whipserName = StringToByte("null", 20);
    }
    public void SetWhisperName(string name)
    {
        _whipserName = StringToByte(name, 20);
    }

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);
		memWrite.Write (this.chanel);
        memWrite.Write(this._whipserName);
		memWrite.Write (this.len);
		memWrite.Write (this.msg);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}

/// <summary>
/// 接受聊天信息
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyChatInfo : NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);

        byte type = memRead.ReadByte();
        string sender = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
        ushort contentLen = memRead.ReadUInt16();
        byte order = memRead.ReadByte();
        byte loop = memRead.ReadByte();
        string content = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(contentLen)));

        TalkType chatType = (TalkType)type;
        if (chatType == TalkType.Post || chatType == TalkType.SystemAndPost)
        { //公告或者 公告与系统
			NoticeMsgVo msgVo = new NoticeMsgVo ();
            msgVo.From = sender;
            int msgLen = contentLen;
            msgVo.Order = order;
            msgVo.Num = loop;
            msgVo.Message = content;
			if (CommonNoticeManager.Instance.MsgList.size == 0) {
				CommonNoticeManager.Instance.MsgList.Add (msgVo);
			} else {
				bool isInsert = false;
				for (int i = 0; i < CommonNoticeManager.Instance.MsgList.size; i++) {
					if (msgVo.Order > CommonNoticeManager.Instance.MsgList [i].Order) {
						isInsert = true;
						CommonNoticeManager.Instance.MsgList.Insert (i, msgVo);
						break;
					}
				}
				if (!isInsert) { //如果没有插入过数据，则添加到末尾
					CommonNoticeManager.Instance.MsgList.Add (msgVo);
				}
			}
			CommonNoticeManager.Instance.ShowInfo ();
		} 

        TalkManager.Instance.AddContent((TalkType)type, sender, content);

        memRead.Close();
        memStream.Close();
	
        
	}
}
//GSNotifySelectResult

/// <summary>
/// 查询玩家结果
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifySelectPlayerResult : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
		byte b=memRead.ReadByte();
        bool isOnline = Convert.ToBoolean(b);
        TalkManager.Instance.SetWhisperResult(isOnline);
        memRead.Close();
        memStream.Close();


    }
}