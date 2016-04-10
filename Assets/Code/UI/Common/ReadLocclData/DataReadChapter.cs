using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using model;

public class DataReadChapter : DataReadBase
{
    
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{		
		ChapterVo chapterVo;		
		if (!RaidManager.Instance.ChapterHash.ContainsKey (key)) 
		{
			chapterVo = new ChapterVo ();
			RaidManager.Instance.ChapterHash.Add (key, chapterVo);
		}	
		chapterVo = (ChapterVo)RaidManager.Instance.ChapterHash[key];	
		switch (name) 
		{
		case "ID":
			chapterVo.chanpterID = int.Parse(value);
			break;
		case "zj_shunxu":
			chapterVo.chapterSequence = int.Parse (value);
			break;
		case "zj_mingcheng":
			chapterVo.chapterName = value;
			break;
		case "bg_image":
			chapterVo.chapterIcon = value;
			break;
		case "jiangli":
			var itemInfoArray = value.Split(',');
			for (int i = 0; i < itemInfoArray.Length; i+=2) {
				var itemInfo = new ItemInfo(uint.Parse(itemInfoArray[i]),0,uint.Parse(itemInfoArray[i+1]));
				chapterVo.itemInfoList.Add(itemInfo);
			}//得到奖励的物品信息
			break;
			
		default:
			break;
		}
		
	}
}