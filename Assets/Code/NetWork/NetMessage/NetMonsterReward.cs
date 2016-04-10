using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;


public enum ZhuiJiType{
	Normal=0,
	Quick=1,
	OneClear=2
}

//追缉请求
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public  class GCAskZhuiJi : NetHead
{    
	sbyte _type;
	uint _monsterAwardId;
	
    public GCAskZhuiJi(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskZhuiJi;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(ZhuiJiType type,uint monsterAwardId)
    {
		_type = (sbyte)type;
		_monsterAwardId = (uint)monsterAwardId;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
		memWrite.Write(_type);
		memWrite.Write(_monsterAwardId);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求得到当前所有的的追缉次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public  class GCAskCurZhuiJiCount : NetHead
{    
    public GCAskCurZhuiJiCount(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskCurZhuiJiCount;
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


//服务器通知客户端当前追缉令剩余的追缉次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyZhuiJiCount : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
		uint num = memRead.ReadUInt32();
		for (int i = 0; i < num; i++) {
			uint id = memRead.ReadUInt32();
			int count =  memRead.ReadInt32();
			byte hasPass = memRead.ReadByte ();
			if (MonsterRewardManager.Instance.DicMonsterReward.ContainsKey((int)id)) {
				var monster = MonsterRewardManager.Instance.DicMonsterReward [(int)id];
				monster.CurrentClearNum = count;
				monster.HasPass = hasPass == 1 ? true : false;
			}
		}
       	
		MonsterRewardManager.Instance.MonsterRewardShow ();
    }
}
