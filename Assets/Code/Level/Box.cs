using UnityEngine;
public class Box : LevelEventHandler 
{
	bool is_open = false;
	public bool is_complete = false;
	public bool drop_from_sky = false;
	public bool auto_open = false;
    public int id = 0;
	private Transform effect = null;
	public Transform effect_transform = null;
	public float trigger_time = 5.0f;
	public LevelEventHandler handler;
	// Use this for initialization
	void Start () 
    {
		if (auto_open) 
        {
			onTrigger();
		}
	}
	
	// Update is called once per frame
	void Update () 
    {
	
		if (is_open) 
        {
			
			if (trigger_time > 0) 
            {
				trigger_time -= Time.deltaTime;
			}
			else 
            {
				if (handler)
					handler.onTrigger();
				
				if (Global.inTowerMap()) 
                {					
					//根据当前是第几波发掉落
					TowerDataItem di = ConfigDataManager.GetInstance().getTowerConfig().getTowerData((int)Global.cur_TowerId);
					if (di!=null) 
                    {
						//发送掉落
                        MessageManager.Instance.sendMessageOpenBox(di.drop, 0);
					}
                    //DemonVo vo = manager.DemonManager.Instance.GetDemonVoById(Global.cur_TowerId);
                    //if (vo.Level < 100) 
                    //{
                    //    BattleEmodongku.GetInstance().waitTowerMonsterWave(Global.cur_TowerId + 1);
                    //}

				}
				else 
                {
					if (is_complete) 
                    {
						MessageManager.Instance.sendMessageOpenBox(id, 1);
					}
					else 
                    {
						MessageManager.Instance.sendMessageOpenBox(id, 0);
					}
				}
				
				Destroy(gameObject);
			}
		}
		
	}
	
	public void openBox() 
    {	
		if (drop_from_sky) 
        {
            float xOffset = Random.Range(-1.0f, 1.0f);
            float zOffset = 0.0f;
            if (xOffset < 0.5f && xOffset > -0.5f)
            {
                if (Random.Range(0, 100) % 2 == 0)
                {
                    zOffset = Random.Range(0.5f, 1.0f);
                }
                else
                {
                    zOffset = Random.Range(-1.0f, -0.5f);
                }
            }
            else
            {
                zOffset = Random.Range(-1.0f, 1.0f);
            }
                

            transform.position = CharacterPlayer.sPlayerMe.getPosition() + new Vector3(xOffset, 0.0f, zOffset);

			animation.Play("dropbox");		
		}
		else 
        {
			animation.Play("openbox");
		}
		
		is_open = true;

        if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
        {
            BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, PathConst.TOLL_GATE_ITEM2,
                (asset) =>
                {
                    GameObject timeObject = BundleMemManager.Instance.instantiateObj(asset, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
                    FlyEffect fe = (FlyEffect)timeObject.AddComponent("FlyEffect");
                    fe.target = CharacterPlayer.sPlayerMe.transform;
                    fe.delta = 1.0f;
                    fe.blast_res = PathConst.TOLL_GATE_ITEM1;
                    if (handler && handler.active)
                    {
                        handler.onTrigger();
                    }

					GameObject.Destroy(timeObject, 1.5f);
                });
        }
        else
        {
            GameObject asset = BundleMemManager.Instance.getPrefabByName(PathConst.TOLL_GATE_ITEM2, EBundleType.eBundleUIEffect);
            GameObject timeObject = BundleMemManager.Instance.instantiateObj(asset, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            FlyEffect fe = (FlyEffect)timeObject.AddComponent("FlyEffect");
            fe.target = CharacterPlayer.sPlayerMe.transform;
            fe.delta = 1.0f;
            fe.blast_res = PathConst.TOLL_GATE_ITEM1;
            if (handler && handler.active)
            {
                handler.onTrigger();
            }

			GameObject.Destroy(timeObject, 1.5f);
        }  
        
	}
	
	/*
	void closeBox() {
		AnimationState animState = animation["Take 001"];
		animState.speed = -0.10f;
		animState.time = animState.length;
		animation.Play("Take 001");
		is_open = false;
	}
	*/
	
	public override void onTrigger() 
    {
		if (!active)
			return;
		
		if (!is_open) 
        {
			if(is_complete) //最后一个宝箱
			{
				ScenarioManager.Instance.showScenario(TaskManager.Instance.MainTask, openBox, eTriggerType.finishGate);
				ScenarioManager.Instance.passGateSuccess = true;
			}
			else
				openBox();
		}
	}
	
	public void showEffect(Transform eff)
    {
        if (eff == null) 
            return;

        GameObject fxEff = BundleMemManager.Instance.instantiateObj(eff, transform.position, transform.rotation);
        Destroy(fxEff, 5.0f);
	}
	
	public void playAudio( AudioClip clip ) 
    {
		if (CharacterPlayer.sPlayerMe.audio) 
        {
			CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip);
		}
	}
}
