using UnityEngine;
using System.Collections;

public class UIGlint : MonoBehaviour {


    public string spriteName1;

    public string spriteName2;

    public float Speed = 0.1f;

    private bool blink = false;

    private UISprite _sp;
    private void Awake()
    {
        _sp = GetComponent<UISprite>();
    }
    private void Start()
    {
        if (_sp)
        {
            StartCoroutine(Glint());
        }
    }


    private IEnumerator Glint()
    {
        while (true)
        {
            if (blink)
                _sp.spriteName = spriteName1;
            else
                _sp.spriteName = spriteName2;
            blink = !blink;
            yield return new WaitForSeconds(Speed);
        }
    }
}
