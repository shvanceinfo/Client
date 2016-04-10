using System;
using System.Collections.Generic;
using NetGame;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using model;
using manager;
using System.Collections;
using MVC.entrance.gate;


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyEModongkuRank : NetHead
{
    public uint m_ui32ID;
    public int m_n32Schedule;
    public byte m_bIfCompleted;
    public byte m_bIfReceived;


    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        //uint nMyRank = memRead.ReadUInt32();
        //uint nMyMaxRankCounter = memRead.ReadUInt32();

        //uint nMyLastRank = memRead.ReadUInt32();
        //uint nMyLastMaxRankCounter = memRead.ReadUInt32();

        //EModongkuManager.sEModongkuManager.SetMyRankInfo((int)nMyRank, (int)nMyMaxRankCounter, (int)nMyLastRank, (int)nMyLastMaxRankCounter);

        //int nRankNum = (int)memRead.ReadUInt32();

        //for (int i = 0; i < nRankNum; ++i)
        //{
        //    string strRoleName = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
        //    uint nMaxCounter = memRead.ReadUInt32();

        //    EModongkuManager.sEModongkuManager.SetEmodongkuRank(i+1, strRoleName, (int)nMaxCounter);
        //}

        //EModongkuManager.sEModongkuManager.InitUIData();
        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCNotifyEModongkuAward : NetHead
{
    public uint m_un32CurTitleID;

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        m_un32CurTitleID = memRead.ReadUInt32();

        memRead.Close();
        memStream.Close();
    }
}



[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCNotifyTowerInfo : NetHead
{ 
//                            GS通知爬塔信息
//        u16NormalGate	    uint16			通关的噩梦最大波数(0表示当天没有通过任何一波)
//        u16HardGate	    uint16			通关的地狱最大波数(0表示当天没有通过任何一波)
//        u16DevilGate	    uint16			通关的炼狱最大波数(0表示当天没有通过任何一波)
//        u32NormalTowerID	uint32			噩梦解锁的ID
//        u32HardTowerID	uint32			地狱解锁的ID
//        u32DevilTowerID	uint32			炼狱解锁的ID
//        u16AwardNum	    uint16			普通挑战目标
//        u32TowerID	    uint32			噩梦挑战目标ID
//        n8Type	        int8			0代表已经领取过,1代表未领取过
//        u16AwardNum	    uint16			地狱挑战目标
//        u32TowerID	    uint32			地狱挑战目标ID
//        n8Type	        int8			0代表已经领取过,1代表未领取过
//        u16AwardNum	    uint16			炼狱挑战目标
//        u32TowerID	    uint32			炼狱挑战目标ID
//        n8Type	        int8			0代表已经领取过,1代表未领取过

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        DemonInfoVo vo = DemonManager.Instance.InfoVo;
        vo.CurGate[DemonDiffEnum.Level1] = (int)memRead.ReadInt16();
        vo.CurGate[DemonDiffEnum.Level2] = (int)memRead.ReadInt16();
        vo.CurGate[DemonDiffEnum.Level3] = (int)memRead.ReadInt16();
        vo.MaxGate[DemonDiffEnum.Level1] = (int)memRead.ReadInt32();
        vo.MaxGate[DemonDiffEnum.Level2] = (int)memRead.ReadInt32();
        vo.MaxGate[DemonDiffEnum.Level3] = (int)memRead.ReadInt32();

        int awardNum = (int)memRead.ReadInt16();
        for (int i = 0; i < awardNum; i++)
        {
            int id = (int)memRead.ReadUInt32();
            int opt = memRead.ReadByte();
            bool b=true;
            if (opt == 0)
            {
                b = true;
            }
            else if(opt==1) {
                b = false;
            }
            DemonManager.Instance.AddAwardInfo(DemonDiffEnum.Level1, id, b);
        }
        awardNum = (int)memRead.ReadInt16();
        for (int i = 0; i < awardNum; i++)
        {
            int id = (int)memRead.ReadUInt32();
            int opt = memRead.ReadByte();
            bool b = true;
            if (opt == 0)
            {
                b = true;
            }
            else if (opt == 1)
            {
                b = false;
            }
            DemonManager.Instance.AddAwardInfo(DemonDiffEnum.Level2, id, b);
        }
        awardNum = (int)memRead.ReadInt16();
        for (int i = 0; i < awardNum; i++)
        {
            int id = (int)memRead.ReadUInt32();
            int opt = memRead.ReadByte();
            bool b = true;
            if (opt == 0)
            {
                b = true;
            }
            else if (opt == 1)
            {
                b = false;
            }
            DemonManager.Instance.AddAwardInfo(DemonDiffEnum.Level3, id, b);
        }
        DemonManager.Instance.OpenWindow();
        memRead.Close();
        memStream.Close();
    }
}


[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCNotifyHistoryRank : NetHead
{ 
    //    u16NormalRank	uint16			噩梦排名(0表示没有排名)
    //u16NormalGate	uint16			噩梦排名的最大爬塔数
    //n8NormalGetReward	int8			噩梦排名是否已经领取过(0代表已经领取过,1代表未领取过)
    //u16HardRank	uint16			地狱排名(0表示没有排名)
    //u16HardGate	uint16			地狱排名的最大爬塔数
    //n8HardGetReward	int8			地狱排名是否已经领取过(0代表已经领取过,1代表未领取过)
    //u16DevilRank	uint16			炼狱排名(0表示没有排名)
    //u16DevilGate	uint16			炼狱排名的最大爬塔数
    //n8DevilGetReward	int8			炼狱排名是否已经领取过(0代表已经领取过,1代表未领取过)

    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        DemonManager.Instance.AddHistoryRankData(DemonDiffEnum.Level1, (int)memRead.ReadUInt16(), (int)memRead.ReadUInt16(), (int)memRead.ReadByte());
        DemonManager.Instance.AddHistoryRankData(DemonDiffEnum.Level2, (int)memRead.ReadUInt16(), (int)memRead.ReadUInt16(), (int)memRead.ReadByte());
        DemonManager.Instance.AddHistoryRankData(DemonDiffEnum.Level3, (int)memRead.ReadUInt16(), (int)memRead.ReadUInt16(), (int)memRead.ReadByte());

        //打开昨日排名奖励
        Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_RANK, true);
        memRead.Close();
        memStream.Close();
    }
}

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class GCNotifyCurRank:NetHead
{ 
    //68							GS通知今日排名
    //        u16NormalRank	        uint16			今日噩梦排名(0表示没有排名)
    //        u16HardRank	        uint16			今日地狱排名(0表示没有排名)
    //        u16DevilRank	        uint16			今日炼狱排名(0表示没有排名)
    //        u16NormalRankNum	    uint16					噩梦排行榜的列表
    //        u16Rank	            uint16			玩家排名
    //        n8Career	            int8			角色职业
    //        u16Level	            uint16			角色等级
    //        strName	            string	20		角色名
    //        u16MaxGate	        uint16			最大爬塔数
    //        u16HardRankNum	    uint16					地狱排行榜的列表
    //        u16Rank	            uint16			玩家排名
    //        n8Career	            int8			角色职业
    public void ToObject(byte[] byteData)
    {
        MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);

        Hashtable curRank = DemonManager.Instance.InfoVo.CurRank;
        curRank[DemonDiffEnum.Level1] = (int)memRead.ReadUInt16();
        curRank[DemonDiffEnum.Level2] = (int)memRead.ReadUInt16();
        curRank[DemonDiffEnum.Level3] = (int)memRead.ReadUInt16();

        DemonManager.Instance.ClearCurRank();   //清除旧数据
        int num = 0;
        num = (int)memRead.ReadUInt16();
        for (int i = 0; i < num; i++)
        {
            int rank = (int)memRead.ReadUInt16();
            CHARACTER_CAREER career = (CHARACTER_CAREER)memRead.ReadByte();
            int lvl = (int)memRead.ReadUInt16();
            string name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
            int gate = (int)memRead.ReadUInt16();
            DemonManager.Instance.AddCurRank(DemonDiffEnum.Level1, rank, career, lvl, name, gate);
        }

        num = (int)memRead.ReadUInt16();
        for (int i = 0; i < num; i++)
        {
            int rank = (int)memRead.ReadUInt16();
            CHARACTER_CAREER career = (CHARACTER_CAREER)memRead.ReadByte();
            int lvl = (int)memRead.ReadUInt16();
            string name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
            int gate = (int)memRead.ReadUInt16();
            DemonManager.Instance.AddCurRank(DemonDiffEnum.Level2, rank, career, lvl, name, gate);
        }

        num = (int)memRead.ReadUInt16();
        for (int i = 0; i < num; i++)
        {
            int rank = (int)memRead.ReadUInt16();
            CHARACTER_CAREER career = (CHARACTER_CAREER)memRead.ReadByte();
            int lvl = (int)memRead.ReadUInt16();
            string name = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20)));
            int gate = (int)memRead.ReadUInt16();
            DemonManager.Instance.AddCurRank(DemonDiffEnum.Level3, rank, career, lvl, name, gate);
        }

        //显示今日排名
        Gate.instance.sendNotification(MsgConstant.MSG_DEMON_DISPLAY_CUR_RANK, true);
        memRead.Close();
        memStream.Close();
    }

}