using UnityEngine;
using System.Collections;

public class TestShowWin : LevelEventHandler {
	
	public GameObject win_lable;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onTrigger() {
		
		if (win_lable) {
			win_lable.SetActive(true);
		}
	}
}
