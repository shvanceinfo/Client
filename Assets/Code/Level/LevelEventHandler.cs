using UnityEngine;
using System.Collections;

public class LevelEventHandler : MonoBehaviour {
	
	public bool active = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void onTrigger() {
		
	}
	
	public void setActive() {
		if (!active) {
			active = true;
		}
	}
}
