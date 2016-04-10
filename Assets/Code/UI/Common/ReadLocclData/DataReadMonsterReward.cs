using UnityEngine;
using System.Collections;
using model;
using manager;

public class DataReadMonsterReward : DataReadBase
{

	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{	
		MonsterRewardVo monsterRewardVo;
		if (MonsterRewardManager.Instance.DicMonsterReward.ContainsKey (key))
			monsterRewardVo = MonsterRewardManager.Instance.DicMonsterReward [key];
		else {
			monsterRewardVo = new MonsterRewardVo ();
			MonsterRewardManager.Instance.MonsterRewardList.Add(monsterRewardVo);
			MonsterRewardManager.Instance.DicMonsterReward.Add (key, monsterRewardVo);
		}
		
		string[] splits = null;
		char[] charSeparators = new char[] {','};
		switch (name) {
		case "ID":
			monsterRewardVo.Id = int.Parse (value);
			break;
		case "paixuID":
			monsterRewardVo.Order = int.Parse (value);
			break;
		case "zhuiji_type":
			monsterRewardVo.Type = (MonsterRewardType)int.Parse (value);
			break;
		case "value":
			monsterRewardVo.TypeValue = uint.Parse (value);
			break;
		case "zhuiji_item":
			monsterRewardVo.ItemId = uint.Parse (value);
			break;
		case "zhuiji_num":
			splits = value.Split (charSeparators);
			for (int i = 0; i < splits.Length; i++) {
				monsterRewardVo.NumList.Add (int.Parse (splits [i]));
			}
			break;
		case "zhuiji_resources":
			monsterRewardVo.ResourceType = (eGoldType)int.Parse (value);
			break;
			
		case "zhuiji_resources_num":
			splits = value.Split (charSeparators);
			for (int i = 0; i < splits.Length; i++) {
				monsterRewardVo.ResourceNumList.Add (int.Parse (splits [i]));
			}
			break;	
		case "monster_dec":
			monsterRewardVo.Des = value;
			break;
		case "map_ID":
			monsterRewardVo.MapId = uint.Parse(value);
			break;
			
		default:
			break;
		}
	}
}
