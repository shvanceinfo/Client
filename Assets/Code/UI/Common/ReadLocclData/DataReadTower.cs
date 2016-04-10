
using manager;
using model;
using System.Collections;

public class TowerDataItem 
{
    public enum ETowerType
    {
        TT_NONE = 0,
        TT_E_MONG,
        TT_E_MO,
        TT_LIAN_YU,
    }

	public TowerDataItem()
    {
		id = 0;
		battlePref = "";
    }
	
	public int id;		//爬塔ID
    public ETowerType eTowerType; 
	public int itemNum;	//物品数量
	public string battlePref;	//战斗预取件
	public string boxPref;	//宝箱预取件
	public int drop;	//掉落
}

public class DataReadTower : DataReadBase {
    
	public override string getRootNodeName() {
		return "RECORDS";
	}

    public override void appendAttribute(int key, string name, string value)
    {
        DemonVo dv;

        if (DemonManager.Instance.DemonHash.ContainsKey(key))
        {
            dv = DemonManager.Instance.DemonHash[key] as DemonVo;
        }
        else {
            dv = new DemonVo();
            DemonManager.Instance.DemonHash.Add(key, dv);
            
            
        }
		switch (name) {
		    case "ID":
			dv.Id = int.Parse(value);
			break;
            case "towerType":
            dv.eTowerType = (TowerDataItem.ETowerType)int.Parse(value);
            break;
            case "battlePref":
            dv.BattlePrefab = value;
            break;
            case "unlockLV":
            dv.UnLockLevel = int.Parse(value);
            break;
            case "unlock":
            dv.UnLockId = int.Parse(value);
            break;
            case "itemID":
            if (!value.Equals("-1"))
            {
                string[] sps = value.Split(',');
                for (int i = 0; i < sps.Length; i += 2)
                {
                    DemonVoItem dvi = new DemonVoItem();
                    dvi.Id = int.Parse(sps[i]);
                    dvi.Nums = int.Parse(sps[i + 1]);
                    dv.ConsumeItems.Add(dvi);
                }
            }
            break;
            case "ResourceID":
            if (!value.Equals("-1"))
            {
                string[] sps = value.Split(',');
                for (int i = 0; i < sps.Length; i += 2)
                {
                    DemonVoItem dvi = new DemonVoItem();
                    dvi.Id = int.Parse(sps[i]);
                    dvi.Nums = int.Parse(sps[i + 1]);
                    dv.ConsumeGolds.Add(dvi);
                }
            }
            break;
            case "boxType":
            dv.BoxType = value;
            break;
            case "dorpOut":
            dv.DropOutBoxId = int.Parse(value);
            break;
            case "towerReward":
            if (!value.Equals("-1"))
            {
                string[] sps = value.Split(',');
                for (int i = 0; i < sps.Length; i += 2)
                {
                    DemonVoItem dvi = new DemonVoItem();
                    dvi.Id = int.Parse(sps[i]);
                    dvi.Nums = int.Parse(sps[i + 1]);
                    dv.RankRewards.Add(dvi);
                }
                int level = key / 10000;

                Hashtable table = DemonManager.Instance.GetLevelHash((DemonDiffEnum)level);
                if (table.ContainsKey(key))
                {
                    table[key] = dv;
                }
                else
                {
                    table.Add(key, dv);
                }
            }
            break;
                
		}
	}
	
	public TowerDataItem getTowerData(int key) {

        if (!DemonManager.Instance.DemonHash.ContainsKey(key))  
        {  
            TowerDataItem di = new TowerDataItem();
			return di;
        }
        TowerDataItem td = new TowerDataItem();
        DemonVo dv = DemonManager.Instance.DemonHash[key] as DemonVo ;
        td.id = dv.Id;   //计算ID，兼容战斗表
        td.drop = dv.DropOutBoxId;
        td.boxPref = dv.BoxType;
        td.battlePref = dv.BattlePrefab;
        td.itemNum = dv.ConsumeItems[0].Nums;
		return td;
	}

}
