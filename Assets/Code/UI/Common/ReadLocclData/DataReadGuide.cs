using UnityEngine;
using System.Collections;
using model;
using manager;
using helper;
using System;
public class DataReadGuide : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuideVo vo;
        if (GuideManager.Instance.Hash.ContainsKey(key))
        {
            vo = GuideManager.Instance.Hash[key] as GuideVo;
        }
        else {
            vo = new GuideVo();
            GuideManager.Instance.Hash.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Group":
                vo.Group = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Step":
                vo.Step = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Enforce":
                vo.Enforce = Convert.ToBoolean(XmlHelper.CallTry(() => (int.Parse(value))));
                break;
            case "Ignore":
                vo.Ignore = Convert.ToBoolean(XmlHelper.CallTry(() => (int.Parse(value))));
                break;
            case "Trigger":
                vo.Trigger = (TriggerType)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Params":
                vo.TriggerParams = value;
                break;

            case "ComplateTrigger":
                vo.ComplateType = (TriggerType)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "ComplateParams":
                vo.ComplateParams = value;
                break;
            case "Tip":
                vo.Tip = value;
                break;
            case "Time":
                vo.Time = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Anchor":
                vo.Anchor = (TextAnchor)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "BindId":
                vo.TipBindId = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "SpecialType":
                vo.Special = (SpecialType)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "SpecialParams":
                vo.SpecialParams = value;
                break;
            case "CloseUI":
                vo.CloseUI = Convert.ToBoolean(XmlHelper.CallTry(() => (int.Parse(value))));
                break;
            default:
                break;
        }
    }
}

public class DataReadGuideInfo : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        GuideInfoVo vo;
        if (GuideInfoManager.Instance.InfoHash.ContainsKey(key))
        {
            vo = GuideInfoManager.Instance.InfoHash[key] as GuideInfoVo;
        }
        else
        {
            vo = new GuideInfoVo();
            GuideInfoManager.Instance.InfoHash.Add(key, vo);
        }
        switch (name)
        {
            case "ID":
                vo.Id = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Trigger":
                vo.Trigger = (GuideInfoTrigger)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "Params":
                vo.Params = value;
                break;
            case "icon_type":
                vo.IconType = (GuideInfoIconType)XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "icon_path":
                vo.IconPath = value;
                break;
            case "icon":
                vo.IconName = value;
                break;
            case "icon_bg":
                vo.IconBackground = value;
                break;
            
            case "TipInfo":
                vo.TipInfo = value;
                break;
            case "TipName":
                vo.TipName = value;
                break;
            case "ButtonText":
                vo.ButtonText = value;
                break;
            case "Function":
                vo.FunctionId = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "level_lim":
                string[] sps=value.Split(',');
                vo.CheckLevelMin = int.Parse(sps[0]);
                vo.CheckLevelMax = int.Parse(sps[1]);
                break;
            default:
                break;
        }
    }
}