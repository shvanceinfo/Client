using UnityEngine;
using System.Collections;
using helper;
using System.Collections.Generic;

public class MazeView :HelperMono {

    private GameObject _road;
    private Transform _root;
    private static int WIDTH;
    private static int HEIGHT;
    private const float TWEENTIME = 0.15f;
    private const string START = "migong_qidian";
    private const string END = "migong_zhongdian";
    private const string BOX = "migong_baoxiang_guan";
    private void Awake()
    {
        WIDTH = F<UISprite>("road/tween/back").width;
        HEIGHT = F<UISprite>("road/tween/back").height;
        _road = F("road");
        _road.SetActive(false);
        _root = F("root").transform;
    }

    private void CreateRoad(int row, int col,string icon,bool debug=true)
    {
        _road.SetActive(true);
        GameObject obj = Instantiate(_road) as GameObject;
        obj.transform.parent = _root;
        obj.name = string.Format("{0},{1}", row, col);
        obj.transform.localPosition = new Vector3((WIDTH >> 1) * (col - row), -(HEIGHT >> 1) * (col + row), 0);
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        UILabel lbl = obj.GetComponentInChildren<UILabel>();
        UITexture txt = obj.F<UITexture>("tween/icon");
        lbl.text = obj.name;
        lbl.alpha = debug ? 1 : 0;

        lbl.depth = row + col + 3;
        txt.depth = row + col + 2;
        obj.F<UISprite>("tween/mask").depth = row + col + 1;
        obj.F<UISprite>("tween/back").depth = row + col;

        txt.mainTexture = SourceManager.Instance.getTextByIconName(icon);
        txt.MakePixelPerfect();
        txt.width=txt.width > 70 ? 70 : txt.width;
        txt.height = txt.height > 70 ? 70 : txt.height;
        TweenPosition pos= obj.F<TweenPosition>("tween");
        pos.ResetToBeginning();
        pos.PlayForward();
        _road.SetActive(false);
    }

    


    private IEnumerator DrawRoads(List<Road> list)
    {
        List<Road>.Enumerator it = list.GetEnumerator();
        while (it.MoveNext())
        {
            string icon = "";
            switch (it.Current.Type)
            {
                    
                case RoadType.Normal:
                    icon = "Gem3LV2";
                    break;
                case RoadType.Start:
                    icon = START;
                    break;
                case RoadType.End:
                    icon = END;
                    break;
                case RoadType.Select:
                    break;
                case RoadType.Box:
                    icon = BOX;
                    break;
                default:
                    break;
            }
            CreateRoad(it.Current.X, it.Current.Y, icon,false);
            yield return new WaitForSeconds(TWEENTIME);
        }
    }

    public enum RoadType
    { 
        Normal,
        Start,
        End,
        Select,
        Box
    }
    public class Road
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string IconName { get; set; }
        public RoadType Type { get; set; }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Draw"))
        {
            StartCoroutine(DrawRoads(new List<Road> { 
            new Road{X=10, Y=10,Type=RoadType.Start},
            new Road{X=9, Y=10,Type=RoadType.Normal},
            new Road{X=8, Y=10,Type=RoadType.Normal},
            new Road{X=1, Y=10,Type=RoadType.End},
            }));
            
        }
    }
}
