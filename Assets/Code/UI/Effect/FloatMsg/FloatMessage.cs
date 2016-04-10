using UnityEngine;
using System.Collections;
using System;

public class FloatMessage  {
    private static FloatMessage instance;
    private UnityEngine.Object prefab;
    private UnityEngine.Object blastPrefab;

    private GameObject obj;
    private UnityEngine.Object awdPrefab;
    private UnityEngine.Object newPrefab;
	private UnityEngine.Object oldPrefab;
	
	private UnityEngine.Object levelUpPrefab;
	private UnityEngine.Object advancePrefab;
    private FloatMessage()
    {
        prefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_MESSAGE) as GameObject;
        blastPrefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_BLAST_MESSAGE) as GameObject; 
        oldPrefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_MESSAGE_OLD) as GameObject;
        levelUpPrefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_LEVELUP) as GameObject;
        advancePrefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_ADVANCE) as GameObject;
        //prefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_MESSAGE);
        //blastPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_BLAST_MESSAGE);
        //oldPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_MESSAGE_OLD);
        //levelUpPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_LEVELUP);
        //advancePrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_ADVANCE);
    }

    public static FloatMessage GetInstance()
    {
        if (instance == null)
        {
            instance = new FloatMessage();
        }
        return instance;
    }

    public void PlayAwardItemMessage(string msg, ItemTemplate item, float delay, Transform parent)
    {
        if (awdPrefab == null)
        {
            awdPrefab = BundleMemManager.Instance.loadResource(PathConst.FLOAT_MESSAGE_AWARD) as GameObject;
            //awdPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOAT_MESSAGE_AWARD);
        }
        obj = BundleMemManager.Instance.instantiateObj(awdPrefab);        
        obj.transform.FindChild("msg").GetComponent<UILabel>().text = msg;
        UITexture itemSp = obj.transform.FindChild("icon").GetComponent<UITexture>();
        DealTexture.Instance.setTextureToIcon(itemSp, item);
        obj.transform.parent = parent;
        obj.GetComponent<PlayFloatFinish>().PlayAwardItem(delay);        
    }

    public void PlayNewFloatMessage(string msg, bool isError, Transform parent)
    {
        if (newPrefab == null)
        {
            newPrefab = BundleMemManager.Instance.getPrefabByName(PathConst.FLOW_MSG_PATH, EBundleType.eBundleUI); 
        }
        obj = BundleMemManager.Instance.instantiateObj(newPrefab);
        //obj.transform.localScale = Vector3.one;
        if (isError)
        {
            obj.transform.FindChild("bg").GetComponent<UISprite>().spriteName = "error_msg_bg";
        }
        obj.transform.FindChild("float_message").GetComponent<UILabel>().text = msg;
        //obj.GetComponent<PlayFloatFinish>().PlayMsg();
        obj.transform.parent = parent;
		obj.transform.localScale = new Vector3(1,1,1);
		obj.transform.localPosition = new Vector3(0,0,-2500);
    }

    public void PlayFloatMessage(string msg, Transform parent, Vector3 from, Vector3 to)
    {
        if (prefab != null)
        {
            obj = BundleMemManager.Instance.instantiateObj(prefab);
            obj.GetComponent<UILabel>().text = msg;
            obj.transform.parent = parent; 
			obj.transform.localScale = new Vector3(1,1,1);
			//obj.transform.localScale = new Vector3(24,24,1);
			obj.transform.localPosition = new Vector3(19,57,-250);
			if(from != Vector3.zero || to != Vector3.zero)
			{
				obj.GetComponent<TweenPosition>().from = from;
				obj.GetComponent<TweenPosition>().to = to;
			}
        }        
    }
	
	public void PlayFloatMessage3D(string msg, float scaleTime, Vector3 orig, Vector3 delta)
    {
        if (oldPrefab != null)
        {
			GameObject floatObj = new GameObject();
			floatObj.transform.position = orig;
			floatObj.AddComponent("BillBoard");

            obj = BundleMemManager.Instance.instantiateObj(oldPrefab);
            obj.GetComponent<UILabel>().text = msg;           
            obj.transform.parent = floatObj.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(0.2f * scaleTime, 0.2f * scaleTime, 1);
            Play(obj, obj, Vector3.zero, delta);
			GameObject.Destroy(floatObj, 2.0f);
        }        
    }

    // 飘血装用
    public void PlayFloatMessage3D(string msg, float scaleTime, Transform parent, Vector3 delta)
    {
        if (oldPrefab != null)
        {
            GameObject floatObj = new GameObject();
            floatObj.transform.position = parent.position;
            //floatObj.transform.parent = parent;
            //floatObj.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            //floatObj.transform.localScale = Vector3.one;
            floatObj.AddComponent("BillBoard");

            obj = BundleMemManager.Instance.instantiateObj(oldPrefab);
            obj.GetComponent<UILabel>().text = msg;
            obj.transform.parent = floatObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.2f * scaleTime, 0.2f * scaleTime, 1);
            //PlayBlood(obj, obj, Vector3.zero, delta);
            GameObject.Destroy(floatObj, 2.0f);
        }        
    }


    public void PlayFloatMessage3DBlast(string msg, float scaleTime, Transform parent, Vector3 delta)
    {
        if (blastPrefab != null)
        {
            GameObject floatObj = new GameObject();
            floatObj.transform.position = parent.position;
            //floatObj.transform.parent = parent;
            //floatObj.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
            //floatObj.transform.localScale = Vector3.one;
            floatObj.AddComponent("BillBoard");

            obj = BundleMemManager.Instance.instantiateObj(blastPrefab);
            obj.GetComponent<UILabel>().text = msg;
            obj.transform.parent = floatObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.2f * scaleTime, 0.2f * scaleTime, 1);
            //PlayBlood(obj, obj, Vector3.zero, delta);
            GameObject.Destroy(floatObj, 2.0f);
        }
    }

    private void PlayBlood(GameObject playObj, GameObject sendObj, Vector3 from, Vector3 to)
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(playObj, 0f);
        alpha.duration = 1.5f;
        alpha.from = 1.5f;
        alpha.to = 1.0f;

        TweenPosition comp = UITweener.Begin<TweenPosition>(playObj, 0.8f);
        comp.method = UITweener.Method.JiangLin;
        comp.from = from;
        comp.to = to;
        comp.callWhenFinished = "OnFinished";
        comp.eventReceiver = sendObj;
    }




    private void Play(GameObject playObj, GameObject sendObj, Vector3 from, Vector3 to)
    {
        TweenAlpha alpha = UITweener.Begin<TweenAlpha>(playObj, 0f);
        alpha.duration = 1.5f;
        alpha.from = 1.5f;
        alpha.to = 0.1f;

        TweenPosition comp = UITweener.Begin<TweenPosition>(playObj, 1.5f);
        comp.method = UITweener.Method.EaseInOut; 
        comp.from = from;
        comp.to = to;
        comp.callWhenFinished = "OnFinished";
        comp.eventReceiver = sendObj;
    }
	
	public void PlayImageLevelUpFloatMessage(Transform parent){
		if (levelUpPrefab!=null) {
            obj = BundleMemManager.Instance.instantiateObj(this.levelUpPrefab);
			obj.transform.parent = parent;
			obj.transform.localPosition = new Vector3(0,0,-5);
		}
	}
	
	public void PlayImageAdvanceFloatMessage(Transform parent){
		if (advancePrefab !=null) {
            obj = BundleMemManager.Instance.instantiateObj(this.advancePrefab);
			obj.transform.parent = parent;
			obj.transform.localPosition = new Vector3(0,0,-5);
		}
	}
	
    //void TweenPosition()
    //{

    //}
}
