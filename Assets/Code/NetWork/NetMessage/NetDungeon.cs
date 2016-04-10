using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;

//请求创建队伍
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskCreateTeam : NetHead
{   
	uint MultiRaidID;

	public GCAskCreateTeam (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_AskCreateTeam;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		MultiRaidID = 0;
	}

	public byte[] ToBytes (uint raidId)
	{
		this.MultiRaidID = raidId; 
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		memWrite.Write (this.MultiRaidID);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}

//请求预览多人副本关卡
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCPreviewMulti : NetHead
{   
	uint MultiRaidID;

	public GCPreviewMulti (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_PreviewMulti;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		MultiRaidID = 0;
	}

	public byte[] ToBytes (uint raidId)
	{
		this.MultiRaidID = raidId; 
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		memWrite.Write (this.MultiRaidID);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}

//请求加入多人副本组
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskAddTeam : NetHead
{   
	uint TeamID;

	public GCAskAddTeam (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_AskAddTeam;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
		TeamID = 0;
	}

	public byte[] ToBytes (uint teamId)
	{
		this.TeamID = teamId; 
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		memWrite.Write (this.TeamID);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}


//请求快速加入副本
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCQuickAddTeam : NetHead
{   
	public GCQuickAddTeam (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_QuickAddTeam;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}



//请求开始多人副本战斗
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBeginFightMulti : NetHead
{   
	public GCAskBeginFightMulti (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_AskBeginFightMulti;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}


//请求发送多人组队消息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCSendFormTeamMsg : NetHead
{   
	public GCSendFormTeamMsg (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_SendFormTeamMsg;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}


//请求离开多人副本队伍
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskLeaveTeam : NetHead
{   
	public GCAskLeaveTeam (): base()
	{
//		NetHead head = new NetHead ();
		this._assistantCmd = (UInt16)eC2GType.C2G_AskLeaveTeam;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
	}

	public byte[] ToBytes ()
	{
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}




//通知多人副本信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyMultiInfo : NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		DungeonManager.Instance.DungeonVo.PassNum = memRead.ReadUInt16 (); //当前的通关次数
		
		uint num = memRead.ReadUInt16 ();
		DungeonManager.Instance.DungeonVo.DungeonTeamList.Release ();
		if (num > 0) {			
			for (int i = 0; i < num; i++) {
				DungeonTeamInfo dTeamInfo = new DungeonTeamInfo ();
				dTeamInfo.name = Global.FromNetString (Encoding.UTF8.GetChars (memRead.ReadBytes (20))); //角色名
				dTeamInfo.playerNum = memRead.ReadUInt16 ();
				dTeamInfo.teamId = memRead.ReadUInt32 ();
				DungeonManager.Instance.DungeonVo.DungeonTeamList.Add (dTeamInfo);
			}
		}
		memRead.Close();
		memStream.Close();
		DungeonManager.Instance.InitDungeon (); //初始化副本信息
	}
}



//通知当前副本的队伍信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyTeamInfo : NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);

        BattleMultiPlay.GetInstance().m_un32TeamID = memRead.ReadUInt32();

		uint num = memRead.ReadUInt16 (); //当前玩的人数
		DungeonManager.Instance.DungeonVo.PeopleList.Release ();
		if (num > 0) {			
			for (int i = 0; i < num; i++) {
				PeopleInfo people = new PeopleInfo ();
				people.name = Global.FromNetString (Encoding.UTF8.GetChars (memRead.ReadBytes (20))); //角色名
				people.level = memRead.ReadUInt16 ();
				people.career = (CHARACTER_CAREER)memRead.ReadByte ();
				people.gender = memRead.ReadByte ();
				people.battlePower = memRead.ReadUInt32 ();
				byte leader = memRead.ReadByte ();
				if (leader == 0x00) {
					people.leader = false;
				} else {
					people.leader = true;
				}
				#region 如果自己是队长则设置为主机,否则为副机
				if (CharacterPlayer.character_property.nick_name == people.name) {
					if (people.leader) 
                    {
						CharacterPlayer.character_property.setHostComputer (true);
					} else {
						CharacterPlayer.character_property.setHostComputer (false);
					}
				}
				#endregion

				DungeonManager.Instance.DungeonVo.PeopleList.Add (people);
			}
		}
		memRead.Close();
		memStream.Close();
//		if (Global.inMultiFightMap()) {
//			return;
//		}
		TipsManager.Instance.CloseAllTipsUI();	//关闭所有tips
		DungeonManager.Instance.CloseDungeonUI();//关闭多人副本界面
		DungeonManager.Instance.ShowDungeonInfoView ();//显示多人副本详细界面
		DungeonManager.Instance.InitDungeonInfo ();	  //初始化多人副本详细界面的内容
	}
}


//汇报多人副本角色死亡
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportPlayerDie : NetHead
{
    UInt32 m_un32TeamID;
    UInt32 m_un32PlayerID;

    public GCReportPlayerDie(UInt32 un32TeamID, UInt32 un32PlayerID) : base()
    {
//        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportMultiPlayerDie;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);

        m_un32TeamID = un32TeamID;
        m_un32PlayerID = un32PlayerID;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);//把一些基本信息写入流中
        memWrite.Write(m_un32TeamID);
        memWrite.Write(m_un32PlayerID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


//通知多人副本开始倒计时
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyStartCountDown : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        memRead.Close();
        memStream.Close();
		
		DungeonManager.Instance.ShowCD();//开始倒计时
    }
}


//通知多人副本在倒计时长有玩家掉线
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyStartCountDownPlayerOffLine : NetHead
{
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        memRead.Close();
        memStream.Close();
    }
}


//通知多人副本结果
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyMultiResult : NetHead
{
    public byte m_n8Result;

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        m_n8Result = memRead.ReadByte();
        memRead.Close();
        memStream.Close();
    }
}



//通知多人副本玩家复活
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyPlayerRelive : NetHead
{
    public UInt32 m_un32PlayerID;
    public float m_fPosX;
    public float m_fPosY;
    public float m_fPosZ;
    public float m_fDirX;
    public float m_fDirY;
    public float m_fDirZ;

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        m_un32PlayerID = memRead.ReadUInt32();
        m_fPosX = memRead.ReadSingle();
        m_fPosY = memRead.ReadSingle();
        m_fPosZ = memRead.ReadSingle();
        m_fDirX = memRead.ReadSingle();
        m_fDirY = memRead.ReadSingle();
        m_fDirZ = memRead.ReadSingle();
        memRead.Close();
        memStream.Close();
    }
}


 





