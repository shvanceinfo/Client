using System.Collections.Generic;
using System.Collections;
using model;
using manager;
public class DataReadRankReward:DataReadBase
{

    public override string getRootNodeName()
    {
        return "RECORDS";
    }

    public override void appendAttribute(int key, string name, string value)
    {
        RankXmlVo rv;

        if (DemonManager.Instance.RankRewardHash.ContainsKey(key))
        {
            rv = DemonManager.Instance.RankRewardHash[key] as RankXmlVo;
        }
        else {
            rv = new RankXmlVo();
            DemonManager.Instance.RankRewardHash.Add(key, rv);
        }
        switch (name)
        {
            case "ID":
                rv.Id = int.Parse(value);
                break;
            case "name":
                rv.Name = value;
                break;
            case "rankLeft":
                rv.MinRank = int.Parse(value);
                break;
            case "rankRight":
                rv.MaxRank = int.Parse(value);
                break;
            case "RewardItem":
                string[] sps = value.Split(',');
                for (int i = 0; i < sps.Length; i+=2)
                {
                    DemonVoItem di = new DemonVoItem();
                    di.Id = int.Parse(sps[i]);
                    di.Nums = int.Parse(sps[i + 1]);
                    rv.Items.Add(di);
                }
                break;
            default:
                break;
        }
    }
}