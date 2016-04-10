using UnityEngine;
using System.Collections;
using model;
using manager;


public class DataReadMedal:DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	public override void appendAttribute (int key, string name, string value)
	{
		MedalVo vo;
		if (MedalManager.Instance.MedalHash.ContainsKey(key)) {
			vo=MedalManager.Instance.MedalHash[key] as MedalVo;
		}else{
			vo=new MedalVo();
			MedalManager.Instance.MedalHash.Add(key,vo);
		}
		switch (name) {
		case "ID":
			vo.ID=int.Parse(value);
			break;
		case "Name":
			vo.Name = value;
			break;
		case "Level":
			vo.Level = int.Parse(value);
            MedalManager.Instance.MaxLevel = vo.Level;
			break;
		case "Icon":
			vo.Icon = value;
			break;
		case "LvUp_resource":
			string[] sps=value.Split(',');
			for (int i = 0; i < sps.Length; i+=2) {
				GoldValue gv=new GoldValue();
				gv.Type=(eGoldType)int.Parse(sps[i]);
				gv.Value=int.Parse(sps[i+1]);
				vo.Consumes.Add(gv);
			}	
			break;
		case "LvUp_item":
			vo.LvUp_item = int.Parse(value);
			break;
		case "XunZhangData":
			string[] s=value.Split(',');
			for(int i=0;i<s.Length;i+=2){
				AttributeValue e = new AttributeValue();
				e.Type = (eFighintPropertyCate)int.Parse(s[i]);
				e.Value = int.Parse(s[i+1]);
				vo.Attributes.Add(e);
			}
			break;
		case "ChengHao":
			vo.ChengHao = value;
			break;
			
		default:
		break;
		}
	}
}