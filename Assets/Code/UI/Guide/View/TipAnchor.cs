using UnityEngine;
using System.Collections;
using model;
using manager;

/// <summary>
/// 用于TIP自适应
/// </summary>
public class TipAnchor : MonoBehaviour {

    private const int Offset = 80;          //TIP对绑定目标的偏移
    private const int ARROW_X = 10;
    private const int ARROW_Y = 10;
    private const float TwOffset = 3f;         //运动偏移
    private const int MASKSIZE = 2000;      //遮挡大小
    private UILabel txt;
    private UISprite bg;
    private UISprite arrow;
    private TweenPosition tween;
    private UISprite boder;
    private UISprite nextBoder;
    public void SetBindData(TipBind data,GuideVo vo, bool isMask=false)
    {
        SetAnchor(vo.Tip, vo.Anchor, data.transform.localPosition, data.Size);
        if (isMask)
        {
            SetBoxMask((int)data.Size.x, (int)data.Size.y);
        }
        if (vo.Enforce)
        {
            SetBoder(data.Size);
        }
        else {
            HideBoder();
        }
        GetComponent<UIPanel>().depth = FindDepth(this.gameObject.transform.parent.gameObject) + 1;
    }

    private int FindDepth(GameObject obj)
    {
        UIPanel panel=obj.GetComponent<UIPanel>();
        if (panel == null)
        {
            return FindDepth(obj.transform.parent.gameObject);
        }
        else {
            return panel.depth;
        }
    }

    private void SetBoder(Vector2 size)
    {
        boder = transform.FindChild("Boder").GetComponent<UISprite>();
        nextBoder = transform.FindChild("BoderNext").GetComponent<UISprite>();
        boder.gameObject.SetActive(true);
        nextBoder.gameObject.SetActive(true);

        boder.width =nextBoder.width=(int)size.x;
        boder.height = nextBoder.height = (int)size.y;

    }
    private void HideBoder()
    {
        boder = transform.FindChild("Boder").GetComponent<UISprite>();
        nextBoder = transform.FindChild("BoderNext").GetComponent<UISprite>();
        boder.gameObject.SetActive(false);
        nextBoder.gameObject.SetActive(false);
    }

    //设置布局
    private void SetAnchor(string content, TextAnchor anchor, Vector3 tagPos, Vector2 size)
    {
        txt = transform.FindChild("Offset/Label").GetComponent<UILabel>();
        bg = transform.FindChild("Offset/Back").GetComponent<UISprite>();
        arrow = transform.FindChild("Offset/Arrow").GetComponent<UISprite>();
        tween = transform.FindChild("Offset").GetComponent<TweenPosition>();
        
        txt.text = content;
        int width = bg.width = txt.width + bg.minWidth * 2;
        int height = bg.height = txt.height + bg.minHeight;
        int lesswidth = width / 2 - ARROW_X;
        int lessheight = height / 2 - ARROW_Y;

        //反过来显示箭头
        switch (anchor)
        {
            case TextAnchor.LowerCenter://下
                SetLabel(arrow, new Vector2(0, lessheight), 270);
                break;
            case TextAnchor.LowerLeft:
                SetLabel(arrow, new Vector2(lesswidth, lessheight), 225);
                break;
            case TextAnchor.LowerRight:
                SetLabel(arrow, new Vector2(-lesswidth, lessheight), 315);
                break;
            case TextAnchor.MiddleCenter:
                Debug.LogError("新手引导，布局不能为Center");
                break;
            case TextAnchor.MiddleLeft:
                SetLabel(arrow, new Vector2(lesswidth, 0), 180);
                break;
            case TextAnchor.MiddleRight:
                SetLabel(arrow, new Vector2(-lesswidth, 0), 0);
                break;
            case TextAnchor.UpperCenter:
                SetLabel(arrow, new Vector2(0, -lessheight),90);
                break;
            case TextAnchor.UpperLeft:
                SetLabel(arrow, new Vector2(lesswidth, -lessheight), 135);
                break;
            case TextAnchor.UpperRight:
                SetLabel(arrow, new Vector2(-lesswidth, -lessheight), 45);
                break;
            default:
                break;
        }

        transform.localPosition = tagPos;

        //对目标的偏移坐标
        int offWidth = ((int)size.x >> 1) + Offset+lesswidth;
        int offHeight = ((int)size.y >> 1) + Offset + lessheight;
        Vector3 offsetv3 = new Vector3();
        switch (anchor)
        {
            case TextAnchor.LowerCenter:
                offsetv3.y = -offHeight;
                break;
            case TextAnchor.LowerLeft:
                offsetv3.x = -offWidth;
                offsetv3.y = -offHeight;
                break;
            case TextAnchor.LowerRight:
                offsetv3.x = offWidth;
                offsetv3.y = -offHeight;
                break;
            case TextAnchor.MiddleCenter:
                break;
            case TextAnchor.MiddleLeft:
                offsetv3.x = -offWidth;
                break;
            case TextAnchor.MiddleRight:
                offsetv3.x = offWidth;
                break;
            case TextAnchor.UpperCenter:
                offsetv3.y = offHeight;
                break;
            case TextAnchor.UpperLeft:
                offsetv3.x = -offWidth;
                offsetv3.y = offHeight;
                break;
            case TextAnchor.UpperRight:
                offsetv3.x = offWidth;
                offsetv3.y = offHeight;
                break;
            default:
                break;
        }
        transform.Find("Offset").localPosition = offsetv3;
        tween.ResetToBeginning();
        Vector3 normal = offsetv3;
        normal.Normalize();
        Vector3 from = normal.normalized*TwOffset+offsetv3;

        tween.from = from;
        tween.to = offsetv3;
        tween.PlayForward();
    }


    private void SetLabel(UISprite sp, Vector2 pos, float rotation)
    {
        sp.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        sp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

    private void SetBoxMask(int width,int height)
    {
        
        int lessSize=MASKSIZE>>1;
        width /= 2;
        height /= 2;
        //left
        CreateBox("left",-(lessSize + width), 0);

        //right
        CreateBox("right", lessSize + width, 0);

        //top
        CreateBox("top", 0, lessSize + height, width*2,MASKSIZE);

        //bottom
        CreateBox("bottom", 0, -(lessSize + height),width*2,MASKSIZE);
    }

    private void CreateBox(string obj,int x,int y)
    {
        GameObject sp = gameObject.transform.Find(obj).gameObject;
        sp.SetActive(true);

        UISprite sprite = sp.GetComponent<UISprite>();
        sprite.width = MASKSIZE;
        sprite.height = MASKSIZE;
        sp.transform.localPosition=new Vector3(x,y,0);
    }

    private void CreateBox(string obj, int x, int y,int w,int h)
    {
        GameObject sp = gameObject.transform.Find(obj).gameObject;
        sp.SetActive(true);

        UISprite sprite = sp.GetComponent<UISprite>();
        sprite.width = w;
        sprite.height = h;
        sp.transform.localPosition = new Vector3(x, y, 0);
    }
}
