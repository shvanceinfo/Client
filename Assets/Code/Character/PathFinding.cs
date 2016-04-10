using manager;
using UnityEngine;
using System.Collections;
using manager;

public class PathFinding : MonoBehaviour 
{
	private NavMeshAgent _agent;
	private CharacterPlayer _player;
	private bool _moving;  //人物开始移动
	private bool _hadMove; //已经移动过
	private NPC _findNPC; //following the main task
	private bool _gotoGate; //寻路到关卡

    private float m_fUpdateTime = 0.0f;


	void Awake()
	{
		_player = CharacterPlayer.sPlayerMe;
        //_agent = PlayerManager.Instance.agent;
		_moving = false;
		_agent.updateRotation = false;
		_agent.updatePosition = false;
		_hadMove = false;
		_gotoGate = false;
		_findNPC = null;
	}
	
	void Update()
	{
		if(_moving)
		{		
//				_moving = false;
			//if(!_agent.pathPending) //路径计算结束才开始走路
//			if(_agent.hasPath)
			{
				if(_agent.remainingDistance == 0 && _hadMove)
				{
					_moving = false;
					//MessageManager.Instance.sendMessageAskMoveArrive(CharacterPlayer.character_property.getSeverID()); //发送人物位置消息
					MessageManager.Instance.sendMessageAskMove(CharacterPlayer.character_property.getSeverID(), _player.getFaceDir(), _player.getPosition());
					if(TaskManager.Instance.isTaskFollow) //click the task following, character face to NPC
					{
						if(_gotoGate)
						{
                            TaskManager.Instance.isTaskFollow = false;
							if(Global.transportId != 0 && UIManager.Instance.isWindowOpen(UiNameConst.ui_raid))
							{
								RaidManager.Instance.initRaid();
							}
						}
						else
						{
						    if (TaskManager.Instance.FolllowPath.size == 0) //已经找到了NPC
						    {
                                TaskManager.Instance.isTaskFollow = false;
						        if (_findNPC == null)
						        {
						            _player.setFaceDir(NPCManager.Instance.mainTaskNPC.FaceDir);
						            NPCManager.Instance.mainTaskNPC.Action.showTaskDialog(true);
						        }
						        else
						        {
						            _player.setFaceDir(_findNPC.FaceDir);
						            _findNPC.Action.showTaskDialog(true);
						        }
						    }
						}
					}
                    
                    EventDispatcher.GetInstance().OnPathFindingArrived();
                    _player.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
				}
				else
				{
					_hadMove = true;
					Vector3 dircection = _agent.nextPosition - _player.getPosition();
					dircection.Normalize();
                    // 这里dircection 有可能为(0, 0, 0); 这时候放技能 特效用人物朝向就会不对 所以判断下
                    if (dircection.x != 0 || dircection.y != 0 || dircection.z != 0)
                    {
                        _player.setFaceDir(dircection);
                    }
                    else
                    {
                        if (!_agent.pathPending)
                        {
                            // 该路点寻不到
                            //_player.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                        }
                        
                    }

                    // 不用这个 直接位置赋值可能会跳人
                    _player.setPosition(_agent.nextPosition);

                    if (m_fUpdateTime > 0.2f)
                    {
                        MessageManager.Instance.sendMessageAskMove(CharacterPlayer.character_property.getSeverID(), _player.getFaceDir(), _player.getPosition());
                        m_fUpdateTime = 0.0f;
                    }
				}
			}

            m_fUpdateTime += Time.deltaTime;
		}
	}
	
	public void beginMove(NPC npc=null, bool gotoGate=false)
	{
		_moving = true;
		_hadMove = false;
		_findNPC = npc;
		_gotoGate = gotoGate;
	}

    public void StopMove()
    {
		if(_gotoGate)
		 	TaskManager.Instance.isTaskFollow = false;
        _moving = false;
        _hadMove = true;
        _agent.Stop();
    }

	public bool Moving
	{
		get {return _moving;}
	}
}
