using UnityEngine;

class BattleEmodongku
{
    static private BattleEmodongku _instance;

    static public BattleEmodongku GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleEmodongku();
            return _instance;
        }

        return _instance;
    }


    public void waitTowerMonsterWave(uint num)
    {
        Global.cur_TowerId = num;
        string langStr = LanguageManager.GetText("devil_current_wave")
                        + LanguageManager.GetText("devil_wave_prefix") + LanguageManager.GetText("devil_wave_color") + Global.cur_TowerId.ToString()
                        + Constant.COLOR_END + LanguageManager.GetText("devil_wave_suffix");
        TowerDataItem di = ConfigDataManager.GetInstance().getTowerConfig().getTowerData((int)num);
        if (di != null)
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(di.battlePref, EBundleType.eBundleTower);
            GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
            obj.name = di.battlePref;
        }
    }

    public void ShowTowerAward(uint tmpId, string boxPrefab)
    {
        if (Global.inTowerMap())
        {
            // generate box
            if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
            {
                BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleUIEffect, boxPrefab,
                        (asset) =>
                        {
                            GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                            obj.GetComponent<Box>().drop_from_sky = true;
                            obj.GetComponent<Box>().openBox();
                        });   
            }
            else
            {
                GameObject asset = BundleMemManager.Instance.getPrefabByName(boxPrefab, EBundleType.eBundleUIEffect);
                GameObject obj = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
                obj.GetComponent<Box>().drop_from_sky = true;
                obj.GetComponent<Box>().openBox();
            }           
        }
    }
}
