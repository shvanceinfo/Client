/**该文件实现的基本功能等
function: 活动的vo
author:zyl
date:2014-5-12
**/
using manager;
using System;
using System.Collections.Generic;
  
namespace model
{
	public enum EventState
	{
		/// <summary>
		/// 可加入
		/// </summary>
		Join,
		/// <summary>
		/// 未开启
		/// </summary>
		NotJoin,
		/// <summary>
		/// 已结束
		/// </summary>
		Finish
	}
	
	public enum ActivityType
	{
		/// <summary>
		/// Constant dungeon.
		/// </summary>
		Dungeon = 1,
		Boss = 2
	}
	
	//时间单位的结构
	public 	struct STimeDuration
	{
		public bool bDuration;		// 是否持续
		public ushort u16Begin;	// 持续开始
		public ushort u16End;		// 持续结束
	}
	
	public struct STimeUnit
	{
		public bool bLoop;	// 时间是否循环
		public List<STimeDuration> kTimeCell ; // 如果不是循环这里就有数据
	}
	
	public struct SDayTimeFormat
	{
		public Byte u8BeginHour;
		public Byte u8BeginMinute;
		public Byte u8EndHour;
		public Byte u8EndMinute;
	}
	
	public struct SActivityTime
	{
		public STimeUnit kYearUnit;
		public STimeUnit kMonthUnit;
		public STimeUnit kDayUnit;
		public STimeUnit kWeekUnit;	// 这个周起到筛选作用
	
		public SDayTimeFormat kDayTime;
	}
	
	public class EventVo
	{
		private int _id;
		private int _instanceId;
		private string _name;
		private string _icon;
		private ActivityType _type;
		private string _award;
		private List<SActivityTime> _Schedule = new List<SActivityTime> ();
		private EventState _eventState = EventState.NotJoin;
		private bool _isServerUpdate;
		/// <summary>
		/// 活动实例ID
		/// </summary>
		/// <value>
		/// The instance identifier.
		/// </value>
		public int InstanceId {
			get {
				return this._instanceId;
			}
			set {
				_instanceId = value;
			}
		}

		public string Award {
			get {
				return this._award;
			}
			set {
				_award = value;
			}
		}

		public  List<SActivityTime>  Schedule {
			get {
				return this._Schedule;
			}
			set {
				_Schedule = value;
			}
		}

		public string Icon {
			get {
				return this._icon;
			}
			set {
				_icon = value;
			}
		}

		public int Id {
			get {
				return this._id;
			}
			set {
				_id = value;
			}
		}

		public string Name {
			get {
				return this._name;
			}
			set {
				_name = value; 
			}
		}
		
		public EventState EventStates {
			get {
 				if (!this.IsServerUpdate) {
					this.CheckEventState (); //确认状态
 				}//如果服务器没有更新过状态，则通过读表解析
				return this._eventState;
			}
			set {
				_eventState = value;
			}
		}
		
		public ActivityType Type {
			get {
				return this._type;
			}
			set {
				_type = value;
			}
		}

		public bool IsServerUpdate {
			get {
				return this._isServerUpdate;
			}
			set {
				_isServerUpdate = value;
			}
		}	
		
//		[*][*][*][1,2,3,4,5,6,7][11:00-12:00];[*][*][*][1,2,3,4,5,6,7][19:00-20:00]
		public void CheckEventState ()
		{
			DateTime nowTime = DateTime.Now;
			
			for (int i = 0,max = this.Schedule.Count; i < max; i++) {
				SActivityTime activityTime = this.Schedule [i];
				bool validStatus = true;
				if (activityTime.kYearUnit.bLoop == false) {
					validStatus = ValidationDate (activityTime.kYearUnit.kTimeCell, nowTime.Year);
				}
				if (validStatus && activityTime.kMonthUnit.bLoop == false) {
					validStatus = ValidationDate (activityTime.kMonthUnit.kTimeCell, nowTime.Month);
				}
				if (validStatus && activityTime.kDayUnit.bLoop == false) {
					validStatus = ValidationDate (activityTime.kDayUnit.kTimeCell, nowTime.Day);
				}
				if (validStatus && activityTime.kWeekUnit.bLoop == false) {
					validStatus = ValidationWeek (activityTime.kWeekUnit.kTimeCell, (int)nowTime.DayOfWeek);
				}
				if (validStatus) {
					validStatus = ValidationTime (activityTime.kDayTime, nowTime.Hour, nowTime.Minute);
				}
				if (validStatus) {
					break;
				}//如果在时间段内则跳出
			}
		}
 
		/// <summary>
		/// 验证当前时间是否在配置的时间段中
		/// </summary>
		/// <returns>
		/// The time.
		/// </returns>
		/// <param name='kTimeCell'>
		/// If set to <c>true</c> k time cell.
		/// </param>
		/// <param name='timeVal'>
		/// If set to <c>true</c> time value.
		/// </param>
		public  bool ValidationDate (List<STimeDuration> kTimeCell, int timeVal)
		{	 
			bool status = false;
			for (int i = 0,max = kTimeCell.Count; i < max; i++) {
				STimeDuration duration = kTimeCell [i];
				if (timeVal < duration.u16Begin) {
					this.EventStates = EventState.NotJoin;
					status = false;
					continue;
				} else if (timeVal > duration.u16End) {
					this.EventStates = EventState.Finish;
					status = false;
					continue;
				} else {								//为了解决服务器不同步问题，这里只能设置
					this.EventStates = EventState.NotJoin;
					status = false;
					break;
				}//满足条件则跳出循环
			}
			return status;
		}
		
		
		/// <summary>
		///  验证周几是否在配置的时间段中
		/// </summary>
		/// <returns>
		/// The week.
		/// </returns>
		/// <param name='kTimeCell'>
		/// If set to <c>true</c> k time cell.
		/// </param>
		/// <param name='timeVal'>
		/// If set to <c>true</c> time value.
		/// </param>
		public  bool ValidationWeek (List<STimeDuration> kTimeCell, int timeVal)
		{	 
			bool status = false;
			for (int i = 0,max = kTimeCell.Count; i < max; i++) {
				STimeDuration duration = kTimeCell [i];
				if (timeVal < duration.u16Begin) {
					this.EventStates = EventState.NotJoin;
					status = false;
					continue;
				} else if (timeVal > duration.u16End) {
					this.EventStates = EventState.NotJoin;
					status = false;
					continue;
				} else {
					this.EventStates = EventState.NotJoin;
					status = false;
					break;
				}//满足条件则跳出循环
			}
			return status;
		}
		
		public bool ValidationTime (SDayTimeFormat dt, int hour, int min)
		{
			
			bool status = false;
			if (hour < dt.u8BeginHour) {
				this.EventStates = EventState.NotJoin;
				status = false;
			} else if (hour > dt.u8EndHour) {
				this.EventStates = EventState.Finish;
				status = false;
			} else {
				
				int subMin = (dt.u8EndHour - dt.u8BeginHour)*60 ;
				subMin += (dt.u8EndMinute - dt.u8BeginMinute); //得到总的时间
				
				int nowMin = (hour - dt.u8BeginHour)*60;
				nowMin += (min - dt.u8BeginMinute);			//得到当前的总的时间
				
				if (nowMin<0) {
					this.EventStates = EventState.Finish;
					status = false;
				}else if(nowMin>subMin){
					this.EventStates = EventState.NotJoin;
					status = false;
				}else{
					this.EventStates = EventState.NotJoin;
					status = false;
				}
  
			}
			return status;
		}
		
		
		
		
		
		
		
		
		
	}
	
 
	
	
}
