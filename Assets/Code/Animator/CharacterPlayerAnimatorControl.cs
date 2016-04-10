using UnityEngine;
using System.Collections;

public class CharacterPlayerAnimatorControl : MonoBehaviour {

    protected Animator animator;
    protected AnimatorStateInfo animatorState;

    int curComboCount = 0;

    private static readonly string IdleAppear = "Base Layer.wait";
    private static readonly string attackState1 = "pingkan.attack1";
    private static readonly string attackState2 = "skill.attack2";


    //private static readonly string attackState3 = "bbb.技能.attack4";

    private static readonly string animSkillId = "animSkillId";

	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
	}
	
	// Update is called once per frame
	void Update () {

        //float v = Input.GetAxis("Vertical");
        //float h = Input.GetAxis("Horizontal");
        //animator.SetFloat("speed", v);

        

        if (!animatorState.IsName(IdleAppear))
        {
            animator.SetInteger(animSkillId, 0);
        }

        
        

        if (animatorState.IsName(attackState1) && (animatorState.normalizedTime > 0.3f) && (curComboCount == 2))
        {
            // 当在攻击1状态下，并且当前状态运行了0.8正交化时间（即动作时长的80%），并且用户在攻击1状态下又按下了“攻击键”  
            animator.SetInteger(animSkillId, 2);
        }

   

        if (Input.GetKeyDown("z"))
        {
            Attack();
        }

        if (Input.GetKeyDown("b"))
        {
            animator.SetInteger(animSkillId, 5);
        }
	}

    void Attack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(IdleAppear))
        {
            // 在待命状态下，按下攻击键，进入攻击1状态，并记录连击数为1  
            animator.SetInteger(animSkillId, 1);
            curComboCount = 1;
        }
        else if (stateInfo.IsName(attackState1))
        {
            // 在攻击1状态下，按下攻击键，记录连击数为2（切换状态在Update()中）  
            curComboCount = 2;
        }
    }

    public void SetTransitionValue(int nValue)
    {
        animator.SetInteger(animSkillId, nValue);
    }
}
