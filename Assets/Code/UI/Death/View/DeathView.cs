using UnityEngine;
using System.Collections;
using manager;
using MVC.entrance.gate;
using mediator;

public class DeathView : MonoBehaviour {

    private UILabel price;
    private UISprite goldIcon;
    private UILabel tips;
    private UILabel time;
    private void Awake()
    {
        price = transform.FindChild("Function/Price").GetComponent<UILabel>();
        goldIcon = transform.FindChild("Function/GoldIcon").GetComponent<UISprite>();
        tips = transform.FindChild("Function/Tips").GetComponent<UILabel>();
        time = transform.FindChild("Function/time").GetComponent<UILabel>();
    }
    private void OnEnable()
    {
        Gate.instance.registerMediator(new DeathMediator(this));
    }

    private void OnDisable()
    {
        Gate.instance.removeMediator(MediatorName.DEATH_MEDIATOR);
    }
    private void Start()
    {
        DisplayInfo();
        StartCoroutine(LoseTime(DeathManager.Instance.ToCityTime));
    }

    private void DisplayInfo()
    {
        price.text = DeathManager.Instance.Price.ToString();
        tips.text = DeathManager.Instance.Tips;
    }
    private IEnumerator LoseTime(int t)
    {
        time.text = t.ToString();
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (t < 0)
            {
                Gate.instance.sendNotification(MsgConstant.MSG_DEATH_TO_CITY);
                yield break;
            }
            time.text = t.ToString();
            t--;
        }
        
    }


}
