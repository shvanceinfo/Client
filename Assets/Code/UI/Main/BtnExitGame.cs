using UnityEngine;
using System.Collections;

public class BtnExitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable()
    {
        EventDispatcher.GetInstance().DialogSure += OnExitGame;
    }

    void OnDisable()
    {
        EventDispatcher.GetInstance().DialogSure -= OnExitGame;
    }
	
	void OnExitGame(eDialogSureType type)
	{
		//Debug.Log("OnExitGame success!");
		//Application.Quit();
	}
	
	void OnShowDialog()
	{
		UIManager.Instance.ShowDialog(eDialogSureType.eExitGame, LanguageManager.GetText("msg_exit_game"));
	}
}
