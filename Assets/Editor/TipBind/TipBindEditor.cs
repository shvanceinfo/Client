using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
[CustomEditor(typeof(TipBind))]
public class TipBindEditor : Editor {

    [MenuItem("GameObject/TipBind/AddBind #&q")]
    public static void Create()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            TipBind tb = obj.AddComponent<TipBind>();
            UISprite sp=obj.GetComponent<UISprite>();
            if (sp != null)
            {
                tb.Size = new Vector2(sp.width , sp.height);
            }
            else {
                sp = obj.GetComponentInChildren<UISprite>();
                if (sp!=null)
                {
                    tb.Size = new Vector2(sp.width , sp.height);
                }
            }
        }
    }

    private TipBind _tag;
    private void OnEnable()
    {
        _tag = (TipBind)target;   
    }
    private void OnSceneGUI()
    {
        
        Handles.Label(_tag.TextPos,"新手引导ID:" + _tag.Id);
    }

}


