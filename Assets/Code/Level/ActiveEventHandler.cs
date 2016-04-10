using UnityEngine;
using System.Collections;

public class ActiveEventHandler : LevelEventHandler {
	
	public LevelEventHandler to_active;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onTrigger() {
		
		if (to_active)
			to_active.setActive();
		
		Destroy(gameObject);
	}
}