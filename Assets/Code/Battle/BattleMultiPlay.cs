using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NetGame;
using manager;


// 同步的物件
public enum NetSyncObj
{
    NSO_NONE = 0,
    NSO_ARCHER_OBJ = 1,     //弓箭手的普通攻击飞翔物件
    NSO_MAG_OBJ = 2,           // 法师的普通攻击飞翔物件
    NSO_MEIDUSHA_OBJ = 3,  // 美杜莎普通攻击飞翔物件
    NSO_END,
}

//玩家状态
public enum PeopleStatus
{
	NORMAL=0,//玩家正常状态
	DEAD ,//玩家死亡状态
	DROP 	//玩家掉线状态
}

public struct BattleMultiInfo{
	public PeopleStatus Status;//默认状态
	public bool IsLeader;//是否是队长
}
 


// 多人副本
class BattleMultiPlay
{
	public enum EMultiResult
	{
		MR_NONE = -1,
		MR_SUCCESS = 0,		// 副本成功
		MR_FAILED_ALL_DEAD, // 副本失败（全部阵亡）
		MR_FAILED_LEADER_OFFLINE,	// 副本失败 (主机（队长）掉线)
	}

    // 由主机发来的 刷怪的信息 先在这里保存信息，等我这个客户端OnLevelLoad完成onEnterLevel这里会清掉所有怪后才能用这个信息刷怪
    // 否则副机有可能刷不出怪
    public GSNotifyObjectAppear m_kMonsterInfo;

    public bool m_bEnterLevel = false;
    public bool m_bSpawed = false;

	public uint m_un32TeamID = 0;
	public Transform m_kBattlePrefab = null;
	static private BattleMultiPlay _instance;


    public Dictionary<NetSyncObj, List<Vector3>> m_kMissilePos = new Dictionary<NetSyncObj, List<Vector3>>();

	static public BattleMultiPlay GetInstance ()
	{
		if (_instance == null) {
			_instance = new BattleMultiPlay ();
			return _instance;
		}

		return _instance;
	}

	public BattleMultiPlay ()
	{
        
	}

    public void ResetData()
    {
        m_bSpawed = false;
        m_bEnterLevel = false;
        m_kMonsterInfo = null;
        OnDestroy();
    }
    

	// 多人副本中有单位收到伤害消息
	public void OnObjectHurt (GSNotifyObjectHurm kHurtData)
	{
		// 主机发来的
		if (kHurtData.m_bHost == 1) 
        {
			if (!CharacterPlayer.character_property.getHostComputer ()) 
            {
				// 如果是主机发来的伤害消息 直接表现
				RealDamageProcess (kHurtData);
			}
		} 
        else 
        {
			// 非主机发来的伤害消息 先给主机来处理
			if (CharacterPlayer.character_property.getHostComputer ()) 
            {
				// 应该是非主机上的怪受击了
				CharacterMonster cm = MonsterManager.Instance.GetMonster ((int)kHurtData.m_un32ObjID);

				if (cm) 
                {
					if (CharacterAI.IsInState (cm, CharacterAI.CHARACTER_STATE.CS_DIE)) 
                    {
						// 怪物已死 其他人还在搞它，不能进入鞭尸状态
						//Debug.Log (string.Format ("多人副本战斗 主机 怪已死"));
					} 
                    else 
                    {
						// 主机开始对怪进行扣血
						cm.GetProperty ().setHP (cm.GetProperty ().getHP () - kHurtData.m_n32HPValue);
						//Debug.Log (string.Format ("多人副本战斗 主机 怪扣血 {0}", kHurtData.m_n32HPValue));
						if (cm.GetProperty ().getHP () == 0) 
                        {
							// 确实死了
							//Debug.Log (string.Format ("多人副本战斗 主机 怪被搞死"));
							cm.GetAI ().SendStateMessage (CharacterAI.CHARACTER_STATE.CS_DIE);
						}
                        else
                        {
                            if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(cm.monster_property.template_id).isShouJi)
                            {
                                ArrayList param = new ArrayList();
                                param.Add(0.0f);
                                param.Add(Vector3.zero);
                                cm.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
                            }

                            //Debug.Log(string.Format("多人副本战斗 主机 怪受伤 {0}", kHurtData.m_n32HPValue));

                            DAMAGE_TYPE eDamageType = (DAMAGE_TYPE)kHurtData.m_n32Effect;
                            eHurtType hType = eHurtType.normalHurt;

                            switch (eDamageType)
                            {
                                case DAMAGE_TYPE.DT_COMMON_DAMAGE:
                                    {
                                        hType = eHurtType.normalHurt;
                                    }
                                    break;
                                case DAMAGE_TYPE.DT_BLAST_DAMAGE:
                                    {
                                        hType = eHurtType.doubleHurt;
                                    }
                                    break;
                                case DAMAGE_TYPE.DT_BUFF_DAMAGE:
                                    {
                                        hType = eHurtType.normalHurt;
                                    }
                                    break;
                                case DAMAGE_TYPE.DT_WITH_STAND:
                                    {
                                        hType = eHurtType.withstandHurt;
                                    }
                                    break;
                            }

                            FloatBloodNum.Instance.PlayFloatBood(false, kHurtData.m_n32HPValue, cm.getTagPoint("help_hp"), hType);
                        }

                        MessageManager.Instance.sendObjectHurt(true,
                            (uint)cm.GetProperty().GetInstanceID(),
                            kHurtData.m_n32HPValue,
                            cm.GetProperty().getHP(),
                            kHurtData.m_n32Effect);
					}
				} 
                else 
                {
					// 怪找不到 或者怪已经死了 被主机Destroy 再有伤害来就不对了
					//Debug.Log ("怪已死 不准鞭尸");
				}
			}
		}
	}

	// 非主机 begin------------------------------------------------------------------------------------------------------------------------------------------
	// 多人副本中伤害消息带来的流程
	public void RealDamageProcess (GSNotifyObjectHurm kHurtData)
	{
		// 判断是否是自己
		if (CharacterPlayer.character_property.GetInstanceID () == kHurtData.m_un32ObjID) 
        {
			// 自己受到怪的攻击
			DamageProcessByType (CharacterPlayer.sPlayerMe, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
			return;
		}

		// 再判断是其他人还是怪
        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)kHurtData.m_un32ObjID);
		if (cpo) 
        {
			// 其他人被怪打 副本中不能人打人 走被怪打流程
            DamageProcessByType(cpo, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
			return;
		}

        CharacterMonster cm = MonsterManager.Instance.GetMonster((int)kHurtData.m_un32ObjID);
		if (cm) 
        {
			// 怪被打了
            DamageProcessByType(cm, (DAMAGE_TYPE)kHurtData.m_n32Effect, kHurtData.m_n32HPValue, kHurtData.m_n32HP);
			return;
		}
	}

	public void DamageProcessByType (Character target, DAMAGE_TYPE type, int damageValue, int hp)
	{
        if (target.getType() == CharacterType.CT_MONSTER)
        {
            if (target.m_bIsNetDead)
            {
                // 主机说怪已死
                Debug.Log("标记死了");
                return;
            }

            target.GetProperty().setHP(hp);

            if (hp == 0)
            {
                Debug.Log("主机发来怪死了 " + target.GetProperty().GetInstanceID());
                target.m_bIsNetDead = true;
                //MonsterManager.Instance.destroyMonster(target as CharacterMonster);
                //target.ChangeAppear(target.m_kDieAppear);
                return;
            }
        }
        else
        {
            target.GetProperty().setHP(hp);

            if (hp == 0)
            {
                if (target.getType() == CharacterType.CT_PLAYER)
                {
                    // 副机的主角需要进入死亡状态
                    target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_DIE);
                }
                else if (target.getType() == CharacterType.CT_PLAYEROTHER)
                {
                    target.ChangeAppear(target.m_kDieAppear);
                }
                
                return;
            }
        }
        


		// 根据伤害类型来走不同的表现
		switch ((DAMAGE_TYPE)type) 
        {
		case DAMAGE_TYPE.DT_COMMON_DAMAGE:
        case DAMAGE_TYPE.DT_BLAST_DAMAGE:
			CommonBlastDamageAppear (target, damageValue, type == DAMAGE_TYPE.DT_COMMON_DAMAGE ? false : true);
			break;
		case DAMAGE_TYPE.DT_WITH_STAND:
			DodgeDamageAppear (target, damageValue);
			break;
		case DAMAGE_TYPE.DT_BUFF_DAMAGE:
			BuffDamageAppear (target, damageValue);
			break;
		}       
	}

	// 普通伤害表现
    public void CommonBlastDamageAppear(Character target, int hurtNum, bool bBlast)
	{
        EffectManager.Instance.BeHitHightlight(target.gameObject);

        if (target.getType() == CharacterType.CT_MONSTER)
        {
            CharacterMonster monster = target as CharacterMonster;

            if (ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(monster.monster_property.template_id).isShouJi)
            {
                if (NonHostMonsterAppearChangeRole(monster, Appear.BATTLE_STATE.BS_BE_HIT))
                {
                    target.ChangeAppear(target.m_kHitAppear);

                    EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", target.getTagPoint("help_body"));
                }
            }
        }
        else
        {
            if (target.getType() == CharacterType.CT_PLAYER)
            {
                ArrayList param = new ArrayList();
                param.Add(0.0f);
                param.Add(Vector3.zero);
                target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
            }
            else if (target.getType() == CharacterType.CT_PLAYEROTHER)
            {
                if (NonHostOtherPlayersAppearChangeRole(target, Appear.BATTLE_STATE.BS_BE_HIT))
                {
                    target.ChangeAppear(target.m_kHitAppear);

                    EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/shouji", target.getTagPoint("help_body"));
                }
            }
        }


		FloatBloodNum.Instance.PlayFloatBood(target.getType() == CharacterType.CT_MONSTER,
            hurtNum, target.getTagPoint("help_hp"), bBlast ? eHurtType.doubleHurt : eHurtType.normalHurt);
	}

	// 招架伤害表现
	public void DodgeDamageAppear (Character target, int hurtNum)
	{
        EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/zhaojia", target.transform, 10.0f);

        if (target.getType() == CharacterType.CT_PLAYER)
        {
            ArrayList param = new ArrayList();
            param.Add(0.0f);
            param.Add(Vector3.zero);
            target.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_BE_HIT, param);
        }
        else
        {
            if (NonHostMonsterAppearChangeRole(target, Appear.BATTLE_STATE.BS_BE_HIT))
            {
                target.ChangeAppear(target.m_kHitAppear);

                EffectManager.Instance.createFX("Effect/Effect_Prefab/Role/Other/zhaojia", target.transform, 10.0f);
            }
        }

		FloatBloodNum.Instance.PlayFloatBood(false,
            hurtNum, target.getTagPoint("help_hp"), eHurtType.withstandHurt);
	}

	// buff伤害表现
	public void BuffDamageAppear (Character target, int hurtNum)
	{
		// buff 特效表现 需要再走另外的消息流程 这里只走伤害
	}

	// 非主机 end------------------------------------------------------------------------------------------------------------------------------------------


	// 多人副本中有人挂了
	public void OnPlayerDie (Character character)
	{
		// 多人副本模式
		if (Global.inMultiFightMap ()) {
			switch (character.getType ()) {
			case CharacterType.CT_PLAYER:
				{
                    if (PlayerManager.Instance.GetPlayerOtherNum() > 0)
                    {
                        #region 弹出死亡界面
						DungeonManager.Instance.ShowDead ();
                        #endregion
					}
				}
				break;
			case CharacterType.CT_PLAYEROTHER:
				{
                    if (CameraFollow.sCameraFollow.GetBindTran() == character.transform)
                    {
                        // 摄像机绑定的人死了
                        if (PlayerManager.Instance.GetPlayerOtherNum() > 0)
                        {
                            #region 弹出死亡界面
                            DungeonManager.Instance.ShowDead();
                            #endregion
                        }
                    }
                    
				}
				break;
			default:
				{
					Debug.Log ("角色类型出错!");
				}
				break;
			}

			GCReportPlayerDie msg = new GCReportPlayerDie (m_un32TeamID, (uint)character.GetProperty ().GetInstanceID ());
			NetBase.GetInstance ().Send (msg.ToBytes ());
		}
	}


	// 收到服务器发来的有人复活了
	public void OnPlayerRelive (GCNotifyPlayerRelive info)
	{
		if (!Global.inMultiFightMap ()) {
			return;
		}

		if (info.m_un32PlayerID == CharacterPlayer.character_property.GetInstanceID ()) {
			// 自己复活了
			CharacterPlayer.sPlayerMe.transform.position = new Vector3 (info.m_fPosX, info.m_fPosY, info.m_fPosZ);
			CharacterPlayer.sPlayerMe.setFaceDir (new Vector3 (info.m_fDirX, info.m_fDirY, info.m_fDirZ));
            CharacterPlayer.sPlayerMe.OnRevive();
			#region 摄像机跟随
			DungeonManager.Instance.CameraFollowMe ();
			#endregion
			
			
		} else {
			//看是否是有其他人复活了
            CharacterPlayerOther other = PlayerManager.Instance.getPlayerOther((int)info.m_un32PlayerID);
			if (other) {
				other.GetProperty ().setHP (other.GetProperty ().getHPMax ());//重置HP
				other.transform.position = new Vector3 (info.m_fPosX, info.m_fPosY, info.m_fPosZ);
				other.setFaceDir (new Vector3 (info.m_fDirX, info.m_fDirY, info.m_fDirZ));
                other.OnRevive();
			}
		}
	}


	// 收到服务器发来的副本结果
	public void OnMultiResult (GSNotifyMultiResult info)
	{
		switch ((EMultiResult)info.m_n8Result) {
		case EMultiResult.MR_SUCCESS:
			{
				DungeonManager.Instance.CloseAllView();//先关闭死亡界面
                // 多人副本中的非主机需要掉宝箱
                if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer())
                {
                    if (m_kBattlePrefab)
                    {
                        Box box = m_kBattlePrefab.GetComponentInChildren<Box>();
                        if (box)
                        {
                            box.openBox();
                        }
                    }
                }
			}
			break;
		case EMultiResult.MR_FAILED_ALL_DEAD:
			{
				// 全部阵亡 弹框让回城
			#region 显示失败
				DungeonManager.Instance.ShowFail ();
			#endregion
					
			}
			break;
		case EMultiResult.MR_FAILED_LEADER_OFFLINE:
			{
				Debug.Log ("back city");
				// 主机掉线 直接回城
				DungeonManager.Instance.ShowMainPlayerDrop ();//显示主机掉线界面
			}
			break;
		default:
			{

			}
			break;



		}
	}

    // 判断在多人副本中 碰撞是否有效
    public bool CollisionJudegeValid(Character charSrc, Character charTarget)
    {
        if (Global.inMultiFightMap())
        {
            // 人对人没有伤害
            if (charSrc.getType() != CharacterType.CT_MONSTER && charTarget.getType() != CharacterType.CT_MONSTER)
            {
                return false;
            }

            // 如果是在主机上
            if (CharacterPlayer.character_property.getHostComputer())
            {
                // 只管主角对怪 怪对所有人 的碰撞
                // 其他人对怪 不管
                if (charSrc.getType() == CharacterType.CT_PLAYEROTHER && charTarget.getType() == CharacterType.CT_MONSTER)
                {
                    return false;
                }
            }

            // 如果在非主机上
            if (!CharacterPlayer.character_property.getHostComputer())
            {
                // 只管主角对怪 的碰撞
                // 其他人对怪 怪对其他人 怪对主角 不管
                if (charSrc.getType() == CharacterType.CT_PLAYEROTHER && charTarget.getType() == CharacterType.CT_MONSTER)
                {
                    return false;
                }

                if (charSrc.getType() == CharacterType.CT_MONSTER && charTarget.getType() == CharacterType.CT_PLAYEROTHER)
                {
                    return false;
                }

                if (charSrc.getType() == CharacterType.CT_MONSTER && charTarget.getType() == CharacterType.CT_PLAYER)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SpawnMultiMonsterByInfo()
    {
        if (m_kMonsterInfo != null)
        {
            GameObject obj = MonsterManager.Instance.spawnMonsterNet(
            new Vector3(m_kMonsterInfo.m_fPosX, m_kMonsterInfo.m_fPosY, m_kMonsterInfo.m_fPosZ),
            new Vector3(m_kMonsterInfo.m_fDirX, m_kMonsterInfo.m_fDirY, m_kMonsterInfo.m_fDirZ),
            (int)m_kMonsterInfo.m_un32TempID,
            (int)m_kMonsterInfo.m_un32ObjID
            );

            m_bEnterLevel = true;
            m_bSpawed = true;
            m_kMonsterInfo = null;

            return;
        }

        m_bEnterLevel = true;
        m_bSpawed = false;
    }

    // 非主机中的怪由于没有Ai在切换表现时需要有规则
    public bool NonHostMonsterAppearChangeRole(Character owner, Appear.BATTLE_STATE eNextAppear)
    {
        if (owner.getType() != CharacterType.CT_MONSTER)
        {
            return true;
        }

        if (owner.m_eAppearState == Appear.BATTLE_STATE.BS_SKILL && eNextAppear == Appear.BATTLE_STATE.BS_BE_HIT)
        {
            // 放技能不能被打断
            return false;
        }

        return true;
    }


    // 其他人的表现由于没有Ai在切换表现时需要有规则
    public bool NonHostOtherPlayersAppearChangeRole(Character owner, Appear.BATTLE_STATE eNextAppear)
    {
        if (owner.getType() != CharacterType.CT_PLAYEROTHER)
        {
            return true;
        }

        if (owner.m_eAppearState == Appear.BATTLE_STATE.BS_SKILL && eNextAppear == Appear.BATTLE_STATE.BS_BE_HIT)
        {
            // 放技能不能被打断
            return false;
        }

        return true;
    }

    public void AddNetSyncPos(NetSyncObj type, float x, float y, float z)
    {
        if ( type < NetSyncObj.NSO_ARCHER_OBJ || type > NetSyncObj.NSO_MEIDUSHA_OBJ )
        {
            return;
        }

        Vector3 netPos = new Vector3(x, y, z);

        // 先存放数据等动作开始播放后开始使用
        if (!BattleMultiPlay.GetInstance().m_kMissilePos.ContainsKey(type))
        {
            List<Vector3> newAdd = new List<Vector3>();
            newAdd.Add(netPos);
            BattleMultiPlay.GetInstance().m_kMissilePos.Add(type, newAdd);
        }
        else
        {
            BattleMultiPlay.GetInstance().m_kMissilePos[type].Add(netPos);
        }
    }
    
    public Vector3 UseNetSyncPos(NetSyncObj type)
    {
        if (type < NetSyncObj.NSO_ARCHER_OBJ || type > NetSyncObj.NSO_MEIDUSHA_OBJ)
        {
            return Vector3.zero;
        }

        
        if (BattleMultiPlay.GetInstance().m_kMissilePos.ContainsKey(type))
        {
            if (BattleMultiPlay.GetInstance().m_kMissilePos[type].Count != 0)
            {
                Vector3 pot = BattleMultiPlay.GetInstance().m_kMissilePos[type][0];
                BattleMultiPlay.GetInstance().m_kMissilePos[type].RemoveAt(0);

                return pot;
            }
        }

        return Vector3.zero;
    }

    public void OnDestroy()
    {
        if (m_kBattlePrefab)
        {
            m_kBattlePrefab = null;
        }
    }
}