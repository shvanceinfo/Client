using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {
	
	public Transform tag_point;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (tag_point) {
			transform.position = tag_point.position;
		}
		else {
			Destroy(gameObject);
		}
	}
}
