using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using manager;


public enum GOBLIN_TIME_CHANGE_REASON
{
    GTCR_ENTER = 0,
    GTCR_DAILY_RESET,
    GTCR_SHOP_BUY,
    GTCR_GATE_GET,
}

//请求进入黄金哥布林
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskEnterGoldenGoblinInstance : NetHead
{
    public GCAskEnterGoldenGoblinInstance()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskEnterGoblin;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}


//请求哥布林剩余次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskGoldenGoblinTimes : NetHead
{
    public GCAskGoldenGoblinTimes()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskGoblinTimes;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}




//请求哥布林多倍收益
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskGoldenGoblinMultiBenefit : NetHead
{
    public UInt32 m_un32BaseRevenue;
    public UInt32 m_un32Times;

    public GCAskGoldenGoblinMultiBenefit()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskGoblinMultiBenefit;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        memWrite.Write(m_un32BaseRevenue);
        memWrite.Write(m_un32Times);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}




//请求购买哥布林剩余次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCAskBuyGoldenGoblinTimes : NetHead
{
    public GCAskBuyGoldenGoblinTimes()
        : base()
    {
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBuyGoblinTimes;
    }

    public byte[] ToBytes()
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.UTF8);
        base.ToBytes(ref memWrite);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}



// 回复剩余的次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyGoblinRemainTimes : NetHead
{
    public uint m_ui32RemainTimes;
    public int m_nReason;
    public uint m_nTodayBuy;

    public GSNotifyGoblinRemainTimes()
        : base()
    {
    }

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        m_ui32RemainTimes = memRead.ReadUInt32();
        m_nReason = memRead.ReadInt32();
        m_nTodayBuy = memRead.ReadUInt32();


        switch ((GOBLIN_TIME_CHANGE_REASON)m_nReason)
        {
        case GOBLIN_TIME_CHANGE_REASON.GTCR_ENTER:
            {
				GoblinManager.Instance.InitGoblin(m_ui32RemainTimes,m_nTodayBuy);
                //UIGoldenGoblin.sUIGoldenGoblin.SetInitGoblinData((int)m_ui32RemainTimes, (int)m_nTodayBuy);
            }
        	break;
        case GOBLIN_TIME_CHANGE_REASON.GTCR_DAILY_RESET:
            {

            }
            break;
        case GOBLIN_TIME_CHANGE_REASON.GTCR_SHOP_BUY:
            {
				MessageManager.Instance.SendAskGoldenGoblinTimes (); //重新初始化一次
                //GoblinManager.Instance.InitGoblin(m_ui32RemainTimes,m_nTodayBuy);
                //GoblinManager.Instance.UpdateGoblinInfo(m_ui32RemainTimes,m_nTodayBuy); //更新信息
				//MessageManager.Instance.SendAskEnterGoldenGoblin();// 购买成功，直接进去
            }
            break;
        case GOBLIN_TIME_CHANGE_REASON.GTCR_GATE_GET:
            {

            }
            break;
        }

     
        

        memRead.Close();
        memStream.Close();
    }
	
	 
	
	
}
