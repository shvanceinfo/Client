using UnityEngine;
using System.Collections;

public class SpriteMesh : MonoBehaviour {
	
	public int u_num = 1;
	public int v_num = 1;
	public int sprite_num = 1;
	public float speed = 1.0f;
	int index = 0;
	float counter = 0;
	Vector2 duv;
	
	// Use this for initialization
	void Start () {
		
		duv = new Vector2(1.0f/u_num, 1.0f/v_num);
		renderer.sharedMaterial.SetTextureScale("_MainTex", duv);

	}
	
	// Update is called once per frame
	void Update () {
		
		counter += Time.deltaTime * speed;
		index = (int)counter;
		float tmp = counter - index;
		
		if (index >= sprite_num) {
			Destroy(gameObject);
		}
		
		int v = (int)index / u_num;
		int u = index % u_num;
		
		Vector2 texOffset = new Vector2(u * duv.x, (1.0f - v * duv.y));
		renderer.sharedMaterial.SetTextureOffset("_MainTex", texOffset);
	}
}
