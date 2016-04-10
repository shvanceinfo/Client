using UnityEngine;
using System.Collections;

public enum ECameraShakeType
{
	CST_UP_DOWN,
	CST_LEFT_RIGHT,
	CST_FRONT_BACK,
	CST_SPHERE,
    CST_FIGHT_SHAKE,
    CST_LAST_HIT_SHAKE,
}

public class CameraFollow : MonoBehaviour 
{
	static public CameraFollow sCameraFollow;
	
	Transform m_kBindTran = null;


    // pinch value
    public float m_fZoomPinchValue = 1.0f;
    
   
    // 摄像机插值的坐标
    private float m_fAvatOffsetX = 0.0f;
    private float m_fAvatOffsetY = 0.0f;
    private float m_fAvatOffsetZ = 0.0f;


    // 对Y轴调节的曲线
    public AnimationCurve m_kAnimationCurve;
    public AnimationCurve m_kCamCurveForBossBegin;
    public AnimationCurve m_kCamCurveForBossEnd;
    
    // 相机移动的近点和远点
    private Vector3 m_kAvataOffNear = Vector3.zero;
    private Vector3 m_kAvataOffFar = Vector3.zero;

    // 最后定格的坐标
    private Vector3 m_kChangedDist = Vector3.zero;

    public Vector3 m_kFightOffset = Vector3.zero;

    // 开放的参数 begin

    // 插值的因子
    public float m_fDistFactor = 0.35f;

    // 摄像机LookAt 主角脚下 + m_fHighOffset
    public float m_fHighOffset = 0.5f;

    // 缩放的速度
    public float m_fZoomSpeed = 0.1f;

    private Vector3 m_kLookAtOffset = Vector3.zero;

    // 开放的参数 end



    // boss 出场桥段 begin
    public float m_fCamZoomToBossTime = 0.1f;
    public float m_fCamZoomOutBossTime = 0.3f;

    public float m_fCamZoomToRealTime = 0.0f;
    public float m_fCamZoomOutRealTime = 0.0f;

    public bool m_bBossBornBegin = false;
    public bool m_bBossBornEnd = false;

    // 开始时摄像机的坐标s
    public Vector3 m_kCamBeginPos = Vector3.zero;
    public Vector3 m_kCamEndPos = Vector3.zero;

	public Camera m_kCam = null;



	// shake begin
	// camera shake offset
	// internal parameters
	private bool m_bBeginShake = false;
	private Vector3 m_kShakeOffset = Vector3.zero;
	// camera shake time
	private float m_fShakeTime = 0.0f;
	// out parameter for shake tiem
	private float m_fParamShakeTime = 0.0f;
	// shake type
	private ECameraShakeType m_eShakeType = ECameraShakeType.CST_UP_DOWN;

	// public parameters
	public float m_fShakeMagnititudeUpDown = 0.1f;
	public float m_fShakeMagnititudeLeftRight = 0.1f;
	public float m_fShakeMagnititudeFrontBack = 0.1f;
	public float m_fShakeMagnititudeRandomRidus = 0.1f;




    // screen effect variables
    private int m_nScreenTexIndex = 0;
    private float m_fUpdateScreenEffectTime = 0.0f;


    // 最后一击摄像机旋转朝向
    private Vector3 m_kLastHitLookPoint = Vector3.zero;
    private Vector3 m_kLastHitInitPoint = Vector3.zero;



	// shake end
    // boss 出场桥段 end
	void Awake () 
    {
        sCameraFollow = this;

		m_kCam = GetComponent<Camera>();
		//SceneManager.Instance.setDontDestroyObj(transform.gameObject);
	}

    void PreCalculateNearPoint(float factor)
    {
        float val = Mathf.Clamp01(factor);

        m_kAvataOffNear = Vector3.zero * (1.0f - val) + m_kAvataOffFar * val;
    }
	
	// Use this for initialization
	void Start () 
    {
        // 默认是绑在主角身上
        m_kBindTran = CharacterPlayer.sPlayerMe.transform;

        m_kFightOffset = new Vector3(Global.m_fCamDistFightH, Global.m_fCamDistFightV, Global.m_fCamDistFightH);

        m_kAvataOffFar = new Vector3(Global.m_fCamDistCityH, Global.m_fCamDistCityV, Global.m_fCamDistCityH);
        PreCalculateNearPoint(m_fDistFactor);


        m_kLookAtOffset = new Vector3(0.0f, m_fHighOffset, 0.0f);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (m_kBindTran)
        {
            if (Global.inCityMap())
            {
				if (EasyTouchJoyStickProperty.sJoystickProperty != null && EasyTouchJoyStickProperty.sJoystickProperty.gameObject.activeSelf)
                {
                    m_fZoomPinchValue = Mathf.Clamp01(m_fZoomPinchValue);
                    Sample(m_fZoomPinchValue);
                    m_kChangedDist = new Vector3(m_fAvatOffsetX, m_fAvatOffsetY, m_fAvatOffsetZ);
					transform.position = m_kBindTran.position + m_kChangedDist + m_kShakeOffset;
                }
                else
					transform.position = m_kBindTran.position + m_kAvataOffFar + m_kShakeOffset;
            }
            else
            {
				transform.position = m_kBindTran.position + m_kFightOffset + m_kShakeOffset;
            }

			if (!m_bBeginShake)
            	transform.LookAt(m_kBindTran.position + m_kLookAtOffset);
        }
        else
        {
            if (m_bBossBornBegin)
            {
                SampleCamBossBegin();
            }

            if (m_bBossBornEnd)
            {
                SampleCamBossEnd();
            }
        }

		if (m_bBeginShake)
		{
			if (m_fShakeTime > m_fParamShakeTime)
			{
				m_fShakeTime = 0.0f;
				m_bBeginShake = false;
				m_kShakeOffset = Vector3.zero;
				m_eShakeType = ECameraShakeType.CST_UP_DOWN;
			}
			else
			{
                ShakeCamera(m_fShakeTime / m_fParamShakeTime);
				m_fShakeTime += Time.deltaTime;
			}
		}

        UpdateScreenEffect();
	}

    public void SetBindTran(Transform bindTran)
    {
        m_kBindTran = bindTran;
    }

    public Transform GetBindTran()
    {
        return m_kBindTran;
    }

    public void Sample(float factor)
    {
        float val = Mathf.Clamp01(factor);

        m_fAvatOffsetX = m_kAvataOffNear.x * (1.0f - val) + m_kAvataOffFar.x * val;
        m_fAvatOffsetZ = m_kAvataOffNear.z * (1.0f - val) + m_kAvataOffFar.z * val;

        if (m_kAnimationCurve != null)
        {
            float value = m_kAnimationCurve.Evaluate(val);
            m_fAvatOffsetY = (m_kAvataOffFar.y - m_kAvataOffNear.y) * value + m_kAvataOffNear.y;
        }
    }

    /// sample for player end
    //////////////////////////////////////////////////////////////////////////


    // boss出场桥段
    public void BossBorn(Vector3 bornPos)
    {
        Global.m_bCameraCruise = true;

        MainLogic.sMainLogic.suspendGame();

        m_kBindTran = null;
        m_bBossBornBegin = true;

        m_kCamBeginPos = CharacterPlayer.sPlayerMe.transform.position;
        m_kCamEndPos = bornPos;
    }

    public void BeginCamZoomOut()
    {
        MainLogic.sMainLogic.suspendGame();

        m_bBossBornBegin = false;
        m_bBossBornEnd = true;

        m_fCamZoomToRealTime = 0.0f;
        m_fCamZoomOutRealTime = 0.0f;

        m_kCamBeginPos = m_kCamEndPos;
        m_kCamEndPos = CharacterPlayer.sPlayerMe.transform.position;
    }

    private void CamForBossBornDataReset()
    {
        Global.m_bCameraCruise = false;

        MainLogic.sMainLogic.resumeGame();

        m_bBossBornBegin = false;
        m_bBossBornEnd = false;

        m_fCamZoomToRealTime = 0.0f;
        m_fCamZoomOutRealTime = 0.0f;

        m_kCamBeginPos = Vector3.zero;
        m_kCamEndPos = Vector3.zero;

        SetBindTran(CharacterPlayer.sPlayerMe.transform);
    }



    public void SampleCamBossBegin()
    {
        if (m_fCamZoomToRealTime < m_fCamZoomToBossTime)
        {
            float factor = m_fCamZoomToRealTime / m_fCamZoomToBossTime;

            float val = Mathf.Clamp01(factor);

            float value = m_kCamCurveForBossBegin.Evaluate(val);

            transform.position = (m_kCamEndPos + m_kFightOffset) * value + (m_kCamBeginPos + m_kFightOffset) * (1 - value);
        }
        else
        {
            //BeginCamZoomOut();
            MainLogic.sMainLogic.resumeGame();
        }

        m_fCamZoomToRealTime += Time.deltaTime;
    }

    public void SampleCamBossEnd()
    {
        if (m_fCamZoomOutRealTime < m_fCamZoomOutBossTime)
        {
            float factor = m_fCamZoomOutRealTime / m_fCamZoomOutBossTime;

            float val = Mathf.Clamp01(factor);

            float value = m_kCamCurveForBossEnd.Evaluate(val);

            transform.position = (m_kCamEndPos + m_kFightOffset) * value + (m_kCamBeginPos + m_kFightOffset) * (1 - value);
        }
        else
        {
            CamForBossBornDataReset();
        }

        m_fCamZoomOutRealTime += Time.deltaTime;
    }


	// shake begin
	public void ShakeBegin(ECameraShakeType type, float time)
	{
		switch (type)
		{
		case ECameraShakeType.CST_UP_DOWN:
			{
				m_bBeginShake = true;
				m_fParamShakeTime = time;
				m_eShakeType = ECameraShakeType.CST_UP_DOWN;
			}
			break;
		case ECameraShakeType.CST_LEFT_RIGHT:
			{
				m_bBeginShake = true;
				m_fParamShakeTime = time;
				m_eShakeType = ECameraShakeType.CST_LEFT_RIGHT;
			}
			break;
		case ECameraShakeType.CST_FRONT_BACK:
			{
				m_bBeginShake = true;
				m_fParamShakeTime = time;
				m_eShakeType = ECameraShakeType.CST_FRONT_BACK;
			}
			break;
		case ECameraShakeType.CST_SPHERE:
			{
				m_bBeginShake = true;
				m_fParamShakeTime = time;
				m_eShakeType = ECameraShakeType.CST_SPHERE;
			}
			break;
        case ECameraShakeType.CST_FIGHT_SHAKE:
            {
                m_bBeginShake = true;
                m_fParamShakeTime = time;
                m_eShakeType = ECameraShakeType.CST_FIGHT_SHAKE;
            }
            break;
        case ECameraShakeType.CST_LAST_HIT_SHAKE:
            {
                m_bBeginShake = true;
                m_fParamShakeTime = time;
                m_eShakeType = ECameraShakeType.CST_LAST_HIT_SHAKE;
            }
            break;  
		}
	}

	private void ShakeCamera(float factor)
	{
		switch (m_eShakeType)
		{
		case ECameraShakeType.CST_UP_DOWN:
			ShakeUpDown();
			break;
		case ECameraShakeType.CST_LEFT_RIGHT:
			ShakeLeftRight();
			break;
		case ECameraShakeType.CST_FRONT_BACK:
			ShakeFrontBack();
			break;
		case ECameraShakeType.CST_SPHERE:
			ShakeRandomRadius();
			break;
        case ECameraShakeType.CST_FIGHT_SHAKE:
            ShakeFightMode();
            break;
        case ECameraShakeType.CST_LAST_HIT_SHAKE:
            ShakeLastHit(factor);
            break;
		}
	}

	private void ShakeUpDown()
	{
		Vector3 kCamUp = CameraFollow.sCameraFollow.transform.up;
		m_kShakeOffset = kCamUp * UnityEngine.Random.Range(-m_fShakeMagnititudeUpDown, m_fShakeMagnititudeUpDown);
	}

	private void ShakeLeftRight()
	{
		Vector3 kCamRight = CameraFollow.sCameraFollow.transform.right;
		m_kShakeOffset = kCamRight * UnityEngine.Random.Range(-m_fShakeMagnititudeLeftRight, m_fShakeMagnititudeLeftRight);
	}

	private void ShakeFrontBack()
	{
		Vector3 kCamForward = CameraFollow.sCameraFollow.transform.forward;
		m_kShakeOffset = kCamForward * UnityEngine.Random.Range(-m_fShakeMagnititudeFrontBack, m_fShakeMagnititudeFrontBack);
	}

	private void ShakeRandomRadius()
	{
		float x = UnityEngine.Random.Range (-m_fShakeMagnititudeRandomRidus, m_fShakeMagnititudeRandomRidus);
		float y = UnityEngine.Random.Range (-m_fShakeMagnititudeRandomRidus, m_fShakeMagnititudeRandomRidus);
		float z = UnityEngine.Random.Range (-m_fShakeMagnititudeRandomRidus, m_fShakeMagnititudeRandomRidus);
		m_kShakeOffset = new Vector3 (x, y, z);
	}

    private void ShakeFightMode()
    {
        // 往玩家反方向拉伸摄像机
        Vector3 vecShakeDir = -CharacterPlayer.sPlayerMe.getFaceDir();

        m_kShakeOffset += vecShakeDir * 0.05f;
    }

    private void ShakeLastHit(float factor)
    {
        // 摄像机往里伸缩
        Vector3 kCamForward = CameraFollow.sCameraFollow.transform.forward;
        m_kShakeOffset += kCamForward * 0.02f;

        // 同时朝向指向最后一击的敌人
        Vector3 kTarget = Camera.main.transform.position + SampleCamLook(factor * 3);
        Camera.main.transform.LookAt(kTarget);
    }


    Vector3 SampleCamLook(float factor)
    {
        return Vector3.Lerp(m_kLastHitInitPoint, m_kLastHitLookPoint, factor);
    }

    public void UpdateScreenEffect()
    {
        if (m_nScreenTexIndex > 0)
        {
            if (Camera.main)
            {
                ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

                if (bloodOverlay)
                {
                    if (m_fUpdateScreenEffectTime > 0.02f)
                    {
                        if (m_nScreenTexIndex % 3 == 0)
                        {
                            bloodOverlay.index = 1;
                        }
                        else if (m_nScreenTexIndex % 3 == 1)
                        {
                            bloodOverlay.index = 2;
                        }
                        else if (m_nScreenTexIndex % 3 == 2)
                        {
                            bloodOverlay.index = 3;
                        }

                        bloodOverlay.intensity = 1.0f;

                        m_nScreenTexIndex++;

                        m_fUpdateScreenEffectTime = 0.0f;
                    }

                    m_fUpdateScreenEffectTime += Time.deltaTime;
                }
            }
        }
    }

    public void ScreenEffect(Vector3 kLookPoint)
    {
        Time.timeScale = 0.2f;

        m_nScreenTexIndex = 1;

        m_fUpdateScreenEffectTime = 0.0f;

        if (Camera.main)
        {
            ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

            if (bloodOverlay)
            {
                bloodOverlay.index = 1;

                m_kLastHitLookPoint = kLookPoint - transform.position;
                m_kLastHitLookPoint.Normalize();
                m_kLastHitInitPoint = Camera.main.transform.forward;
                ShakeBegin(ECameraShakeType.CST_LAST_HIT_SHAKE, 1.0f);
            }
        }
    }

    public void StopEffect()
    {
        m_fUpdateScreenEffectTime = 0.0f;

        m_nScreenTexIndex = 0;

        Time.timeScale = 1.0f;

        if (Camera.main)
        {
            ScreenOverlayTex bloodOverlay = Camera.main.GetComponent<ScreenOverlayTex>();

            if (bloodOverlay)
            {
                bloodOverlay.index = 0;
                m_kLastHitLookPoint = Vector3.zero;
                m_kLastHitInitPoint = Vector3.zero;
            }
        }
    }
}
