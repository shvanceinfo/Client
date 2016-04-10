using UnityEngine;
using System.Collections;


public class TelegramContext 
{
    
}

public class Telegram {

	public AIMessage msg;
	public TelegramContext context;
}

public class StateEvent
{
    public CharacterAI.CHARACTER_STATE state;
    public ArrayList paramList = new ArrayList();
}

public class TelegramContextBeattack : TelegramContext {
	
	public Vector3 hitBackDir;
	public float speedRate;
	public int perf;
	public bool alive;
}

// 进入战斗待机
public class TelegramContextIdle : TelegramContext
{

}


// 点击地面
public class TelegramContextEscape : TelegramContext
{
    public Vector3 vEscPot;
}

// 追击
public class TelegramContextPursue : TelegramContext
{
    public Vector3 vPursuePos;
}


// 进入战斗待机
public class TelegramContextFightIdle : TelegramContext
{
    
}

// 技能状态
public class TelegramContextSkill : TelegramContext
{
    public int skillId;
}

// 受击状态
public class TelegramContextBeHit : TelegramContext
{

}


