using UnityEngine;
using System.Collections;

public class FireRain : SpecialEffect {
	
	public float life_time = 10.0f;
	public float step_delay = 1.0f;
	public int point_num;
	public float range = 3.0f;
	public float unit_delta = 2.0f;
	public SkillAppear skill_cmd;
	string step1_res = "Effect/Effect_Prefab/Monster/Act/str_boss_skill03";
	//string step2_res = "Skill/FireRainUnit";
	
	
	class Unit {
		public Vector3 pos;
		public bool dirty1;
		public float delta1;
		public bool dirty2;
		public float delta2;
	}
	
	ArrayList rains;
	
	void Awake () {
		
		rains = new ArrayList();
	}
	
	// Use this for initialization
	void Start () {
	
		for (int i = 0; i < point_num; i++) {
			Unit u = new Unit();
			u.pos = new Vector3(Random.Range(-range,range),0.2f,Random.Range(-range,range)) + transform.position;
			u.dirty1 = false;
			u.delta1 = Random.Range(0, unit_delta);
			u.dirty2 = false;
			u.delta2 = u.delta1 + step_delay;
			rains.Add(u);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i = 0; i < rains.Count; i++) {
			Unit u = (Unit)rains[i];
			if (!u.dirty1) {
				if (u.delta1 < 0) {
                    GameObject asset = BundleMemManager.Instance.getPrefabByName(step1_res, EBundleType.eBundleMonster);
                    GameObject U1 = BundleMemManager.Instance.instantiateObj(asset, u.pos, Quaternion.identity);
					Destroy(U1, step_delay);
					
					u.dirty1 = true;
				}
				else {
					u.delta1 -= Time.deltaTime;
				}
			}
			
			if (!u.dirty2) {
				if (u.delta2 < 0) {
                    //GameObject asset = BundleMemManager.Instance.getPrefabByName(step2_res);
                    //GameObject U2 = BundleMemManager.Instance.instantiateObj(asset, u.pos, Quaternion.identity);
                    //SkillArea sa = U2.GetComponentInChildren<SkillArea>();
                    //if (sa) {
                    //    sa.setSkill(skill_cmd);
                    //}
                    //Destroy(U2, 1.5f);
					
                    //u.dirty2 = true;
				}
				else {
					u.delta2 -= Time.deltaTime;
				}
			}
			
			if (u.dirty1 && u.dirty2) {
				rains.RemoveAt(i);
				i--;
			}
		}
		
		if (life_time > 0) {
			life_time -= Time.deltaTime;
		}
		else {
			Destroy(gameObject);
		}
	}
}
