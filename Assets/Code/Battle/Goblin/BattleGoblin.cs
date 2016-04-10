using UnityEngine;
using MVC.entrance.gate;

class BattleGoblin
{
    static private BattleGoblin _instance;

    public int m_nGoblinId = 0;

    public int m_nKilledNum = 0;
    public int m_nKilledGaintNum = 0;

    // 不带Greed buff杀死的哥布林金钱
    public int m_nKilledGoblin = 0;

    // 带Greed buff杀死的金钱
    public int m_nKilledGoblinWithGreed = 0;

    public int m_nHitGoblinMoney = 0;

    public int m_nGoblinHitMeMoney = 0;

    public int m_nOneMoney = 0;

    public const int GOBLIN_MONEY_MULTIPLE = 5;

    private Object m_kHeadBuffPrefab;

    private GameObject m_kHeadBuff;

    static public BattleGoblin GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleGoblin();
            return _instance;
        }

        return _instance;
    }

    public void OnDestroy()
    {
        if (m_kHeadBuff)
        {
            m_kHeadBuff = null;
        }

        if (m_kHeadBuffPrefab)
        {
            m_kHeadBuffPrefab = null;
        }
    }


    BattleGoblin()
    {
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, PathConst.GOBULIN_BUFF,
                (asset) =>
                {
                    m_kHeadBuffPrefab = asset;
                });
        }
        else
        {
            m_kHeadBuffPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.GOBULIN_BUFF, EBundleType.eBundleUIEffect);
        }       
    }

    public void ResetData()
    {
        m_nKilledNum = 0;
        m_nKilledGaintNum = 0;
        m_nKilledGoblin = 0;
        m_nKilledGoblinWithGreed = 0;
        m_nHitGoblinMoney = 0;
        m_nGoblinHitMeMoney = 0;

        m_nOneMoney = OneGoblinMoney();
    }

    public void Init()
    {
        GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.GOBULIN_GOLD, EBundleType.eBundleGobulin);
        BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);      
    }

    public int CalGainedMoney()
    {
        int nOneGoblinMoney = OneGoblinMoney();

        int nRet = m_nKilledGoblin + m_nKilledGoblinWithGreed + m_nHitGoblinMoney - m_nGoblinHitMeMoney;
        Debug.ClearDeveloperConsole();
        Debug.Log("黄金哥布林数据 Start: ");
        Debug.Log("非贪婪击杀获得金钱 "+ m_nKilledGoblin);
        Debug.Log("贪婪击杀获得金钱 " + m_nKilledGoblinWithGreed);
        Debug.Log("击杀单只获得金钱 " + nOneGoblinMoney);
        Debug.Log("击中获得金钱 " + m_nHitGoblinMoney);
        Debug.Log("被偷金钱 " + m_nGoblinHitMeMoney);
        Debug.Log("金钱改变值 " + nRet);
        Debug.Log("黄金哥布林数据 End: ");

        return nRet;
    }

    public int OneGoblinMoney()
    {
        double fTableMoney = (double)ConfigDataManager.GetInstance().getMonsterConfig().getMonsterData(m_nGoblinId).gold;
        int nPlayerLevel = CharacterPlayer.character_property.getLevel();

        int nOneGoblinMoney = (int)
            (fTableMoney +
            (1.16 * nPlayerLevel * nPlayerLevel * nPlayerLevel - 57.76 * nPlayerLevel * nPlayerLevel + 3150.79 * nPlayerLevel + 9046.67) / 86);

        return nOneGoblinMoney;
    }

    public void ShowHeadGainGold(Character goblin, int gold)
    {
//        FloatMessage.GetInstance().PlayFloatMessage3D(
//                Global.FormatStrimg(LanguageManager.GetText("golden_goblin_hit_gain"), gold),
//                1.0f,
//                goblin.getTagPoint("help_hp"),
//                new Vector3(0, 0.8f, 0));
		FloatBloodNum.Instance.PlayFloatBood(false, -gold, goblin.getTagPoint("help_hp"), eHurtType.goblinMoney);
        //UiFightMainMgr.Instance.SetGoblinGainNum(CalGainedMoney());
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_GOLD_NUM, CalGainedMoney());
    }

    public void ShowHeadLostGold(int gold)
    {
//        FloatMessage.GetInstance().PlayFloatMessage3D(
//                Global.FormatStrimg(LanguageManager.GetText("golden_goblin_lost_gain"), gold),
//                1.0f,
//                CharacterPlayer.sPlayerMe.getTagPoint("help_hp"),
//                new Vector3(0, 0.8f, 0));
		FloatBloodNum.Instance.PlayFloatBood(true, gold, CharacterPlayer.sPlayerMe.getTagPoint("help_hp"), eHurtType.goblinMoney);

        //UiFightMainMgr.Instance.SetGoblinGainNum(CalGainedMoney());
        Gate.instance.sendNotification(MsgConstant.MSG_FIGHT_DISPLAY_GOBLIN_GOLD_NUM, CalGainedMoney());
    }
    



    public void SetHeadBuffBoard(int nBuffId)
    {
        if (!m_kHeadBuff)
        {
            m_kHeadBuff = GenerateBillBoard(CharacterPlayer.sPlayerMe.getTagPoint("help_hp"), nBuffId);
        }
        else
        {
            m_kHeadBuff.transform.FindChild("text/title/icon").GetComponent<UISprite>().spriteName = ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(nBuffId).buffIcon;
            m_kHeadBuff.transform.FindChild("text/title/icon").gameObject.SetActive(true);
        }
    }

    public GameObject GenerateBillBoard(Transform parent, int nBuffId)
    {
        if (nBuffId != 0)
        {
            GameObject billboard = new GameObject();
            billboard.transform.parent = parent;
            billboard.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            billboard.transform.localScale = Vector3.one;

            billboard.AddComponent("BillBoard");
            billboard.name = "billboard";


            GameObject obj = BundleMemManager.Instance.instantiateObj(m_kHeadBuffPrefab);
            obj.name = "text";
            obj.transform.parent = billboard.transform;
            obj.transform.localPosition = Vector3.zero;
            billboard.transform.localScale = new Vector3(0.01f,0.01f,1) ;

            obj.transform.FindChild("title/icon").GetComponent<UISprite>().spriteName =
            ConfigDataManager.GetInstance().getSkillEffectConfig().getSkillEffectData(nBuffId).buffIcon;

            return billboard;
        }
        else
            return null;
    }

    public void RemoveHeadBuff()
    {
        if (m_kHeadBuff)
        {
            m_kHeadBuff.transform.FindChild("text/title/icon").gameObject.SetActive(false);
        }
    }
}