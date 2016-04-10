using UnityEngine;
using System.Collections;
using manager;

public class UiDialog1 : MonoBehaviour {
	
	void OnEnable()
	{
		DialogManager.Instance.OnSetDialogMsg += SetMessage;
	}
	
	void OnDisable()
	{
		DialogManager.Instance.OnSetDialogMsg -= SetMessage;
	}
	
	void SetMessage(string msg)
	{
		UILabel msgLabel = transform.FindChild("lbl_message").GetComponent<UILabel>();
		msgLabel.text = msg;
	}
}
