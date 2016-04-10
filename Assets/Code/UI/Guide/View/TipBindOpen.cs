using UnityEngine;
using System.Collections;
using manager;
using helper;
public class TipBindOpen : MonoBehaviour {

    private const string PACKAGE = "package";
    void OnClick()
    {
        TipBind bind = GetComponent<TipBind>();
        if (bind)
        {
            if (bind.Bind != null)
            {
                switch (bind.Bind.Special)
                {
                    case model.SpecialType.Normal:
                        break;
                    case model.SpecialType.Bag:
                        BagManager.Instance.OpenBag(bind.Bind.SpecialParams.toInt32());
                        break;
                    default:
                        break;
                }
            }
            else
            {
                OtherFun();
            }
        }
        else {
            OtherFun();
        }
    }
    void OtherFun()
    {
        switch (gameObject.name)
        {
            case PACKAGE:
                BagManager.Instance.OpenBag();
                break;
            default:
                break;
        }
    }
}
