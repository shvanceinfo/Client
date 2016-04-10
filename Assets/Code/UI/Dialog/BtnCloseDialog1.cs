using UnityEngine;
using System.Collections;
using manager;

public class BtnCloseDialog1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		UIManager.Instance.closeWindow(UiNameConst.ui_dialog1);
	}
}
