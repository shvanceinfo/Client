using UnityEngine;
using System.Collections;

public class IndicatorArea : MonoBehaviour 
{
    public GameObject nextArea;

	void Awake()
    {

    }

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	
	}

    void OnTriggerEnter(Collider other)
    {
        CharacterPlayer cp = other.gameObject.GetComponent<CharacterPlayer>();
        if (!cp) return;

        if (nextArea != null)
        {
            Vector3 vecLookAtTarget = new Vector3(nextArea.transform.position.x, 0.0f, nextArea.transform.position.z);
            CharacterPlayer.sPlayerMe.GetComponent<BindProperty>().GenerateIndicator(vecLookAtTarget, CharacterPlayer.sPlayerMe.transform.position);  
        }
        else
            // destroy indicator
            CharacterPlayer.sPlayerMe.GetComponent<BindProperty>().RemoveIndicator();


       

        
        DestoryAllThings();
    }

    void DestoryAllThings()
    {
        
        // at last
        Destroy(gameObject);
    }
}
