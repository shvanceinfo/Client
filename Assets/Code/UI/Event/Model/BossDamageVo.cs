/**该文件实现的基本功能等
function: 世界boss伤害的vo
author:zyl
date:2014-5-27
**/
using manager;
using System;
using System.Collections.Generic;

namespace model
{
	public struct PlayerDamageVo
	{
		private uint _playerId;
		private string _name;
		private uint _totalDamage;
		
		public string Name {
			get {
				return this._name;
			}
			set {
				_name = value;
			}
		}

		public uint PlayerId {
			get {
				return this._playerId;
			}
			set {
				_playerId = value;
			}
		}

		public uint TotalDamage {
			get {
				return this._totalDamage;
			}
			set {
				_totalDamage = value;
			}
		}
	}
	
	public class BossDamageVo
	{
		private uint _meDamage;
		private uint _bossRemainHp;
		private List<PlayerDamageVo> _playerDamageList = new List<PlayerDamageVo> ();

		public uint MeDamage {
			get {
				return this._meDamage;
			}
			set {
				_meDamage = value;
			}
		}

		public List<PlayerDamageVo> PlayerDamageList {
			get {
				return this._playerDamageList;
			}
			set {
				_playerDamageList = value;
			}
		}

		public uint BossRemainHp {
			get {
				return this._bossRemainHp;
			}
			set {
				_bossRemainHp = value;
			}
		}
	}
}