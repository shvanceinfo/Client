using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using manager;
using mediator;
using MVC.entrance.gate;
using model;
using System.Text;

public class EventView : MonoBehaviour
{
	
	const int HEIGHT = -115;
	Transform _trans;
	Transform _temp;

	void Awake ()
	{
		this._trans = this.transform;
		this._temp = this._trans.FindChild ("eventlist/temp");
		
	}
 
	void OnEnable ()
	{
		Gate.instance.registerMediator (new EventMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.EVENT_MEDIATOR);
	}
	
	public void Show (List<int> keys, Dictionary<int,EventVo> dicEvent)
	{
		
		this._temp.gameObject.SetActive (true);
		
		int count = 0;
		foreach (var key in keys) {
			var eventVo = dicEvent [key];
			
			GameObject obj = NGUITools.AddChild (this._temp.parent.gameObject, this._temp.gameObject);
			obj.name = key.ToString ();
			Transform objTrans = obj.transform;
			objTrans.localPosition = new Vector3 (this._temp.localPosition.x, this._temp.localPosition.y + count * HEIGHT, 0);
			objTrans.FindChild ("icon").GetComponent<UISprite> ().spriteName = eventVo.Icon;
			objTrans.FindChild ("name").GetComponent<UILabel> ().text = eventVo.Name;
			objTrans.FindChild ("eventaward/time").GetComponent<UILabel> ().text = eventVo.Award;
			
			UILabel time = objTrans.FindChild ("eventtime/time").GetComponent<UILabel> ();
			
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < eventVo.Schedule.Count; i++) {
				
				GetFormatTime(sb,eventVo.Schedule [i].kDayTime.u8BeginHour);
				sb.Append(":");
				GetFormatTime(sb,eventVo.Schedule [i].kDayTime.u8BeginMinute);
				sb.Append("-");
				GetFormatTime(sb,eventVo.Schedule [i].kDayTime.u8EndHour);
				sb.Append(":");
				GetFormatTime(sb,eventVo.Schedule [i].kDayTime.u8EndMinute);
				sb.Append(",");
			}
			time.text = sb.Remove(sb.Length-1,1).ToString();
			
			
			
//			print(System.DateTime.Now.Year +" "+System.DateTime.Now.Month+" "+System.DateTime.Now.Day+" "+(int)System.DateTime.Now.DayOfWeek);
			
			SetButtonStatus (eventVo,objTrans);
			
			count++;
		}
 
		this._temp.gameObject.SetActive (false);
	}
	
	
	public void GetFormatTime(StringBuilder sb, byte time){
		if (time<10) {
			sb.Append("0").Append(time);
		}else{
			sb.Append(time);
		}
	}
	
	
	
	public void UpdateInfo (EventVo vo)
	{
		StringBuilder sb = new StringBuilder ();
		sb.Append ("eventlist/").Append (vo.Id.ToString ());
		Transform itemObj = this._trans.FindChild (sb.ToString ());
		if (itemObj != null) {
			SetButtonStatus (vo, itemObj);
		}
		
 		 
	}
	
	
	/// <summary>
	/// 设置按钮状态
	/// </summary>
	/// <param name='eventVo'>
	/// Event vo.
	/// </param>
	/// <param name='objTrans'>
	/// Object trans.
	/// </param>
	public static void SetButtonStatus (EventVo eventVo, Transform objTrans)
	{
		switch (eventVo.EventStates) {
		case EventState.Join:
			objTrans.FindChild ("btn/Label").GetComponent<UILabel> ().text = "加入";
			objTrans.FindChild ("btn/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(true);
			break;
		case EventState.NotJoin:
			objTrans.FindChild ("btn/Label").GetComponent<UILabel> ().text = "未开启";
			objTrans.FindChild ("btn/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			break;
		case EventState.Finish:
			objTrans.FindChild ("btn/Label").GetComponent<UILabel> ().text = "已结束";
			objTrans.FindChild ("btn/Sprite").GetComponent<UISprite> ().spriteName = SourceManager.Instance.GetCommonButton1SpriteNameByStatus(false);
			break;
		default:
			break;
		}
	}
}
