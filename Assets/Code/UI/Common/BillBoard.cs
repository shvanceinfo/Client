using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void LateUpdate()
    {

        if (Camera.main)
        {
            Vector3 v = Camera.main.transform.forward;
            transform.rotation = Quaternion.LookRotation(v);
        }
	}
}
