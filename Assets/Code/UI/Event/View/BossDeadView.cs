using UnityEngine;
using System.Collections;
using MVC.entrance.gate;
using mediator;
using manager;

public class BossDeadView : MonoBehaviour
{
	public float waitTime = 1f; //间隔时间
	private int _cdTime;
	private Transform _trans;
	private UILabel _price;
	private UILabel _time;
	private UISprite _img;
	
	void Awake ()
	{
		
		this._trans = this.transform;
		this._img = this._trans.FindChild ("diamond/Sprite").GetComponent<UISprite> ();
		this._price = this._trans.FindChild ("diamond/lblCost").GetComponent<UILabel> ();
		this._time = this._trans.Find ("info/time").GetComponent<UILabel> ();
	}
	
	void OnEnable ()
	{
		BossManager.Instance.BossDeadRegisterEvent ();
		Gate.instance.registerMediator (new BossDeadMediator (this));
	}
	
	void OnDisable ()
	{
		BossManager.Instance.BossDeadCancelEvent ();
		Gate.instance.removeMediator (MediatorName.BOSS_DEAD_MEDIATOR);
	}
	
	//显示错误信息
	public void ShowErr (string msg)
	{
		FloatMessage.GetInstance ().PlayNewFloatMessage (LanguageManager.GetText (msg), false, UIManager.Instance.getRootTrans ());
	}
	
	
	//显示死亡界面
	public void ShowDead (eGoldType type, int price, int cdTime)
	{
		this._price.text = price.ToString ();
		this._img.spriteName = SourceManager.Instance.getIconByType(type);
		this._cdTime = cdTime;
		StartCoroutine (ShowCD ());
	}
	
 	 
 	
	//开始cd倒计时
	IEnumerator ShowCD ()
	{
		WaitForSeconds wait = new WaitForSeconds (waitTime);
		
		for (int i = this._cdTime; i >=0; i--) {
            //this._time.text = string.Format(LanguageManager.GetText("msg_world_boss_cdtime"), string.Format("[eb5302]{0:00}:{1:00}[-]", i / 60, i % 60));
            this._time.text = string.Format("[eb5302]{0:00}:{1:00}[-]", i / 60, i % 60);
			if (i == 0) {
				Gate.instance.sendNotification (MsgConstant.MSG_BOSS_DEAD_TIME_REVIVE);
			}
			yield return wait;
		}
  		
	}
}
