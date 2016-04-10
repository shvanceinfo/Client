using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using manager;
using model;
using helper;

public class DataReadEvent  : DataReadBase
{
	public override string getRootNodeName ()
	{
		return "AIConfig";
	}
	
	public override void appendAttribute (int key, string name, string value)
	{
		
		EventVo model;
		if (EventManager.Instance.DictionaryEvent.ContainsKey (key)) {
			model = EventManager.Instance.DictionaryEvent [key];
		} else {
			model = new EventVo ();
			EventManager.Instance.DictionaryEvent.Add (key, model);
		}
 
		switch (name) {
		case "ID":
			model.Id = int.Parse (value);
			EventManager.Instance.KeyList.Add (model.Id);
			break;
		case "huodong_name":
			model.Name = value;
			break;
		case "huodong_icon":
			model.Icon = value;
			break;
		case "huodong_type":
			model.Type = (ActivityType)int.Parse (value);
			break;
		case "huodong_time"	:
			XmlHelper.CallTry(()=>{
				this.SplitStringByColon (value, model.Schedule);
			});
			 
			break;
		case "huodong_jiangli"	:
			model.Award = value;
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
			if (string.IsNullOrEmpty(strArray[i])) {
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
					kTimeUnit.kTimeCell  = new List<STimeDuration>();
				}
				kTimeUnit.kTimeCell.Add (kTimeDuration);
			}
			
		}
	}

	void ParseScheduleDayTime (string pDayTime, ref SDayTimeFormat  kDayTime)
	{
		if (string.IsNullOrEmpty(pDayTime)) {
			return  ;
		}
		
		char split1 = '-';
		char split2 = ':';
		var beArray = pDayTime.Split(split1);
		var beginHMArray = beArray[0].Split(split2);
        var endHMArray = beArray[1].Split(split2);

		kDayTime.u8BeginHour = Convert.ToByte(beginHMArray[0]);
		kDayTime.u8BeginMinute = Convert.ToByte(beginHMArray[1]);
		kDayTime.u8EndHour = Convert.ToByte(endHMArray[0]);
		kDayTime.u8EndMinute =  Convert.ToByte(endHMArray[1]);
	}
	
	
	
	 
}
