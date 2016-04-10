using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;

public class DungeonDeadView : MonoBehaviour
{
	
	public int CDTime = 5;  //倒计时时间
	public float waitTime =1f; //间隔时间
 
	private Transform _trans;
	private Transform _info;
	private Transform _bg;
	private Transform _btnRet;
	private Transform _btnRevive;
	private Transform _diamond;
	private Transform _failInfo;
	private Transform _dropInfo;
	private UILabel _time;
	void Awake ()
	{
		this._trans = this.transform;
		this._info = this._trans.Find ("info");
		this._bg = this._trans.Find ("bg");
		this._btnRet = this._trans.Find ("btnRet");
		this._btnRevive = this._trans.Find ("btnRevive");
		this._diamond = this._trans.Find ("diamond");
		this._time = this._info.Find("time").GetComponent<UILabel>();
		this._failInfo = this._trans.Find("failinfo");
		this._dropInfo = this._trans.FindChild ("bg/dropinfo");
	}
	
	void OnEnable ()
	{
		Gate.instance.registerMediator (new DungeonDeadMediator (this));
	}
	
	void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.DUNGEONDEAD_MEDIATOR);
		NPCManager.Instance.createCamera (false); //消除3D相机
	}
	
 	//显示错误信息
	public void ShowErr(string msg){
		FloatMessage.GetInstance ().PlayNewFloatMessage (LanguageManager.GetText (msg), false, UIManager.Instance.getRootTrans());
	}
	
	
	//显示死亡界面
	public void ShowDead (int price)
	{
		this._info.gameObject.SetActive (true);
		this._bg.Find ("info").gameObject.SetActive (false);
		this._btnRet.gameObject.SetActive (false);
		this._btnRevive.gameObject.SetActive (true);
		
		this._diamond.Find("lblCost").GetComponent<UILabel>().text = price.ToString();
		this._diamond.gameObject.SetActive (true);
		this._failInfo.gameObject.SetActive(false);
 
		StartCoroutine(ShowCD());
	}
	
	//显示失败界面
	public	void ShowFail ()
	{
		this._info.gameObject.SetActive (false);
		this._bg.Find ("info").gameObject.SetActive (true);
		this._btnRet.gameObject.SetActive (true);
		this._btnRevive.gameObject.SetActive (false);
		this._diamond.gameObject.SetActive (false);
		this._failInfo.gameObject.SetActive(true);
	}

	//显示主机掉线
	public void ShowMainPlayerDrop(){
		this._dropInfo.GetComponent<UILabel>().text = LanguageManager.GetText("dungeon_main_player_drop");
		this._dropInfo.gameObject.SetActive (true);
		this._info.gameObject.SetActive (false);
		this._bg.Find ("info").gameObject.SetActive (false);
		this._btnRet.gameObject.SetActive (true);
		this._btnRevive.gameObject.SetActive (false);
		this._diamond.gameObject.SetActive (false);
		this._failInfo.gameObject.SetActive(true);
	}
 
	//开始cd倒计时
	IEnumerator ShowCD ()
	{
		WaitForSeconds wait = new WaitForSeconds(waitTime);
		
		for (int i = this.CDTime; i >=0 ; i--) {
			this._time.text = i.ToString();
			if (i==0) {
				//print(1);
				DungeonManager.Instance.CameraFollowFriend();
			}
			yield return wait;
		}
  		
	}
	
	 
}
