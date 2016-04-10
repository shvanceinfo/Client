/**该文件实现的基本功能等
function: 副本的vo
author:zyl
date:2014-3-19
**/
using manager;
using System;

namespace model
{
	public class DungeonVo
	{
		private ushort _passNum;
		private BetterList<DungeonTeamInfo> _dungeonTeamList ;
		private BetterList<PeopleInfo> _peopleList;
		
		public DungeonVo ()
		{
			this._dungeonTeamList = new BetterList<DungeonTeamInfo> ();
			this._peopleList = new BetterList<model.PeopleInfo>();
		}

		public ushort PassNum {
			get {
				return this._passNum;
			}
			set {
				_passNum = value;
			}
		}

		public BetterList<DungeonTeamInfo> DungeonTeamList {
			get {
				return this._dungeonTeamList;
			}
		}

		public BetterList<PeopleInfo> PeopleList {
			get {
				return this._peopleList;
			}
		}
	}
	
	public class DungeonTeamInfo
	{
		public string name;       //副本队长的名字
		public ushort playerNum;  //队伍人数
		public uint teamId;		//队伍ID
	}
	
	public class PeopleInfo
	{
		public string name;		//角色名
		public ushort level;	//角色等级
		public CHARACTER_CAREER career; 	//职业
		public byte gender; 	//性别
		public uint   battlePower; //角色战斗力
		public bool	  leader;	//是否是队长
		
	}
	
	
	
}
