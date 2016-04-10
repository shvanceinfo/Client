using UnityEngine;
using System.Collections.Generic;

public class WaterBase3 : MonoBehaviour {
	
	int sprite_num = 80;
	public float speed = 6.0f;
	int index = 0;
	float counter = 0;
	List<Texture2D> texture_list = null;
	
	void Awake() {
        texture_list = new List<Texture2D>();
	}
	
	// Use this for initialization
	void Start () 
    {
        for (int i = 1; i <= sprite_num; i++)
        {
            Texture2D tex =
                BundleMemManager.Instance.loadResource(PathConst.WATER_EFFECT_PATH + i.ToString("D2")) as Texture2D;
            texture_list.Add(tex);
        }
	}
	
	// Update is called once per frame
	void Update () {

        counter += Time.deltaTime * speed;
        index = (int)counter;
        float tmp = counter - index;

        if (index >= sprite_num)
        {
            index = index % sprite_num;
            counter = index + tmp;
        }

        renderer.sharedMaterial.SetTexture("_MainTex", texture_list[index]);
	}
	
	void create_textures() 
	{
		
	}
}
