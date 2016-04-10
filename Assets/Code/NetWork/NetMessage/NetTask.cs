using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// 请求接受任务
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAcceptTask : NetHead
{
    UInt32 taskID; //任务ID
    
    public GCAcceptTask(): base()
    {
        this.taskID = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AcceptTask;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint taskID)
    {
    	this.taskID = (UInt32)taskID;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.taskID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

// 请求提交任务
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCSubmitTask : NetHead
{
    UInt32 taskID; //任务ID
    
    public GCSubmitTask(): base()
    {
        this.taskID = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_SubmitTask;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint taskID)
    {
    	this.taskID = (UInt32)taskID;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.taskID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//GC汇报任务进度
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportTaskProgress : NetHead
{
    UInt32 taskID; //任务ID
    int param1;
	int param2;
	int param3;	
    
    public GCReportTaskProgress(): base()
    {
        //this.taskID = 0;
        //this.param1 = 0;
        //this.param2 = 0;
        //this.param3 = 0;
        //NetHead head = new NetHead();
        //this._assistantCmd = (UInt16)eC2GType.C2G_ReportTaskProgress;
        //this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    //public byte[] ToBytes(uint taskID, object[] checkParams)
    //{
    //    this.taskID = (UInt32)taskID;
    //    if (checkParams.Length > 0)
    //        this.param1 = int.Parse(checkParams[0].ToString());
    //    else
    //        this.param1 = 0;
    //    if (checkParams.Length > 1)
    //        this.param1 = int.Parse(checkParams[1].ToString());
    //    else
    //        this.param1 = 0;
    //    if (checkParams.Length > 2)
    //        this.param1 = int.Parse(checkParams[2].ToString());
    //    else
    //        this.param1 = 0;
    //    MemoryStream memStream = new MemoryStream();
    //    BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
    //    base.ToBytes(ref memWrite);
    //    memWrite.Write(this.taskID);
    //    memWrite.Write(this.param1);
    //    memWrite.Write(this.param2);
    //    memWrite.Write(this.param3);
    //    byte[] bytesData = memStream.ToArray();
    //    memWrite.Close();
    //    return bytesData;
    //}
}

//GC汇报任务进度
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportScenario : NetHead
{
    UInt32 taskID; //任务ID
    UInt32 scenarioID;
    UInt32 scenarioStep; //
    
    public GCReportScenario(): base()
    {
        this.taskID = 0;
        this.scenarioID = 0;
        this.scenarioStep = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportScenario;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint taskID, uint scenarioID, int step)
    {
    	this.taskID = (UInt32)taskID;
    	this.scenarioID = (UInt32)scenarioID;
    	this.scenarioStep = (UInt32)step;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.taskID);
        memWrite.Write(this.scenarioID);
        memWrite.Write(this.scenarioStep);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//服务器返回所有的任务信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyTaskInfo : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32();
        if(num == 0)
        {
        	TaskManager.Instance.processTask(num);
        } 
        else
        {
	    	uint[] taskIDs = new uint[num];
	    	int[] taskStates = new int[num];
	    	uint[] scenarioIDs = new UInt32[num];
	    	UInt16[] scenarioSteps = new UInt16[num];
	    	BetterList<uint[]> conditionList = new BetterList<uint[]>();
	    	BetterList<int[]> conditionNumList = new BetterList<int[]>();
	        for (int i = 0; i < num; i++)
	        {
	        	taskIDs[i] = memRead.ReadUInt32();
	        	taskStates[i] = (int)memRead.ReadUInt16();
	        	scenarioIDs[i] = memRead.ReadUInt32();
	        	scenarioSteps[i] = memRead.ReadUInt16();
	        	int conditionCount = (int)memRead.ReadUInt16();
	        	uint[] conditonIds = new UInt32[conditionCount];
	    		int[] conditionNums = new int[conditionCount];
	        	for(int j=0; j<conditionCount; j++)
	        	{
	        		conditonIds[j] = memRead.ReadUInt32();
					conditionNums[j] = memRead.ReadInt32();
	        	}
	        	conditionList.Add(conditonIds);
	        	conditionNumList.Add(conditionNums);
			}
	        TaskManager.Instance.processTask(num, taskIDs, taskStates, conditionList, conditionNumList, scenarioIDs);
        }
		memRead.Close ();
    }
}