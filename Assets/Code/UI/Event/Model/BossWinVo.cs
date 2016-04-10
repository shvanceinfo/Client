/**该文件实现的基本功能等
function: 世界BOSS胜利的vo
author:zyl
date:2014-5-14
**/
using manager;
using System;
using System.Collections.Generic;
using System.Collections;

public class BossWinVo
{
	PlayerAward _me;
	PlayerAward _last;
	List<PlayerAward> _playList = new List<PlayerAward> ();
	
	public PlayerAward Last {
		get {
			return this._last;
		}
		set {
			_last = value;
		}
	}

	public PlayerAward Me {
		get {
			return this._me;
		}
		set {
			_me = value;
		}
	}

	public List<PlayerAward> PlayList {
		get {
			return this._playList;
		}
		set {
			_playList = value;
		}
	}
}

public struct PlayerAward
{
	public uint playerId;
	public string playerName;
	public uint dps;
	public List<Award> awardList;
	
}

public struct Award
{
	public byte type;
	public uint num;
}