using UnityEngine;
using System.Collections;

public class GoblinItemArea : MonoBehaviour
{
    public int m_nAreaID;

    public bool m_bItemUsed;

    public GameObject m_kItemObject;
    ItemTemplate m_kItemInfo;

    public int m_nItemTemplateID;

    GoblinAreaMgr m_kParent;

    float m_fDuration = 10.0f;

	void Awake () 
    {
        m_kParent = transform.parent.GetComponent<GoblinAreaMgr>();
        m_bItemUsed = true;
	}

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
       if (!m_bItemUsed && m_kItemObject)
       {
           m_fDuration -= Time.deltaTime;

           if (m_fDuration < 0.0f)
           {
               DeleteItem();
               Debug.Log("重新生成的item");
               m_kParent.GenerateRandomItem();
           }
       }
	}
	
	void OnTriggerEnter(Collider other)
    {
        // 使用道具流程
        Character character = other.GetComponent<Character>();

        if (character == null)
        {
            return;
        }

        if (character.getType() != CharacterType.CT_PLAYER)
        {
            return;
        }
        
        UseItem();
    }

    void OnTriggerStay(Collider other)
    {
        // 使用道具流程
        Character character = other.GetComponent<Character>();

        if (character == null)
        {
            return;
        }

        if (character.getType() != CharacterType.CT_PLAYER)
        {
            return;
        }

        UseItem();
    }

    void DeleteItem()
    {
        if (m_kItemObject != null && !m_bItemUsed)
        {
            GameObject.Destroy(m_kItemObject);
            m_kItemObject = null;

            m_nItemTemplateID = 0;
            m_bItemUsed = true;
        }
    }

    void UseItem()
    {
        if (!m_bItemUsed)
        {
            if (m_kItemObject != null)
            {

                CharacterPlayer.sPlayerMe.GetComponent<HUD>().InsGoblinIcon();

                Debug.Log("特效销毁了");
                GameObject.Destroy(m_kItemObject);
                m_kItemObject = null;

                m_nItemTemplateID = 0;
                

                int nBuffId = 0;
                if (m_kItemInfo.id == 2000901)
                {
                    // 随机哥布林buff
                    nBuffId = Random.Range(6, 10);
                }
                else
                {
                    nBuffId = (int)m_kItemInfo.usedEffect;
                }
                
                CharacterPlayer.sPlayerMe.AddBuff(nBuffId);

                string textBuff = ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(nBuffId).name;
                // 头顶buff UI
                BattleGoblin.GetInstance().SetHeadBuffBoard(nBuffId);
                FloatMessage.GetInstance().PlayNewFloatMessage(
                    string.Format(LanguageManager.GetText("golden_goblin_gain_buff"), textBuff)
                    , true, UIManager.Instance.getRootTrans());

                if (CharacterPlayer.sPlayerMe != null)
                {
                    CharacterPlayer.sPlayerMe.GetComponent<HUD>().ShowGoblinIcon(ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(nBuffId).buffIcon);
                }

                m_bItemUsed = true;

            }
        }
    }


    public void GenerateItem(int id)
    {
        Debug.Log("buff时间到了");
        if (m_bItemUsed && m_kItemObject == null)
        {
            Debug.Log("哥布林区域" + m_nAreaID + "产生了" + id + "道具");

            m_nItemTemplateID = id;
            m_kItemInfo = ConfigDataManager.GetInstance().getItemTemplate().getTemplateData(id);
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, m_kItemInfo.model,
                    (asset) =>
                    {                        
                        m_kItemObject = BundleMemManager.Instance.instantiateObj(asset);
                        m_kItemObject.transform.parent = this.transform;
                        m_kItemObject.transform.localPosition = Vector3.zero;
                        m_kItemObject.transform.localRotation = Quaternion.identity;
                        m_kItemObject.transform.localScale = Vector3.one;
                        m_bItemUsed = false;
                        m_fDuration = 10.0f;
                    });
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(m_kItemInfo.model, EBundleType.eBundleUIEffect);
                m_kItemObject = BundleMemManager.Instance.instantiateObj(asset);
                m_kItemObject.transform.parent = this.transform;
                m_kItemObject.transform.localPosition = Vector3.zero;
                m_kItemObject.transform.localRotation = Quaternion.identity;
                m_kItemObject.transform.localScale = Vector3.one;
                m_bItemUsed = false;
                m_fDuration = 10.0f;
            }          
        }
    }
}
