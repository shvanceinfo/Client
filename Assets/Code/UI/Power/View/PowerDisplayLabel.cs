using UnityEngine;
using System.Collections;

public class PowerDisplayLabel : MonoBehaviour {

    public float speed=100;
    public float activetime = 2;
    public string text {
        set {
            transform.GetComponent<UILabel>().text = value; 
        }
    }
    void Awake()
    {
        
        Destroy(this.gameObject, activetime);
    }
    void Update()
    { 
        Vector3 v3=transform.localPosition;
        v3.y+=Time.deltaTime*speed;
        transform.localPosition = v3;
    }
}
