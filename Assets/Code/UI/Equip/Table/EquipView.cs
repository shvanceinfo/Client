using UnityEngine;
using System.Collections;
using manager;
using mediator;
using MVC.entrance.gate;

public class EquipView : MonoBehaviour
{

	Transform _trans;
	private GameObject Obj1;    //强化
	private GameObject Obj2;    //进阶
	private GameObject Obj3;    //xx
	private GameObject Obj4;    //镶嵌

	private bool IsInitiat1;
	private bool IsInitiat2;
	private bool IsInitiat3;
	private bool IsInitiat4;

	private void Awake ()
	{
		this._trans = this.transform;
 
		if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Strengthen,false)) {
			this._trans.FindChild ("Table/Table1").GetComponent<UICheckBoxColor> ().enabled = false;
		}
		if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Advanced,false)) { 
			this._trans.FindChild ("Table/Table2").GetComponent<UICheckBoxColor> ().enabled = false;
		}
		if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Refine,false)) {
			this._trans.FindChild ("Table/Table3").GetComponent<UICheckBoxColor> ().enabled = false;
		}
		if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Inlay,false)) {
			this._trans.FindChild ("Table/Table4").GetComponent<UICheckBoxColor> ().enabled = false;
		}

		Obj1 = transform.FindChild ("Strengthen").gameObject;
		Obj2 = transform.FindChild ("Advanced").gameObject;
		Obj3 = transform.FindChild ("Refine").gameObject;
		Obj4 = transform.FindChild ("Inlay").gameObject;

		IsInitiat1 = false;
		IsInitiat2 = false;
		IsInitiat3 = false;
		IsInitiat4 = false;
	}

	private void Start ()
	{
//        Gate.instance.sendNotification(MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table1);
	}

	private void OnEnable ()
	{
		Gate.instance.registerMediator (new EquipMediator (this));
	}

	private void OnDisable ()
	{
		Gate.instance.removeMediator (MediatorName.EQUIP_MEDIATOR);
	}

	public void SwitchingTable (Table table)
	{
		Obj1.SetActive (false);
		Obj2.SetActive (false);
		Obj3.SetActive (false);
		Obj4.SetActive (false);
		switch (table) {
		case Table.None:
			break;
		case Table.Table1:
			Obj1.SetActive (true);
			if (!IsInitiat1) {
				IsInitiat1 = true;
				StrengThenManager.Instance.Initial (); 
			}
			StrengThenManager.Instance.UpdateView ();
			break;
		case Table.Table2:
			Obj2.SetActive (true);
			if (!IsInitiat2) {
				IsInitiat2 = true;
				AdvancedManager.Instance.Initial ();
			}
			AdvancedManager.Instance.UpdateView ();
			break;
		case Table.Table3:
			Obj3.SetActive (true);
            Obj3.transform.FindChild("Description/Buttom/Label1").GetComponent<UILabel>().text = LanguageManager.GetText("refine_info_first");
            Obj3.transform.FindChild("Description/Buttom/Label2").GetComponent<UILabel>().text = LanguageManager.GetText("refine_info_second");
			if (!IsInitiat3) {
				IsInitiat3 = true;
				RefineManager.Instance.Initial ();
			}
			RefineManager.Instance.UpdateView ();
			break;
		case Table.Table4:
			Obj4.SetActive (true);
			if (!IsInitiat4) {
				IsInitiat4 = true;
				InlayManager.Instance.Initial ();
			}
			InlayManager.Instance.UpdateView ();
			break;
		default:
			break;
		}
	}
}
