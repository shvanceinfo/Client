using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

public class EffectManager 
{
    public delegate void FxPostProcess(Character character);

    private static EffectManager _instance = null;

	public static EffectManager Instance
    {
        get
        {
            if(_instance == null)
                _instance = new EffectManager();
            return _instance;
        }
    }

    private EffectManager()
    {
    }
	
	public void InitEffect()
	{
	}
	


	// Update is called once per frame
	void Update () {
	
	}
	
	//type 0-主角朝向, 1-上下朝向， 2-全向
	public void createCameraShake(int type, float amplitude = 0.1f, float time = 0.3f) {
		
		if (type == 0) {
			
			if (CharacterPlayer.sPlayerMe) {
				iTween.ShakePosition(Camera.main.gameObject, 
					CharacterPlayer.sPlayerMe.transform.forward * amplitude,
					time);
			}
		}
		else if (type == 1) {
			
			if (CharacterPlayer.sPlayerMe) {
				iTween.ShakePosition(Camera.main.gameObject, 
					-CharacterPlayer.sPlayerMe.transform.up * amplitude,
					time);
			}
		}
		else if (type == 2) {
		
			if (Camera.main) {
			
				iTween.ShakePosition(Camera.main.gameObject, new Vector3(amplitude,amplitude,0), time);
			}
		}
		
	}
	
	public void createFX(string eff_name, Vector3 pos, Quaternion rot, float life = 5.0f) {
		GameObject fxEff = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_name, EBundleType.eBundleBattleEffect);
		if (asset != null)
		{
            fxEff = BundleMemManager.Instance.instantiateObj(asset, pos, rot);
		}
		else
		{
			Debug.LogError(eff_name + " effect manager not in asset bundle");
		}
		Object.Destroy(fxEff, life);
	}

    public void createFX(string eff_name, Vector3 pos, Quaternion rot, FxPostProcess callback, Character callbackParam, float life = 5.0f) 
    {
		GameObject fxEff = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_name, EBundleType.eBundleBattleEffect);
		if (asset != null)
		{
            fxEff = BundleMemManager.Instance.instantiateObj(asset, pos, rot);
		}
		else
		{
            Debug.LogError("not found in effect manager " + eff_name);
		}
		GameObject.Destroy(fxEff, life);
        callback(callbackParam);
	}

    
	
	public GameObject createFX(string eff_name, Transform parent, float life = 20.0f) {
		
		if(parent) {
			GameObject fxEff = null;
            GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_name, EBundleType.eBundleBattleEffect);
			if (asset != null)
			{
                fxEff = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
			}
			else
			{
                Debug.LogError("not found in effect manager " + eff_name);
			}
			
			fxEff.transform.parent = parent;
            fxEff.transform.name = "effectObj";
			fxEff.transform.localPosition = Vector3.zero;
			fxEff.transform.localRotation = Quaternion.identity;
			GameObject.Destroy(fxEff, life);
			return fxEff;
		}
		else {
			return null;
		}
	}

    // 永久挂的特效
    public GameObject CreateFX(string eff_name, Transform parent)
    {
        if (parent)
        {
			GameObject fxEff = null;
            GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_name, EBundleType.eBundleBattleEffect);
			if (asset != null)
			{
                fxEff = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
			}
			else
			{
                Debug.LogError("not found in effect manager " + eff_name);
			}
            
            fxEff.transform.parent = parent;
            fxEff.transform.name = "effectObj";
            fxEff.transform.localPosition = Vector3.zero;
            fxEff.transform.localRotation = Quaternion.identity;
            return fxEff;
        }

        return null;
    }
	
	public void createFX(GameObject eff_perf, Vector3 pos, Quaternion rot, float life = 10.0f)
	{
		GameObject fxEff = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_perf.name, EBundleType.eBundleBattleEffect);
		if (asset != null)
		{
            fxEff = BundleMemManager.Instance.instantiateObj(asset, pos, rot);
		}
		else
		{
            fxEff = BundleMemManager.Instance.instantiateObj(eff_perf, pos, rot);
		}
		GameObject.Destroy(fxEff, life);
	}

    public GameObject createFX(GameObject eff_perf, Transform parent, float life = 5.0f)
    {
        GameObject fxEff = null;
        GameObject asset = BundleMemManager.Instance.getPrefabByName(eff_perf.name, EBundleType.eBundleBattleEffect);
        if (asset != null)
        {
            fxEff = BundleMemManager.Instance.instantiateObj(asset, Vector3.zero, Quaternion.identity);
        }
        else
        {
            fxEff = BundleMemManager.Instance.instantiateObj(eff_perf, Vector3.zero, Quaternion.identity);
        }

        if (fxEff == null)
        {
            Debug.LogError("特效为空 " + eff_perf.name);
        }

        NcAttachPrefab[] prefabs = fxEff.GetComponentsInChildren<NcAttachPrefab>(true);
        foreach (NcAttachPrefab attachPrefab in prefabs)
        {
            if (attachPrefab.name.Equals(Constant.IN_ACTIVE_PREFAB_NAME))
                attachPrefab.gameObject.SetActive(true);
        }

        fxEff.transform.parent = parent;
        fxEff.transform.name = "effectObj";
        fxEff.transform.localPosition = Vector3.zero;
        fxEff.transform.localRotation = Quaternion.identity;
        GameObject.Destroy(fxEff, life);
        return fxEff;
    }

    public void BeHitHightlight(GameObject beHitObj, float duration = 0.3f)
    {
        Highlight hl = Camera.main.GetComponent<Highlight>();
        hl.enabled = true;
        hl.m_fHightlightTime = duration;
        hl.SetHightLightGameObj(beHitObj);
    }

    public void BeginCloseUpEffect(Vector3 kLookPoint)
    {
        if (CameraFollow.sCameraFollow)
        {
            if (CameraFollow.sCameraFollow.gameObject.activeSelf)
            {
                CameraFollow.sCameraFollow.ScreenEffect(kLookPoint);
            }
            else
            {
                ArenaCamera kArenaCam = Camera.main.GetComponent<ArenaCamera>();
                if (kArenaCam != null)
                {
                    kArenaCam.ScreenEffect();
                }
            }
        }
    }

    public void EndCloseUpEffect()
    {
        if (CameraFollow.sCameraFollow)
        {
            if (CameraFollow.sCameraFollow.gameObject.activeSelf)
            {
                CameraFollow.sCameraFollow.StopEffect();
            }
            else
            {
                ArenaCamera kArenaCam = Camera.main.GetComponent<ArenaCamera>();
                if (kArenaCam != null)
                {
                    kArenaCam.StopEffect();
                }
            }
        }
    }
}
