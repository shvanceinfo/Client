using UnityEngine;
using System.Collections;

public class FlyEffect : MonoBehaviour {
	
	float fly_time = 1.0f;
	public Vector3 fly_speed = new Vector3(0, 6.0f, 0);
	public Transform target;
	Transform self;
	int deceleration = 1;
	float max_speed = 6.0f;
	float acc = 0.5f;
	public float delta = 2.0f;
	public string blast_res;

	// Use this for initialization
	void Start () {
		self = transform;
	}
	
	// Update is called once per frame
	void Update () 
    {		
		if (delta > 0) 
        {
			delta -= Time.deltaTime;
			return;
		}	
        if (target != null)
        {
            Vector3 toTarget = target.position + new Vector3(0, 1.0f, 0) - self.position;
            float dist = toTarget.magnitude;
            if (dist > 0.25f)
            {
                float decelerationTweaker = 2.5f;
                float speed = dist / deceleration * decelerationTweaker;
                speed = Mathf.Min(speed, max_speed);
                Vector3 desiredDir = toTarget * speed / dist;
                Vector3 retDir = desiredDir * max_speed - fly_speed;
                retDir.Normalize();
                fly_speed += retDir * acc;
            }
            else
            {
                if (BundleMemManager.Instance.isTypeInCache(EBundleType.eBundleUIEffect))
                {
                    Vector3 pos = transform.position;
                    BundleMemManager.Instance.loadPrefabViaWWW<GameObject>(EBundleType.eBundleUIEffect, blast_res,
                        (asset) =>
                        {
                            GameObject blastObject = BundleMemManager.Instance.instantiateObj(asset, pos, Quaternion.identity);
                            Destroy(blastObject, 1.0f);
                            //Destroy(gameObject);
                        });
                }
                else
                {
                    GameObject asset = BundleMemManager.Instance.getPrefabByName(blast_res, EBundleType.eBundleUIEffect);
                    GameObject blastObject = BundleMemManager.Instance.instantiateObj(asset, transform.position, Quaternion.identity);
                    Destroy(blastObject, 1.0f);
                    Destroy(gameObject);
                }               
            }
            if (fly_speed.sqrMagnitude > max_speed * max_speed)
            {
                fly_speed.Normalize();
                fly_speed *= max_speed;
            }
            self.position += fly_speed * Time.deltaTime;
        }
	}
}
