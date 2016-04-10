/**该文件实现的基本功能等
function: 关卡单个章节的数据表现
author:ljx
date:date:2014-04-03
**/
using UnityEngine;
using manager;
using model;
using MVC.entrance.gate;

public class RaidView : MonoBehaviour
{
	private UILabel _title;  			//标题
	private GameObject[] _activeStars;	//激活的星星数目
	private GameObject[] _inActiveStars;	//非激活的星星
	private GameObject _taskTag;		//任务标记
	private UISprite _iconSprite;		//背景使用哪个图集
	private MapVo _myMapVo;				//该视图对应的VO
	
	private float _totalDeltaY;			//拖拽的距离
	
	void Awake ()
	{
		_activeStars = new GameObject[MapInfoView.STAR_NUM];
		_inActiveStars = new GameObject[MapInfoView.STAR_NUM]; 
		for (int i=1; i<=MapInfoView.STAR_NUM; i++) {
			_activeStars [i - 1] = transform.Find ("star/active/star" + i).gameObject;
			_activeStars [i - 1].SetActive (false); //默认都没有星星
			_inActiveStars [i - 1] = transform.Find ("star/inActive/star" + i).gameObject;
			_inActiveStars [i - 1].SetActive (false); //默认都没有星星
		}
		_taskTag = transform.Find ("taskTag").gameObject;
		_taskTag.SetActive (false);
		_iconSprite = transform.Find ("bg").GetComponent<UISprite> ();
		_title = transform.Find ("title/Label").GetComponent<UILabel> ();

	}
	
	//初始化视图
	public void initView ()
	{
		if (_myMapVo != null) {
			if (_myMapVo == RaidManager.Instance.CurrentRaid )  //如果是任务关卡并且是点击任务追踪
				_taskTag.SetActive (true);
			for (int i=0; i<_myMapVo.FormatStarNum; i++)
				_activeStars [i].SetActive (true);
			if (_myMapVo.isPass) { //如果通关了显示星星
				for (int i=_myMapVo.starNum; i<MapInfoView.STAR_NUM; i++)
					_inActiveStars [i].SetActive (true);
			}
			if (_myMapVo.canEnter)
				_iconSprite.spriteName = _myMapVo.gateIcon;
			else
				_iconSprite.spriteName = _myMapVo.gateIcon + SpriteNameConst.RAID_INACTIVE_SUFFIX;
			if (_myMapVo == RaidManager.Instance.CurrentRaid) //当前关卡标记为点亮状态
				_iconSprite.spriteName = _myMapVo.gateIcon + SpriteNameConst.RAID_ACTIVE_SUFFIX;
			_iconSprite.MakePixelPerfect ();
			_title.text = _myMapVo.gateName;
		}
	}
	
	//切换任务视图状态
	public void transferView (MapVo oldVo)
	{
        _taskTag.SetActive(false);
		if (oldVo == _myMapVo) {
			if (_myMapVo.canEnter)
				_iconSprite.spriteName = _myMapVo.gateIcon;
			else
				_iconSprite.spriteName = _myMapVo.gateIcon + SpriteNameConst.RAID_INACTIVE_SUFFIX;
        }
        else if (_myMapVo == RaidManager.Instance.CurrentRaid)
        {
            _iconSprite.spriteName = _myMapVo.gateIcon + SpriteNameConst.RAID_ACTIVE_SUFFIX; //当前关卡标记为点亮状态
            _taskTag.SetActive(true);
        }
			

		_iconSprite.MakePixelPerfect ();
	} 
	
	//设置VO
	public MapVo MyMapVo {
		set { _myMapVo = value; }
	}
	
 
	
	#region 关卡切换的滑动
	// 滑动
	void OnDrag (Vector2 delta)
	{ 
		if (rolRotation.HasUnPress) {
			rolRotation.MapRotation(ref this._totalDeltaY,delta);
		}
		
	}

 
 	void OnPress (bool press)
	{ 
		if ( !press) {
 
			rolRotation.HasUnPress = true;
		}
	}
	#endregion
	
	
}

