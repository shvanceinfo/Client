using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using NetGame;

public class XCodeToUnityMsgInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SendRequestPayment(string szAppleReceipt)
	{
		string[] szArray = szAppleReceipt.Split(';');
		bool bIsDebug = false;
		if (szArray[0] == "true")
		{
			bIsDebug = true;
		}
		else
		{
			bIsDebug = false;
		}

		GCAsChargeIOS chargeIos = new GCAsChargeIOS(bIsDebug, szArray[1]);
		NetBase.GetInstance().Send(chargeIos.ToBytes());
	}
}

