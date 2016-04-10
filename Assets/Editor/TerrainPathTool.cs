using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Grid
{
    public Grid(int id, int x, int z, int tl, int bl, int br, int tr)
    {
        m_nID = id;
        m_nIndexX = x;
        m_nIndexZ = z;
        m_nTopLeft = tl;
        m_nBottomLeft = bl;
        m_nBottomRight = br;
        m_nTopRight = tr;
    }

    public void SetValue(bool value)
    {
        m_byteValue = value ? (byte)1 : (byte)0;
    }

    public int[] GetTri()
    {
        int[] ret = new int[6];

        ret[0] = m_nTopLeft;
        ret[1] = m_nBottomRight;
        ret[2] = m_nTopRight;
        ret[3] = m_nTopLeft;
        ret[4] = m_nBottomLeft;
        ret[5] = m_nBottomRight;


        return ret;
    }

    public int m_nID;

    public int m_nIndexX;
    public int m_nIndexZ;

    // all index into map vertex array
    public int m_nTopLeft;
    public int m_nBottomLeft;
    public int m_nBottomRight;
    public int m_nTopRight;

    public byte m_byteValue;
}

public class TerrainPathTool : EditorWindow
{
    // width height size of map
    private static int m_nMapWidth = 0;
    private static int m_nMapHeight = 0;

    private static bool m_bElementGrid = false;

    // row column num
    private static int m_nRowNum = 0; // 一行中多少列
    private static int m_nColumnNum = 0; // 一列中多少行

    private static int m_nGetFromInputUnitSize = 1;
    private static float m_fUnitSize = 0.0f;
    private static float m_fUniformHeight = 8.0f;

    static GameObject m_kMeshPoly = null;

    // line data
    static GameObject m_kDummyObj = null;
    static List<GameObject> m_listMapGridLineObjs = null;
    static LineRenderer lineRenderer = null;


    // total info
    static Dictionary<int, Dictionary<int, Grid>> m_dirGrid = null;
    static Vector3[] m_arrayMapVertex = null;


    static Vector3[] m_arrayMapNormal = null;

    static Vector2[] m_arrayMapUV = null;


    //static List<Vector3> m_listMapVext;

    


    // real brushed grid vertex list
    static HashSet<int> m_HashSetRealGridVertex = null;

    // real brushed grid tri list
    static List<int> m_listRealGridTri = null;



    
    // grid graphic
    static Mesh m_kGridMesh = null;



    // output data
    static int[] m_listTerrainInfo = null;


    // init window
    static bool m_bInitialized = false;


    // current map info
    static UInt32 m_nMapID = 0;
    static UInt32 m_nMapVer = 0;

    static string m_strMapName = "";

    const int m_nMaxMapName = 20;

    // for test
    static int m_nStartIndex = -1;
    static int m_nDestIndex = -1;
    [MenuItem("Window/" + "Terrain Path Tool")]
    public static void Init()
    {
        // Create Data Containers
        m_kMeshPoly = new GameObject();
        m_listMapGridLineObjs = new List<GameObject>();
        m_HashSetRealGridVertex = new HashSet<int>();
        m_listRealGridTri = new List<int>();
        m_kGridMesh = new Mesh();
        m_dirGrid = new Dictionary<int, Dictionary<int, Grid>>();

        
        // Reset Data
        ResetData();

        // Set Unity Editor Menu
        TerrainPathTool window = GetWindow<TerrainPathTool>();
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        m_bInitialized = true;
    }


    public static void ResetData()
    {
        if (m_nGetFromInputUnitSize == 0)
        {
            return;
        }

        if (m_nGetFromInputUnitSize < 0)
        {
            m_nRowNum = m_nMapWidth * Math.Abs(m_nGetFromInputUnitSize);
            m_nColumnNum = m_nMapHeight * Math.Abs(m_nGetFromInputUnitSize);

            m_fUnitSize = 1.0f / Math.Abs(m_nGetFromInputUnitSize);
        }
        else
        {
            m_nRowNum = m_nMapWidth / m_nGetFromInputUnitSize;
            m_nColumnNum = m_nMapHeight / m_nGetFromInputUnitSize;

            m_fUnitSize = (float)m_nGetFromInputUnitSize;
        }

        // clear map grid line
        DestroyMapGridLines();
        CreateMapGridLines();

        // clear map grid
        DestroyGridsMesh();
        CreateGridMesh();

        // clear map data
        DestroyMapData();
        CreateMapData();
        


        // generate map vext
        DestroyMapVertex();
        GenerateMapVertex();


        // clear render data
        DrawGridMesh();
    }

    void OnGUI()
    {
        if (!m_bInitialized)
        {
            return;
        }

        // UI setting
        GUILayout.Label("基本设置", EditorStyles.boldLabel);

        int nPreWidth = m_nMapWidth;
        int nPreHeight = m_nMapHeight;
        int nPreGetUnit = m_nGetFromInputUnitSize;
        bool bPreElement = m_bElementGrid;

        m_nMapWidth = EditorGUILayout.IntField("地图宽度(偶数)", m_nMapWidth);
        m_nMapHeight = EditorGUILayout.IntField("地图高度(偶数)", m_nMapHeight);
        m_strMapName = EditorGUILayout.TextField("地图名称", m_strMapName);
        m_nGetFromInputUnitSize = EditorGUILayout.IntField("单位长度 负数为小数", m_nGetFromInputUnitSize);

        m_bElementGrid = EditorGUILayout.Toggle("消除", m_bElementGrid);



        if (m_nMapWidth != nPreWidth ||
            m_nMapHeight != nPreHeight ||
            m_nGetFromInputUnitSize != nPreGetUnit && m_nGetFromInputUnitSize != 0)
        {
            //Debug.Log("重新");
            //OnDestroy();

            ResetData();

            return;
        }

        if (bPreElement != m_bElementGrid)
        {
            
        }

        if (GUILayout.Button("导出寻路/阻挡信息"))
        {
            ExportMapGridData();
            return;
        }


        if (GUILayout.Button("导入寻路/阻挡信息"))
        {
            ImportMapGridData();
            return;
        }


        
        

        // draw GUI
        DrawMapGridLines();

        if (m_listRealGridTri.Count != 0)
        {
            DrawGridMesh();
        }
    }


    void OnDestroy()
    {
        m_nStartIndex = -1;
        m_nDestIndex = -1;

        DestroyMapGridLines();
        DestroyGridsMesh();
        DestroyMapVertex();

        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    public static void CreateMapGridLines()
    {
        if (m_kDummyObj == null)
        {
            m_kDummyObj = new GameObject();
            m_kDummyObj.name = "线条容器";
        }

    }

    public static void DestroyMapGridLines()
    {
        if (m_kDummyObj)
        {
            m_kDummyObj.transform.DetachChildren();
        }
        
        if (m_listMapGridLineObjs != null && m_listMapGridLineObjs.Count != 0)
        {
            for (int i = 0; i < m_listMapGridLineObjs.Count; ++i)
            {
                DestroyImmediate(m_listMapGridLineObjs[i]);
                m_listMapGridLineObjs[i] = null;
            }
        }

        if (m_kDummyObj)
        {
            DestroyImmediate(m_kDummyObj);
            m_kDummyObj = null;
        }
    }


    public static void CreateGridMesh()
    {
        if (m_kMeshPoly == null)
        {
            m_kMeshPoly = new GameObject();
            m_kMeshPoly.name = "Mesh 容器";
        }


        m_kMeshPoly.AddComponent<MeshFilter>();
        m_kMeshPoly.AddComponent<MeshRenderer>();
        m_kMeshPoly.GetComponent<MeshRenderer>().castShadows = false;
        //m_kMeshPoly.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
        //Material mat = new Material(m_kMeshPoly.GetComponent<MeshRenderer>().sharedMaterial);
        //mat.color = Color.red;
        //m_kMeshPoly.GetComponent<MeshRenderer>().sharedMaterial = mat;


        //Material lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
        //        "Properties { _Color (\"Main Color\", Color) = (1,0,0,1)}}");

        //lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        //lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        //lineMaterial.color = Color.yellow;

        //m_kMeshPoly.GetComponent<MeshRenderer>().material = lineMaterial;



        m_kMeshPoly.GetComponent<MeshFilter>().mesh = m_kGridMesh;
    }

    public static void DestroyGridsMesh()
    {
        if (m_kMeshPoly)
        {
            DestroyImmediate(m_kMeshPoly);
            m_kMeshPoly = null;
        }
    }

    public static void CreateMapData()
    {
        if (m_arrayMapVertex == null)
        {
            m_arrayMapVertex = new Vector3[(m_nRowNum + 1) * (m_nColumnNum + 1)];
        }

        if (m_arrayMapNormal == null)
        {
            m_arrayMapNormal = new Vector3[(m_nRowNum + 1) * (m_nColumnNum + 1)];
        }

        if (m_arrayMapUV == null)
        {
            m_arrayMapUV = new Vector2[(m_nRowNum + 1) * (m_nColumnNum + 1)];
        }
    }

    public static void DestroyMapData()
    {
        m_arrayMapVertex = null;

        m_arrayMapNormal = null;

        m_arrayMapUV = null;
    }


    public static void DestroyMapVertex()
    {
        if (m_dirGrid != null)
        {
            m_dirGrid.Clear();
        }
        
        if (m_HashSetRealGridVertex != null)
        {
            m_HashSetRealGridVertex.Clear();
        }
        
        if (m_listRealGridTri != null)
        {
            m_listRealGridTri.Clear();
        }

        m_listTerrainInfo = null;
    }

    public static void GenerateMapVertex()
    {
        for (int j = 0; j <= m_nColumnNum; ++j)
        {
            for (int i = 0; i <= m_nRowNum; ++i)
            {
                float x = -m_nMapWidth / 2 +  i * m_fUnitSize;
                float y = m_fUniformHeight;
                float z = m_nMapHeight / 2 - j * m_fUnitSize;

                int nArrayIndex = j * (m_nRowNum + 1) + i;
                


                m_arrayMapVertex[nArrayIndex] = new Vector3(x, y, z);
                m_arrayMapNormal[nArrayIndex] = new Vector3(0.0f, 1.0f, 0.0f);
                m_arrayMapUV[nArrayIndex] = new Vector2(1.0f, 1.0f);

                if (i != 0 && j != 0)
                {
                    // we have a grid
                    int id = (j - 1) * m_nRowNum + (i - 1);

                    //Debug.Log("ver " + id + " " + x + " " + z);

                    int indexX = 0;
                    int indexZ = 0;

                    int nAreaLenWidth = m_nRowNum / 2;
                    int nAreaLenHeight = m_nColumnNum / 2;

                    if (j <= nAreaLenHeight)
                    {
                        if (i <= nAreaLenWidth)
                        {
                            // 左上
                            
                            indexX = i - nAreaLenWidth - 1;
                            indexZ = nAreaLenHeight - j + 1;

                            //indexX = 0;
                            //indexZ = 22222;
                            
                        }
                        else if (i > nAreaLenWidth)
                        {
                            // 右上
                            indexX = i - nAreaLenWidth;
                            indexZ = nAreaLenHeight - j + 1;

                            //indexX = 1;
                            //indexZ = 11;
                        }
                    }
                    else
                    {
                        if (i <= nAreaLenWidth)
                        {
                            // 左下
                            indexX = i - nAreaLenWidth - 1;
                            indexZ = nAreaLenHeight - j;

                            //indexX = 2;
                            //indexZ = 22;
                        }
                        else if (i > nAreaLenWidth)
                        {
                            // 右下
                            indexX = i - nAreaLenWidth;
                            indexZ = nAreaLenHeight - j;

                            //indexX = 3;
                            //indexZ = 33;
                        }
                    }





                    int tlIndex = (j - 1) * (m_nRowNum + 1) + (i - 1);
                    int blIndex = j * (m_nRowNum + 1) + (i - 1);
                    int brIndex = j * (m_nRowNum + 1) + i;
                    int trIndex = (j - 1) * (m_nRowNum + 1) + i;


                    //Debug.Log("index " + tlIndex + " " + blIndex + " " + brIndex + " " + trIndex);


                    if (m_dirGrid.ContainsKey(indexX))
                    {
                        if (!m_dirGrid[indexX].ContainsKey(indexZ))
                        {
                            m_dirGrid[indexX].Add(indexZ, new Grid(id, indexX, indexZ, tlIndex, blIndex, brIndex, trIndex));
                        }
                    }
                    else
                    {
                        Dictionary<int, Grid> subDic = new Dictionary<int, Grid>();

                        subDic.Add(indexZ, new Grid(id, indexX, indexZ, tlIndex, blIndex, brIndex, trIndex));

                        m_dirGrid.Add(indexX, subDic);
                    }
                }
            }
        }

        m_listTerrainInfo = new int[m_nRowNum * m_nColumnNum];
    }


    private static void OnSceneGUI(SceneView sceneview)
    {
        if (Event.current.type == EventType.MouseDrag)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(worldRay, out hitInfo))
            {

                //Undo.RegisterUndo(target, "Add Path Node");

                //((Path)target).AddNode(hitInfo.point);
                //Debug.Log(hitInfo.point);

                //Gizmos.DrawLine(Vector3.zero, Vector3.one);

                if (!m_bElementGrid)
                {
                    BrushGrid(hitInfo.point);
                }
                else
                    DeleteGrid(hitInfo.point);
                

            }

            Event.current.Use();
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(worldRay, out hitInfo))
            {
                int nIndexX = 0;
                int nIndexZ = 0;

                if (HitPointToIndex(hitInfo.point, out nIndexX, out nIndexZ))
                {
                    if (m_nStartIndex == -1 && m_nDestIndex == -1)
                    {
                        m_nStartIndex = XZLocationToIndex(nIndexX, nIndexZ);
                        EditorUtility.DisplayDialog("起始点: " + m_nStartIndex, "点击鼠标设置目标点", "OK");
                    }
                    else if (m_nStartIndex != -1 && m_nDestIndex == -1)
                    {
                        m_nDestIndex = XZLocationToIndex(nIndexX, nIndexZ);
                        EditorUtility.DisplayDialog("目标点: " + m_nDestIndex, "开始寻路", "OK");

                        AISystem.AStarAlgorithm.GetInstance().ParseMapPathFile("c:\\MapPathInfo.dat");
                        AISystem.AStarAlgorithm.GetInstance().FindPath(m_nStartIndex, m_nDestIndex);

                        for (int i = 0; i < AISystem.AStarAlgorithm.GetInstance().m_kFoundPath.Count; ++i )
                        {
                            Debug.Log("路点 " + AISystem.AStarAlgorithm.GetInstance().m_kFoundPath[i]);
                                }
                            }
                    else if (m_nStartIndex != -1 && m_nDestIndex != -1)
                    {
                        m_nStartIndex = -1;
                        m_nDestIndex = -1;
                        EditorUtility.DisplayDialog("重置路点 ", "", "OK");
                    }
                }
                
                Event.current.Use();
            }
        }
    }



    public static void DrawGridMesh()
    {
        //Vector3[] vertex = new Vector3[m_HashSetRealGridVertex.Count];

        m_kGridMesh.vertices = m_arrayMapVertex;
        
        int[] triInfo = new int[m_listRealGridTri.Count];
        for (int i = 0; i < m_listRealGridTri.Count; ++i )
        {
            triInfo[i] = m_listRealGridTri[i];
        }
        m_kGridMesh.triangles = triInfo;

        m_kGridMesh.normals = m_arrayMapNormal;

        m_kGridMesh.uv = m_arrayMapUV;

        
        //Material polyMaterial = new Material(Shader.Find("Diffuse"));
        //polyMaterial.SetColor("_Color", Color.red);

        //m_kMeshPoly.GetComponent<MeshRenderer>().material = polyMaterial;

        //m_kMeshPoly.GetComponent<MeshRenderer>().receiveShadows = false;
    }


    public void DrawMapGridLines()
    {
        for (int i = 0; i <= m_nRowNum; ++i)
        {
            // top point 
            Vector3 topPt = new Vector3(-m_nMapWidth / 2 + i * m_fUnitSize, m_fUniformHeight, m_nMapHeight / 2);
            // bottom point
            Vector3 bottomPt = new Vector3(-m_nMapWidth / 2 + i * m_fUnitSize, m_fUniformHeight, -m_nMapHeight / 2);

            GameObject linObj = new GameObject();
            LineRenderer line = linObj.AddComponent<LineRenderer>();
            //line.material = new Material(Shader.Find("Diffuse"));
            line.SetColors(Color.yellow, Color.red);
            line.SetWidth(0.01f, 0.05f);
            line.SetVertexCount(2);
            line.SetPosition(0, topPt);
            line.SetPosition(1, bottomPt);

            m_listMapGridLineObjs.Add(linObj);

            linObj.transform.parent = m_kDummyObj.transform;
        }

        for (int j = 0; j <= m_nColumnNum; ++j)
        {
            // left point 
            Vector3 leftPt = new Vector3(-m_nMapWidth / 2, m_fUniformHeight, -m_nMapHeight / 2 + j * m_fUnitSize);

            // right point
            Vector3 rightPt = new Vector3(m_nMapWidth / 2, m_fUniformHeight, -m_nMapHeight / 2 + j * m_fUnitSize);


            GameObject linObj = new GameObject();
            LineRenderer line = linObj.AddComponent<LineRenderer>();
            //line.material = new Material(Shader.Find("Diffuse"));
            line.SetColors(Color.yellow, Color.red);
            line.SetWidth(0.01f, 0.05f);
            line.SetVertexCount(2);
            line.SetPosition(0, leftPt);
            line.SetPosition(1, rightPt);

            m_listMapGridLineObjs.Add(linObj);

            linObj.transform.parent = m_kDummyObj.transform;
        }
    }

    public static void BrushGrid(Vector3 hitPt)
    {
        int nIndexX = 0;
        int nIndexZ = 0;

        if (!HitPointToIndex(hitPt, out nIndexX, out nIndexZ))
        {
            return;
        }
        
        //Debug.Log(" brush " + nIndexX + " " + nIndexZ);

        if (m_dirGrid.ContainsKey(nIndexX))
        {
            if (m_dirGrid[nIndexX].ContainsKey(nIndexZ))
            {
                m_HashSetRealGridVertex.Add(m_dirGrid[nIndexX][nIndexZ].m_nTopLeft);
                m_HashSetRealGridVertex.Add(m_dirGrid[nIndexX][nIndexZ].m_nBottomLeft);
                m_HashSetRealGridVertex.Add(m_dirGrid[nIndexX][nIndexZ].m_nBottomRight);
                m_HashSetRealGridVertex.Add(m_dirGrid[nIndexX][nIndexZ].m_nTopRight);

                //Debug.Log("vertex " + m_dirGrid[nIndexX][nIndexZ].m_nTopLeft + " " + m_arrayMapVertex[m_dirGrid[nIndexX][nIndexZ].m_nTopLeft]);
                //Debug.Log("vertex " + m_dirGrid[nIndexX][nIndexZ].m_nBottomLeft + " " + m_arrayMapVertex[m_dirGrid[nIndexX][nIndexZ].m_nBottomLeft]);
                //Debug.Log("vertex " + m_dirGrid[nIndexX][nIndexZ].m_nBottomRight + " " + m_arrayMapVertex[m_dirGrid[nIndexX][nIndexZ].m_nBottomRight]);
                //Debug.Log("vertex " + m_dirGrid[nIndexX][nIndexZ].m_nTopRight + " " + m_arrayMapVertex[m_dirGrid[nIndexX][nIndexZ].m_nTopRight]);



                foreach (int triValue in m_dirGrid[nIndexX][nIndexZ].GetTri())
                {
                    //Debug.Log("mmm " + triValue);
                    m_listRealGridTri.Add(triValue);
                }
                
                m_dirGrid[nIndexX][nIndexZ].SetValue(true);

                //Debug.Log("有网格" + nIndexX + " " + nIndexZ + " id " + m_dirGrid[nIndexX][nIndexZ].m_nID);
            }
        }

        DrawGridMesh();
    }



    public static void DeleteGrid(Vector3 hitPt)
    {
        if (hitPt.x > m_nMapWidth * 0.5f ||
            hitPt.x < -m_nMapWidth * 0.5f ||
            hitPt.z > m_nMapHeight * 0.5f ||
            hitPt.z < -m_nMapHeight * 0.5f)
        {
            //Debug.Log("return");
            return;
        }

        int nIndexX = 0;
        int nIndexZ = 0;

        if (hitPt.x > 0.0f)
        {
            nIndexX = (int)(hitPt.x / m_fUnitSize) + 1;
            //Debug.Log(" 111111 " + nIndexX + " " + hitPt.x);
        }
        else
            nIndexX = (int)(hitPt.x / m_fUnitSize) - 1;


        if (hitPt.z > 0.0f)
        {
            nIndexZ = (int)(hitPt.z / m_fUnitSize) + 1;
        }
        else
            nIndexZ = (int)(hitPt.z / m_fUnitSize) - 1;

        if (m_dirGrid.ContainsKey(nIndexX))
        {
            if (m_dirGrid[nIndexX].ContainsKey(nIndexZ))
            {
                m_dirGrid[nIndexX][nIndexZ].SetValue(false);
            }
        }

        m_listRealGridTri.Clear();

        // regenerate triangles
        foreach (KeyValuePair<int, Dictionary<int, Grid>> item in m_dirGrid)
        {
            foreach (KeyValuePair<int, Grid> value in item.Value)
            {
                if (value.Value.m_byteValue == 1)
                {
                    foreach (int triValue in value.Value.GetTri())
                    {
                        m_listRealGridTri.Add(triValue);
                    }
                }
            }
        }

        DrawGridMesh();
    }


    public static void ExportMapGridData()
    {
        // check 
        if (m_nColumnNum % 2 != 0 || m_nRowNum % 2 != 0 || m_nColumnNum == 0 || m_nRowNum == 0)
        {
            EditorUtility.DisplayDialog("高度或者宽度需要偶数!", "", "确定");
            return;
        }

        if (m_nGetFromInputUnitSize == 0)
        {
            EditorUtility.DisplayDialog("单位长度不允许为0!", "", "确定");
            return;
        }

        if (m_strMapName == "")
        {
            EditorUtility.DisplayDialog("地图名不能为空!", "", "确定");
            return;
        }

        foreach (KeyValuePair<int, Dictionary<int, Grid> > item in m_dirGrid)
        {
            foreach (KeyValuePair<int, Grid> value in item.Value)
            {
                //if (value.Value.m_byteValue == 1)
                {

                    m_listTerrainInfo[value.Value.m_nID] = 1;
                }
            }
        }

        UInt32 mapID = 0;
        UInt32 mapVer = 1;

        char[] mapName = m_strMapName.ToCharArray();

        string strFileName = "c:\\";
        strFileName += m_strMapName;
        strFileName += ".txt";

        BinaryWriter writer = new BinaryWriter(File.Open(strFileName, FileMode.Create));
        writer.Write(mapID);
        writer.Write(mapVer);

        for (int i = 0; i < m_nMaxMapName; ++i)
        {
            if (i < mapName.Length)
            {
                writer.Write(mapName[i]);
            }
            else
                writer.Write(' ');
        }

        writer.Write(m_nRowNum);
        writer.Write(m_nColumnNum);
        writer.Write(m_nMapWidth);
        writer.Write(m_nMapHeight);
        writer.Write(m_fUnitSize);

        for (int i = 0; i < m_nRowNum * m_nColumnNum; ++i)
        {
            byte value = (byte)m_listTerrainInfo[i];
            writer.Write(value);
        }
        writer.Close();

        EditorUtility.DisplayDialog("导出成功!", "", "确定");
    }



    public static void ImportMapGridData()
    {
        string strFileName = "c:\\";
        strFileName += m_strMapName;
        strFileName += ".txt";

        BinaryReader reader = new BinaryReader(File.Open(strFileName, FileMode.Open));

        m_nMapID = reader.ReadUInt32();
        m_nMapVer = reader.ReadUInt32();

        char[] mapName = new char[m_nMaxMapName];

        for (int i = 0; i < m_nMaxMapName; ++i )
        {
            mapName[i] = reader.ReadChar();
        }

        m_strMapName = new string(mapName);

        m_nRowNum = reader.ReadInt32();
        m_nColumnNum = reader.ReadInt32();
        m_nMapWidth = reader.ReadInt32();
        m_nMapHeight = reader.ReadInt32();
        m_fUnitSize = reader.ReadSingle();


        // clear map grid line
        DestroyMapGridLines();
        CreateMapGridLines();

        // clear map grid
        DestroyGridsMesh();
        CreateGridMesh();

        // clear map data
        DestroyMapData();
        CreateMapData();

        // generate map vext
        DestroyMapVertex();
        GenerateMapVertex();



        int nLeft = -(m_nRowNum / 2);
        int nRight = -nLeft;

        int nTop = m_nColumnNum / 2;
        int nBottom = -nTop;


        
        for (int j = nTop; j >= nBottom; --j)
        {
            if (j == 0)
            {
                continue;
            }

            for (int i = nLeft; i <= nRight; ++i)
            {
                if (i == 0)
                {
                    continue;
                }

                byte value = reader.ReadByte();

                if (m_dirGrid.ContainsKey(i))
                {
                    if (m_dirGrid[i].ContainsKey(j))
                    {
                        m_dirGrid[i][j].m_byteValue = value;

                        if (value == 1)
                        {
                            foreach (int triValue in m_dirGrid[i][j].GetTri())
                            {
                                //Debug.Log("mmm " + triValue);
                                m_listRealGridTri.Add(triValue);
                            }
                        }
                    }
                }
            }
        }

        reader.Close();

        DrawGridMesh();
    }


    public static bool HitPointToIndex(Vector3 hitPt, out int x, out int z)
    {
         if (hitPt.x > m_nMapWidth * 0.5f ||
                hitPt.x < -m_nMapWidth * 0.5f ||
                hitPt.z > m_nMapHeight * 0.5f ||
                hitPt.z < -m_nMapHeight * 0.5f)
            {
                x = 0;
                z = 0;
                return false;
            }

            int nIndexX = 0;
            int nIndexZ = 0;

            if (hitPt.x > 0.0f)
            {
                nIndexX = (int)(hitPt.x / m_fUnitSize) + 1;
            }
            else
                nIndexX = (int)(hitPt.x / m_fUnitSize) - 1;


            if (hitPt.z > 0.0f)
            {
                nIndexZ = (int)(hitPt.z / m_fUnitSize) + 1;
            }
            else
                nIndexZ = (int)(hitPt.z / m_fUnitSize) - 1;

            x = nIndexX;
            z = nIndexZ;
            return true;
    }

    public static int XZLocationToIndex(int nIndexX, int nIndexZ)
    {
        return m_dirGrid[nIndexX][nIndexZ].m_nID;
    }

    //public void DrawMapGrid()
    //{
    //    int nWidthNum = (int)(m_nMapWidth / m_fUnitSize);
    //    int nHeightNum = (int)(m_nMapHeight / m_fUnitSize);

    //    for (int i = 0; i <= nWidthNum; ++i)
    //    {
    //        // top point 
    //        Vector3 topPt = new Vector3(-m_nMapWidth / 2 + i * m_fUnitSize, m_fUniformHeight, m_nMapHeight / 2);
    //        // bottom point
    //        Vector3 bottomPt = new Vector3(-m_nMapWidth / 2 + i * m_fUnitSize, m_fUniformHeight, -m_nMapHeight / 2);

    //        //Gizmos.DrawLine(topPt, bottomPt);
    //        //Handles.DrawLine(topPt, bottomPt);
    //        Debug.DrawLine(topPt, bottomPt);
    //    }

    //    for (int j = 0; j <= nHeightNum; ++j)
    //    {
    //        // left point 
    //        Vector3 leftPt = new Vector3(-m_nMapWidth / 2, m_fUniformHeight, -m_nMapHeight / 2 + j * m_fUnitSize);

    //        // right point
    //        Vector3 rightPt = new Vector3(m_nMapWidth / 2, m_fUniformHeight, -m_nMapHeight / 2 + j * m_fUnitSize);

    //        //Gizmos.DrawLine(leftPt, rightPt);
    //        //Handles.DrawLine(leftPt, rightPt);
    //        Debug.DrawLine(leftPt, rightPt);
    //    }
    //}





    //[CustomEditor(typeof(GameObject))]
    //void OnSceneGUI()
    //{

    //    Debug.Log("adsfadsfasf");
    //    if (Event.current.type == EventType.MouseDown)
    //    {

    //        if (Event.current.button == 0)
    //        {

    //            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

    //            RaycastHit hitInfo;



    //            if (Physics.Raycast(worldRay, out hitInfo))
    //            {

    //                //Undo.RegisterUndo(target, "Add Path Node");

    //                //((Path)target).AddNode(hitInfo.point);
    //                Debug.Log(hitInfo.point);

    //            }



    //            Event.current.Use();

    //        }

    //    }
    //}



        //for (int i = 0; i < ((Path)target).nodes.Count; i++)

        //((Path)target).nodes[i] = Handles.PositionHandle(((Path)target).nodes[i], Quaternion.identity);



        //Handles.DrawPolyLine(((Path)target).nodes.ToArray());



        //if (GUI.changed)

        //    EditorUtility.SetDirty(target);


    //[DrawGizmo(GizmoType.Active)]
    //static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmotype)
    //{
    //    Debug.Log(objectTransform.name + "   " + gizmotype);
    //}

    
}
