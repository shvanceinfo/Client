using UnityEngine;

public class MonsterDieAppear : DieAppear 
{
    public EffectManager.FxPostProcess m_kDiePostFun;

    CharacterMonster monster;
    float m_fFlyTime;

    float m_fBrokenTime = 0.4f;

    float m_fPoisionDie = 0.0f;

    public MonsterDieAppear()
    {
        m_kDiePostFun = DieAppearProcess;
    }

	public override void active() 
    {
        is_active = true;

        owner.m_eAppearState = battle_state;
        
        monster = owner as CharacterMonster;

        if (monster.HasBuff(BUFF_TYPE.BT_POISON_HURT))
        {
            m_fPoisionDie = 0.0f;
        }
        else
            m_fPoisionDie = 1.0f;

        if (monster.monster_property.getBroken() != 1)
        {
            ActiveFly(monster);
        }
        else
        {
            if (monster.GetProperty().m_eDamageReason == DAMAGE_TYPE.DT_BLAST_DAMAGE)
            {
                CorpseExplosion(monster);
            }
            else
            {
                if (Random.Range(0, 100) < 15)
                {
                    CorpseExplosion(monster);
                }
                else
                    ActiveFly(monster);
            }
        }


        EffectManager.Instance.createFX("Effect/Effect_Prefab/Monster/Blood_di", monster.transform.position, Quaternion.identity);
	}

    public void ActiveFly(CharacterMonster monster)
    {
        // delete path finding
        //PathFindingMonster pathFinding = monster.GetComponent<PathFindingMonster>();
        //if (pathFinding)
        //{
        //    pathFinding.StopMove();
        //}

        owner.playAnimation("die2a");
        m_fFlyTime = owner.animation["die2a"].length;

        if (!CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            Debug.Log("我发来怪死了 " + owner.GetProperty().GetInstanceID());
            // 是怪就要给非主机也要做加钱 加经验处理
            BattleManager.Instance.onPlayerkillMonster(owner as CharacterMonster);
        }
    }

    public void CorpseExplosion(CharacterMonster monster)
    {
        EffectManager.Instance.createFX(monster.monster_property.getBrokenPrefab(),
                monster.getPosition(),
                Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up),
                m_kDiePostFun,
                monster,
                5.0f);

        BattleManager.Instance.PlayAudioOneShot();

        if (!CharacterPlayer.character_property.getHostComputer() && Global.inMultiFightMap())
        {
            // 是怪就要给非主机也要做加钱 加经验处理
            BattleManager.Instance.onPlayerkillMonster(owner as CharacterMonster);
        }
    }

    public override void update(float delta)
    {
        // 怪物死亡按怪物自身的朝向的反方向飞出
        if (time_since_begin < m_fFlyTime)
        {
            float fPreviousY = owner.transform.position.y;

            owner.movePosition(-monster.getFaceDir() * delta * m_fPoisionDie);

            if (Global.InWorldBossMap())
            {
                owner.transform.position = new Vector3(owner.transform.position.x, fPreviousY, owner.transform.position.z);
            }
        }
        else
        {
            is_active = false;

            if (!Global.InWorldBossMap())
            {
                MonsterManager.Instance.destroyMonster(owner as CharacterMonster, 0.8f);
            }
        }

        time_since_begin += delta;
    }

    void DieAppearProcess(Character theMonster)
    {
        // delete path finding
        //PathFindingMonster pathFinding = theMonster.GetComponent<PathFindingMonster>();
        //if (pathFinding)
        //{
        //    pathFinding.StopMove();
        //}

        MonsterManager.Instance.destroyMonster(theMonster as CharacterMonster, m_fBrokenTime);
    }
}
