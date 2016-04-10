using UnityEngine;
using System.Collections;
using helper;
using MVC.entrance.gate;
using manager;

public class BtnEquipTableMsg : MonoBehaviour
{

	const string Table1 = "Table1";
	const string Table2 = "Table2";
	const string Table3 = "Table3";
	const string Table4 = "Table4";
	const string Close = "Close";
 	
	Transform _trans;
	bool _tab1;
	bool _tab2;
	bool _tab3;
	bool _tab4;

	void Awake ()
	{
		this._trans = this.transform.parent;
	}

	void OnClick ()
	{

		switch (gameObject.name) {
		case Table1:
		
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Strengthen)) {
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table1);
			   
			break;
		case Table2:
		 
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Advanced)) { 
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table2);
			 
			break; 
		case Table3:
		 
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Refine)) {
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table3);
			 
			break;
		case Table4:
		 
			if (!FastOpenManager.Instance.CheckFunctionIsOpen (FunctionName.Inlay)) {
				return;
			}
			Gate.instance.sendNotification (MsgConstant.MSG_EQUIP_SWITCHING_TABLE, Table.Table4);
			 
			break;
		case Close:
			UIManager.Instance.closeWindow (UiNameConst.ui_equip);
            SkillTalentManager.Instance.ClearEffect();
			break;

		default:
			break;
		}
	}
}
