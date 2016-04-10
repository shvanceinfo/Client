using UnityEngine;
using System.Collections;
using MVC.entrance.gate;

public class HelperMono : MonoBehaviour {


    protected T F<T>(string path) where T : Component
    {
        return this.transform.FindChild(path).GetComponent<T>();
    }
    protected T F<T>(string path,Transform t) where T : Component
    {
        return t.FindChild(path).GetComponent<T>();
    }

    protected GameObject F(string path)
    {
        return this.transform.FindChild(path).gameObject;
    }

    


    protected void OnEnable()
    {
        Gate.instance.registerMediator(Register());
    }
    protected void OnDisable()
    {
        Gate.instance.removeMediator(RemoveMediator());
    }
    protected virtual ViewMediator Register()
    {
        return new ViewMediator();
    }

    protected virtual uint RemoveMediator()
    {
        return 0;
    }

}
