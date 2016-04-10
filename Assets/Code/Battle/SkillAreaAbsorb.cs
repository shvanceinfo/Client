using UnityEngine;
using System.Collections;

public class SkillAreaAbsorb : MonoBehaviour {
	
	SkillAppear skill;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update  () {
		
	}
	
	public void setSkill(SkillAppear s) {
		skill = s;
	}
	
	public SkillAppear getSkill() {
		return skill;
	}
	
	void OnTriggerStay(Collider other) {
		
		Character c = other.gameObject.GetComponent<Character>();
		if (!c) return;
		
		if (c == skill.getOwner()) return;
		
		if (c.getType() == CharacterType.CT_MONSTER) {
			BattleManager.Instance.onSkillAreaAbsorbMonster(skill, (CharacterMonster)c, 5.0f, Time.deltaTime);
		}
		
    }
}
