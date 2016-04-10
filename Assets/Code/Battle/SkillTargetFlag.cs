using UnityEngine;
using System.Collections;

public class SkillTargetFlag : MonoBehaviour {
	
	public Character target;
	bool visible = true;
	Renderer target_flag_render;
	
	// Use this for initialization
	void Start () {
		target_flag_render = GetComponentInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target && !CharacterAI.IsInState(target, CharacterAI.CHARACTER_STATE.CS_DIE)) {
			
			if (!visible) {
				target_flag_render.enabled = true;
				visible = true;
			}
			
			transform.position = target.getPosition() + new Vector3(0,0.15f,0);
		}
		else {
			
			if (visible) {
				target_flag_render.enabled = false;
				visible = false;
			}
		}
	}
	
	public void playAnimation() {
		animation.Play("xuanren");
	}
}
