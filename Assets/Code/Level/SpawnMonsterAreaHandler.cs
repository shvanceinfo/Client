using UnityEngine;
using System.Collections;

public class SpawnMonsterAreaHandler : LevelEventHandler {
	
	public int monster_area_id;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void onTrigger() {

       // BattleEmodongku.GetInstance().waitTowerMonsterWave((uint)monster_area_id);
	}
}
