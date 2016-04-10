using UnityEngine;
using System.Collections;

public class TowerSpawnHandler : LevelEventHandler {
	
	public int monster_area_id;
	public float trigger_time = 5.0f;
	bool spawn = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (spawn) {
			
			if (trigger_time > 0) {
				trigger_time -= Time.deltaTime;
			}
			else {

                //BattleEmodongku.GetInstance().waitTowerMonsterWave(monster_area_id);
				
				Destroy(gameObject);
			}
		}
	}
	
	public override void onTrigger() {
		
		if (!spawn) {
			spawn = true;
			
			//notice title
			if (monster_area_id == 100) {
				//
			}
			else {
				
			}
		}
	}
}
