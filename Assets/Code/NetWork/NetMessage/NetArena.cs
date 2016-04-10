using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;
using UnityEngine;

//请求竞技场信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskArenaInfo : NetHead
{   
    public GCAskArenaInfo(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskArenaInfo;
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

//请求挑战竞技场
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskChallenge : NetHead
{
    UInt32 challengerPos; 
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]       
    byte[] roleName;		//挑战角色名称
    
    public GCAskChallenge(): base()
    {
        challengerPos = 0;
        roleName = null;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskChallenge;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint challengerPos, string roleName)
    {
    	this.challengerPos = (UInt32)challengerPos;
    	this.roleName = StringToByte(roleName, 20);      
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.challengerPos);
        memWrite.Write(this.roleName);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求英雄榜列表
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskHeroRankList : NetHead
{   
    public GCAskHeroRankList(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskHeroList;
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

//汇报竞技场战斗结果
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCReportChallengeResult : NetHead
{   
	byte result;
	
    public GCReportChallengeResult(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_ReportChallengeResult;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(bool success)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        if(success)
        	this.result = 0x01;
        else
        	this.result = 0x00;
        memWrite.Write(this.result);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求购买挑战次数
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBuyChallengeNum : NetHead
{   
    public GCAskBuyChallengeNum(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBuyChallengeNum;
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

//请求荣誉商店信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskHonorShopInfo : NetHead
{   
    public GCAskHonorShopInfo(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskHonorShopInfo;
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

//请求购买荣誉商店物品
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskBuyHonorItem : NetHead
{   
	UInt32 productID;
	
    public GCAskBuyHonorItem(): base()
    {
    	productID = 0;
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskBuyHonorItem;
        this._length = (UInt16)(Marshal.SizeOf(this) - 2);
    }

    public byte[] ToBytes(uint productID)
    {
    	this.productID = (UInt32)productID;
        MemoryStream memStream = new MemoryStream();
        BinaryWriter memWrite = new BinaryWriter(memStream, Encoding.GetEncoding("utf-8"));
        base.ToBytes(ref memWrite);
        memWrite.Write(this.productID);
        byte[] bytesData = memStream.ToArray();
        memWrite.Close();
        return bytesData;
    }
}

//请求清除技能CD
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskClearChallengeCD : NetHead
{   
    public GCAskClearChallengeCD(): base()
    {
        NetHead head = new NetHead();
        this._assistantCmd = (UInt16)eC2GType.C2G_AskClearChallengeCD;
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

//服务器通知竞技场信息，将竞技场信息存储，后续列表变化进行处理
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyArenaInfo : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        ArenaInfo info = ArenaManager.Instance.ArenaVo.ArenaInfo;
        info.currentRank = (uint)memRead.ReadUInt32(); //当前竞技场排名
        info.continuousWin = (uint)memRead.ReadUInt32(); //连胜场数
        info.currentHonor = (uint)memRead.ReadUInt32(); //当前荣誉值
        info.totalHonor = (uint)memRead.ReadUInt32(); //总荣誉值
        info.honorLevel = (uint)memRead.ReadUInt32(); //荣誉等级
        info.remainChallengeNum = (uint)memRead.ReadUInt32(); //剩余挑战次数
        info.buyChallengNum = (uint)memRead.ReadUInt32(); //购买挑战次数
        info.remainTime = (uint)memRead.ReadUInt32(); //挑战冷却时间(秒)
        info.lessReceiveTime = memRead.ReadInt32();
        info.isReceiveed = Convert.ToBoolean(memRead.ReadByte());
        ArenaManager.Instance.setCDAndNum();
    }
}

//服务器返回挑战列表变化
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyChallengerList : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32(); //可挑战数量
        if(num > 0)
        {
	    	ArenaManager.Instance.ArenaVo.ChallengerList.Clear(); //先清除原来的挑战列表
	        for (int i = 0; i < num; i++)
	        {
	        	ChallengeSigle challenge = new ChallengeSigle();
	        	challenge.rank = memRead.ReadUInt32(); //排名
	        	challenge.roleName = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20))); //角色名
	        	challenge.level = memRead.ReadUInt32(); //等级
	        	byte gender = memRead.ReadByte();  //性别(0:女;1:男)
	        	if(gender == 0x00)
	        		challenge.gender = false;
	        	else
	        		challenge.gender = true;
	        	challenge.suitID = memRead.ReadUInt32(); 	//服装类型ID
	        	challenge.weaponID = memRead.ReadUInt32(); //巨剑类型ID
                //challenge.rowID = memRead.ReadUInt32(); 	//弓类型ID
                //challenge.staffID = memRead.ReadUInt32(); //双剑类型ID
	        	challenge.wingID = memRead.ReadUInt32(); 	//翅膀类型ID
	        	challenge.vocation = memRead.ReadInt32(); //职业ID
	        	challenge.power = memRead.ReadUInt32();	//战斗力
	        	ArenaManager.Instance.ArenaVo.ChallengerList.Add(challenge);
			}
	        ArenaManager.Instance.ArenaVo.ChallengerList.Sort(ChallengeSigle.compare); //先排序
        }
        if (SceneManager.Instance.currentScene != SCENE_POS.IN_ARENA
            && !Constant.LOADING_SCENE.Equals(Application.loadedLevelName)) //不在竞技场
        {
            //Debug.LogError("Net Open ArenaUI");
            ArenaManager.Instance.showArenaUI(); //显示竞技场的UI
        }
    }
}

//通知英雄榜列表
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyHeroList : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        ArenaManager.Instance.ArenaVo.HeroList.Clear();
        uint num = memRead.ReadUInt32();
        if(num > 0) //必须存在英雄榜
        {			
	        for (int i = 0; i < num; i++)
	        {
				HeroListInfo heroInfo = new HeroListInfo();
	        	heroInfo.challengerName = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20))); //角色名
	        	heroInfo.level = memRead.ReadUInt32();  //等级
	        	heroInfo.powerStrength = memRead.ReadUInt32(); //战斗力
	        	heroInfo.trend = memRead.ReadInt32();		//趋势(0:无变化)
	        	int vocation = memRead.ReadInt32(); //角色职业
	        	heroInfo.vocation = (CHARACTER_CAREER)vocation;
	        	ArenaManager.Instance.ArenaVo.HeroList.Add(heroInfo);
			}
        }
        ArenaManager.Instance.showHeroList();
    }
}

//服务器通知战报信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyArenaResult : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32(); //战报数量
//        ArenaManager.Instance.ArenaVo.ResultList.Clear(); //清除原来战报
        if(num > 0)
        {
            ArenaManager.Instance.ArenaVo.ResultList.Clear();
	        for (int i = 0; i < num; i++)
	        {
	        	ResultInfo result = new ResultInfo();
	        	result.roleName = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20))); //对方角色名
	        	byte fight = memRead.ReadByte(); //战斗类型(0:挑战,1:被挑战)
	        	if(fight == 0x00)
	        		result.beFight = false;
	        	else
	        		result.beFight = true;
	        	fight = memRead.ReadByte(); //战斗结果(0:失败,1:胜利)
	        	if(fight == 0x00)
	        		result.fightResult = false;
	        	else
	        		result.fightResult = true;
	        	result.varyRank = (int)memRead.ReadInt16(); //排名变化数
	        	result.newRank = memRead.ReadUInt32(); 	//新排名
	        	uint fightTime = memRead.ReadUInt32();	//挑战的时间
				DateTime dt=new DateTime(fightTime);

				result.Time=TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1)).AddSeconds(fightTime);
	        	ArenaManager.Instance.ArenaVo.ResultList.Add(result);
                ArenaManager.Instance.ArenaVo.ResultList.Sort(
                    (ResultInfo r1, ResultInfo r2) =>
                    {
                        if (r1.Time<r2.Time)
                        {
                            return 1;
                        }
                        return -1;
                    });
			}
	        //ArenaManager.Instance.showChallengeResult();
        }
    }
}

//通知荣誉商店列表
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyShopInfo : NetHead
{
    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        uint num = memRead.ReadUInt32(); //商品数量
        ArenaManager.Instance.ArenaVo.HonorItemList.Clear();
        if(num > 0)
        {
	        for (int i = 0; i < num; i++)
	        {
				HonorItem item = new HonorItem();
	        	item.id = memRead.ReadUInt32(); //商品类型ID
	        	item.itemPrice = memRead.ReadUInt32(); //荣誉价格
				ArenaManager.Instance.ArenaVo.HonorItemList.Add(item);
			}
        }
        ArenaManager.Instance.showHonorShop();
    }
}

//通知竞技场挑战角色基础信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyChallegerInfo : NetHead
{
    public string roleName;
    public uint vocationID;
    public uint level;
    public uint suitID;
    public uint legID;
    public uint shoeID;
    public uint necklaceID;
    public uint ringID;
    public uint weaponID;
    //public uint bowID;
    //public uint staffID;
    public CharacterFightProperty m_fightProperty = new CharacterFightProperty();
    public uint curHp;
    public uint curMp;
    public uint skill1Id;
    public uint skill2Id;
    public uint skill3Id;
    public uint skill4Id;
    public uint wingId;


    public void ToObject(byte[] byteData)
    {
    	MemoryStream memStream = new MemoryStream(byteData);
        BinaryReader memRead = new BinaryReader(memStream);
        base.ToObject(ref memRead);
        roleName = Global.FromNetString(Encoding.UTF8.GetChars(memRead.ReadBytes(20))); //角色名
        vocationID = (uint)memRead.ReadInt32(); //职业ID
        level = (uint)memRead.ReadInt32(); //等级
        suitID = (uint)memRead.ReadUInt32(); //衣服物品类型ID
        legID = (uint)memRead.ReadUInt32(); //护腿物品类型ID
        shoeID = (uint)memRead.ReadUInt32(); //鞋物品类型ID
        necklaceID = (uint)memRead.ReadUInt32(); //项链物品类型ID
        ringID = (uint)memRead.ReadUInt32(); //指环物品类型ID
        weaponID = (uint)memRead.ReadUInt32(); //巨剑物品类型ID
        //bowID = (uint)memRead.ReadUInt32(); //弓物品类型ID
        //staffID = (uint)memRead.ReadUInt32(); //双剑物品类型ID
        int length = (int)eFighintPropertyCate.eFpc_End; //战斗属性
        for (int i = 1; i < length; i++)
        {
            m_fightProperty.fightData.Add((eFighintPropertyCate)i, memRead.ReadInt32());
        }
        curHp = (uint)memRead.ReadUInt32(); // 血
        curMp = (uint)memRead.ReadUInt32(); // 蓝
        skill1Id = (uint)memRead.ReadUInt32(); // 技能1
        skill2Id = (uint)memRead.ReadUInt32(); // 技能2
        skill3Id = (uint)memRead.ReadUInt32(); // 技能3
        skill4Id = (uint)memRead.ReadUInt32(); // 技能4

        wingId = (uint)memRead.ReadUInt32(); // 翅膀
    }
}