using UnityEngine;
using System.Collections;

public class PlayerState
{

    protected PlayerStateMachine machine;

    public void setStateMachine(PlayerStateMachine m)
    {
        machine = m;
    }

    public virtual void enter(PlayerAI ai) { }

    public virtual void execute(PlayerAI ai) { }

    public virtual void exit(PlayerAI ai) { }

    public virtual void handleMessage(PlayerAI ai, Telegram tel) { }


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
