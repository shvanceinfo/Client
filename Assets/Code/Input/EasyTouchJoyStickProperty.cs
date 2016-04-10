using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasyTouchJoyStickProperty : MonoBehaviour
{
    static public EasyTouchJoyStickProperty sJoystickProperty;

    void Awake()
    {
        sJoystickProperty = this;
        //SceneManager.Instance.setDontDestroyObj(transform.gameObject);
    }


    void Start()
    {
        
    }

    void Update()
    {

    }
    
    public static void ShowJoyTouch(bool isVisible)
    {
		if(EasyTouchJoyStickProperty.sJoystickProperty != null)
		{
	    	ResetJoyStickTouchPos();
	    	EasyTouchJoyStickProperty.sJoystickProperty.gameObject.SetActive(isVisible);
			SetJoyStickEnable(isVisible);
		}
    }

    private static void ResetJoyStickTouchPos()
    {
        if (EasyTouchJoyStickProperty.sJoystickProperty)
        {
            
            EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().JoystickTouch = Vector2.zero;
        }
    }

    public static void SetJoyStickEnable(bool enable)
    {
        if (EasyTouchJoyStickProperty.sJoystickProperty)
        {
            EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().resetFingerExit = !enable;
            EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().enable = enable;

            if (!enable)
            {
                EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().JoyAnchor = EasyJoystick.JoystickAnchor.None;
            }
            else
            {
                EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().JoyAnchor = EasyJoystick.JoystickAnchor.LowerLeft;
            }
        }
    }

    public static bool IsJoyStickEnable()
    {
        if (EasyTouchJoyStickProperty.sJoystickProperty)
        {
            return EasyTouchJoyStickProperty.sJoystickProperty.GetComponent<EasyJoystick>().enable;
        }

        return false;
    }
}