using UnityEngine;
using System.Collections;

public class LevelTrigger : MonoBehaviour {
	
	public LevelEventHandler trigger_handler;
	public bool once = true;
	//0: 进入触发 1: 停留触发
	public int trigger_type = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider other) 
    {
		if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
		{
            // 非主机不能触发
            return;
		}
		
		if (trigger_type == 1) return;
		
        CharacterPlayer cp = other.gameObject.GetComponent<CharacterPlayer>();
		if (!cp) return;
		
		if (trigger_handler) {
			
			if (!trigger_handler.active) return;
			
			//if (!triggered) {
				trigger_handler.onTrigger();
			
				if (once) {
					Destroy(gameObject);
				}
			//	triggered = true;
			//}
		}
    }
	
	void OnTriggerStay (Collider other) 
    {
        if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
        {
            // 非主机不能触发
            return;
        }


		if (trigger_type == 0) return;
		
        CharacterPlayer cp = other.gameObject.GetComponent<CharacterPlayer>();
		if (!cp) return;
		
		if (trigger_handler) {
			
			if (!trigger_handler.active) return;
			
			//if (!triggered) {
				trigger_handler.onTrigger();
			
				if (once) {
					Destroy(gameObject);
				}
			//	triggered = true;
			//}
		}
    }
}
