using UnityEngine;

public class BattleJiGuan
{	
	private static BattleJiGuan _instance = null;

    public static BattleJiGuan Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BattleJiGuan();
            return _instance;
        }
    }

    public void OnJiGuanTrigger(JiGuanItem.EJiGuanType type, int nJiGuanID, Vector3 position, SkillAppear skill = null)
    {
        if (nJiGuanID == 0)
        {
            return;
        }

		if (skill != null && skill.m_bSkillTriggedJiGuan)
		{
			return;
		}

        JiGuanItem kItem = ConfigDataManager.GetInstance().GetJiGuanConfig().GetJiGuanData(nJiGuanID);

        if (kItem.eJiGuanType != type)
        {
            return;
        }

        GameObject kCollider = BundleMemManager.Instance.getPrefabByName(kItem.strColliderPrefab, EBundleType.eBundleRoleEffect);

        if (kCollider == null)
        {
            return;
        }

        GameObject obj = BundleMemManager.Instance.instantiateObj(kCollider);

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        JiGuan jiguan = obj.GetComponent<JiGuan>();
        
        if (jiguan == null)
        {
            obj.AddComponent<JiGuan>();
        }

        obj.transform.position = position;

        switch (type)
        {
            case JiGuanItem.EJiGuanType.JGT_SKILL_BEGIN:
            case JiGuanItem.EJiGuanType.JGT_SKILL_HIT:
                {
                    obj.GetComponent<JiGuan>().SetSkillInfo(skill, nJiGuanID);
                }
                break;
            case JiGuanItem.EJiGuanType.JGT_SCENE:
                {
                    obj.GetComponent<JiGuan>().SetSkillInfo(nJiGuanID);
                }
                break;
        }
    }
}
