using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using manager;

public enum RotationType
{
	/// <summary>
	/// Constant player.
	/// </summary>
	Player = 1,
	/// <summary>
	/// Constant wing.
	/// </summary>
	Wing = 2,
	/// <summary>
	/// Constant map.
	/// </summary>
	Map =3,
	Pet =4,
	MonsterReward=5
}

public class rolRotation : MonoBehaviour
{
	const float PRECISION = 0.8f;
	public RotationType type = RotationType.Player;
	private Transform _trans;
	private static bool _hasDrag;
	private float _totalDeltaY; //y轴移动的距离
	private Vector3 _origPos; //原来的位置
	private static bool _hasUnPress; //已经释放过按钮了
	private static object itemObj = new object ();
 
	void Awake ()
	{
		_hasDrag = false;
		_totalDeltaY = 0f;
		_hasUnPress = true;
	}
	
	void Start ()
	{
		_hasDrag = false;
		_hasUnPress = true;
	}
	
	
	// 滑动
	void OnDrag (Vector2 delta)
	{//print(1);
//		print (_hasUnPress);
		if (_trans == null) {
			
			switch (type) {
			case RotationType.Player:
				_trans = NPCManager.Instance.ModelCamera.transform.FindChild ("ui_player");
				break;
			case RotationType.Wing:
				_trans = NPCManager.Instance.ModelCamera.transform.FindChild ("wing_ui");
				_origPos = _trans.localPosition;
				break;
			case RotationType.Map:
				//print (UIManager.Instance.Camera2D.transform.parent.name);
				_trans = UIManager.Instance.Camera2D.transform.FindChild ("ui_raid/moveLayer");
				_origPos = _trans.localPosition;
				break;
			case RotationType.Pet:
				_trans = NPCManager.Instance.ModelCamera.transform.FindChild ("pet_ui");
				_origPos = _trans.localPosition;
				break;
			case RotationType.MonsterReward:
				_trans = UIManager.Instance.Camera2D.transform.FindChild ("ui_monster_reward/cmdlist/currentcmd/touch");
				_origPos = _trans.localPosition;
				break;
			default:
				break;
			}
					
		}
		if (_trans != null) {
			if (type == RotationType.Player) {
				if (delta.x > 0.5f)
					_trans.localEulerAngles = new Vector3 (_trans.localEulerAngles.x, _trans.localEulerAngles.y - delta.x, _trans.localEulerAngles.z);
				else if (delta.x < -0.5f)
					_trans.localEulerAngles = new Vector3 (_trans.localEulerAngles.x, _trans.localEulerAngles.y - delta.x, _trans.localEulerAngles.z);
			}
			if (type == RotationType.Wing && _hasUnPress) {
				_hasDrag = true;
				_totalDeltaY += delta.x / 100;
				if (_totalDeltaY < 0) {
					if (WingManager.Instance.previewLadder < 10) { //上滑的时候必须没有满阶
						_trans.localPosition = new Vector3 (_origPos.x + _totalDeltaY, _origPos.y, _origPos.z);
						if (_totalDeltaY < -PRECISION) {
							_trans = null; //置位翅膀，重新放置新翅膀到模型
							_totalDeltaY = 0f;
							Gate.instance.sendNotification (MsgConstant.MSG_WING_DRAG_NEXT);
							_hasUnPress = false;
						}
					}
				} else if (_totalDeltaY > 0) {
					if (WingManager.Instance.previewLadder > 1) { //下滑的时候必须大于一阶
						_trans.localPosition = new Vector3 (_origPos.x + _totalDeltaY, _origPos.y, _origPos.z);
						if (_totalDeltaY > PRECISION) {
							_trans = null; //置位翅膀，重新放置新翅膀到模型
							_totalDeltaY = 0f;
							Gate.instance.sendNotification (MsgConstant.MSG_WING_DRAG_PREV);
							_hasUnPress = false;
						}
					}
				}
			}
			if (type == RotationType.Map && _hasUnPress) {
				MapRotation (ref this._totalDeltaY, delta);
			}
			if (type == RotationType.Pet && _hasUnPress) {
				_hasDrag = true;
				_totalDeltaY += delta.x / 100;
				if (_totalDeltaY < 0) {
					if (PetManager.Instance.PreviewLadder < PetManager.Instance.DictionaryPet.Count) { //上滑的时候必须没有满阶
						_trans.localPosition = new Vector3 (_origPos.x + _totalDeltaY, _origPos.y, _origPos.z);
						if (_totalDeltaY < -PRECISION) {
							_trans = null; //置位翅膀，重新放置新翅膀到模型
							_totalDeltaY = 0f;
							Gate.instance.sendNotification (MsgConstant.MSG_PET_DRAG_NEXT);
							_hasUnPress = false;
						}
					}
				} else if (_totalDeltaY > 0) {
					if (PetManager.Instance.PreviewLadder > 1) { //下滑的时候必须大于一阶
						_trans.localPosition = new Vector3 (_origPos.x + _totalDeltaY, _origPos.y, _origPos.z);
						if (_totalDeltaY > PRECISION) {
							_trans = null; //置位翅膀，重新放置新翅膀到模型
							_totalDeltaY = 0f;
							Gate.instance.sendNotification (MsgConstant.MSG_PET_DRAG_PREV);
							_hasUnPress = false;
						}
					}
				}
			}
			if (type == RotationType.MonsterReward && _hasUnPress) {
				MonsterRewardRotation (ref this._totalDeltaY, delta);
			}
		}
	}

	// 停止滑动
	void OnPress (bool press)
	{//print(2);
		if (_hasDrag && !press) {
//			if (_trans != null)
//				_trans.localPosition = new Vector3 (_origPos.x, _origPos.y, +_origPos.z);
			_totalDeltaY = 0;
			_hasDrag = false;
			_hasUnPress = true;
		}
	}

	/// <summary>
	/// Maps the rotation.
	/// </summary>
	/// <param name='_totalDeltaY'>
	/// _total delta y.
	/// </param>
	/// <param name='delta'>
	/// Delta.
	/// </param>
	public static void MapRotation (ref float _totalDeltaY, Vector2 delta)
	{
		_hasDrag = true;
		_totalDeltaY += delta.x / 100;
		if (_totalDeltaY < 0) {
			if (_totalDeltaY < -PRECISION) {
				_totalDeltaY = 0f;
				Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_SHOW_NEXT);
//				print (1);
				_hasUnPress = false;
			}
		} else if (_totalDeltaY > 0) {
			if (_totalDeltaY > PRECISION) {
  
				_totalDeltaY = 0f;
				Gate.instance.sendNotification (MsgConstant.MSG_RAID_BTN_SHOW_PREV);
//				print (2);
				_hasUnPress = false;
			}
		}
		
	}
	
	public static void MonsterRewardRotation (ref float _totalDeltaY, Vector2 delta)
	{
		_hasDrag = true;
		_totalDeltaY += delta.x / 100;
		if (_totalDeltaY < 0) {
			if (_totalDeltaY < -PRECISION) {
				_totalDeltaY = 0f;
				
				MonsterRewardManager.Instance.NextPage ();
				_hasUnPress = false;
//				print (1);
 
			}
		} else if (_totalDeltaY > 0) {
			if (_totalDeltaY > PRECISION) {
				
				_totalDeltaY = 0f;
				MonsterRewardManager.Instance.PrevPage ();
				_hasUnPress = false;
//				print (2);
 
			}
		}
	}
	
	public static bool HasUnPress {
		get {
			return _hasUnPress;
		}
		set {
			_hasUnPress = true;
		}
	}
	
 
	
	
}
