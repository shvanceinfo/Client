using UnityEngine;
using System.Collections;

public class PlayerOtherAnimCallback : MonoBehaviour {
	
	CharacterPlayerOther player;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<CharacterPlayerOther>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public void playWeaponSound() {
		if (!player) return;
		Weapon w = player.GetComponentInChildren<Weapon>();
		if (!w) return;
		w.playWeaponSound();
	}
	
	public void playEffect(GameObject eff) {
		if (eff && player) {
            EffectManager.Instance.createFX(eff, player.transform, 2.0f);
		}
	}
	
	public void playEffectWorldCoord(GameObject eff) {
		if (eff && player) {
            EffectManager.Instance.createFX(eff, player.transform.position, player.transform.rotation, 2.0f);
		}
	}
	
	public void showWeaponTrail() {
		if (!player) return;
		player.showWeaponTrail();
	}
	
	public void hideWeaponTrail() {
		if (!player) return;
		player.hideWeaponTrail();
	}
	
	public void shakeCameraSmallPlayerFront(float time) {

        //EffectManager.Instance.createCameraShake(0, 0.1f, time);
	}
	
	public void shakeCameraBigPlayerFront(float time) {

        //EffectManager.Instance.createCameraShake(0, 0.6f, time);
	}
	
	public void shakeCameraSmallUpDown(float time) {

        //EffectManager.Instance.createCameraShake(1, 0.1f, time);
	}
	
	public void shakeCameraBigUpDown(float time) {

        //EffectManager.Instance.createCameraShake(1, 0.6f, time);
	}
	
	public void shakeCameraSmall(float time) {

        //EffectManager.Instance.createCameraShake(2, 0.1f, time);
	}
	
	public void shakeCameraBig(float time) {

        //EffectManager.Instance.createCameraShake(2, 0.6f, time);
	}
	
	public void createDashArea() {

	}
	
	public void createSmashArea() {
		
	}
	
	public void playAudio( AudioClip clip ) {
		
		if (audio) {
			audio.PlayOneShot(clip);
		}
	}

    public void RangeDamageHurt()
    {
        if (!player)
            return;

        SkillAppear kCurSkill = player.skill.getCurrentSkill();
        if (kCurSkill != null)
        {
            // draw lines
            DrawRangeLines(kCurSkill.m_kSkillInfo.skillAngle, kCurSkill.m_kSkillInfo.skillRadius);
        }
    }

    void DrawRangeLines(int nDegree, float radius)
    {
        Vector3 vecFaceDir = player.getFaceDir();
        vecFaceDir.Normalize();



        Quaternion rotL = Quaternion.Euler(0, -nDegree * 0.5f, 0);
        GraphicsUtil.m_vecLineStart = player.getPosition() +
            rotL * vecFaceDir * radius;

        GraphicsUtil.m_vecLineStart += new Vector3(0.0f, 0.1f, 0.0f);

        Quaternion rotR = Quaternion.Euler(0, nDegree * 0.5f, 0);
        GraphicsUtil.m_vecLineEnd = player.getPosition() +
            rotR * vecFaceDir * radius;

        GraphicsUtil.m_vecLineEnd += new Vector3(0.0f, 0.1f, 0.0f);

        GraphicsUtil.m_vecOriginal = player.getPosition();

    }

    public void clearMonsterFightDirty()
    {
        // 其他人的就先不管        
    }

    public void GeneradeCollider()
    {
        // 同上
    }
}
