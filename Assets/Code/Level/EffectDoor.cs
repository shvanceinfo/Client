using UnityEngine;
using System.Collections;

public class EffectDoor : LevelEventHandler {
	
	bool is_open = false;
	
	public Transform open_perf;
	public Transform close_effect;
	
	//ArrayList close_cache;
	
	void Awake() {
		//close_cache = new ArrayList();
	}

	// Use this for initialization
	void Start () {
	
		is_open = false;
		
		/*
		Transform[] transformChildren = close_effect.GetComponentsInChildren<Transform>();
		foreach (Transform transformChild in transformChildren) {
		    close_cache.Add(transformChild);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void openDoor() {
		
		Transform[] transformChildren = close_effect.GetComponentsInChildren<Transform>();
		foreach (Transform transformChild in transformChildren) {
		    if (transformChild.renderer) {
				transformChild.renderer.enabled = false;
			}
			if (transformChild.collider) {
				transformChild.collider.enabled = false;
			}
		}
		
		if (open_perf) {
            GameObject open_effect = BundleMemManager.Instance.instantiateObj(open_perf, transform.position, transform.rotation);
			Destroy(open_effect, 2.0f);
		}
		
		is_open = true;
	}
	
	void closeDoor() {
		
		Transform[] transformChildren = close_effect.GetComponentsInChildren<Transform>();
		foreach (Transform transformChild in transformChildren) {
		    if (transformChild.renderer) {
				transformChild.renderer.enabled = true;
			}
			if (transformChild.collider) {
				transformChild.collider.enabled = true;
			}
		}
		
		is_open = false;
	}
	
	public override void onTrigger() {
		
		if (!active)
			return;
		
		if (is_open) {
			closeDoor();
		}
		else {
			openDoor();
		}
	}
}
