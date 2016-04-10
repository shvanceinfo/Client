using UnityEngine;
using System.Collections;

public class fps : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI() {
		float fps = 1 / (Time.deltaTime);
		GUILayout.Label(fps.ToString("f0"));
	}
}
