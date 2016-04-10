/**该文件实现的基本功能等
function: 关卡视图的管理器
author:ljx
date:2013-11-09
**/
using UnityEngine;
using System.Collections;
using model;
using MVC.entrance.gate;

namespace manager
{
	public class RaidManager
	{
		public const uint NORMAL_MAP_SUFFIX = 0;
		public const uint HARD_MAP_SUFFIX = 1;
		const uint FIRST_MAP_ID = 20010001;
		private static RaidManager _instance;
		private Hashtable _chapterHash; //章节的相关信息
		private Hashtable _mapVoHash; //存储mapVo的Hash
		private Hashtable _passMapHash; //通关地图数据
		private Object _raidPrefab;	//关卡的预制件
		private ChapterVo _currentChapter; 	//当前关卡
		private MapVo _currentRaid;				//当前的关卡VO
		private bool _hasInitChapter;			//是否初始化过章节Hash
		private uint _normalMaxPassID;      //普通已通关的最大ID
		private uint _hardMaxPassID;        //精英已通关的最大ID
		private int _maxChapterSequence;   //当前通关的最大章节
		private bool _showChapter1;          //是否显示第一个Panel
		private bool _isHardOpen;           //精英难度是否开
		private int _prevNormalChapter;     //预览的普通难度章节
		private int _prevHardChapter;        //预览或打开的精英难度章节
		private bool _showHard;             //是否显示精英按钮
			
		private RaidManager ()
		{
			_chapterHash = new Hashtable ();
			_mapVoHash = new Hashtable ();
			_passMapHash = new Hashtable ();
			_currentChapter = null;
			_currentRaid = null;
			_raidPrefab = null;
			_hasInitChapter = false;
			_showChapter1 = true;
			_prevNormalChapter = 1; //默认可预览第一章节
			_prevHardChapter = 101;   //默认可预览第一章节
			_showHard = false;
		}
		
		//初始化关卡
		public void initRaid ()
		{
			initRaidVo (); //先初始化数据	
			if (_currentRaid == null)
				_showHard = false;
			else
				_showHard = _currentRaid.isHard;
			UIManager.Instance.openWindow (UiNameConst.ui_raid); //在这里打开UI才可以注册事件
			_raidPrefab = loadRaidSource (UiNameConst.ui_raid_item);
			_showChapter1 = true; //默认显示第一个章节
			setVoViaPassMap (); //根据历史记录设置VO,不管是否有VO都要设置最大章节
			if (TaskManager.Instance.isTaskFollow) {      
				Task task = TaskManager.Instance.followTask;
				if (task != null && task.taskState == eTaskState.eInProgress && task.mapId != 0) {
					_currentRaid = _mapVoHash [task.mapId] as MapVo; //当前关卡
					_showHard = _currentRaid.isHard;
					_currentRaid.isTaskGate = true; //该关卡属于任务寻路状态
					_currentChapter = _chapterHash [_currentRaid.whichChapter] as ChapterVo; //当前章节
				}
			}

			Gate.instance.sendNotification (MsgConstant.MSG_RAID_OPEN);
		}
		
		//获取相应地图ID的VO
		public MapVo getRaidVo (uint mapID)
		{
			MapVo vo = null;
			if (mapID > 0) {
				if (_mapVoHash.Contains (mapID)) {
					vo = _mapVoHash [mapID] as MapVo;
				} else {
					vo = new MapVo ();
					_mapVoHash.Add (mapID, vo);
				}
			}
			return vo;
		}

		//替换新的关卡
		public MapVo changeCurentRaid (uint mapID)
		{
			if (_currentRaid.mapID == mapID)
				return _currentRaid;
			else {
				MapVo vo = _currentRaid;
				_currentRaid = _mapVoHash [mapID] as MapVo;
				return vo;
			}
		}

		//切换普通跟精英
		public bool changeNormalHard (bool isHard)
		{            
			if (isHard) {
				if (_showHard) //如果当前在精英关卡,无需切换
					return false;
				_currentChapter = _chapterHash [_prevHardChapter] as ChapterVo;			
				_showHard = true;
				MapVo maxVo = _currentChapter.getMaxPassVo (_showHard); //获取当前能激活的关卡
				if (maxVo != null) {
					_currentRaid = maxVo;
					this._maxChapterSequence = _currentRaid.whichChapter + 1;
				} else {
					this._maxChapterSequence = _prevHardChapter;
				}
				return true;
			} else {
				if (!_showHard) //如果当前在普通关卡
					return false;
				_currentChapter = _chapterHash [_prevNormalChapter] as ChapterVo;
				_showHard = false;
				MapVo maxVo = _currentChapter.getMaxPassVo (_showHard); //获取当前能激活的关卡
				if (maxVo != null) {
					_currentRaid = maxVo;
					this._maxChapterSequence = _currentRaid.whichChapter + 1;
				} else {
					this._maxChapterSequence = _prevNormalChapter;
				}
					
				return true;
			}
		}

		//预览关卡, showNext是否显示下一章还是上一章
		public bool previewChapter (bool showNext)
		{
			int chapter = _currentChapter.chapterSequence;
			if (showNext)
				chapter++;
			else
				chapter--;
			if (chapter >= this.MinChapterSequence && chapter <= _maxChapterSequence) { //只能预览最大关卡
				if (_chapterHash.ContainsKey (chapter)) {
					_currentChapter = _chapterHash [chapter] as ChapterVo;
					MapVo maxVo = _currentChapter.getMaxPassVo (_showHard); //副本类型跟当前类型肯定一致
					if (maxVo != null)
						_currentRaid = maxVo;
					if (_showHard)
						_prevHardChapter = chapter;
					else
						_prevNormalChapter = chapter;
					_showChapter1 = !_showChapter1; //重置标记,更新数据
					return true;
				}
			}
			return false;
		}

		//切换账户清空关卡历史记录
		public void resetAllMapVos()
		{
            _hardMaxPassID = 0;
            _maxChapterSequence = 0;
            _currentChapter = null;
            _currentRaid = null;
            _raidPrefab = null;
            _hasInitChapter = false;
            _showChapter1 = true;
            _prevNormalChapter = 1; //默认可预览第一章节
            _prevHardChapter = 101;   //默认可预览第一章节
            _showHard = false;
            _isHardOpen = false;
		    foreach (MapVo mapVo in _mapVoHash.Values)
		    {
                mapVo.isTaskGate = false;
                mapVo.isPass = false;
                mapVo.canEnter = false;
                mapVo.starNum = 0;
		    }
		}
		
		//根据通关最大的ID获取VO,并且获取章节
		private void setVoViaPassMap ()
		{
			_normalMaxPassID = FIRST_MAP_ID;
			_hardMaxPassID = getRelateID (_normalMaxPassID, true) - 1;
			uint realMapID = _normalMaxPassID * 10 + NORMAL_MAP_SUFFIX;
			_maxChapterSequence = 1; //默认最大关卡为1
			if (_passMapHash.Count > 0) {
				foreach (sPassMap passMap in _passMapHash.Values) {
					int starNum = 0;
					if (passMap.easy != 0x00) {
						realMapID = passMap.mapID * 10 + NORMAL_MAP_SUFFIX;
						if (passMap.mapID > _normalMaxPassID)
							_normalMaxPassID = passMap.mapID; //获取最大的通关关卡
						starNum = passMap.easy;
					}
					if (passMap.normal != 0x00) {
						realMapID = passMap.mapID * 10 + HARD_MAP_SUFFIX;
						if (passMap.mapID > _hardMaxPassID)
							_hardMaxPassID = passMap.mapID; //获取最大的通关关卡
						_isHardOpen = true; //有通关记录必然可以打开精英副本
						starNum = passMap.normal;
					}
					MapVo vo = _mapVoHash [realMapID] as MapVo;
					vo.isPass = true; //设置VO为通关
					vo.canEnter = true; //通关的关卡都可以进入
					vo.starNum = starNum; //先设置通关的星星数目为3颗
				}
				uint checkMapID = getRelateID ((_hardMaxPassID + 1), false) * 10 + NORMAL_MAP_SUFFIX;
				if (_mapVoHash.ContainsKey (checkMapID) && (_mapVoHash [checkMapID] as MapVo).isPass) { //只有对应的普通关卡通关了才能进入对应精英关卡
					checkMapID = (_hardMaxPassID + 1) * 10 + HARD_MAP_SUFFIX; //置位为精英关卡ID
					(_mapVoHash [checkMapID] as MapVo).canEnter = true; //下关的精英关卡可以进入
					_isHardOpen = true; //精英副本已经打开了
				}
				realMapID = (_normalMaxPassID + 1) * 10 + NORMAL_MAP_SUFFIX; //最后判断普通关卡,并作为第一次打开的参数
				if (!_mapVoHash.ContainsKey (realMapID)) //如果下一关卡不在地图列表中
					realMapID = _normalMaxPassID * 10 + NORMAL_MAP_SUFFIX;
				else
					(_mapVoHash [realMapID] as MapVo).canEnter = true; //下关的普通关卡可以进入
				if (_currentRaid == null) { //第一次打开副本,不存在打开页面的历史记录
					_currentRaid = _mapVoHash [realMapID] as MapVo;
					_currentRaid.canEnter = true; //有历史记录获取到的肯定可以进入
					_currentChapter = _chapterHash [_currentRaid.whichChapter] as ChapterVo;
					_maxChapterSequence = _currentRaid.whichChapter + 1;
				} else { //不重置历史记录
					if (_showHard && _mapVoHash.ContainsKey (checkMapID)) { //当前在精英副本中
						if ((_mapVoHash [checkMapID] as MapVo).isHard)
							_currentRaid = _mapVoHash [checkMapID] as MapVo;
						_currentRaid.canEnter = true; //下关的精英关卡可以进入
					} else {
						_currentRaid = _mapVoHash [realMapID] as MapVo;
						_currentRaid.canEnter = true; //下关的普通关卡可以进入
					}
					_currentChapter = _chapterHash [_currentRaid.whichChapter] as ChapterVo; //重置章节到当前历史记录
					_maxChapterSequence = _currentRaid.whichChapter + 1; //重置可查看的最大章节
				}
			} else { //无通关记录记录
				_currentRaid = _mapVoHash [realMapID] as MapVo;
				_currentRaid.canEnter = true; //无关卡普通关卡第一关就可以进入
				_currentChapter = _chapterHash [_currentRaid.whichChapter] as ChapterVo;
				_maxChapterSequence = _currentRaid.whichChapter + 1;
			}
			if (_showHard)
				_prevHardChapter = _currentRaid.whichChapter;
			else
				_prevNormalChapter = _currentRaid.whichChapter;
		}
		
		//加载关卡的资源,关卡需要两个加载比较特殊
		private UnityEngine.Object loadRaidSource (string uiName)
		{
			Object obj = null;
			UiSourceVo sourceVo = UIManager.Instance.getSourceVo(uiName);
			if(sourceVo != null)
			{
			    obj = BundleMemManager.Instance.getPrefabByName(sourceVo.loadPath, EBundleType.eBundleUI);
			}	
			return obj;			
		}
		
		//在读取配置文件结束时候设置相应章节的VO
		private void initRaidVo ()
		{
			if (!_hasInitChapter) {
				foreach (MapVo vo in _mapVoHash.Values) {
					ChapterVo chapterVo = _chapterHash [vo.whichChapter] as ChapterVo;
					if (chapterVo != null) {
						BetterList<MapVo> mapVos = chapterVo.mapVos;
						mapVos.Add (vo);
					}
				}
				_hasInitChapter = true;
			}
			if (_currentRaid != null)  //如果存在关卡历史记录,先清除当前关卡的任务标记
				_currentRaid.isTaskGate = false; 
		}

		//根据ID获取相应的精英或普通难度副本,getHard=true根据普通获取精英
		private uint getRelateID (uint mapID, bool getHard)
		{
			uint newID = 0;
			if (getHard) {
				newID = (mapID / 10000 + 1) * 10000 + mapID % 10000;
			} else {
				newID = (mapID / 10000 - 1) * 10000 + +mapID % 10000;
			}
			return newID;
		}
		
		/// <summary>
		/// Updates the chapter award.
		/// </summary>
		/// <param name='isHard'>
		/// Is hard.
		/// </param>
		public void UpdateChapterAward (bool isHard)
		{
			Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_UPDATE_CHAPTER_AWARD, isHard);
		}
		
		//getter and setter
		public static RaidManager Instance {
			get { 
				if (_instance == null)
					_instance = new RaidManager ();
				return _instance; 
			}
		}
		
		public Hashtable ChapterHash {
			get { return _chapterHash; }
		}
		
		public Object RaidPrefab {
			get { return _raidPrefab; }
		}
		
		public ChapterVo CurrentChapter {
			get { return _currentChapter; }
		}
		
		public MapVo CurrentRaid {
			get { return _currentRaid; }
		}
						
		public Hashtable PassMapHash {
			get { return _passMapHash; }
		}

		public bool ShowChapter1 {
			get { return _showChapter1; }
		}

		public int MaxChapterSequence {
			get { return _maxChapterSequence; }
		}

		public bool IsHardOpen {
			get { return _isHardOpen; }
		}

		public bool showHard {
			get { return _showHard; }
		}
		
		public int MinChapterSequence {
			get {
				if (this.showHard) {
					return 101;
				} else {
					return 1;
				}
			}
		}
		
	}
}
