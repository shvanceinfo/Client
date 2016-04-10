using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;


 
 
//	GSNotifyPandoraInfo
 


//前往挑战潘多拉
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCChallengePandora : NetHead
{    
	public GCChallengePandora(): base()
	{
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_ChallengePandora;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes()
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}


//重置潘多拉挑战次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCResetPandoraNum : NetHead
{    
	ushort resetNum;
	public GCResetPandoraNum(): base()
	{
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_ResetPandoraNum;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes(ushort resetNum)
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		memWrite.Write (resetNum);

		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}

//全部挑战
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCChallengeAllPandora : NetHead
{    
	ushort challengeNum;
	public GCChallengeAllPandora(): base()
	{
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_ChallengeAllPandora;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes(ushort challengeNum)
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		memWrite.Write (challengeNum);
		
		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}

//开启潘多拉宝盒
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCOpenPandora : NetHead
{    
	public GCOpenPandora(): base()
	{
		NetHead head = new NetHead();
		this._assistantCmd = (UInt16)eC2GType.C2G_ChallengePandora;
		this._length = (UInt16)(Marshal.SizeOf(this) - 2);
	}
	
	public byte[] ToBytes()
	{
		MemoryStream memStream = new MemoryStream();
		BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
		base.ToBytes(ref memWrite);
		byte[] bytesData = memStream.ToArray();
		memWrite.Close();
		return bytesData;
	}
}


//服务器通知客户端潘多拉数据
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyPandoraInfo : NetHead
{
	public void ToObject(byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream(byteData);
		BinaryReader memRead = new BinaryReader(memStream);
		base.ToObject(ref memRead);

		ushort challengeNum = memRead.ReadUInt16 ();
		ushort resetNum = memRead.ReadUInt16 ();
		ushort openNum = memRead.ReadUInt16 ();

		memRead.Close ();


	}
}


