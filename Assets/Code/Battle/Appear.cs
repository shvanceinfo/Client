using System;
using UnityEngine;
using System.Collections;

// 角色表现
public class Appear
{
    public enum BATTLE_STATE
    {
        BS_NULL = 0,
        BS_IDLE,
        BS_MOVE,
        BS_PURSUE,
        BS_BORN,
        BS_PING_KAN,                // 平砍表现
        BS_SKILL,                   // 技能表现 不包括平砍
        BS_BE_HIT,                  // 受击表现
        BS_BE_HIT_BACK,
        BS_BE_HIT_FLY,
        BS_BE_HIT_BROKEN,
        BS_ROLL,
        BS_DIE,

        BS_DIZZY,
        BS_SMOOTH_POS,

        BS_GOBLIN_RUN,          // 哥布林玩法 跑步表现
    }

    protected bool is_active = false;

    protected float time_length = 0;

    protected float time_since_begin = 0;

    protected string animation_name = "";

    protected Character owner = null;

    protected Appear next_inner = null;

    protected Appear next = null;

    protected BATTLE_STATE battle_state = BATTLE_STATE.BS_NULL;


    public virtual void init()
    {

    }

    public virtual void active()
    {
        
    }

    public virtual void deActive()
    {
        on_deActive();
    }

    public virtual void update(float delta)
    {
        updateTime(delta);
    }


    public void setOwner(Character c)
    {

        owner = c;
    }

    public Character getOwner()
    {
        return owner;
    }

    public void setNextCmdInner(Appear n)
    {

        next_inner = n;
    }

    public Appear getNextCmdInner()
    {

        return next_inner;
    }

    public void setNextCmd(Appear n)
    {

        next = n;
    }

    public Appear getNextCmd()
    {

        return next;
    }

    public virtual bool isActive()
    {

        if (!is_active) return false;

        if (time_since_begin >= time_length) return false;

        return true;
    }


    public BATTLE_STATE GetBattleState() 
    { 
        return battle_state; 
    }

    protected virtual bool IsLoopAnimation()
    {
        return false;
    }

    protected virtual void on_active(float fAnimTime = 0)
    {
        time_since_begin = 0;
        is_active = true;

        // 动画 begin
        // 为0使用默认动画长度
        if (fAnimTime == 0)
        {
            time_length = owner.animation[animation_name].length;
        }
        else
        {
            time_length = fAnimTime;
        }

		if (!IsLoopAnimation())
		{
			time_length /= owner.animation[animation_name].speed;
		}

        if (animation_name != "")
        {
            owner.playAnimation(animation_name);
        }
        // 动画 end

        // 设置状态
        owner.SetState(battle_state);
    }

    protected virtual void on_deActive()
    {
        is_active = false;
    }

    protected float updateTime(float delta)
    {

        if (time_since_begin >= time_length)
        {
            is_active = false;
            return 0;
        }

        float t = Mathf.Clamp(delta, 0, time_length - time_since_begin);

        time_since_begin += t;

        return t;
    }

    public void setAnimationTime(float time)
    {

        owner.setAnimationTime(animation_name, time);
    }

    public void setAnimationSpeed(float speed)
    {

        owner.setAnimationSpeed(animation_name, speed);
    }
}