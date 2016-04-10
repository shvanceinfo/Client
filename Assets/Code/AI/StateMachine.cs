using UnityEngine;
using System.Collections;

public class StateMachine 
{
	public CharacterAI m_kOwner;
	
	State m_kCurrentState;
	State m_kPreviousState;
	State m_kGobalState;
	
	public void ChangeState(State newState)
	{
		
        m_kPreviousState = m_kCurrentState;
		
		
        m_kCurrentState.Exit(m_kOwner);
		
		
        m_kCurrentState = newState;
		
		
        m_kCurrentState.Enter(m_kOwner);
	}
	
	public void SetCurrentState(State st) 
    {
        m_kCurrentState = st;
	}
	
	public void SetPreviousState(State st) 
    {
		m_kPreviousState = st;
	}
	
  	public void SetGlobalState(State st) 
    {
        m_kGobalState = st;
	}
	
	public virtual void Update() 
    {
        if (m_kGobalState != null)
            m_kGobalState.Execute(m_kOwner);

        if (m_kCurrentState != null)
            m_kCurrentState.Execute(m_kOwner);
	}
	
    //public void handleMessage(Telegram tel){
		
    //    if (global_state!=null)   
    //        global_state.handleMessage(owner, tel);
		
    //    if (current_state!=null) 
    //        current_state.handleMessage(owner, tel);
    //}


    public virtual void ProcessMessage(StateEvent stEvent)
    {

    }
}
