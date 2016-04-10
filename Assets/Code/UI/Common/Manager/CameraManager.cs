using UnityEngine;
using System.Collections;

public class EffectPath
{
    
}
public class CameraManager  {

    const string EffectCamera = "UI/Camera_UI/EffectCamera";
    const string Effect = "EffectCamera";
    static CameraManager _instance;

    Hashtable _effectResources;
    
    private GameObject _effectCamera;
     
    private CameraManager()
    {
        _effectResources = new Hashtable();
        
    }

    private void FindEffectCamera()
    {
        if (_effectCamera == null)
        {
            _effectCamera = GameObject.Find(Effect);
            if (_effectCamera == null)
            {
                GameObject prefab = BundleMemManager.Instance.getPrefabByName(EffectCamera, EBundleType.eBundleCommon);
                _effectCamera = BundleMemManager.Instance.instantiateObj(prefab);
                _effectCamera.name = Effect;
                _effectCamera.transform.localPosition = Vector3.zero;
                _effectCamera.transform.localScale = new Vector3(1, 1, 1);
                _effectCamera.transform.localRotation = Quaternion.identity;
                Camera.SetupCurrent(_effectCamera.GetComponent<Camera>());
                
            }
        }
    }

    public void ShowEffect(string path, float DestroyTime)
    {
        FindEffectCamera();            
        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, path,
                (asset) =>
                {
                    GameObject prefab = (GameObject)asset;
                    Vector3 pos = prefab.transform.localPosition;
                    Vector3 scale = prefab.transform.localScale;
                    Quaternion rotation = prefab.transform.localRotation;
                    GameObject obj = BundleMemManager.Instance.instantiateObj(prefab);
                    obj.transform.parent = _effectCamera.transform;
                    obj.transform.localPosition = pos;
                    obj.transform.localScale = scale;
                    obj.transform.localRotation = rotation;
                    GameObject.Destroy(obj, DestroyTime);
                });
        }
        else
        {
            GameObject prefab = BundleMemManager.Instance.getPrefabByName(path, EBundleType.eBundleUIEffect);
            Vector3 pos = prefab.transform.localPosition;
            Vector3 scale = prefab.transform.localScale;
            Quaternion rotation = prefab.transform.localRotation;
            GameObject obj = BundleMemManager.Instance.instantiateObj(prefab);
            obj.transform.parent = _effectCamera.transform;
            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;
            obj.transform.localRotation = rotation;
            GameObject.Destroy(obj, DestroyTime);
        }                       
    }



    public static CameraManager Instance
    {
        get {
            if (_instance==null)
            {
                _instance = new CameraManager();
            }
            return _instance;
        }
    }

}
