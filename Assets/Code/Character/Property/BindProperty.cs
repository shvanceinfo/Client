using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// 绑定属性 用来绑定 翅膀 宠物 指示器之类的
public class BindProperty : MonoBehaviour
{
    // 脚下的任务只是光标
    public GameObject m_kIndicator;

    private Character m_kOwner;
    private Vector3 m_vecIndicatorLookAtTarget;
    private Vector3 m_vecEnterPoint;

    private Vector3 m_vecFromTarget;
    private Vector3 m_vecToTarge;

    private bool m_bBeginFadeoff;
    private float m_fFadeoffTime;
   

    void Awake()
    {
        m_kOwner = gameObject.GetComponent<Character>();
    }

    void Start()
    {
        m_bBeginFadeoff = false;
        m_fFadeoffTime = 0.0f;
    }

    void Update()
    {
        if (m_kIndicator)
        {
            if (m_bBeginFadeoff)
            {
                if (m_fFadeoffTime < 2.0f)
                {
                    Quaternion lerpDir = FadeOffSample(m_vecIndicatorLookAtTarget - m_vecEnterPoint,
                        m_vecToTarge - m_vecEnterPoint,
                        m_fFadeoffTime / 2.0f);

                    //m_kIndicator.transform.LookAt(
//                        CharacterPlayer.sPlayerMe.transform.position + lerpDir);
                    m_kIndicator.transform.rotation = lerpDir;
                }
                else
                {
                    m_vecIndicatorLookAtTarget = m_vecToTarge;

                    m_bBeginFadeoff = false;
                    m_fFadeoffTime = 0.0f;
                }

                m_fFadeoffTime += Time.deltaTime;
            }
            else
            {
                m_kIndicator.transform.LookAt(m_vecIndicatorLookAtTarget);
            }
            
        }
    }

    public void GenerateIndicator(Vector3 target, Vector3 enterPoint)
    {
        m_vecEnterPoint = enterPoint;

        if (m_kIndicator == null)
        {
            // 初始触发点
            GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.LEAD_WAY_CAMERA, EBundleType.eBundleBattleEffect);
            m_kIndicator = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            m_kIndicator.transform.parent = m_kOwner.transform;
            m_kIndicator.transform.localPosition = Vector3.zero;
            m_vecIndicatorLookAtTarget = target;         
        }
        else
        {
            // 转向
            ChangeIndictorLookAt(target);
        }
    }

    public void RemoveIndicator()
    {
        if (m_kIndicator != null)
        {
            GameObject.Destroy(m_kIndicator);
            m_kIndicator = null;
        }
    }

    public void ChangeIndictorLookAt(Vector3 target)
    {
        m_bBeginFadeoff = true;
        m_vecToTarge = target;
    }

    public Quaternion FadeOffSample(Vector3 vecBegin, Vector3 vecEnd, float factor)
    {
        // 统一到vecBegin上
        // end 朝向
        vecBegin.Normalize();
        vecEnd.Normalize();

        return Quaternion.Slerp(Quaternion.FromToRotation(Vector3.forward, vecBegin),
            Quaternion.FromToRotation(Vector3.forward, vecEnd), factor);
        

        //return vecBegin * (1f - factor) + vecEnd * factor;
    }
}
