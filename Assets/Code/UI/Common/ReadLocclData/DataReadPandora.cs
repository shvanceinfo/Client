using UnityEngine;
using System.Collections;
using model;
using manager;
using System.Collections.Generic;
using helper;
using System;

public class DataReadPandora  : DataReadBase
{

	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{	
		PandoraVo pandoraVo;

		if (PandoraManager.Instance.DicPandora.ContainsKey (key))
			pandoraVo = PandoraManager.Instance.DicPandora [key];
		else {
			pandoraVo = new PandoraVo ();
			PandoraManager.Instance.PandoraList.Add(pandoraVo);
			PandoraManager.Instance.DicPandora.Add (key, pandoraVo);
		}

		switch (name) {
		case "ID":
			pandoraVo.ID = int.Parse (value);
			break;
		case "Name":
			pandoraVo.Name = value;
			break;
		case "MapID":
			pandoraVo.MapId = uint.Parse (value);
			break;	
		case "MonsterName":
			pandoraVo.MonsterName = value;
			break;
		case "OpenTime":
			XmlHelper.CallTry (() => {
				this.SplitStringByColon (value, pandoraVo.Schedule);
			});
			break;
		case "RuleDesc":
			pandoraVo.RuleDesc = value;
			break;
		case "Desc":
			pandoraVo.Desc = value;
			break;


		default:
			break;
		}

	}

	/// <summary>
	/// 分割字符串，得到总的条目
	/// </summary>
	/// <param name='pInputString'>
	/// P input string.
	/// </param>
	/// <param name='kParseData'>
	/// K parse data.
	/// </param>
	private void SplitStringByColon (string  pInputString, List<SActivityTime> kParseData)
	{
		
		const char split = ';';
		
		var strArray = pInputString.Split (split);	
		for (int i = 0,max = strArray.Length; i < max; i++) {
			SActivityTime kTime = new SActivityTime ();
			int res = FormatActivityScheduleTime (strArray [i], ref kTime);
			if (res != 0) {
				kParseData.Add (kTime);
			}
		}
		
	}
	
	/// <summary>
	/// 格式化活动时间
	/// </summary>
	/// <returns>
	/// The activity schedule time.
	/// </returns>
	/// <param name='pActivityTime'>
	/// P activity time.
	/// </param>
	/// <param name='kFormatedTime'>
	/// K formated time.
	/// </param>
	private int FormatActivityScheduleTime (string pActivityTime, ref SActivityTime kFormatedTime)
	{
		if (string.IsNullOrEmpty (pActivityTime)) {
			return 0;
		}
		
		
		char split = ']';
		var strArray = pActivityTime.Split (split);
		
		int nBranket = 0;
		
		for (int i = 0,max = strArray.Length; i < max; i++) {
			if (string.IsNullOrEmpty (strArray [i])) {
				continue;
			}
			if (strArray [i] [0] == '[') {
				string pContent = strArray [i].Substring (1);
				
				if (!string.IsNullOrEmpty (pContent)) {
					switch (nBranket) {
					case 0:
						ParseScheduleTimeCell (pContent, ref kFormatedTime.kYearUnit);
						break;
					case 1:
						ParseScheduleTimeCell (pContent, ref kFormatedTime.kMonthUnit);
						break;
					case 2:
						ParseScheduleTimeCell (pContent, ref kFormatedTime.kDayUnit);
						break;
					case 3:
						ParseScheduleTimeCell (pContent, ref kFormatedTime.kWeekUnit);
						break;
					case 4:
						ParseScheduleDayTime (pContent, ref kFormatedTime.kDayTime);
						break;
					}
					
					nBranket++;
				}
			}
		}
		return 1;
	}
	
	private void ParseScheduleTimeCell (string pContent, ref STimeUnit  kTimeUnit)
	{
		if (string.IsNullOrEmpty (pContent)) {
			return;
		}
		
		int nLen = pContent.Length;
		if (nLen == 1 && pContent == "*") {
			kTimeUnit.bLoop = true;
		} else {
			kTimeUnit.bLoop = false;
			
			char split = ',';
			
			STimeDuration kTimeDuration = new STimeDuration ();
			string[] strArray = pContent.Split (split);
			
			for (int i = 0,max = strArray.Length; i < max; i++) {
				char searchChar = '-';
				if (strArray [i].IndexOf (searchChar) > 0) {
					kTimeDuration.bDuration = true;
					
					var beArray = strArray [i].Split (searchChar);
					
					kTimeDuration.u16Begin = Convert.ToUInt16 (beArray [0]);
					kTimeDuration.u16End = Convert.ToUInt16 (beArray [1]);
				} else {
					kTimeDuration.bDuration = false;
					kTimeDuration.u16Begin = Convert.ToUInt16 (strArray [i]);
					kTimeDuration.u16End = Convert.ToUInt16 (strArray [i]);
				}
				
				if (kTimeUnit.kTimeCell == null) {
					kTimeUnit.kTimeCell = new List<STimeDuration> ();
				}
				kTimeUnit.kTimeCell.Add (kTimeDuration);
			}
			
		}
	}
	
	void ParseScheduleDayTime (string pDayTime, ref SDayTimeFormat  kDayTime)
	{
		if (string.IsNullOrEmpty (pDayTime)) {
			return;
		}
		
		char split1 = '-';
		char split2 = ':';
		var beArray = pDayTime.Split (split1);
		var beginHMArray = beArray [0].Split (split2);
		var endHMArray = beArray [1].Split (split2);
		
		kDayTime.u8BeginHour = Convert.ToByte (beginHMArray [0]);
		kDayTime.u8BeginMinute = Convert.ToByte (beginHMArray [1]);
		kDayTime.u8EndHour = Convert.ToByte (endHMArray [0]);
		kDayTime.u8EndMinute = Convert.ToByte (endHMArray [1]);
	}

}

public class DataReadPandoraNum  : DataReadBase
{
	
	public override string getRootNodeName ()
	{
		return "RECORDS";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{	
		PandoraNumVo pandoraNumVo;
		
		if (PandoraManager.Instance.DicPandoraNum.ContainsKey (key))
			pandoraNumVo = PandoraManager.Instance.DicPandoraNum [key];
		else {
			pandoraNumVo = new PandoraNumVo ();
			PandoraManager.Instance.DicPandoraNum.Add (key, pandoraNumVo);
		}

		char split = ',';
		switch (name) {
		case "ID":
			pandoraNumVo.ID = int.Parse (value);
			break;
		case "TiaoZhan_resource":
			if (value != "0") {
				var tiaoZhanResString = value.Split (split);
				pandoraNumVo.TiaoZhanRes = (eGoldType)int.Parse (tiaoZhanResString [0]);
				pandoraNumVo.TiaoZhanResNum = int.Parse (tiaoZhanResString [1]);
			} 
			break;
		case "TiaoZhan_item":
			if (value != "0") {
				var tiaoZhanItemString = value.Split (split);
				pandoraNumVo.TiaoZhanItemId = uint.Parse (tiaoZhanItemString [0]);
				pandoraNumVo.TiaoZhanItemNum = int.Parse (tiaoZhanItemString [1]);
			}
			break;
		case "ChongZhi_resource":
			if (value != "0") {
				var chongZhiResString = value.Split (split);
				pandoraNumVo.ResetRes = (eGoldType)int.Parse (chongZhiResString [0]);
				pandoraNumVo.ResetResNum = int.Parse (chongZhiResString [1]);
			}
			break;
		case "ChongZhi_item":
			if (value != "0") {
				var chongzhiItemString = value.Split (split);
				pandoraNumVo.ResetItemId = uint.Parse (chongzhiItemString [0]);
				pandoraNumVo.ResetItemNum = int.Parse (chongzhiItemString [1]);
			}

			break;
		case "Pandora_resource":
			if (value != "0") {
				var pandoraRes = value.Split (split);
				pandoraNumVo.PandoraRes = (eGoldType)int.Parse (pandoraRes [0]);
				pandoraNumVo.PandoraResNum = int.Parse (pandoraRes [1]);
			}
			break;
		case "Pandora_item":
			if (value !="0") {
				var pandoraItem = value.Split(split);
				pandoraNumVo.PandoraItemId = uint.Parse(pandoraItem[0]);
				pandoraNumVo.PandoraItemNum = int.Parse(pandoraItem[1]);
			}
			break;


		default:
			break;
		}
		
	}
	 
 	
}
