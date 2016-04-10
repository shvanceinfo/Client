using UnityEngine;
using System.Collections;

public class WillRenderTransparent : MonoBehaviour
{
	bool need_update;
	bool is_transparent_material;
	Vector3 body_center;
	Material body_material;
	Shader origin_shader;
	Shader tranparent_shader;
	Color orgin_color;
	Color transparent_color;
	
	// Use this for initialization
	void Start () {

        if (!renderer.sharedMaterial.HasProperty("_Color"))
        {
			need_update = false;
			return;
		}
		
		body_center = transform.position + new Vector3(0, 0.5f, 0);
        body_material = renderer.sharedMaterial;
		origin_shader = body_material.shader;
		tranparent_shader = Shader.Find("GUI/Text Shader");
		orgin_color = body_material.color;
		transparent_color = orgin_color;
		transparent_color.a = 0.3f;
		is_transparent_material = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnWillRenderObject() {
		
		if (!need_update) return;
		
		RaycastHit hit;
		
		if (!Camera.main) return;
		//float dist = Vector3.Distance(Camera.main.transform.position, bodyCenter);
		
		Vector3 cameraPos = Camera.main.transform.position;
		
        if (Physics.Raycast(cameraPos, body_center - cameraPos, out hit, 15.0f)) {
			if (hit.collider && hit.collider.renderer && hit.collider.renderer.enabled) {	
				if (!is_transparent_material) {
					body_material.shader = tranparent_shader;
					body_material.SetColor("_Color", transparent_color);
					return;
				}
			}	
		}
		
		if (is_transparent_material) {
			body_material.shader = origin_shader;
			body_material.SetColor("_Color", orgin_color);
		}
	}
}
