/**该文件实现的基本功能等
function:实现鼠标按下图标放大，松开恢复正常的
author:ljx
date:2013-10-31
**/
using UnityEngine;
using System.Collections;

public class MouseAction : MonoBehaviour
{
	private ParticleSystem _particle;
	public bool playAnim = false;
	private UISprite _btnBg;
	private UILabel[] _labels;
	private const float SCALE_SIZE = 1.2f;
	
	void Awake ()
	{
		_btnBg = null;
		_particle = null;
		_labels = null; 
	}
	
	void Start ()
	{
	}
		
	void OnPress (bool isPressed)
	{
		if (_btnBg == null) {
			_btnBg = gameObject.GetComponentInChildren<UISprite> ();
			if (_btnBg == null)
				_btnBg.gameObject.GetComponent<UISprite> ();
			_labels = gameObject.GetComponentsInChildren<UILabel> ();
		}
		if (enabled) {
			Vector3 vec3;
			if (isPressed) {
				if (_btnBg != null) {
					vec3 = _btnBg.transform.localScale;
					_btnBg.transform.localScale = vec3 * SCALE_SIZE;
				}
				if (_labels != null && _labels.Length > 0) {
					for (int i=0; i<_labels.Length; i++) {
						UILabel label = _labels [i];
						vec3 = label.transform.localScale;
						label.transform.localScale = vec3 * SCALE_SIZE;
					}
				}
			} else {
				if (_btnBg != null) {
					vec3 = _btnBg.transform.localScale;
					_btnBg.transform.localScale = vec3 / SCALE_SIZE;
					_btnBg = null;
				}
				if (_labels != null) {
					for (int i=0; i<_labels.Length; i++) {
						UILabel label = _labels [i];
						vec3 = label.transform.localScale;
						label.transform.localScale = vec3 / SCALE_SIZE;
					}
					_labels = null;
				}
				if (playAnim && _particle != null)
					_particle.Play ();
			}
		}
	}
	
	void OnHover ()
	{
		
	}
	
	void OnDisable ()
	{ 
		if (_btnBg != null) { 
			_btnBg.transform.localScale = Vector3.one;
		}
		if (_labels != null) {
			for (int i=0; i<_labels.Length; i++) {
				UILabel label = _labels [i];
				 
				label.transform.localScale = Vector3.one;
			}
		}
	}
	
}
