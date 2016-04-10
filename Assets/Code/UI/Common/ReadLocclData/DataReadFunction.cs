using UnityEngine;
using System.Collections;
using model;
using manager;
using helper;

public class DataReadFunction : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}

	public override void appendAttribute (int key, string name, string value)
	{

		FastOpenVo vo;
		if (FastOpenManager.Instance.FastOpenHash.ContainsKey (key)) {
			vo = FastOpenManager.Instance.FastOpenHash [key] as FastOpenVo;
		} else {
			vo = new FastOpenVo ();
			FastOpenManager.Instance.FastOpenHash.Add (key, vo);
			FastOpenManager.Instance.FastOpenList.Add (vo);
		}

		//更多字段待补充
		switch (name) {
		case "ID":
			vo.Id = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "param":
			vo.Param = XmlHelper.CallTry (() => (int.Parse (value)));
			break;    
		case "OpenType":
			vo.Type = (OpenType)int.Parse (value);
			break;
		case "OpenValue":
			vo.Value = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "UIUrl":
			vo.UIUrl = value;
			break;
		case "Rank":
			vo.order = XmlHelper.CallTry (() => (int.Parse (value)));
			break;
		case "Location":
			vo.Location = (LocationType)int.Parse (value);
			break;
		case "FeedBackText":
			vo.Description = value;
			break;
		case "functionIcon":
			vo.FunctionIcon = value;
			break;
		case "noticeType":
			vo.IsNotice = value == "0" ? false : true;
			break;

		}
	}
}
