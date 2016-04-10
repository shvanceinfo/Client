using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterPet : MonoBehaviour {

    public struct MasterPath 
    {
        public Vector3 pos;    // 当前主人的坐标
        public float deltaTime;    // 上次坐标到这次坐标的间隔时间

        public MasterPath(Vector3 position, float time)
        {
            pos = position;
            deltaTime = time;
        }
    }

    // 外部配置暴露 begin

    // pose动画随机上限
    public float m_fPoseRandomUp = 1.0f;
    // pose动画随机下限
    public float m_fPoseRandomDown = 6.0f;
    // 跟随半径
    public float m_fFollowRadius = 0.5f;
    // 反应时间
    public float m_fReactionTime = 0.3f;

    // 外部配置暴露 end


	public Dictionary<eFighintPropertyCate, int> m_kPetAddedProperty = null;
	
	private const string m_strIdleName = "idle";
	private const string m_strPoseName = "pose";
    private const string m_strSkillName = "skill_pet";
	
	// current animation
	public string m_strCurAnimation;

    public Animation m_kPetAnimation = null;

    
    // 下个pose即将来的时间
    private float m_fPoseComingTime = 0.0f;

    // 每个采样中的计时
    private float m_fSampleTime = 0.0f;

    // 每个反应中的计时
    private float m_fPetReaction = 0.0f;

    // 主人的轨迹容器
    public List<MasterPath> m_kMasterPosContainer = null;

    public List<Vector3> m_kMasterPos = null;

    // 主人
    public Character m_kMaster = null;

    private bool m_bMove = false;
 
	
	void Awake() {
		
		m_kPetAddedProperty = new Dictionary<eFighintPropertyCate, int>();

        m_kPetAnimation = GetComponentInChildren<Animation>();

        m_kMasterPosContainer = new List<MasterPath>();

        m_kMasterPos = new List<Vector3>();
	}
	
	// Use this for initialization
	protected void Start () {

        PlayAnimation(m_strIdleName);

        m_fPoseComingTime = PoseRandomTime();

        m_fPetReaction = m_fReactionTime;
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        if (SkillIsPlaying())
            return;

        // animation part
        if (m_kPetAnimation.animation.IsPlaying(m_strIdleName))
        {
            if (!m_bMove)
            {
                // idle
                if (m_fPoseComingTime < 0.0f)
                {
                    PlayAnimation(m_strPoseName);
                }

                m_fPoseComingTime -= Time.deltaTime;
            }
        }
        else if (m_kPetAnimation.animation.IsPlaying(m_strPoseName))
        {
            
        }
        else
        {
            PlayAnimation(m_strIdleName);
            m_fPoseComingTime = PoseRandomTime();
        }
        
        // follow part
        if (m_kMasterPosContainer.Count != 0)
        {
            // 主人有移动 开始反应时间倒计时
            //if (m_fPetReaction < 0.0f)
            //{
            //    // 移动
            //    if (m_fSampleTime < m_kMasterPosContainer[0].deltaTime)
            //    {
            //        Vector3 kMovePos = SamplePos(m_fSampleTime / m_kMasterPosContainer[0].deltaTime, m_kMasterPosContainer[0].pos);
            //        transform.position = kMovePos;

            //        m_fSampleTime += Time.deltaTime;
            //    }
            //    else
            //    {
            //        // 移动到了
            //        m_kMasterPosContainer.RemoveAt(0);
            //        m_fSampleTime = 0.0f;
            //    }

            //    // 主人没有移动了
            //    if (m_kMasterPosContainer.Count == 0)
            //    {
            //        m_fPetReaction = m_fReactionTime;
            //    }
            //}
            //else
            //{
            //    // 反应中
            //    m_fPetReaction -= Time.deltaTime;
            //}
        }
	
		if (m_kMasterPos.Count <= ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(1021001).type2Data)
        {
            m_bMove = false;
            return;
        }


        if (m_kMasterPos.Count != 0)
        {
            m_bMove = true;

            // 直接的方法
            if (m_fPetReaction < 0.0f)
            {
                // 移动
                transform.position = m_kMasterPos[0];

                if (m_kMasterPos.Count > 1)
                {
                    // 设置朝向
                    transform.LookAt(m_kMasterPos[1]);
                }
                else
                {
                    transform.LookAt(CharacterPlayer.sPlayerMe.transform.position);
                }

                m_kMasterPos.RemoveAt(0);

                // 主人没有移动了
                if (m_kMasterPos.Count <= ConfigDataManager.GetInstance().GetPublicDataConfig().getPublicData(1021001).type2Data)
                {
                    m_kMasterPos.Clear();
                    m_fPetReaction = m_fReactionTime;
                }
            }
            else
            {
                // 反应中
                m_fPetReaction -= Time.deltaTime;

                Vector3 kMovePos = SamplePos(m_fPetReaction / m_fReactionTime, m_kMasterPos[0]);
                transform.position = kMovePos;
            }
        }
        else
        {
            m_bMove = false;
        }
        
	}

    public void PlaySkill()
    {
        if (!SkillIsPlaying())
        PlayAnimation(m_strSkillName);
    }
    private bool SkillIsPlaying()
    {
        return m_kPetAnimation.animation.IsPlaying(m_strSkillName);
    }

    public void SetMaster(Character master)
    {
        m_kMaster = master;
    }

	private bool PlayAnimation(string name)
	{
        if (m_kPetAnimation == null)
        {
            return false;
        }

        m_kPetAnimation.animation.Play(name);

        return true;
	}
	
	private float PoseRandomTime()
    {
        return UnityEngine.Random.Range(m_fPoseRandomUp, m_fPoseRandomDown);
    }
    

    private Vector3 SamplePos(float factor, Vector3 targetPos)
    {
        float val = Mathf.Clamp01(factor);

        Vector3 kRetPos = new Vector3();

        kRetPos = transform.position * val + targetPos * (1.0f - val);

        return kRetPos;
    }
}
