using UnityEngine;
using System.Collections;
using model;
using manager;
using System;
using helper;
public class DataReadLuckStone : DataReadBase
{
    public override string getRootNodeName()
    {
        return "RECORDS";
    }
    public override void appendAttribute(int key, string name, string value)
    {
        LuckStoneVo vo;
        if (LuckStoneManager.Instance.LuckStoneHash.ContainsKey(key))
        {
            vo = LuckStoneManager.Instance.LuckStoneHash[key] as LuckStoneVo;
        }else{
            vo = new LuckStoneVo();
            LuckStoneManager.Instance.LuckStoneHash.Add(key, vo);
        }

        switch (name)
        {
            case "ID":
                vo.Id = XmlHelper.CallTry(() => (int.Parse(value)));
                break;

            case "Name":
                vo.Name = value;
                break;
            case "Successrate":
                vo.Successrate = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            case "LuckStoneUsed":
                XmlHelper.CallTry(() =>
                {
                    string[] sps = value.Split(',');
                    for (int i = 0; i < sps.Length; i += 2)
                    {
                        vo.ConsumeItem.Add(new IdStruct(int.Parse(sps[i]), int.Parse(sps[i + 1])));
                    }
                });
                break;
            case "Diamond":
                vo.ConsumeDiamond = XmlHelper.CallTry(() => (int.Parse(value)));
                break;
            default:
                
                break;
        }

    }
   
    


   
}