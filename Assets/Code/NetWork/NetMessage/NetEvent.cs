using System;
using System.IO;
using NetGame;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using manager;
using model;
using UnityEngine;

//请求加入活动
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GCAskJoinActivity : NetHead
{   
	int ActivityID; //活动ID

	public GCAskJoinActivity (): base()
	{
		this._assistantCmd = (UInt16)eC2GType.C2G_AskJoinActivity;
		this._length = (UInt16)(Marshal.SizeOf (this) - 2);
	}

	public byte[] ToBytes (int actId)
	{
		this.ActivityID = actId; 
		MemoryStream memStream = new MemoryStream ();
		BinaryWriter memWrite = new BinaryWriter (memStream, Encoding.GetEncoding ("utf-8"));
		base.ToBytes (ref memWrite);//把一些基本信息写入流中
		memWrite.Write (this.ActivityID);
		byte[] bytesData = memStream.ToArray ();
		memWrite.Close ();
		return bytesData;
	}
}


//通知世界boss活动开始了
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWorldBossOpen: NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		int key = (int)memRead.ReadUInt32 ();
		Debug.Log (key);
		int instanceid = memRead.ReadInt32 ();
		if (EventManager.Instance.DictionaryEvent.ContainsKey (key)) {
			var obj = EventManager.Instance.GetEventVoFromDictionary (key);
			obj.InstanceId = instanceid;
			obj.IsServerUpdate = true;
			obj.EventStates = EventState.Join;
			EventManager.Instance.EventUpdate (obj);
		}
		
		memRead.Close ();
		memStream.Close ();
	}
}

//通知活动关闭了
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyActivityClose: NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		int key = (int)memRead.ReadUInt32 ();
		Debug.Log (key);
		int instanceid = memRead.ReadInt32 ();
		if (EventManager.Instance.DictionaryEvent.ContainsKey (key)) {
			var obj = EventManager.Instance.GetEventVoFromDictionary (key);
			obj.InstanceId = instanceid;
			obj.IsServerUpdate = true;
			obj.EventStates = EventState.Finish;
			EventManager.Instance.EventUpdate (obj);
		}
		
		memRead.Close ();
		memStream.Close ();
	}
}




//通知世界boss奖励信息
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWorldBossAward : NetHead
{
	private int SLen = 20;

	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		Debug.Log ("世界BOSS奖励");
		
		PlayerAward me = new PlayerAward ();
		me.dps = memRead.ReadUInt32 ();
		byte meCount = memRead.ReadByte ();
		
		
		PlayerAward last = new PlayerAward ();
		last.playerId = memRead.ReadUInt32 ();
		last.playerName = Global.FromNetString (Encoding.UTF8.GetChars (memRead.ReadBytes (SLen))); //用户名
		last.dps = memRead.ReadUInt32 ();
		byte lastCount = memRead.ReadByte ();
 
		byte awardCount = memRead.ReadByte ();
		
		
		me.awardList = new List<Award> ();
		for (int i = 0; i < meCount; i++) {
			Award aw = new Award ();
			aw.type = memRead.ReadByte ();
			aw.num = memRead.ReadUInt32 ();
			me.awardList.Add (aw);
		}
		BossManager.Instance.BossWinVo.Me = me;
		
	 
		
		last.awardList = new List<Award> ();
 		 
		for (int i = 0; i < lastCount; i++) {
			Award aw = new Award ();
			aw.type = memRead.ReadByte ();
			aw.num = memRead.ReadUInt32 ();
			last.awardList.Add (aw);
		}
 
		BossManager.Instance.BossWinVo.Last = last;
		
 		BossManager.Instance.BossWinVo.PlayList.Clear();
		for (int i = 0 ; i < awardCount; i++) {
			
			PlayerAward player = new PlayerAward ();
			player.playerId = memRead.ReadUInt32 ();
			player.playerName = Global.FromNetString (Encoding.UTF8.GetChars (memRead.ReadBytes (SLen))); //用户名
			player.dps = memRead.ReadUInt32 ();
			player.awardList = new List<Award> ();
			
			for (int j = 0, max = memRead.ReadByte(); j < max; j++) {
				Award aw = new Award ();
				aw.type = (byte)memRead.ReadByte ();
				aw.num = memRead.ReadUInt32 ();
				player.awardList.Add (aw);
			}
 
			BossManager.Instance.BossWinVo.PlayList.Add (player);
		} 
		
		
		BossManager.Instance.ShowBossWinUI ();
		
		memRead.Close ();
		memStream.Close ();
		
		
		
		
	}
}


//通知世界boss伤害名单
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWorldBossDamageList: NetHead
{
	int len = 20;

	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		if (!BossManager.Instance.IsInWorldBoss) { //如果不在世界boss场景则跳出
			memRead.Close ();
			memStream.Close ();
			return;
		}
		
		uint meDamage = memRead.ReadUInt32 (); //得到自己的伤害总值
		uint bossRemainHp = memRead.ReadUInt32 (); //得到boss剩余血量
		byte count = memRead.ReadByte ();		//得到伤害排名数目
		BossManager.Instance.BossDamageVo.PlayerDamageList.Clear ();
		BossManager.Instance.BossDamageVo.MeDamage = meDamage;
		BossManager.Instance.BossDamageVo.BossRemainHp = bossRemainHp;
		for (int i = 0; i < count; i++) {
			PlayerDamageVo player = new PlayerDamageVo ();
			player.PlayerId = memRead.ReadUInt32 ();
			player.Name = Global.FromNetString (Encoding.UTF8.GetChars (memRead.ReadBytes (len))); //用户名
			player.TotalDamage = memRead.ReadUInt32 ();
			BossManager.Instance.BossDamageVo.PlayerDamageList.Add (player);
		}
 		
		BossManager.Instance.UpdateWorldBossInfoUI ();
		Debug.Log (" me " + meDamage + " boss " + bossRemainHp);
		memRead.Close ();
		memStream.Close ();
 
	}
}


//通知世界boss模板ID
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class GSNotifyWorldBossTemplateID:NetHead
{
	public void ToObject (byte[] byteData)
	{
		MemoryStream memStream = new MemoryStream (byteData);
		BinaryReader memRead = new BinaryReader (memStream);
		base.ToObject (ref memRead);
		
		uint bossTempId = memRead.ReadUInt32 (); //boss模板
 		
		BossManager.Instance.BossTempId = bossTempId;
		Debug.Log (" bossTempId " + bossTempId);
		memRead.Close ();
		memStream.Close ();
	}
}




