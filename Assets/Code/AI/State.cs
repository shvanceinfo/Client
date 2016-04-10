using UnityEngine;
using System.Collections;

public class State 
{
	protected StateMachine m_kMachine;
	
	public void SetStateMachine(StateMachine machine) 
    {
        m_kMachine = machine;
	}
	
	public virtual void Enter(CharacterAI ai)
    {

    }

    public virtual void Execute(CharacterAI ai)
    {

    }

    public virtual void Exit(CharacterAI ai)
    {

    }

    public virtual void handleMessage(CharacterAI ai, Telegram tel)
    {

    }


    public virtual void SetParam(Vector3 param)
    {

    }

    public virtual void SetParam(int param)
    {

    }

    public virtual void SetParam()
    {

    }

    public virtual void SetParam(float param1, float param2)
    {

    }

    public virtual void SetParam(ArrayList param)
    {

    }
}
