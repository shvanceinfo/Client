using System;
using UnityEngine;
using System.Collections;
//using System.Reflection.Emit;  
using System.Reflection;

// 角色表现
public class AnimSpeedRandomizer : MonoBehaviour
{

    public float minSpeed = 0.7f;
    public float maxSpeed = 1.5f;

	public float birth = 0.0f;
    public float idle = 0.0f;
	public float wait = 0.0f;
    public float pose = 0.0f;
    public float stand = 0.0f;
	public float run = 0.0f;
    public float run2 = 0.0f;
    public float hurt = 0.0f;
    public float rebound = 0.0f;

	public float skill1 = 0.0f;
	public float skill2 = 0.0f;
	public float die1 = 0.0f;
    public float die2a = 0.0f;



    public float attack1 = 0.0f;
    public float attack2 = 0.0f;
    public float attack3 = 0.0f;
    public float attack4 = 0.0f;
    public float attack5 = 0.0f;
    public float attack6 = 0.0f;
    public float attack7 = 0.0f;
    public float attack8 = 0.0f;
    public float attack9 = 0.0f;
    public float attack10 = 0.0f;
    public float attack11 = 0.0f;
	public float attack12 = 0.0f;




   
    public float chongzhuang1 = 0.0f;
    public float chongzhuang2 = 0.0f;
    public float chongjibo = 0.0f;
    public float chongjibo2 = 0.0f;
    public float tiaozhan = 0.0f;
    public float xuanfengzhan = 0.0f;


    public float skill_longyan1 = 0.0f;
    public float skill_longyan2 = 0.0f;
	public float skill_longyan3 = 0.0f;
    public float skill_huangxiang1 = 0.0f;
    public float skill_huangxiang2 = 0.0f;
    public float skill_huangxiang3 = 0.0f;
    public float skill_leiyun = 0.0f;
    public float skill_binghuan = 0.0f;


    public float skill_shalu01 = 0.0f;
    public float skill_shalu02 = 0.0f;
    public float skill_shalu03 = 0.0f;
    public float skill_bingshuang = 0.0f;
    public float skill_chuanci = 0.0f;
    public float skill_jianyu = 0.0f;


	public float skill1_1 = 0.0f;
	public float skill1_2 = 0.0f;
	public float skill1_3 = 0.0f;
	public float skill2_1 = 0.0f;
	public float skill2_2 = 0.0f;
	public float skill2_3 = 0.0f;
    

	public float skill_pet_01 = 0.0f;
    


	private const float PRECISION = 0.000001f;

    void Awake()
    {

    }

    void Start()
    {
        foreach (AnimationState anim in animation) 
        {
            if (anim.name.Equals("birth"))
            {
                SetAnimSpeed(anim, birth);
            }
			else if (anim.name.Equals("idle"))
			{
                SetAnimSpeed(anim, idle);
			}
			else if (anim.name.Equals("wait"))
			{
                SetAnimSpeed(anim, wait);
			}
            else if (anim.name.Equals("pose"))
            {
                SetAnimSpeed(anim, pose);
            }
            else if (anim.name.Equals("stand"))
            {
                SetAnimSpeed(anim, stand);
            }
            else if (anim.name.Equals("run"))
            {
                SetAnimSpeed(anim, run);
            }
            else if (anim.name.Equals("run2"))
            {
                SetAnimSpeed(anim, run2);
            }
            else if (anim.name.Equals("hurt"))
            {
                SetAnimSpeed(anim, hurt);
            }
            else if (anim.name.Equals("rebound"))
            {
                SetAnimSpeed(anim, rebound);
            }
            else if (anim.name.Equals("skill1"))
            {
                SetAnimSpeed(anim, skill1);
            }
            else if (anim.name.Equals("skill2"))
            {
                SetAnimSpeed(anim, skill2);
            }
			else if (anim.name.Equals("die1"))
			{
                SetAnimSpeed(anim, die1);
			}
            else if (anim.name.Equals("die2a"))
            {
                SetAnimSpeed(anim, die2a);
            }
            else if (anim.name.Equals("attack1"))
            {
                SetAnimSpeed(anim, attack1);
            }
            else if (anim.name.Equals("attack2"))
            {
                SetAnimSpeed(anim, attack2);
            }
            else if (anim.name.Equals("attack3"))
            {
                SetAnimSpeed(anim, attack3);
            }
            else if (anim.name.Equals("attack4"))
            {
                SetAnimSpeed(anim, attack4);
            }
            else if (anim.name.Equals("attack5"))
            {
                SetAnimSpeed(anim, attack5);
            }
            else if (anim.name.Equals("attack6"))
            {
                SetAnimSpeed(anim, attack6);
            }
            else if (anim.name.Equals("attack7"))
            {
                SetAnimSpeed(anim, attack7);
            }
            else if (anim.name.Equals("attack8"))
            {
                SetAnimSpeed(anim, attack8);
            }
            else if (anim.name.Equals("attack9"))
            {
                SetAnimSpeed(anim, attack9);
            }
            else if (anim.name.Equals("attack10"))
            {
                SetAnimSpeed(anim, attack10);
            }
            else if (anim.name.Equals("attack11"))
            {
                SetAnimSpeed(anim, attack11);
            }
			else if (anim.name.Equals("attack12"))
			{
				SetAnimSpeed(anim, attack11);
			}
            else if (anim.name.Equals("chongzhuang1"))
            {
                SetAnimSpeed(anim, chongzhuang1);
            }
            else if (anim.name.Equals("chongzhuang2"))
            {
                SetAnimSpeed(anim, chongzhuang2);
            }
            else if (anim.name.Equals("chongjibo"))
            {
                SetAnimSpeed(anim, chongjibo);
            }
            else if (anim.name.Equals("chongjibo2"))
            {
                SetAnimSpeed(anim, chongjibo2);
            }
            else if (anim.name.Equals("tiaozhan"))
            {
                SetAnimSpeed(anim, tiaozhan);
            }
            else if (anim.name.Equals("xuanfengzhan"))
            {
                SetAnimSpeed(anim, xuanfengzhan);
            }
            else if (anim.name.Equals("skill_longyan1"))
            {
                SetAnimSpeed(anim, skill_longyan1);
            }
            else if (anim.name.Equals("skill_longyan2"))
            {
                SetAnimSpeed(anim, skill_longyan2);
            }
			else if (anim.name.Equals("skill_longyan3"))
			{
				SetAnimSpeed(anim, skill_longyan3);
			}
            else if (anim.name.Equals("skill_huangxiang1"))
            {
                SetAnimSpeed(anim, skill_huangxiang1);
            }
            else if (anim.name.Equals("skill_huangxiang2"))
            {
                SetAnimSpeed(anim, skill_huangxiang2);
            }
            else if (anim.name.Equals("skill_huangxiang3"))
            {
                SetAnimSpeed(anim, skill_huangxiang3);
            }
            else if (anim.name.Equals("skill_leiyun"))
            {
                SetAnimSpeed(anim, skill_leiyun);
            }
            else if (anim.name.Equals("skill_binghuan"))
            {
                SetAnimSpeed(anim, skill_binghuan);
            }
            else if (anim.name.Equals("skill_shalu01"))
            {
                SetAnimSpeed(anim, skill_shalu01);
            }
            else if (anim.name.Equals("skill_shalu02"))
            {
                SetAnimSpeed(anim, skill_shalu02);
            }
            else if (anim.name.Equals("skill_shalu03"))
            {
                SetAnimSpeed(anim, skill_shalu03);
            }
            else if (anim.name.Equals("skill_bingshuang"))
            {
                SetAnimSpeed(anim, skill_bingshuang);
            }
            else if (anim.name.Equals("skill_chuanci"))
            {
                SetAnimSpeed(anim, skill_chuanci);
            }
            else if (anim.name.Equals("skill_jianyu"))
            {
                SetAnimSpeed(anim, skill_jianyu);
            }
			else if (anim.name.Equals("skill1_1"))
			{
				SetAnimSpeed(anim, skill1_1);
			}
			else if (anim.name.Equals("skill1_2"))
			{
				SetAnimSpeed(anim, skill1_2);
			}
			else if (anim.name.Equals("skill1_3"))
			{
				SetAnimSpeed(anim, skill1_3);
			}
			else if (anim.name.Equals("skill2_1"))
			{
				SetAnimSpeed(anim, skill2_1);
			}
			else if (anim.name.Equals("skill2_2"))
			{
				SetAnimSpeed(anim, skill2_2);
			}
			else if (anim.name.Equals("skill2_3"))
			{
				SetAnimSpeed(anim, skill2_3);
			}
			else if (anim.name.Equals("skill_pet_01"))
			{
				SetAnimSpeed(anim, skill2_3);
			}
            else
            {
                anim.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);

				Debug.Log(anim.name + " " + anim.speed);
            }
		}
    }

    void SetAnimSpeed(AnimationState anim, float value)
    {
        if (JudgeZero(value))
            anim.speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        else
            anim.speed = UnityEngine.Random.Range(value, value);
    }
//	public static void SetPropertyValue(object classInstance, string propertyName, object propertSetValue)  
//	{  
//		classInstance.GetType().InvokeMember(propertyName, BindingFlags.SetProperty,  
//		                                     null, classInstance, new object[] { Convert.ChangeType(propertSetValue, propertSetValue.GetType()) });  
//	} 

	bool JudgeZero(float value)
	{
		if (Math.Abs (value) <= PRECISION)
		{
			return true;
		}

		return false;
	}

    void Update()
    {

    }
}