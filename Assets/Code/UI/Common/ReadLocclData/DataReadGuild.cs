using UnityEngine;
using System.Collections;
using manager;
using model;
using helper;
using System;

/// <summary>
/// GuildBase
/// </summary>
public class DataReadGuildBase : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        GuildBaseVo vo;
        if (GuildManager.Instance.XmlGuildBase.ContainsKey(key))
        {
            vo=GuildManager.Instance.XmlGuildBase[key] as GuildBaseVo;
        }else{
            vo = new GuildBaseVo();
            GuildManager.Instance.XmlGuildBase.Add(key, vo);
        }
        string[] sps;
        char sl = ',';
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Level":
                vo.Level = int.Parse(value);
                GuildManager.Instance.CenterGetServderData(vo.Level);
                break;
            case "MemberNum":
                vo.MaxMember = int.Parse(value);
                break;
            case "OfficerNum":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.Offices.Add(new OfficeMember {
                     Type=(GuildOffice)int.Parse(sps[i]),
                     MaxMember = int.Parse(sps[i+1])
                    });
                }
                break;
            case "GongHuiFlagMax":
                vo.MaxFlagLevel = int.Parse(value);
                break;
            case "GongHuiShopMax":
                vo.MaxShopLevel = int.Parse(value);
                break;
            case "GongHuiSkillMax":
                vo.MaxSkillLevel = int.Parse(value);
                break;
            case "AcceptNum_apply":
                vo.MaxAcceptCount = int.Parse(value);
                break;
            case "ContributeLv1":
                sps = value.Split(sl);
                vo.Donates.Add(DonateLevel.Low, new TypeStruct(
                    int.Parse(sps[1]),
                    (ConsumeType)int.Parse(sps[0]),
                    int.Parse(sps[2])
                    ));
                break;
            case "ContributeLv1_reward":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i+=3)
                {
                    vo.Awards[DonateLevel.Low].Add(new TypeStruct { 
                      Type=(ConsumeType)int.Parse(sps[i]),
                       Id=int.Parse(sps[i+1]),
                        Value=int.Parse(sps[i+2])
                    });
                }

                break;
            case "ContributeLv2":
                sps = value.Split(sl);
                vo.Donates.Add(DonateLevel.Middle, new TypeStruct(
                    int.Parse(sps[1]),
                    (ConsumeType)int.Parse(sps[0]),
                    int.Parse(sps[2])
                    ));
                break;
            case "ContributeLv2_reward":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i+=3)
                {
                    vo.Awards[DonateLevel.Middle].Add(new TypeStruct { 
                      Type=(ConsumeType)int.Parse(sps[i]),
                       Id=int.Parse(sps[i+1]),
                        Value=int.Parse(sps[i+2])
                    });
                }
                break;
            case "ContributeLv3":
                sps = value.Split(sl);
                vo.Donates.Add(DonateLevel.High, new TypeStruct(
                    int.Parse(sps[1]),
                    (ConsumeType)int.Parse(sps[0]),
                    int.Parse(sps[2])
                    ));
                break;
            case "ContributeLv3_reward":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i+=3)
                {
                    vo.Awards[DonateLevel.High].Add(new TypeStruct { 
                      Type=(ConsumeType)int.Parse(sps[i]),
                       Id=int.Parse(sps[i+1]),
                        Value=int.Parse(sps[i+2])
                    });
                }
                break;
            case "GongHuiLog":
                vo.EventId = int.Parse(value);
                break;
            case "gonggaoID":
                vo.PostId = int.Parse(value);
                break;
            default:
                break;
        }
    }
}

/// <summary>
/// 职位表
/// </summary>
public class DataReadGuildOffcer : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildOffcerVo vo;
        if (GuildManager.Instance.XmlGuildOffice.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildOffice[key] as GuildOffcerVo;
        }
        else
        {
            vo = new GuildOffcerVo();
            GuildManager.Instance.XmlGuildOffice.Add(key, vo);
        }
        string[] sps;
        char sl = ',';
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "OfficerLv":
                vo.OfficeLevel = (GuildOffice)int.Parse(value);
                break;
            case "CheckApply":
                vo.CptCheckApply = Convert.ToBoolean(int.Parse(value));
                break;
            case "NoticeModify":
                vo.CptChangPost = Convert.ToBoolean(int.Parse(value));
                break;
            case "OfficerModify":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i++)
                {
                    int v = int.Parse(sps[i]);
                    if (v!=0)
                    {
                        vo.ChangOffices.Add((GuildOffice)v);
                    }
                }
                break;
            case "MessageSend":
                vo.CptSendMsg = Convert.ToBoolean(int.Parse(value));
                break;
            case "MessageSend_cost":
                sps=value.Split(sl);
                vo.SendMsgConsume = new IdStruct(int.Parse(sps[1]), int.Parse(sps[1]));
                break;
            case "GongHuiDestroy":
                vo.CptDestroyGuild = Convert.ToBoolean(int.Parse(value));
                break;
            case "GongHuiLeave":
                vo.CptKickingGuild = Convert.ToBoolean(int.Parse(value));
                break;
            case "EventDemon":
                vo.CptEventDemon = Convert.ToBoolean(int.Parse(value));
                break;
            case "EventBossTime_set":
                vo.CptBossTimeChang = Convert.ToBoolean(int.Parse(value));
                break;
            case "CentreUp":
                vo.CptCenterUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "CentreUp_time":
                vo.CptCenterSpeedUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "FlagUp":
                vo.CptFlagUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "FlagUp_time":
                vo.CptFlagSpeedUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "SkillUp":
                vo.CptSkillUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "SkillUp_time":
                vo.CptSkillSpeedUp = Convert.ToBoolean(int.Parse(value));
                break;
            case "SkillUp_yanjiu":
                vo.CptSkillStudy = Convert.ToBoolean(int.Parse(value));
                break;
            case "GongHuiLog":
                vo.EventId = int.Parse(value);
                break;
            case "gonggaoID":
                vo.PostId = int.Parse(value);
                break;
            default:
                
                break;
        }
    }
}

/// <summary>
/// 公会旗帜
/// </summary>
public class DataReadGuildFlag : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildFlagVo vo;
        if (GuildManager.Instance.XmlGuildBuildFlag.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildBuildFlag[key] as GuildFlagVo;
        }
        else
        {
            vo = new GuildFlagVo();
            GuildManager.Instance.XmlGuildBuildFlag.Add(key, vo);
        }
        string[] sps;
        char sl = ',';
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Icon":
                vo.Icon = value;
                break;
            case "Level":
                vo.Level = int.Parse(value);
                GuildManager.Instance.FlagGetServderData(vo.Level);
                break;
            case "FlagData":
                sps = value.Split(sl);
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.Attrubutes.Add(new AttributeValue
                    {
                        Type = (eFighintPropertyCate)int.Parse(sps[i]),
                        Value = int.Parse(sps[i + 1])
                    });
                }
                break;
            case "FlagOfficerData_1":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                vo.OfficeAttrs[GuildOffice.Leader] = new BetterList<AttributeValue>();
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.OfficeAttrs[GuildOffice.Leader].Add(
                        new AttributeValue
                        {
                            Type = (eFighintPropertyCate)int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            case "FlagOfficerData_2":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                vo.OfficeAttrs[GuildOffice.DeputyLeader] = new BetterList<AttributeValue>();
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.OfficeAttrs[GuildOffice.Leader].Add(
                        new AttributeValue
                        {
                            Type = (eFighintPropertyCate)int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            case "FlagOfficerData_3":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                vo.OfficeAttrs[GuildOffice.Statesman] = new BetterList<AttributeValue>();
                for (int i = 0; i < sps.Length; i += 2)
                {
                    vo.OfficeAttrs[GuildOffice.Leader].Add(
                        new AttributeValue
                        {
                            Type = (eFighintPropertyCate)int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            case "FlagOfficerData_4":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                vo.OfficeAttrs[GuildOffice.Elite] = new BetterList<AttributeValue>();
                for (int i = 0; i < sps.Length; i += 2)
                {
                    vo.OfficeAttrs[GuildOffice.Leader].Add(
                        new AttributeValue
                        {
                            Type = (eFighintPropertyCate)int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            case "FlagUp_resource":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.FlagLvlupConsume.Add(new TypeStruct {
                     Type=ConsumeType.Gold,
                      Id=int.Parse(sps[i]),
                       Value=int.Parse(sps[i+1])
                    });
                }
                break;
            case "FlagUp_item":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                for (int i = 0; i < sps.Length; i+=2)
                {
                    vo.FlagLvlupConsume.Add(new TypeStruct {
                     Type=ConsumeType.Item,
                      Id=int.Parse(sps[i]),
                       Value=int.Parse(sps[i+1])
                    });
                }
                break;
            case "FlagUp_time":
                vo.FlagUpTime = int.Parse(value);
                break;
            case "CurLvDesc":
                vo.CurLevelDesc = value;
                break;
            case "NextLvDesc":
                vo.NextLevelDesc = value;
                break;
            default:

                break;
        }
    }
}

/// <summary>
/// 公会大厅
/// </summary>
public class DataReadGuildCenter: DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildCenterVo vo;
        if (GuildManager.Instance.XmlGuildBuildCenter.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildBuildCenter[key] as GuildCenterVo;
        }
        else
        {
            vo = new GuildCenterVo();
            GuildManager.Instance.XmlGuildBuildCenter.Add(key, vo);
        }
        string[] sps;
        char sl = ',';
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Icon":
                vo.Icon = value;
                break;
            case "Level":
                vo.Level = int.Parse(value);
                break;
            case "CentreUp_resource":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                for (int i = 0; i < sps.Length; i += 2)
                {
                    vo.CenterLvlupConsume.Add(new TypeStruct
                    {
                        Type = ConsumeType.Gold,
                        Id = int.Parse(sps[i]),
                        Value = int.Parse(sps[i + 1])
                    });
                }
                break;
            case "CentreUp_item":
                sps = value.Split(sl);
                if (sps.Length == 1 && int.Parse(sps[0]) == 0)
                    return;
                for (int i = 0; i < sps.Length; i += 2)
                {
                    vo.CenterLvlupConsume.Add(new TypeStruct
                    {
                        Type = ConsumeType.Item,
                        Id = int.Parse(sps[i]),
                        Value = int.Parse(sps[i + 1])
                    });
                }
                break;
            case "CentreUp_time":
                vo.CenterUpTime = int.Parse(value);
                break;
            case "CurLvDesc":
                vo.CurLvlDesc = value;
                break;
            case "NextLvDesc":
                vo.NextLvlDesc = value;
                break;
            default:

                break;
        }
    }
}

/// <summary>
/// 公会活动
/// </summary>
public class DataReadGuildEvent : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildEventVo vo;
        if (GuildManager.Instance.XmlGuildEvent.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildEvent[key] as GuildEventVo;
        }
        else
        {
            vo = new GuildEventVo();
            GuildManager.Instance.XmlGuildEvent.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Icon":
                vo.Icon = value;
                break;
            case "GongHuiLv":
                vo.EnableLevel = int.Parse(value);
                break;
            case "Type":
                vo.EventType = int.Parse(value);
                break;
            case "Number":
                vo.EventCount = int.Parse(value);
                break;
            case "EventTime":
                vo.EventTime = value;
                break;
            case "EventType":
                vo.EventCustomType = int.Parse(value);
                break;
            case "EventTime_set":
                vo.EventCustonTime = value;
                break;
            case "EventButton1":
                vo.EventCustonButtonName1 = value;
                break;
            case "EventButtonOpen1":
                vo.EventFunction1 = 0;
                break;
            case "EventButton2":
                vo.EventCustonButtonName2 = value;
                break;
            case "EventButtonOpen2":
                vo.EventFunction2 = 0;
                break;
            case "EventDesc":
                vo.EventDesc = value;
                break;
            default:

                break;
        }
    }
}

/// <summary>
/// 公会活动——商店
/// </summary>
public class DataReadGuildShop : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildShopVo vo;
        if (GuildManager.Instance.XmlGuildShop.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildShop[key] as GuildShopVo;
        }
        else
        {
            vo = new GuildShopVo();
            GuildManager.Instance.XmlGuildShop.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "name":
                vo.Name = value;
                break;
            case "sort":
                vo.ShopType = (SellShopType)int.Parse(value);
                break;
            case "itemNum":
                vo.DisplayId = int.Parse(value);
                break;
            case "itemID":
                vo.ItemId = int.Parse(value);
                break;
            case "getItemNum":
                vo.BuyCount = int.Parse(value);
                break;
            case "ItemMax":
                vo.SellLimit = int.Parse(value);
                break;
            case "GongHuiShopLv":
                vo.SellGuildLevel = int.Parse(value);
                break;
            case "ResourceType":
                vo.ConsumePrice.Type = (eGoldType)int.Parse(value);
                break;
            case "ResourceNum":
                vo.ConsumePrice.Value = int.Parse(value);
                break;
            default:

                break;
        }
    }
}



/// <summary>
/// 公会建筑列表
/// </summary>
public class DataReadGuildBulid: DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildBulidVo vo;
        if (GuildManager.Instance.XmlGuildBuild.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildBuild[key] as GuildBulidVo;
        }
        else
        {
            vo = new GuildBulidVo();
            GuildManager.Instance.XmlGuildBuild.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Icon":
                vo.Icon = value;
                break;
            case "GongHuiLv":
                vo.OpenLevel = int.Parse(value);
                break;
            case "BuildInitLv":
                vo.DefaultLevel = int.Parse(value);
                break;
            case "BuildUpButton":
                vo.ButtonDesc1 = value;
                break;
            case "BuildUpOpen":
                vo.FunctionId1 = int.Parse(value);
                break;
            case "BuildInButton":
                vo.ButtonDesc2 = value;
                break;
            case "BuildInOpen":
                vo.FunctionId2 = int.Parse(value);
                break;
            default:

                break;
        }
    }
}

/// <summary>
/// 公会技能
/// </summary>
public class DataReadGuildSkill : DataReadBase
{ 
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuildSkillVo vo;
        if (GuildManager.Instance.XmlGuildSkill.ContainsKey(key))
        {
            vo = GuildManager.Instance.XmlGuildSkill[key] as GuildSkillVo;
        }
        else
        {
            vo = new GuildSkillVo();
            GuildManager.Instance.XmlGuildSkill.Add(key, vo);
        }
        string s = "";
        string[] sps;
        switch (name)
        {
            case "ID":
                vo.ID = int.Parse(value);
                break;
            case "Name":
                vo.Name = value;
                break;
            case "Icon":
                vo.Icon = value;
                break;
            case "Type":
                vo.Type = int.Parse(value);
                break;
            case "Level":
                vo.Level = int.Parse(value);
                break;
            case "SkillType":
                s = value;
                break;
            case "SkillValue":
                vo.Attributes.Add(
                    new AttributeValue
                    {
                        Type = (eFighintPropertyCate)int.Parse(s),
                        Value = int.Parse(value)
                    });
                break;
            case "BuildSkillLv":
                vo.BuildSkillLv = int.Parse(value);
                break;
            case "SkillUp_resource":
                sps = value.Split(',');
                for(int i=0;i<sps.Length;i++)
                {
                    vo.SkillConsume.Add(
                        new TypeStruct 
                        {
                            Type = ConsumeType.Gold,
                            Id = int.Parse( sps[i] ),
                            Value = int.Parse( sps[i+1] )
                        });
                }
                break;
            case "SkillUp_item":
                sps = value.Split(',');
                for(int i=0;i<sps.Length;i++)
                {
                    vo.SkillConsume.Add(
                        new TypeStruct 
                        {
                            Type = ConsumeType.Item,
                            Id = int.Parse( sps[i] ),
                            Value = int.Parse( sps[i+1] )
                        });
                }
                break;
            case "SkillLearn_resource":
                sps = value.Split(',');
                for (int i = 0; i < sps.Length; i++)
                {
                    vo.SkillConsumeLearn.Add(
                        new TypeStruct
                        {
                            Type = ConsumeType.Gold,
                            Id = int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            case "SkillLearn_item":
                sps = value.Split(',');
                for (int i = 0; i < sps.Length; i++)
                {
                    vo.SkillConsumeLearn.Add(
                        new TypeStruct
                        {
                            Type = ConsumeType.Item,
                            Id = int.Parse(sps[i]),
                            Value = int.Parse(sps[i + 1])
                        });
                }
                break;
            default:
                break;
        }
    }
}