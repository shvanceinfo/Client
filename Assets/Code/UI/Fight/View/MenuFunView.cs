using UnityEngine;
using System.Collections;
using manager;

public class MenuFunView : MonoBehaviour
{
 
	private static  bool isOpen = false; //菜单是否处于打开状态
	
	private Transform _trans;
	private GameObject _exp;
	Transform _rightTrans;
	TweenPosition _rightTp;
	TweenPosition _tp;

	public static bool IsOpen {
		set {
			isOpen = value;
		}
		get {
			return isOpen;
		}
	}
	
	void Awake ()
	{
		this._trans = this.transform;
		this._exp = this._trans.parent.FindChild ("Exp_Function").gameObject;
		this._rightTrans = this._trans.parent.parent.FindChild ("Bottom_Right");
		this._rightTp = _rightTrans.GetComponent<TweenPosition> ();

		this._tp = this._trans.GetComponent<TweenPosition> ();
	}
	
	// Use this for initialization
	void Start ()
	{
		Vector3 rightPos = _rightTrans.localPosition;
		this._rightTp.from = new Vector3 (rightPos.x, rightPos.y, rightPos.z);
		this._rightTp.to = new Vector3 (rightPos.x + 415, rightPos.y, rightPos.z);
	}

	
	// Update is called once per frame
	void Update ()
	{
		if (isOpen && Input.GetButtonDown ("Fire1")) {
			if (UICamera.hoveredObject == null) {//判断是否击中UI,只要击中空白处则隐藏
				Close ();
			}  
 
		}
	}

	public void Open ()
	{
		isOpen = true;
		this._tp.Play (true);
		this._rightTp.Play (true);
		ShowOrHideExp (false);
		EasyTouchJoyStickProperty.ShowJoyTouch (false);
	}
	
	public void Close ()
	{
		isOpen = false;
		this._tp.Play (false);
		this._rightTp.Play (false);
		ShowOrHideExp (true);
		EasyTouchJoyStickProperty.ShowJoyTouch (true);
	}

 
	
	/// <summary>
	/// 开关按钮
	/// </summary>
	public  void OnOpenFunc ()
	{
		if (Global.InArena ()) {
			return;
		}
 
		
		if (!isOpen) {
			Open ();
		} else {
			Close ();
		}
	}
	
	public void ShowOrHideExp (bool active)
	{
		this._exp.SetActive (active);
	}
	
	
	
	
}
