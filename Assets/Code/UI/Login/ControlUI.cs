/**该文件实现的基本功能等
function:使用缓动来实现UI的出现跟隐藏
author:ljx
date:2013-10-28
**/
using UnityEngine;
using System.Collections;
using manager;

public class ControlUI : MonoBehaviour 
{

	enum AnimState 
	{		
		PLAY_FIRST = 0,
		PLAY_SECOND,
		PLAY_THIRD,
		CLOSE_ANIM
	}
	
	public GameObject _serverRoleUI; 		//选择服务器跟角色界面的UI 
	public GameObject _backgroundUI;		//背景UI
	//public GameObject _downloadSceneUI;		//下载场景资源ui
	
	private GameObject _selectServerUI; 	//选择服务器界面 
	private GameObject _createRoleUI; 		//创角界面UI 
	private GameObject _roleBottom;   		//创角界面底部
	private GameObject _backGround;			//进入游戏与Login的背景
	private GameObject _serSelectBg;		//选择服务器的背景框
	private GameObject _serverSelectBtn;	//选择服务器底部的框框
	private GameObject _serverMask;			//选服的遮板
	private GameObject _selectVocation1;
	private GameObject _selectVocation2;
	private GameObject _selectVocation3;
	
    //private GameObject _maleAnim; 		//男剑士
    //private GameObject _femaleAnim; 	//女剑士
    //private GameObject _maleStar;		//男剑士身上星星动画
    //private GameObject _femaleStar;		//女剑士身上星星动画
    //private GameObject _bossAnim; 		//BOSS动画
    //private GameObject _smokeAnim; 		//地面的烟
    //private GameObject _smokeAnim1; 	//地面的烟
    //private GameObject _skyAnim;
    //private GameObject _skyAnim1; 
	
	private AnimState _animState;  	//播放状态
	private bool _playWaitNow; 		//开始播放boss待机
	private bool _playBossFly; 		//开始播放boss的fly动作
	
	public GameObject m_AccountName; //当前帐号名

    public GameObject _loginFunction;   //登陆模块
    public GameObject _createFunction;  //创建角色
    public GameObject _backgroundFunction;  //背景
	// Use this for initialization
	void Awake()
	{
        _backgroundFunction = GameObject.Find("create_role").transform.FindChild("create_role_cam/Background/myBgSpite").gameObject;
        _createFunction = GameObject.Find("create_role").transform.FindChild("create_role_cam/ui_createRole").gameObject;
        _createFunction.SetActive(false);
        _loginFunction = GameObject.Find("create_role").transform.FindChild("create_role_cam/server_role/game_bg").gameObject;
		LanguageManager.LoadTxt();
		_serverRoleUI = GameObject.Find(Constant.CREATE_SERVER_ROLE);
		_backgroundUI = _serverRoleUI.transform.parent.FindChild("Background").gameObject;
		//_downloadSceneUI = _serverRoleUI.transform.parent.FindChild("LoadingPanel").gameObject;
		_selectServerUI = GameObject.Find(Constant.CHOOSE_SERVER);
		_createRoleUI = GameObject.Find(Constant.CREATE_ROLE);
		_selectVocation1 = GameObject.Find(Constant.SELECT_VOCATION1);
		_selectVocation2 = GameObject.Find(Constant.SELECT_VOCATION2);
		_selectVocation3 = GameObject.Find(Constant.SELECT_VOCATION3);
		_backGround = GameObject.Find("game_bg_sp");
		//_gameLogo = GameObject.Find("game_logo");
		_serSelectBg = GameObject.Find("ser_bg_obj");
		_serverSelectBtn = GameObject.Find("ser_btn");
		_serverMask = GameObject.Find("server_mask");
		_roleBottom = GameObject.Find("role_bottom");
		//_roleInfo = GameObject.Find("roleInfo");
		//setLabelText("enterLabel", "btn_enter_game", 24);     不需要了
		//setLabelText("reselectLabel", "btn_rechoose_server", 24);
		//setLabelText("groupLabel", "server_list", 18);
		//setLabelText("loginLabel", "latest_login", 18);
		//setLabelText("current_label","current_server", 25);
		//setLabelText(Constant.SERVER_ROLE_TITLE, "select_server", 28);
			
        //_maleAnim = GameObject.Find("jianshi_pose");
        //_femaleAnim = GameObject.Find("longnv_pose");
        //_maleStar = GameObject.Find("juese_nan");
        //_femaleStar = GameObject.Find("juese_nv");
        //_bossAnim = GameObject.Find("xtt_boss");
        //_smokeAnim = GameObject.Find("xuanren_yan");
        //_smokeAnim1 = GameObject.Find("xuanren_yan1");
        //_skyAnim = GameObject.Find("XR_01");
        //_skyAnim1 = GameObject.Find("XR_011");
        //m_AccountName = GameObject.Find("accountName");
		
//		UISprite sp = DealTexture.Instance.mirrorSprite(GameObject.Find("titleBg").GetComponent<UISprite>(), DealTexture.MIRROR_POS.right); //镜像标题
//		sp.depth -= 2; //低于label
		_createRoleUI.SetActive(false); 		//不显示创角界面
        _selectVocation1.SetActive(false);
        _selectServerUI.SetActive(false);
		_selectVocation2.SetActive(false);
		_selectVocation3.SetActive(false);
//		UISprite serSelectBg = GameObject.Find("game_ser_bg").GetComponent<UISprite>(); //选服的的背景
//		DealTexture.Instance.mirrorSprite(serSelectBg, DealTexture.MIRROR_POS.topRight);
//		DealTexture.Instance.mirrorSprite(serSelectBg, DealTexture.MIRROR_POS.bottomLeft);
//		DealTexture.Instance.mirrorSprite(serSelectBg, DealTexture.MIRROR_POS.bottomRight);
		_serSelectBg.SetActive(false);
		_serverMask.SetActive(false);
		//_roleInfo.SetActive(false);
		
        //_maleAnim.SetActive(false); 
        //_femaleAnim.SetActive(false);
        //_maleStar.SetActive(false);
        //_femaleStar.SetActive(false);
        //_bossAnim.SetActive(false);
        //_smokeAnim.SetActive(false);
        //_smokeAnim1.SetActive(false);
        //_skyAnim.SetActive(false);
        //_skyAnim1.SetActive(false);
		_animState = AnimState.CLOSE_ANIM;
		_playWaitNow = false;
		_playBossFly = false;
		
		//_serverRoleUI.SetActive(false);
		//_backgroundUI.SetActive(false);

	}
	
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
//        if(_bossAnim.animation.isPlaying) //标记开始播放
//            _playBossFly = true;
//        if(_playBossFly && !_bossAnim.animation.isPlaying) //标记播放结束
//            _playWaitNow = true;
//        if(_animState != AnimState.CLOSE_ANIM && !animation.isPlaying && _playWaitNow) //直到动画不再播放
//        {	
//            if(_animState == AnimState.PLAY_FIRST)
//            {
//                _createRoleUI.SetActive(true);
//                _roleBottom.SetActive(false);
//                _selectVocation1.SetActive(true);  //开启鼠标响应区域
//                _selectVocation2.SetActive(true);
//                _selectVocation3.SetActive(true);
//                _roleInfo.SetActive(true);
//                AnimationClip clip = _bossAnim.animation.GetClip("wait");
//                if(clip != null)
//                {
//                    _bossAnim.animation.clip = clip;
//                    _bossAnim.animation.wrapMode = WrapMode.Loop;
//                    _bossAnim.animation.Play();
//                }
//            }
//            else if(_animState == AnimState.PLAY_SECOND)
//            {
//                _createRoleUI.SetActive(true);
////				if(CreateRole.Vocation == CHARACTER_CAREER.CC_MAGICIAN)
////				{
////					_roleBottom.transform.Find("bottom/inputAll").gameObject.SetActive(false);
////					_roleBottom.transform.Find("bottom/sureObj").gameObject.SetActive(false);
////					_roleBottom.transform.Find("bottom/notOpen").gameObject.SetActive(true);
////				}
////				else
//                {
//                    _roleBottom.transform.Find("bottom/inputAll").gameObject.SetActive(true);
//                    _roleBottom.transform.Find("bottom/sureObj").gameObject.SetActive(true);
//                    _roleBottom.transform.Find("bottom/notOpen").gameObject.SetActive(false);
//                }
//                _roleBottom.SetActive(true);
//            }
//            _animState = AnimState.CLOSE_ANIM;
//        }
	}
	
	public void playNewAnimation(CHARACTER_CAREER vocation)
	{
        //string clipName;
        //if(vocation == CHARACTER_CAREER.CC_ARCHER)
        //{
        //    clipName = "xuanren_je02";
        //    _maleStar.SetActive(true);
        //}
        //else if(vocation == CHARACTER_CAREER.CC_SWORD)
        //{
        //    clipName = "xuanren_je01";
        //    _femaleStar.SetActive(true);
        //}
        //else
        //{
        //    clipName = "xuanren_je03";
        //    _femaleStar.SetActive(true);
        //}
        //AnimationClip clip = animation.GetClip(clipName);
        ////animation.AddClip(clip, clipName);
        //_animState = AnimState.PLAY_SECOND;
        //animation.clip = clip;
        //animation[clipName].speed = 1f;	
        //animation.Play();
	}
	
	public void rollBackAnimation()
	{
		string clipName = animation.clip.name;
		animation[clipName].speed = -1f;		
		animation[clipName].time = animation.clip.length;
		animation.Play();
		_animState = AnimState.PLAY_FIRST;
        //_maleStar.SetActive(false);
        //_femaleStar.SetActive(false);
	}
	
	public void realCreateRole()
	{
        //_selectServerUI.SetActive(false);  //隐藏一切关于服务器有关的UI
        //_backGround.SetActive(false);			
        //_serSelectBg.SetActive(false);		
        ////_gameLogo.SetActive(false);			
        //_serverSelectBtn.SetActive(false);	
        //_animState = AnimState.PLAY_FIRST;
        //_maleAnim.active = true;
        //_femaleAnim.active = true;
        //_bossAnim.active = true;
        //_smokeAnim.active = true;
        //_smokeAnim1.active = true;
        //_skyAnim.active = true;
        //_skyAnim1.active = true;
        //m_AccountName.SetActive(false);
        //animation.Play();
        _createFunction.SetActive(true);
        _backgroundFunction.SetActive(false);
	}
	
	private void setLabelText(string labelName, string txtName, int size)
	{
		UILabel label = GameObject.Find(labelName).GetComponent<UILabel>();
		
		//label.font.dynamicFontSize = size;
		label.fontSize = size;
		label.text = LanguageManager.GetText(txtName);
	}
	
	//getter and setter	
	public GameObject SerSelectBg
	{
		get { return _serSelectBg; }
	}
	
	public GameObject SelectServerUI
	{
		get { return _selectServerUI; }
	}
	
	public GameObject ServerMask
	{
		get { return _serverMask; }
	}
	
	public GameObject ServerRoleUI
	{
		get { return _serverRoleUI; }
	}
	

	
	public GameObject ServerSelectBtn
	{
		get { return _serverSelectBtn; }
	}
	
}
