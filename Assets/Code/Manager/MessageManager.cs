using UnityEngine;
using System.Collections;
using NetGame;
using System;



public class MessageManager
{
	ArrayList message_list;
	GameMessage message;
	
	public string role_name;
	public CharacterProperty my_property;
	
	//还未通知服务器的累积经验(monster)
	public int exp_acc = 0;
	public bool send_exp_dirty = true;
	//还未通知服务器的累积(monster)
	public int gold_acc = 0;
	
    private static MessageManager _instance = null;

	public static MessageManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new MessageManager();
            return _instance;
        }
    }

    private MessageManager()
    {
        message_list = new ArrayList();
        my_property = new CharacterProperty();
    }
	
	public void pushMeassage(byte[] data) {
		lock (message_list.SyncRoot)
        {
           message_list.Add(data);
        }
	}
	
	public void getMessage(out ArrayList messages) {
		lock (message_list.SyncRoot)
        {
            messages = new ArrayList();
			for (int i = 0; i < message_list.Count; i++) {
				messages.Add(message_list[i]);
			}
			message_list.Clear();
        }
	}
	
	public void processMessage(byte[] data) {
		
		NetHead head = new NetHead();
		head.ToObject(data);
		
		GameMessage gmsg = new GameMessage();
		gmsg.OnMessage(head._assistantCmd, data);
	}
	
	public void doNothing() 
    {
		
	}
	
	public void sendMessageSelectRole() {
		
		NetBase net = NetBase.GetInstance();
        GCAskSelectRole selectRoleMsg = new GCAskSelectRole();
        if (net.IsConnected)
        {
			selectRoleMsg.SetRoleName(my_property.getNickName());
            net.Send(selectRoleMsg.ToBytes());
        }
	}
	
	public void sendMessageAskMove(int id, Vector3 faceDir, Vector3 target) {
		
		//if (Global.inFightMap()) return ;
			
		NetBase net = NetBase.GetInstance();
        GCAskMove askMoveMsg = new GCAskMove();
        
		askMoveMsg.m_un32ObjID = (UInt32)id;
        askMoveMsg.m_fCurPosX = faceDir.x;
        askMoveMsg.m_fCurPosY = faceDir.y;
        askMoveMsg.m_fCurPosZ = faceDir.z;
		askMoveMsg.node_num = 1;
		askMoveMsg.m_fTarPosX = target.x;
		askMoveMsg.m_fTarPosY = target.y;
		askMoveMsg.m_fTarPosZ = target.z;
        net.Send(askMoveMsg.ToBytes(), false);
	}
	
	public void sendMessageAskMoveArrive(int id) {
		NetBase net = NetBase.GetInstance();
        GCAskMove askMoveMsg = new GCAskMove();
		
		askMoveMsg.m_un32ObjID = (UInt32)id;
		askMoveMsg.m_fCurPosX = CharacterPlayer.sPlayerMe.getPosition().x;
		askMoveMsg.m_fCurPosY = CharacterPlayer.sPlayerMe.getPosition().y;
		askMoveMsg.m_fCurPosZ = CharacterPlayer.sPlayerMe.getPosition().z;
		askMoveMsg.node_num = 0;
        net.Send(askMoveMsg.ToBytes(), false);
	}
	
	//开宝箱
	public void sendMessageOpenBox(int id, byte isLastBox = 0) {
		
		NetBase net = NetBase.GetInstance();
	    GCAskTreasureBoxDropGoods openBoxMsg = new GCAskTreasureBoxDropGoods();
		openBoxMsg.m_un32BoxID = (UInt32)id;
        openBoxMsg.m_isShowAward = isLastBox;
        net.Send(openBoxMsg.ToBytes());
	}
	
	//离开副本
	public void sendMessageCompleteBattleLevel(bool pickAllTempGoods) {
        ///通过时间
	    uint consTime = 0;//(uint)UIManager.Instance.getUIFromMemory(UiNameConst.ui_fight).GetComponent<UiFightMainMgr>().consTime;
		NetBase net = NetBase.GetInstance();
	    GCReportPassGate passGateMsg = new GCReportPassGate();
		
		passGateMsg.m_un32MapID = (UInt32)CharacterPlayer.character_property.getServerMapID();
		passGateMsg.m_un32SceneID = (UInt32)CharacterPlayer.character_property.getServerSceneID();
		passGateMsg.m_un32HPVessel = CharacterPlayer.character_property.getCurHPVessel();
		passGateMsg.m_un32MPVessel = CharacterPlayer.character_property.getCurMPVessel();
		passGateMsg.m_un32GotExpNum = (UInt32)exp_acc;
        passGateMsg.m_un32UseSecond = consTime;
		exp_acc = 0;
		passGateMsg.m_un32GotSilverNum = (UInt32)gold_acc;
		gold_acc = 0;
        passGateMsg.m_bPickAllTempGoods = (byte)((pickAllTempGoods) ? 1 : 0);
        net.Send(passGateMsg.ToBytes());
	}
	
	//请求进入场景
	public void sendMessageChangeScene(int mapId, bool bEnterByUI) 
    {
		
		NetBase net = NetBase.GetInstance();
	    GCAskEnterScene enterSceneMsg = new GCAskEnterScene();
		enterSceneMsg.m_n32MapID = (Int32)mapId;
		enterSceneMsg.m_n32SceneID = 0;
        enterSceneMsg.m_bEnterByUI = bEnterByUI ? (byte)1 : (byte)0;
		enterSceneMsg.m_un32HPVessel = CharacterPlayer.character_property.getCurHPVessel();
		enterSceneMsg.m_un32MPVessel = CharacterPlayer.character_property.getCurMPVessel();
        enterSceneMsg.m_un32GotExp = (UInt32)exp_acc;
		exp_acc = 0;
        enterSceneMsg.m_un32GotSilver = (UInt32)gold_acc;
		gold_acc = 0;
        net.Send(enterSceneMsg.ToBytes());

	}
	
	//请求进入主城
	public void sendMessageReturnCity() 
    {	
		NetBase net = NetBase.GetInstance();
	    GCAskEnterScene enterSceneMsg = new GCAskEnterScene();
		enterSceneMsg.m_n32MapID = (Int32)100;
		enterSceneMsg.m_n32SceneID = 100;
        enterSceneMsg.m_bEnterByUI = (byte)0;
		enterSceneMsg.m_un32HPVessel = CharacterPlayer.character_property.getCurHPVessel();
		enterSceneMsg.m_un32MPVessel = CharacterPlayer.character_property.getCurMPVessel();
        enterSceneMsg.m_un32GotExp = (UInt32)exp_acc;
		exp_acc = 0;
        enterSceneMsg.m_un32GotSilver = (UInt32)gold_acc;
		gold_acc = 0;
        net.Send(enterSceneMsg.ToBytes());
	}
    //请求复活
    public void sendMessageBorn(ReliveType reliveType = ReliveType.Time, eGoldType assetType = eGoldType.none, UInt32 num = 0)
    {
        GCAskRelive reLive = new GCAskRelive(reliveType, assetType, num);
        MainLogic.SendMesg(reLive.ToBytes());
    }
	
	public void sendExpChange() {
			
		NetBase net = NetBase.GetInstance();
        GCReportExp reportExpMsg = new GCReportExp();
		
		reportExpMsg.m_un32GotExp = (UInt32)exp_acc;
		exp_acc = 0;
		
        net.Send(reportExpMsg.ToBytes());
		
	}
	
	public void sendUseSkill(UInt32 obj_id, UInt32 skill_id, Vector3 pos, Vector3 face) {
		
		NetBase net = NetBase.GetInstance();
        GCReportUseSkill reportUseSkill = new GCReportUseSkill();
		
		reportUseSkill.m_un32ObjID = obj_id;
		reportUseSkill.m_un32SkillID = skill_id;
		reportUseSkill.m_fPosX = pos.x;
		reportUseSkill.m_fPosY = pos.y;
		reportUseSkill.m_fPosZ = pos.z;
		reportUseSkill.m_fDirX = face.x;
		reportUseSkill.m_fDirY = face.y;
		reportUseSkill.m_fDirZ = face.z;
		
        net.Send(reportUseSkill.ToBytes(), false);
		
	}
	
	public void sendObjectAppear(UInt32 objId, UInt32 tmpId, Vector3 pos, Vector3 face) {
			
		NetBase net = NetBase.GetInstance();
        GCReportOBjectAppear reportOBjectAppear = new GCReportOBjectAppear();
		
		reportOBjectAppear.m_un32ObjID = objId;
		reportOBjectAppear.m_un32TempID = tmpId;
		reportOBjectAppear.m_fPosX = pos.x;
		reportOBjectAppear.m_fPosY = pos.y;
		reportOBjectAppear.m_fPosZ = pos.z;
		reportOBjectAppear.m_fDirX = face.x;
		reportOBjectAppear.m_fDirY = face.y;
		reportOBjectAppear.m_fDirZ = face.z;
		
        net.Send(reportOBjectAppear.ToBytes(), false);
	}
	
	public void sendObjectDisappear(UInt32 objId) {
			
		NetBase net = NetBase.GetInstance();
        GCReportOBjectDisappear reportOBjectDisappear = new GCReportOBjectDisappear();
		
		reportOBjectDisappear.m_un32ObjID = objId;
		reportOBjectDisappear.m_n32Reason = 1;
		
        net.Send(reportOBjectDisappear.ToBytes(), false);
	}
	
	public void sendObjectBehavior(UInt32 objId, int type, Vector3 dir, Vector3 pos) {
			
		NetBase net = NetBase.GetInstance();
        GCReportObjectAction reportOBjectBehavior = new GCReportObjectAction();
		
		reportOBjectBehavior.m_un32ObjID = objId;
		reportOBjectBehavior.m_n32ActionCate = type;
		reportOBjectBehavior.m_fDirX = dir.x;
		reportOBjectBehavior.m_fDirY = dir.y;
		reportOBjectBehavior.m_fDirZ = dir.z;
		reportOBjectBehavior.m_fPosX = pos.x;
		reportOBjectBehavior.m_fPosY = pos.y;
		reportOBjectBehavior.m_fPosZ = pos.z;
		
        net.Send(reportOBjectBehavior.ToBytes(), false);
	}
	
    
	public void sendObjectHurt(bool bInHost, UInt32 objId, int hurt, int hp, int hurtType) 
    {
		NetBase net = NetBase.GetInstance();
        GCReportObjectHurm reportOBjectHurt = new GCReportObjectHurm();

        reportOBjectHurt.m_bHost = (byte)(bInHost ? 1 : 0);
		reportOBjectHurt.m_un32ObjID = objId;
        reportOBjectHurt.m_n32CurHP = hp;
		reportOBjectHurt.m_n32HPValue = hurt;
		reportOBjectHurt.m_n32Effect = hurtType;
		
        net.Send(reportOBjectHurt.ToBytes(), false);
	}
	
	public void sendAskEnterTower(UInt32 ConsumeCardNum) {
			
		NetBase net = NetBase.GetInstance();
        GCAskEnterTowerInstance askEnterTowerInstance = new GCAskEnterTowerInstance();
		
		//askEnterTowerInstance.m_un32ConsumeCardNum = ConsumeCardNum;
		
        net.Send(askEnterTowerInstance.ToBytes());
	}
	
	public void sendReportTowerScore(UInt32 towerID) {
			
		NetBase net = NetBase.GetInstance();
        GCReportTowerInstanceScore reportTowerInstanceScore = new GCReportTowerInstanceScore();	
		reportTowerInstanceScore.m_un32TowerId = towerID;		
        net.Send(reportTowerInstanceScore.ToBytes());
	}
	
	public void sendAskTowerInstanceRank() {
			
		NetBase net = NetBase.GetInstance();
        GCAskTowerInstanceRank askTowerInstanceRank = new GCAskTowerInstanceRank();	
        net.Send(askTowerInstanceRank.ToBytes());
	}
	
	public void sendAskTowerInstanceAward() {
			
		NetBase net = NetBase.GetInstance();
        GCAskTowerInstanceAward askTowerInstanceAward = new GCAskTowerInstanceAward();
        net.Send(askTowerInstanceAward.ToBytes());
	}
	
	public void receiveMessageObjectMove(GSObjectMove moveMsg) 
    {
        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)moveMsg.m_un32ObjID);
		if (cpo) 
        {
            if (moveMsg.m_fCurPosX == 99999.0f && moveMsg.m_fCurPosY == 0.0f && moveMsg.m_fCurPosZ == 0.0f)
            {
                // 旋风斩专用
                cpo.m_kXuanFengZhanPos.Add(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));
                return;
            }


            if (moveMsg.m_fCurPosX == 0.0f && moveMsg.m_fCurPosY == 0.0f && moveMsg.m_fCurPosZ == 99999.0f)
            {
                // 普通攻击专用
                cpo.m_kCommonAttackPos.Add(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));
                return;
            }
            
            
			//技能移动
			if (cpo.m_kSkillAnimShowAppear.isActive()) 
            {
				cpo.m_kSkillAnimShowAppear.skillMoveTo(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));
			}
			else 
            {
                // 同步过来的朝向 预先把朝向设置
                Vector3 dir = new Vector3(moveMsg.m_fCurPosX, 0.0f, moveMsg.m_fCurPosZ);
                dir.Normalize();
                cpo.setFaceDir(dir);

                // 移动之后的朝向和位置 用来纠正
                cpo.m_kMoveAppear.m_kMoveDir = dir;
                cpo.SetMovePath(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));
			}
			return;
		}


        CharacterMonsterNet cm = MonsterManager.Instance.GetMonster((int)moveMsg.m_un32ObjID) as CharacterMonsterNet;
		if (cm && !CharacterPlayer.character_property.getHostComputer()) 
        {
            //技能移动
            if (cm.m_kSkillAnimShowAppear.isActive())
            {
                cm.m_kSkillAnimShowAppear.skillMoveTo(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));
            }
            else
            {
                // 1
                //cm.setFaceDir(new Vector3(moveMsg.m_fCurPosX, moveMsg.m_fCurPosY, moveMsg.m_fCurPosZ));
                //cm.transform.position = new Vector3(moveMsg.m_fTarPosX, 0f, moveMsg.m_fTarPosZ);
                //cm.ChangeAppear(cm.m_kMoveAppear);


                // 2
                //cm.m_kMoveAppear.m_kMoveDir = new Vector3(moveMsg.m_fCurPosX, moveMsg.m_fCurPosY, moveMsg.m_fCurPosZ);
                //cm.SetMovePath(new Vector3(moveMsg.m_fTarPosX, 0f, moveMsg.m_fTarPosZ));


                // 3
                // 同步过来的朝向 预先把朝向设置
                Vector3 dir = new Vector3(moveMsg.m_fCurPosX, 0.0f, moveMsg.m_fCurPosZ);
                dir.Normalize();
                cm.setFaceDir(dir);

                // 移动之后的朝向和位置 用来纠正
                cm.m_kMoveAppear.m_kMoveDir = dir;
                cm.SetMovePath(new Vector3(moveMsg.m_fTarPosX, 0.0f, moveMsg.m_fTarPosZ));




                // 4
                //技能移动
                //if (cm.m_kSkillAnimShowAppear.isActive())
                //{
                //    cm.m_kSkillAnimShowAppear.skillMoveTo(new Vector3(moveMsg.m_fTarPosX, moveMsg.m_fTarPosY, moveMsg.m_fTarPosZ));
                //}
                //else
                //{
                //    cm.pushMoveTarget(new Vector3(moveMsg.m_fTarPosX, 0f, moveMsg.m_fTarPosZ));
                //}
                //return;
            }
            return;
		}


        // 走到这里表示是多人中的 飞翔类物件
        BattleMultiPlay.GetInstance().AddNetSyncPos((NetSyncObj)moveMsg.m_un32ObjID, moveMsg.m_fTarPosX, moveMsg.m_fTarPosY, moveMsg.m_fTarPosZ);
		
		Loger.Log("Object not exist!");
	}
	
	public void receiveMessageRoleInfoChange(GSNotifyRoleProfileChange roleInfoChangeMsg) {

        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)roleInfoChangeMsg.m_un32ObjID);
		if (cpo) {
			cpo.character_other_property.setWeapon((int)roleInfoChangeMsg.m_un32WeaponTypeID);
			cpo.character_other_property.setArmor((int)roleInfoChangeMsg.m_un32CoatTypeID);
			cpo.character_other_property.setLevel((int)roleInfoChangeMsg.m_un32Level);
			cpo.applyProperty();
		}
		else {
			Loger.Log("Player not exist!");
		}
	}
	
	public void receiveMessageOtherUseSkill(GSNotifySkillReleased skillReleasedMsg) {

        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)skillReleasedMsg.m_un32ObjectID);
		if (cpo) 
        {
			PlayerOtherSkill otherSkill = new PlayerOtherSkill();
			otherSkill.skill_id = (int)skillReleasedMsg.m_un32SkillID;
			otherSkill.pos = new Vector3(skillReleasedMsg.m_fPosX,skillReleasedMsg.m_fPosY,skillReleasedMsg.m_fPosZ);
			if (otherSkill.skill_id != cpo.m_kSkillAnimShowAppear.getCurrentSkillId()) 
            {
				cpo.pushSkill(otherSkill);
            }

			cpo.setFaceDir(new Vector3(skillReleasedMsg.m_fDirX,skillReleasedMsg.m_fDirY,skillReleasedMsg.m_fDirZ));

			return;
		}


        CharacterMonsterNet cm = MonsterManager.Instance.GetMonster((int)skillReleasedMsg.m_un32ObjectID) as CharacterMonsterNet;
		if ((cm && !CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap()) ||
        (cm && Global.InWorldBossMap()))
        {
            PlayerOtherSkill otherSkill = new PlayerOtherSkill();
            otherSkill.skill_id = (int)skillReleasedMsg.m_un32SkillID;
            otherSkill.pos = new Vector3(skillReleasedMsg.m_fPosX, skillReleasedMsg.m_fPosY, skillReleasedMsg.m_fPosZ);
            if (otherSkill.skill_id != cm.m_kSkillAnimShowAppear.getCurrentSkillId())
            {
                cm.pushSkill(otherSkill);
            }

            cm.setFaceDir(new Vector3(skillReleasedMsg.m_fDirX, skillReleasedMsg.m_fDirY, skillReleasedMsg.m_fDirZ));

            return;
		}
	}
	
	public void receiveMessageObjectAppear(GSNotifyObjectAppear objectAppearMsg) 
    {
		if (Global.inMultiFightMap() && !CharacterPlayer.character_property.getHostComputer()) 
        {
            if (!BattleMultiPlay.GetInstance().m_bEnterLevel)
            {
                BattleMultiPlay.GetInstance().m_kMonsterInfo = objectAppearMsg;
            }
            else
            {
                if (!BattleMultiPlay.GetInstance().m_bSpawed)
                {
                    GameObject obj = MonsterManager.Instance.spawnMonsterNet(
                    new Vector3(objectAppearMsg.m_fPosX, objectAppearMsg.m_fPosY, objectAppearMsg.m_fPosZ),
                    new Vector3(objectAppearMsg.m_fDirX, objectAppearMsg.m_fDirY, objectAppearMsg.m_fDirZ),
                    (int)objectAppearMsg.m_un32TempID,
                    (int)objectAppearMsg.m_un32ObjID
                    );
                }
                else
                {
                    // 刷后续怪
                    GameObject obj = MonsterManager.Instance.spawnMonsterNet(
                    new Vector3(objectAppearMsg.m_fPosX, objectAppearMsg.m_fPosY, objectAppearMsg.m_fPosZ),
                    new Vector3(objectAppearMsg.m_fDirX, objectAppearMsg.m_fDirY, objectAppearMsg.m_fDirZ),
                    (int)objectAppearMsg.m_un32TempID,
                    (int)objectAppearMsg.m_un32ObjID
                    );
                }
            }
		}
        else if (Global.InWorldBossMap())
        {
            if (!BattleWorldBoss.GetInstance().m_bEnterLevel)
            {
                BattleWorldBoss.GetInstance().m_kMonsterInfo = objectAppearMsg;
            }
            else
            {
                if (!BattleWorldBoss.GetInstance().m_bSpawed)
                {
                    GameObject obj = MonsterManager.Instance.spawnMonsterNet(
                    new Vector3(objectAppearMsg.m_fPosX, objectAppearMsg.m_fPosY, objectAppearMsg.m_fPosZ),
                    new Vector3(0.0f, 0.0f, 0.0f),
                    (int)objectAppearMsg.m_un32TempID,
                    (int)objectAppearMsg.m_un32ObjID
                    );

                    Vector3 oldFace = obj.GetComponent<CharacterMonsterNet>().getFaceDir();

                    Quaternion rot = Quaternion.Euler(objectAppearMsg.m_fDirX, objectAppearMsg.m_fDirY, objectAppearMsg.m_fDirZ);

                    oldFace = rot * oldFace;

                    obj.GetComponent<CharacterMonsterNet>().setFaceDir(oldFace);
                }
                else
                {
                    // 刷后续怪
                    GameObject obj = MonsterManager.Instance.spawnMonsterNet(
                    new Vector3(objectAppearMsg.m_fPosX, objectAppearMsg.m_fPosY, objectAppearMsg.m_fPosZ),
                    new Vector3(0.0f, 0.0f, 0.0f),
                    (int)objectAppearMsg.m_un32TempID,
                    (int)objectAppearMsg.m_un32ObjID
                    );

                    Vector3 oldFace = obj.GetComponent<CharacterMonsterNet>().getFaceDir();

                    Quaternion rot = Quaternion.Euler(objectAppearMsg.m_fDirX, objectAppearMsg.m_fDirY, objectAppearMsg.m_fDirZ);

                    oldFace = rot * oldFace;

                    obj.GetComponent<CharacterMonsterNet>().setFaceDir(oldFace);
                }
            }
        }
	}
	
	public void receiveMessageObjectDisappear(GSNotifyObjectDisappear objectDisappear) {

        CharacterPlayerOther cpo = PlayerManager.Instance.getPlayerOther((int)objectDisappear.m_un32ObjID);
		if (cpo) {
            PlayerManager.Instance.destroyPlayerOther((int)objectDisappear.m_un32ObjID);
			return;
		}

        CharacterMonster cm = MonsterManager.Instance.GetMonster((int)objectDisappear.m_un32ObjID);
		if (cm) {
            MonsterManager.Instance.destroyMonster((int)objectDisappear.m_un32ObjID);
			return;
		}
	}
	
	public void receiveMessageObjectBehavior(GSNotifyObjectAction objectBehavior) 
    {
        Character obj = PlayerManager.Instance.getPlayerOther((int)objectBehavior.m_un32ObjID);

        if (!obj)
        {
            obj = MonsterManager.Instance.GetMonster((int)objectBehavior.m_un32ObjID);
        }

        if (obj)
        {
            obj.setPositionLikeGod(new Vector3(objectBehavior.m_fPosX, objectBehavior.m_fPosY, objectBehavior.m_fPosZ));
            obj.setFaceDir(new Vector3(objectBehavior.m_fDirX, objectBehavior.m_fDirY, objectBehavior.m_fDirZ));


            if (Global.InWorldBossMap())
            {
                switch (objectBehavior.m_n32ActionCate)
                {
                case 0:
                    obj.ChangeAppear(Appear.BATTLE_STATE.BS_IDLE);
                	break;
                case 1:
                    obj.ChangeAppear(Appear.BATTLE_STATE.BS_PING_KAN);
                    break;
                case 2:
                    obj.ChangeAppear(Appear.BATTLE_STATE.BS_DIE);
                    break;
                }
            }
            else
                obj.ChangeAppear((Appear.BATTLE_STATE)objectBehavior.m_n32ActionCate);

            if (objectBehavior.m_n32ActionCate == (int)Appear.BATTLE_STATE.BS_DIE 
                && obj.getType() == CharacterType.CT_MONSTER)
            {
                // 标记怪已死
                obj.GetProperty().setHP(0);
                obj.m_bIsNetDead = true;
            }
            
        }
        
	}

    public void receiveMessageObjectHurtInMulti(GSNotifyObjectHurm objectHurt)
    {
        BattleMultiPlay.GetInstance().OnObjectHurt(objectHurt);
    }

    public void receiveMessageObjectHurtInWorldBoss(GSNotifyObjectHurm objectHurt)
    {
        BattleWorldBoss.GetInstance().RealDamageProcess(objectHurt);
    }

    public void SendAskEnterGoldenGoblin()
    {
        NetBase net = NetBase.GetInstance();
        GCAskEnterGoldenGoblinInstance askEnterGoblin = new GCAskEnterGoldenGoblinInstance();
        net.Send(askEnterGoblin.ToBytes());
    }

    public void SendAskGoldenGoblinTimes()
    {
        NetBase net = NetBase.GetInstance();
        GCAskGoldenGoblinTimes askTowerInstanceRank = new GCAskGoldenGoblinTimes();
        net.Send(askTowerInstanceRank.ToBytes());
    }

    public void SendAskBuyGoldenGoblinTimes()
    {
        GCAskBuyGoldenGoblinTimes askBuyGoldenGoblinTimes = new GCAskBuyGoldenGoblinTimes();
        NetBase.GetInstance().Send(askBuyGoldenGoblinTimes.ToBytes());
    }
        
    public void SendAskGoblinMultiBenifit(int nTimes)
    {
        GCAskGoldenGoblinMultiBenefit askMultiBenefit = new GCAskGoldenGoblinMultiBenefit();
        askMultiBenefit.m_un32BaseRevenue = (UInt32)BattleGoblin.GetInstance().CalGainedMoney();
        askMultiBenefit.m_un32Times = (UInt32)nTimes;
        NetBase.GetInstance().Send(askMultiBenefit.ToBytes());
    }
}