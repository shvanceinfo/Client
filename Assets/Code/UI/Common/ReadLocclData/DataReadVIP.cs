using UnityEngine;
using System.Collections;
using model;
using manager;
using helper;

public class  VIPDataItem
{
	public int id;    //VIP等级
	public uint price; //VIP充值价格     
	public uint sd_jiasu;//每次扫荡加速千分比 SD_jiasu*副本扫荡时间
	public uint tl_num;  //每天体力购买次数
	public uint jjc_num; //每天竞技场购买次数
	public uint gbl_num; //每天黄金哥布林购买次数
	public string vip_libao; // 成为VIP时,奖励的礼包ID
}

public class DataReadVIP:DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{
		VipVo vo;
		if (VipManager.Instance.Vips.ContainsKey (key)) {
			vo = VipManager.Instance.Vips [key] as VipVo;
		} else {
			vo = new VipVo ();
			VipManager.Instance.Vips.Add (key, vo);
		}
		string[] sps;
		char sp = ',';
		switch (name) {
		case "ID":
			vo.VipId = (uint)XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "price":
			vo.Price = (uint)XmlHelper.CallTry (() => (int.Parse (value)));
			vo.Price *= 10;
			break;
		case "VIP_libao":
			vo.ItemId = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "VIP_model":
			vo.ModelPath = value;
			break;
		case "VIP_modle_xyz":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				vo.ModelPosition = new Vector3 (float.Parse (sps [0]), float.Parse (sps [1]), float.Parse (sps [2]));
                    
			});
			break;
		case "VIP_modle_rola":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				vo.ModelRotation = new Vector3 (float.Parse (sps [0]), float.Parse (sps [1]), float.Parse (sps [2]));

			});
			break;
		case "VIP_tu1":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				if (sps.Length == 1) {
					vo.VipPictures [0].TipType = VipPicType.None;
				} else {
					vo.VipPictures [0].TipType = (VipPicType)int.Parse (sps [0]);
					vo.VipPictures [0].TextureName = sps [1];
				}

			});
			break;
		case "VIP_tu1_tips":
			XmlHelper.CallTry (() =>
			{
				vo.VipPictures [0].TipValue = value;

			});
			break;
		case "VIP_tu2":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				if (sps.Length == 1) {
					vo.VipPictures [1].TipType = VipPicType.None;
				} else {
					vo.VipPictures [1].TipType = (VipPicType)int.Parse (sps [0]);
					vo.VipPictures [1].TextureName = sps [1];
				}

			});
			break;
		case "VIP_tu2_tips":
			XmlHelper.CallTry (() =>
			{
				vo.VipPictures [1].TipValue = value;

			});
			break;
		case "VIP_tu3":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				if (sps.Length == 1) {
					vo.VipPictures [2].TipType = VipPicType.None;
				} else {
					vo.VipPictures [2].TipType = (VipPicType)int.Parse (sps [0]);
					vo.VipPictures [2].TextureName = sps [1];
				}

			});
			break;
		case "VIP_tu3_tips":
			XmlHelper.CallTry (() =>
			{
				vo.VipPictures [2].TipValue = value;

			});
			break;
		case "VIP_zhandouli":
			vo.VipPower = XmlHelper.CallTry (() => (int.Parse (value)));
			break; 
		case "VIP_shuxing":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				for (int i = 0; i < sps.Length; i++) {
					AttributeValue v = new AttributeValue();
					v.Type = (eFighintPropertyCate)int.Parse (sps [i]);
					vo.Attributes [i] = v;
					VipManager.Instance.AddAttribute (i, int.Parse (sps [i]));
				}
                    
			});
			break;
		case "VIP_shuxing_zhi":
			XmlHelper.CallTry (() =>
			{
				sps = value.Split (sp);
				for (int i = 0; i < sps.Length; i++) {
					AttributeValue v = vo.Attributes [i];
					v.Value = int.Parse (sps [i]);
					vo.Attributes [i] = v;
					VipManager.Instance.AddValue (key, i, int.Parse (sps [i]));
				}
                    
			});
			break;
		case "SD_jiasu":
			vo.Privileges ["SD_jiasu"] = new Privilege { Type=SuffixType.Second,Value=value };
			break;
		case "TL_num":
			vo.Privileges ["TL_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "JJC_num":
			vo.Privileges ["JJC_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "GBL_num":
			vo.Privileges ["GBL_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "Bag_num":
			vo.Privileges ["Bag_num"] = new Privilege { Type = SuffixType.Number, Value = value };
			break;
		case "FH_num":
			vo.Privileges ["FH_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "HPMP_num":
			vo.Privileges ["HPMP_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "FriendNum":
			vo.Privileges ["FriendNum"] = new Privilege { Type = SuffixType.Number, Value = value };
			break;
		case "FriendTiLi_song":
			vo.Privileges ["FriendTiLi_song"] = new Privilege { Type = SuffixType.Number, Value = value };
			break;
		case "FriendTiLi_ling":
			vo.Privileges ["FriendTiLi_ling"] = new Privilege { Type = SuffixType.Number, Value = value };
			break;
		case "FriendFree_song":
			vo.Privileges ["FriendFree_song"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "FriendFree_ling":
			vo.Privileges ["FriendFree_ling"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "FriendAgree_yes":
			vo.Privileges ["FriendAgree_yes"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "FriendAgree_no":
			vo.Privileges ["FriendAgree_no"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "huanjing_num":
			vo.Privileges ["huanjing_num"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "xuanshang_num":
			vo.Privileges ["xuanshang_num"] = new Privilege { Type = SuffixType.Count, Value = value };
			break;
		case "xuanshang_kuaisu":
			vo.Privileges ["xuanshang_kuaisu"] = new Privilege { Type = SuffixType.Boolean, Value = value };
			break;
		case "xuanshang_yijian":
			vo.Privileges ["xuanshang_yijian"] = new Privilege { Type = SuffixType.Boolean, Value = value };
            break;
        case "MG_XDLmax":
            vo.Privileges["MG_XDLmax"] = new Privilege { Type = SuffixType.Count, Value = value };
            break;
        case "MG_XDLbuy":
            vo.Privileges["MG_XDLbuy"] = new Privilege { Type = SuffixType.Count, Value = value };
            break;	
		
				
			
		}
	}
	
	public VIPDataItem getVIPData (int key)
	{
		
		if (!data.ContainsKey (key)) {  
			VIPDataItem item = new VIPDataItem ();
			return item;
		}
		
		return (VIPDataItem)data [key];
	}
	
	
}

public class DataReadPrivileges : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{
		PrivilegeVo vo;
		if (VipManager.Instance.Privileges.ContainsKey (key)) {
			vo = VipManager.Instance.Privileges [key] as PrivilegeVo;
		} else {
			vo = new PrivilegeVo ();
			VipManager.Instance.Privileges.Add (key, vo);
		}

		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "paixu":
			vo.OrderId = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "tequan_des":
			vo.Disction = value;
			break;
		case "xianshi":
			vo.Key = value;
			break;

		}
	}
}