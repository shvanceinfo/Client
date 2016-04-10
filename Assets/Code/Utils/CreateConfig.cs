///**该文件实现的基本功能等
//function: 根据预取件生成配置文件信息
//author:ljx
//date:2014-02-09
//**/
//using UnityEngine;
//using System.IO;
//using System.Text;

//public class CreateConfig : MonoBehaviour
//{
//#if UNITY_EDITOR
//    public string configPrefab = "";
//    public string createPath = "C:/monsterInstance.xml";

//    const int ROW_HEIGHT = 25;
//    const int MAX_ROW = 20;
//    const string DEFAULT_NAME = "NPC_instance";
//    const string RES_RELATIVE_PATH = "Assets/Resources/Model/prefab/BattlePref";
//    const string RES_PATH = "Model/prefab/BattlePref/";
//    const string TOWER_PATH = "tower/";
//    const string MULTI_PATH = "duoren/";
//    const string AREA_PREFIX = "MonsterArea";

//    private UILabel _progressLbl;
//    private UILabel _createFinish;
//    private FileStream _fs;
//    private StreamWriter _sw;
//    private int _counter;
//    private int _progressCount;
//    private int _normalID;
//    private int _eliteID;
//    private int _towerID;
//    private int _multiID;

//    void Awake()
//    {
//        _progressLbl = transform.Find("progress").GetComponent<UILabel>();
//        _createFinish = transform.Find("createFinish").GetComponent<UILabel>();
//        _createFinish.gameObject.SetActive(false);
//        _counter = 0;
//        _progressCount = 1;
//        _normalID = 1000000;
//        _eliteID = 2000000;
//        _towerID = 3000000;
//        _multiID = 4000000;
//        _fs = null;
//        _sw = null;
//    }

//    void Update()
//    {
//        if (_progressCount > 20)
//            _progressCount = 1;
//        _progressLbl.text = "";
//        for (int i = 0; i < _progressCount; i++)
//            _progressLbl.text += ".";
//        _progressCount++;
//    }

//    void Start()
//    {
//        if (configPrefab == "")
//        {
//            generateFile();
//            DirectoryInfo forder = new DirectoryInfo(RES_RELATIVE_PATH);
//            transferPrefab(forder, RES_PATH, "single");
//            forder = new DirectoryInfo(RES_RELATIVE_PATH + "/" + TOWER_PATH);
//            transferPrefab(forder, RES_PATH + TOWER_PATH, "300200010");
//            forder = new DirectoryInfo(RES_RELATIVE_PATH + "/" + MULTI_PATH);
//            transferPrefab(forder, RES_PATH + MULTI_PATH, "multi");
//            generateFile(null, true);
//        }
//        else
//        {

//        }
//    }

//    void transferPrefab(DirectoryInfo forder, string resPath, string mapName)
//    {
//        foreach (FileInfo file in forder.GetFiles())
//        {
//            if (file.Name.IndexOf(".prefab") == -1 || file.Name.IndexOf(".meta") != -1)
//                continue;
//            int prefabIndex = file.Name.IndexOf(".prefab");
//            string path = resPath + file.Name.Remove(prefabIndex);
//            GameObject obj = BundleMemManager.Instance.getPrefabByName(path, EBundleType.eBundleUI);
//            if (obj != null)
//            {
//                if (mapName == "single")
//                {
//                    string mission = "mission";  //找地图编号的前缀
//                    string normalPrexix = "200100";
//                    string elitePrefix = "200200";
//                    string normalSuffix = "0";
//                    string eliteSuffix = "1";
//                    int index1 = file.Name.IndexOf(mission) + mission.Length;
//                    int index2 = file.Name.IndexOf("_p");
//                    if (index2 == -1)
//                        index2 = file.Name.IndexOf("_P");
//                    if (index1 == -1 || index2 == -1 || index1 >= index2)
//                        mission = "mission";
//                    string mapNumber = file.Name.Substring(index1, index2 - index1);
//                    if (mapNumber.Length == 3)
//                    {
//                        normalPrexix = "20010";
//                        elitePrefix = "20020";
//                    }
//                    else if (mapNumber.Length == 2)
//                    {
//                        normalPrexix = "200100";
//                        elitePrefix = "200200";
//                    }
//                    else if (mapNumber.Length == 1)
//                    {
//                        normalPrexix = "2001000";
//                        elitePrefix = "2002000";
//                    }
//                    string normalMap = normalPrexix + mapNumber + normalSuffix;
//                    string eliteMap = elitePrefix + mapNumber + eliteSuffix;
//                    foreach (Transform child in obj.transform)
//                    {
//                        if (child.name.Contains(AREA_PREFIX))
//                        {
//                            MonsterArea area = child.GetComponent<MonsterArea>();
//                            if (area != null)
//                            {
//                                foreach (Transform subChild in child.transform)
//                                {
//                                    int templateID = 0;
//                                    switch (subChild.name)
//                                    {
//                                        case "m1":
//                                            templateID = area.monster1_template_id;
//                                            break;
//                                        case "m2":
//                                            templateID = area.monster2_template_id;
//                                            break;
//                                        case "m3":
//                                            templateID = area.monster3_template_id;
//                                            break;
//                                        case "m4":
//                                            templateID = area.monster4_template_id;
//                                            break;
//                                        default:
//                                            break;
//                                    }
//                                    if (templateID != 0)
//                                    {
//                                        _normalID++;
//                                        string xmlText1 = "<RECORD ID=\"" + _normalID.ToString() + "\" pref=\"Model/prefab/BattlePref/"
//                                            + obj.name + "\" area=\"" + child.name + "\" templateID=\"" + templateID.ToString() + "\" mapID=\"" + normalMap + "\"/>\n";
//                                        generateFile(xmlText1);
//                                        _eliteID++;
//                                        string xmlText2 = "<RECORD ID=\"" + _eliteID.ToString() + "\" pref=\"Model/prefab/BattlePref/"
//                                            + obj.name + "\" area=\"" + child.name + "\" templateID=\"" + (templateID + 100000).ToString() + "\" mapID=\"" + eliteMap + "\"/>\n";
//                                        generateFile(xmlText2);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                else if (mapName == "multi")
//                {
//                    string multiPrefix = "400100";
//                    string mission = "duoren_";
//                    string multiSuffix = "0";
//                    int index1 = file.Name.IndexOf(mission) + mission.Length;
//                    int index2 = file.Name.IndexOf(".prefab");
//                    string mapNumber = file.Name.Substring(index1, index2 - index1);
//                    if (mapNumber.Length == 3)
//                    {
//                        multiPrefix = "40010";
//                    }
//                    else if (mapNumber.Length == 2)
//                    {
//                        multiPrefix = "400100";
//                    }
//                    else if (mapNumber.Length == 1)
//                    {
//                        multiPrefix = "4001000";
//                    }
//                    string multiMap = multiPrefix + mapNumber + multiSuffix;
//                    foreach (Transform child in obj.transform)
//                    {
//                        if (child.name.Contains(AREA_PREFIX))
//                        {
//                            MonsterArea area = child.GetComponent<MonsterArea>();
//                            if (area != null)
//                            {
//                                foreach (Transform subChild in child.transform)
//                                {
//                                    int templateID = 0;
//                                    switch (subChild.name)
//                                    {
//                                        case "m1":
//                                            templateID = area.monster1_template_id;
//                                            break;
//                                        case "m2":
//                                            templateID = area.monster2_template_id;
//                                            break;
//                                        case "m3":
//                                            templateID = area.monster3_template_id;
//                                            break;
//                                        case "m4":
//                                            templateID = area.monster4_template_id;
//                                            break;
//                                        default:
//                                            break;
//                                    }
//                                    if (templateID != 0)
//                                    {
//                                        _multiID++;
//                                        string xmlText = "<RECORD ID=\"" + _multiID.ToString() + "\" pref=\"Model/prefab/BattlePref/duoren/"
//                                            + obj.name + "\" area=\"" + child.name + "\" templateID=\"" + templateID.ToString() + "\" mapID=\"" + multiMap + "\"/>\n";
//                                        generateFile(xmlText);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    MonsterArea area = obj.GetComponent<MonsterArea>();
//                    if (area != null)
//                    {
//                        foreach (Transform child in obj.transform)
//                        {
//                            int templateID = 0;
//                            switch (child.name)
//                            {
//                                case "m1":
//                                    templateID = area.monster1_template_id;
//                                    break;
//                                case "m2":
//                                    templateID = area.monster2_template_id;
//                                    break;
//                                case "m3":
//                                    templateID = area.monster3_template_id;
//                                    break;
//                                case "m4":
//                                    templateID = area.monster4_template_id;
//                                    break;
//                                default:
//                                    break;
//                            }
//                            if (templateID != 0)
//                            {
//                                _towerID++;
//                                string xmlText1 = "<RECORD ID=\"" + _towerID.ToString() + "\" pref=\"Model/prefab/BattlePref/tower/"
//                                    + obj.name + "\" templateID=\"" + templateID.ToString() + "\" mapID=\"" + mapName + "\"/>\n";
//                                generateFile(xmlText1);
//                                templateID += 10000;
//                                _towerID++;
//                                string xmlText2 = "<RECORD ID=\"" + _towerID.ToString() + "\" pref=\"Model/prefab/BattlePref/tower/"
//                                    + obj.name + "\" templateID=\"" + templateID.ToString() + "\" mapID=\"" + mapName + "\"/>\n";
//                                generateFile(xmlText2);
//                                templateID += 10000;
//                                _towerID++;
//                                string xmlText3 = "<RECORD ID=\"" + _towerID.ToString() + "\" pref=\"Model/prefab/BattlePref/tower/"
//                                    + obj.name + "\" templateID=\"" + templateID.ToString() + "\" mapID=\"" + mapName + "\"/>\n";
//                                generateFile(xmlText3);
//                            }
//                        }
//                    }
//                }

//            }
//        }
//    }

//    void generateFile(string text = "", bool end = false)
//    {
//        if (text == "")
//        {
//            FileStream fs = new FileStream(createPath, FileMode.Create);
//            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
//            string xmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\n<RECORDS>\n";
//            sw.Write(xmlHead);
//            sw.Close();
//            fs.Close();
//        }
//        else if (end)
//        {
//            if (_sw != null)
//            {
//                string xmlEnd = "</RECORDS>";
//                _sw.Write(xmlEnd);
//                _sw.Close();
//            }
//            if (_fs != null)
//                _fs.Close();
//            string newText = _createFinish.text.Replace("{0}", _counter.ToString());
//            _createFinish.text = newText;
//            _createFinish.gameObject.SetActive(true);
//            _progressLbl.gameObject.SetActive(false);
//        }
//        else
//        {
//            if (_fs == null)
//                _fs = new FileStream(createPath, FileMode.Append);
//            if (_sw == null)
//                _sw = new StreamWriter(_fs, Encoding.UTF8);
//            _sw.Write(text);
//            _counter++;
//        }
//    }
//#endif
//}
