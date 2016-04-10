/**该文件实现的基本功能等
function: 实现扫荡跟服务端的协议
author:ljx
date:2013-11-09
**/
using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
//客户端请求扫荡
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBeginSweep : NetHead
{    
	UInt32 tollgateID;
    public GCAskBeginSweep(): base()
    {
    	tollgateID = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBeginSweep;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint tollgateID)
    {
    	this.tollgateID = (UInt32)tollgateID;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.tollgateID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求停止扫荡
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskStopSweep : NetHead
{
    public GCAskStopSweep(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskStopSweep;
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

//请求加速扫荡
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskAccelerateSweep : NetHead
{ 
    int sweepNum;
    public GCAskAccelerateSweep(): base()
    {
    	sweepNum = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskAccelerateSweep;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(int sweepNum)
    {
    	this.sweepNum = sweepNum;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.sweepNum);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//回复扫荡结果
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifySweepResult : NetHead
{	
    public GSNotifySweepResult()
    {    
    }

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        UInt16 num = memRead.ReadUInt16();
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				UInt32  typeID = memRead.ReadUInt32();
				UInt32  itemNum = memRead.ReadUInt32();
				SweepManager.Instance.setSweepResult(true, (int)itemNum, (uint)typeID);
			}			
		}  
		SweepManager.Instance.setNextSweep(); //设置下一次扫荡
        memRead.Close();
        memStream.Close();
    }
}
