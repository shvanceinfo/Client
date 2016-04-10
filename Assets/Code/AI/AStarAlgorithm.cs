using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AISystem
{
    public enum EGridStatus
    {
        GS_INIT = 0,
        GS_OPEN,
        GS_CLOSE,
    }

    public enum EGridDirect
    {
        GD_TOP_LEFT = 0,
        GD_TOP,
        GD_TOP_RIGHT,
        GD_RIGH,
        GD_BOTTOM_RIGHT,
        GD_BOTTOM,
        GD_BOTTOM_LEFT,
        GD_LEFT,
        GD_MAX,
    }

    public class AStarGrid
    {
        public int m_nID = 0;           // 节点ID
        public float m_fGoalCost = 0.0f;     // 移动到目的地开销
        public float m_fFinalCost = 0.0f;    // 预估的开销
        public bool m_bWalkable = false;    // 是否能行走
        public int m_nParentID = -1;        // 父节点ID
        public EGridStatus m_eStatus = EGridStatus.GS_INIT; // 记录节点状态 
        public int[] m_arrayNeighbor = null;

        public AStarGrid()
        {
            m_arrayNeighbor = new int[(int)EGridDirect.GD_MAX];

            for (int i = (int)EGridDirect.GD_TOP_LEFT; i < (int)EGridDirect.GD_MAX; ++i)
            {
                m_arrayNeighbor[i] = -1;
            }
        }

        public void Reset()
        {
            m_fGoalCost = 0.0f;
            m_fFinalCost = 0.0f;
            m_nParentID = -1;
            m_eStatus = EGridStatus.GS_INIT;
        }
    }

    public class AStarAlgorithm
    {
        private const UInt32 m_un32MaxMapName = 20;

        private int m_nRowNum = 0; // 一行中多少列 （宽度）
        private int m_nColumnNum = 0; // 一列中多少行 （高度）
        private float m_fUnitSize = 0.0f;
        private int m_nMapWidth = 0;
        private int m_nMapHeight = 0;
        private char[] m_kGridName = null;


        public Dictionary<int, int> m_kDicParentContainer = new Dictionary<int, int>();

        public AStarGrid[] m_arrayMapGridInfo = null;

        public HashSet<int> m_kOpenList = null;
        public HashSet<int> m_kCloseSet = null;

        public int m_nCurNodeID = -1;

        public List<int> m_kFoundPath = null;

        public List<Vector3> m_kFoundPointContainer = null;

        public List<GameObject> m_kWallObjs = new List<GameObject>();

        public List<int> m_kNeighborIndex = new List<int>();

        public enum EFindPathStatus
        {
            FPS_NONE = 0,
            FPS_FOUND_INIT,
            FPS_FOUND,
            FPS_START_NOT_WALKABLE,
            FPS_END_NOT_WALKABLE,
            FPS_NOT_FOUND,
        }

        static private AStarAlgorithm _instance = null;

        static public AStarAlgorithm GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AStarAlgorithm();
                return _instance;
            }

            return _instance;
        }

        // 寻路算法A*
        public EFindPathStatus FindPath(int nStart, int nTarget)
        {
            // 1. quick path checks: under some circumstances no path needs to be generated...
            // 2. assume these situations are not exist..
            if (!m_arrayMapGridInfo[nStart].m_bWalkable)
            {
                Debug.Log("PathFinding start not walkable");
                //return EFindPathStatus.FPS_START_NOT_WALKABLE;
            }

            if (!m_arrayMapGridInfo[nTarget].m_bWalkable)
            {
                //return EFindPathStatus.FPS_END_NOT_WALKABLE;
                Debug.Log("PathFinding end not walkable");
            }

            if (nStart == nTarget)
            {
                RecordPathPoint(nStart);
                return EFindPathStatus.FPS_FOUND_INIT;
            }

            // 2. init data 

            // clear close set at begin
            m_kCloseSet.Clear();
            m_kOpenList.Clear();
            m_kFoundPath.Clear();
            m_kFoundPointContainer.Clear();


            //for (int i = 0; i < m_nColumnNum; ++i)
            //{
            //    for (int j = 0; j < m_nRowNum; ++j)
            //    {
            //        m_arrayMapGridInfo[i * m_nRowNum + j].m_eStatus = EGridStatus.GS_INIT;
            //        m_arrayMapGridInfo[i * m_nRowNum + j].m_fFinalCost = 0;
            //        m_arrayMapGridInfo[i * m_nRowNum + j].m_fGoalCost = 0;
            //        m_arrayMapGridInfo[i * m_nRowNum + j].m_nParentID = -1;
            //    }
            //}

            m_kDicParentContainer.Clear();

            // add start node int open set
            m_kOpenList.Add(nStart);
            

            // 3. calculate cost value of each node
            m_arrayMapGridInfo[nStart].m_fGoalCost = 0;
            m_arrayMapGridInfo[nStart].m_fFinalCost = m_arrayMapGridInfo[nStart].m_fGoalCost + 
                HeuristicValue(nStart, nTarget);

            while (m_kOpenList.Count != 0)
            {
                m_nCurNodeID = FindLowestCostNodeInOpenSet();

                if (m_nCurNodeID == nTarget)
                {
                    ReconstructPath(m_nCurNodeID);

                    return EFindPathStatus.FPS_FOUND;
                }

                // remove current node from openset and add it to closeset
                m_kOpenList.Remove(m_nCurNodeID);
                m_kCloseSet.Add(m_nCurNodeID);

                // for each neighbor 
                for (int i = (int)EGridDirect.GD_TOP_LEFT; i < (int)EGridDirect.GD_MAX; ++i)
                {
                    int nNeighBor = m_arrayMapGridInfo[m_nCurNodeID].m_arrayNeighbor[i];

                    if (nNeighBor == -1)
                    {
                        continue;
                    }

                    if (!m_arrayMapGridInfo[nNeighBor].m_bWalkable)
                    {
                        continue;
                    }

                    // neighbor in closeset
                    if (m_kCloseSet.Contains(nNeighBor))
                    {
                        continue;
                    }

                    float nTenTativeCost = m_arrayMapGridInfo[m_nCurNodeID].m_fGoalCost +
                        DistBetween(m_nCurNodeID, nNeighBor);
                    
                    if (!m_kOpenList.Contains(nNeighBor) ||
                        nTenTativeCost < m_arrayMapGridInfo[nNeighBor].m_fGoalCost)
                    {
                        //m_arrayMapGridInfo[nNeighBor].m_nParentID = m_nCurNodeID;

                        if (!m_kDicParentContainer.ContainsKey(nNeighBor))
                        {
                            m_kDicParentContainer.Add(nNeighBor, m_nCurNodeID);
                        }
                        else
                        {
                            m_kDicParentContainer[nNeighBor] = m_nCurNodeID;
                        }
                        
                        m_arrayMapGridInfo[nNeighBor].m_fGoalCost = nTenTativeCost;
                        m_arrayMapGridInfo[nNeighBor].m_fFinalCost = m_arrayMapGridInfo[nNeighBor].m_fGoalCost +
                            HeuristicValue(nNeighBor, nTarget);

                        if (!m_kOpenList.Contains(nNeighBor))
                        {
                            m_kOpenList.Add(nNeighBor);
                        }
                    }
                }
            }

            return EFindPathStatus.FPS_FOUND;
        }


        // 寻路算法A*
        public EFindPathStatus FindPath(int nStartX, int nStartZ, int nTargetX, int nTargetZ)
        {
            int nStartPoint = ConvertXZCoord2OneCoord(nStartX, nStartZ);
            int nTargetPoint = ConvertXZCoord2OneCoord(nTargetX, nTargetZ);

            if (nStartPoint != -1 && nTargetPoint != -1)
            {
                // 如果该点不可走 选择相邻的八个点中能走的
                if (!m_arrayMapGridInfo[nTargetPoint].m_bWalkable)
                {
                    for (int i = (int)EGridDirect.GD_TOP_LEFT; i < (int)EGridDirect.GD_MAX; ++i)
                    {
                        int nNeighborID = m_arrayMapGridInfo[nTargetPoint].m_arrayNeighbor[i];

                        if (nNeighborID != -1)
                        {
                            if (m_arrayMapGridInfo[nNeighborID].m_bWalkable)
                            {
                                return FindPath(nStartPoint, nNeighborID);
                            }
                        }
                    }

                    // if get here means the grid map is very wrecked, we should extend the neighbors 
                    Debug.Log("PathFinding some thing wrong");

                    m_kNeighborIndex.Clear();
                    m_kNeighborIndex.Add(nTargetPoint);

                    int nFindNeighbor = RecursiveFindWalkableNeighbor();

                    if (nFindNeighbor != -1)
                    {
                        return FindPath(nStartPoint, nFindNeighbor);
                    }

                    Debug.Log("Is not possible");
                }

                return FindPath(nStartPoint, nTargetPoint);
            }

            return EFindPathStatus.FPS_NONE;
        }

        public int RecursiveFindWalkableNeighbor()
        {
            if (m_kNeighborIndex.Count != 0)
            {
                int nCurID = m_kNeighborIndex[0];

                if (IsGridWalkable(nCurID))
                {
                    return nCurID;
                }

                m_kNeighborIndex.RemoveAt(0);

                for (int i = (int)EGridDirect.GD_TOP_LEFT; i < (int)EGridDirect.GD_MAX; ++i)
                {
                    int nNeighborID = m_arrayMapGridInfo[nCurID].m_arrayNeighbor[i];

                    if (nNeighborID != -1)
                    {
                        if (IsGridWalkable(nNeighborID))
                        {
                            return nNeighborID;
                        }
                        else
                        {
                            bool bFound = false;

                            for (int j = 0; j < m_kNeighborIndex.Count; ++j )
                            {
                                if (m_kNeighborIndex[j] == nNeighborID)
                                {
                                    bFound = true;
                                    break;
                                }
                            }

                            if (!bFound)
                            {
                                m_kNeighborIndex.Add(nNeighborID);
                            }
                        }
                    }
                }

                return RecursiveFindWalkableNeighbor();
            }
            else
            {
                return -1;
            }
        }

        // 分配数据内存
        public void AllocateDataContainer(int nWidth, int nHeight)
        {
            m_kOpenList = new HashSet<int>();
            m_kCloseSet = new HashSet<int>();
            m_kFoundPath = new List<int>();
            m_kFoundPointContainer = new List<Vector3>();

            m_arrayMapGridInfo = new AStarGrid[nWidth * nHeight];

            for (int i = 0; i < nWidth * nHeight; ++i )
            {
                m_arrayMapGridInfo[i] = new AStarGrid();
            }
        }

        // 计算预估值
        public int HeuristicValue(int nCurNodeID, int nTargetID)
        {
            int nCurIndexX = nCurNodeID / m_nRowNum;
            int nCurIndexZ = nCurNodeID % m_nRowNum;

            int nTarIndexX = nTargetID / m_nRowNum;
            int nTarIndexZ = nTargetID % m_nRowNum;

            return Math.Abs(nTarIndexX - nCurIndexX) + Math.Abs(nTarIndexZ - nCurIndexZ);
        }

        // distant between two node
        public float DistBetween(int nCurID, int nNeighborID)
        {
            int nCurIndexX = nCurID / m_nRowNum;
            int nCurIndexZ = nCurID % m_nRowNum;

            int nNeiIndexX = nNeighborID / m_nRowNum;
            int nNeiIndexZ = nNeighborID % m_nRowNum;

            int nDistX = Math.Abs(nNeiIndexX - nCurIndexX);
            int nDistZ = Math.Abs(nNeiIndexZ - nCurIndexZ);

            if (nDistX + nDistZ == 1)
            {
                return 1.0f;
            }
            else if (nDistX + nDistZ == 2)
            {
                return 1.414f;
            }

            Debug.Log("计算距离出错");
            return 0.0f;
        }

        // 构建路径
        public void ReconstructPath(int nCurID)
        {
            //int nParentID = m_arrayMapGridInfo[nCurID].m_nParentID;

            int nParentID = -1;

            if (m_kDicParentContainer.ContainsKey(nCurID))
            {
                nParentID = m_kDicParentContainer[nCurID];
            }

            if (nParentID != -1)
            {
                RecordPathPoint(nCurID);
                ReconstructPath(nParentID);
            }
        }

        public void RecordPathPoint(int nCurID)
        {
            m_kFoundPath.Add(nCurID);

            int nIndexX = 0;
            int nIndexZ = 0;

            AStarAlgorithm.GetInstance().ConvertOneCoord2XZCoord(nCurID, out nIndexX, out nIndexZ);
            Vector3 tarpot = AStarAlgorithm.GetInstance().GetPositionByIndexCoord(nIndexX, nIndexZ);
            m_kFoundPointContainer.Add(tarpot);
        }

        // 得到OpenSet中消耗值最小的
        public int FindLowestCostNodeInOpenSet()
        {
            int nRetIndex = -1;

            float fMinValue = float.MaxValue;
            foreach (int value in m_kOpenList)
            {
                if (m_arrayMapGridInfo[value].m_fFinalCost < fMinValue)
                {
                    nRetIndex = value;
                    fMinValue = m_arrayMapGridInfo[value].m_fFinalCost;
                }
            }

            return nRetIndex;
        }

        // 解析地图文件
        public void ParseMapPathFile(string strFileName)
        {
            GraphicsUtil.m_kPoint.Clear();
            GraphicsUtil.m_kGrid.Clear();
            GraphicsUtil.m_LinePoint.Clear();

            TextAsset asset = BundleMemManager.Instance.loadResource(strFileName) as TextAsset;
            Stream s = new MemoryStream(asset.bytes);
            BinaryReader reader = new BinaryReader(s);

            UInt32 un32MapID = reader.ReadUInt32();
            UInt32 un32MapVer = reader.ReadUInt32();

            m_kGridName = new char[m_un32MaxMapName];

            for (int i = 0; i < m_un32MaxMapName; ++i)
            {
                m_kGridName[i] = reader.ReadChar();
            }

            m_nRowNum = reader.ReadInt32();
            m_nColumnNum = reader.ReadInt32();
            m_nMapWidth = reader.ReadInt32();
            m_nMapHeight = reader.ReadInt32();
            m_fUnitSize = reader.ReadSingle();

            AllocateDataContainer(m_nColumnNum, m_nRowNum);


            for (int i = 0; i < m_nColumnNum; ++i )
            {
                for (int j = 0; j < m_nRowNum; ++j )
                {
                    int nArrayIndex = i * m_nRowNum + j;
                    m_arrayMapGridInfo[nArrayIndex].m_nID = nArrayIndex;
                    m_arrayMapGridInfo[nArrayIndex].m_eStatus = EGridStatus.GS_INIT;
                    m_arrayMapGridInfo[nArrayIndex].m_fFinalCost = 0;
                    m_arrayMapGridInfo[nArrayIndex].m_fGoalCost = 0;
                    //m_arrayMapGridInfo[nArrayIndex].m_nParentID = -1;
                    m_arrayMapGridInfo[nArrayIndex].m_bWalkable = (reader.ReadByte() == 1);
                    
                    InitNeighBorData(nArrayIndex);

                    int nIndexX = 0;
                    int nIndexZ = 0;
                    AStarAlgorithm.GetInstance().ConvertOneCoord2XZCoord(nArrayIndex, out nIndexX, out nIndexZ);
                    Vector3 kCenter = AStarAlgorithm.GetInstance().GetPositionByIndexCoord(nIndexX, nIndexZ);

                    // check the wall layer to write the walkable variable
                    if (m_arrayMapGridInfo[nArrayIndex].m_bWalkable)
                    {
                        GraphicsUtil.m_kPoint.Add(kCenter);
                    }
                    else
                    {
                        GraphicsUtil.m_kGrid.Add(kCenter);
                    }
                }
            }

            Debug.Log("总格子数 " + GraphicsUtil.m_kPoint.Count);

            reader.Close();
            s.Close();
        }




        // 解析地图文件带写文件 跑第一次地图写到配置文件中
        public void ParseMapPathFileWithWrite(string strFileName)
        {
            //BinaryReader reader = new BinaryReader(File.Open(strFileName, FileMode.Open));

            GraphicsUtil.m_kPoint.Clear();
            GraphicsUtil.m_kGrid.Clear();
            GraphicsUtil.m_LinePoint.Clear();

            TextAsset asset = BundleMemManager.Instance.loadResource(strFileName) as TextAsset;
            Stream s = new MemoryStream(asset.bytes);
            BinaryReader reader = new BinaryReader(s);

            UInt32 un32MapID = reader.ReadUInt32();
            UInt32 un32MapVer = reader.ReadUInt32();

            m_kGridName = new char[m_un32MaxMapName];

            for (int i = 0; i < m_un32MaxMapName; ++i)
            {
                m_kGridName[i] = reader.ReadChar();
            }

            m_nRowNum = reader.ReadInt32();
            m_nColumnNum = reader.ReadInt32();
            m_nMapWidth = reader.ReadInt32();
            m_nMapHeight = reader.ReadInt32();
            m_fUnitSize = reader.ReadSingle();

            AllocateDataContainer(m_nColumnNum, m_nRowNum);

            FindWallObj();

            for (int i = 0; i < m_nColumnNum; ++i)
            {
                for (int j = 0; j < m_nRowNum; ++j)
                {
                    int nArrayIndex = i * m_nRowNum + j;
                    m_arrayMapGridInfo[nArrayIndex].m_nID = nArrayIndex;
                    m_arrayMapGridInfo[nArrayIndex].m_eStatus = EGridStatus.GS_INIT;
                    m_arrayMapGridInfo[nArrayIndex].m_fFinalCost = 0;
                    m_arrayMapGridInfo[nArrayIndex].m_fGoalCost = 0;
                    //m_arrayMapGridInfo[nArrayIndex].m_nParentID = -1;
                    m_arrayMapGridInfo[nArrayIndex].m_bWalkable = (reader.ReadByte() == 1);

                    InitNeighBorData(nArrayIndex);

                    int nIndexX = 0;
                    int nIndexZ = 0;
                    AStarAlgorithm.GetInstance().ConvertOneCoord2XZCoord(nArrayIndex, out nIndexX, out nIndexZ);
                    Vector3 kCenter = AStarAlgorithm.GetInstance().GetPositionByIndexCoord(nIndexX, nIndexZ);

                    // check the wall layer to write the walkable variable
                    if (m_arrayMapGridInfo[nArrayIndex].m_bWalkable)
                    {
                        if (CheckPointInWall(kCenter))
                        {
                            m_arrayMapGridInfo[nArrayIndex].m_bWalkable = false;
                            GraphicsUtil.m_kGrid.Add(kCenter);
                        }
                        else
                        {
                            GraphicsUtil.m_kPoint.Add(kCenter);
                        }
                    }
                    else
                        GraphicsUtil.m_kGrid.Add(kCenter);
                }
            }

            reader.Close();
            s.Close();

            // 通过内存数据 写到相应文件中
            WriteMapGridData(strFileName);
        }



        public void InitNeighBorData(int nCurNodeID)
        {
            int nCurIndexX = nCurNodeID / m_nRowNum;
            int nCurIndexZ = nCurNodeID % m_nRowNum;

            
            if (nCurIndexX - 1 >= 0 && nCurIndexZ - 1 >= 0)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_TOP_LEFT] = (nCurIndexX - 1) * m_nRowNum + (nCurIndexZ - 1);
            }

            if (nCurIndexX - 1 >= 0)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_TOP] = (nCurIndexX - 1) * m_nRowNum + (nCurIndexZ);
            }

            if (nCurIndexX - 1 >= 0 && nCurIndexZ + 1 < m_nRowNum)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_TOP_RIGHT] = (nCurIndexX - 1) * m_nRowNum + (nCurIndexZ + 1);
            }

            if (nCurIndexZ + 1 < m_nRowNum)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_RIGH] = (nCurIndexX) * m_nRowNum + (nCurIndexZ + 1);
            }

            if (nCurIndexX + 1 < m_nColumnNum && nCurIndexZ + 1 < m_nRowNum)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_BOTTOM_RIGHT] = (nCurIndexX + 1) * m_nRowNum + (nCurIndexZ + 1);
            }

            if (nCurIndexX + 1 < m_nColumnNum)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_BOTTOM] = (nCurIndexX + 1) * m_nRowNum + (nCurIndexZ);
            }

            if (nCurIndexX + 1 < m_nColumnNum && nCurIndexZ - 1 >= 0)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_BOTTOM_LEFT] = (nCurIndexX + 1) * m_nRowNum + (nCurIndexZ - 1);
            }

            if (nCurIndexZ - 1 >= 0)
            {
                m_arrayMapGridInfo[nCurNodeID].m_arrayNeighbor[(int)EGridDirect.GD_LEFT] = (nCurIndexX) * m_nRowNum + (nCurIndexZ - 1);
            }
        }


        //////////////////////////////////////////////////////////////////////////
        // Game coordination to one dimestion index
        //////////////////////////////////////////////////////////////////////////
        // 游戏坐标转到寻路的Index
        public bool GamePointToIndex(Vector3 kGamePt, out int x, out int z)
        {
            if (kGamePt.x > m_nMapWidth * 0.5f ||
                   kGamePt.x < -m_nMapWidth * 0.5f ||
                   kGamePt.z > m_nMapHeight * 0.5f ||
                   kGamePt.z < -m_nMapHeight * 0.5f)
            {
                x = 0;
                z = 0;
                return false;
            }

            int nIndexX = 0;
            int nIndexZ = 0;

            if (kGamePt.x > 0.0f)
            {
                nIndexX = (int)(kGamePt.x / m_fUnitSize) + 1;
            }
            else
                nIndexX = (int)(kGamePt.x / m_fUnitSize) - 1;


            if (kGamePt.z > 0.0f)
            {
                nIndexZ = (int)(kGamePt.z / m_fUnitSize) + 1;
            }
            else
                nIndexZ = (int)(kGamePt.z / m_fUnitSize) - 1;

            x = nIndexX;
            z = nIndexZ;
            return true;
        }

        // 通过中心点的寻路的Index转换到行列坐标 再转到一维坐标
        public int ConvertXZCoord2OneCoord(int nIndexX, int nIndexZ)
        {
            int n2DArrayIndexX = 0;
            int n2DArrayIndexZ = 0;

            int nHeightHalf = m_nColumnNum / 2;
            int nWidthHalf = m_nRowNum / 2;



            if (nIndexX > 0)
            {
                if (nIndexZ > 0)
                {
                    // 第一象限
                    n2DArrayIndexX = nHeightHalf - nIndexZ;
                    n2DArrayIndexZ = nIndexX + nWidthHalf - 1;
                }
                else if (nIndexZ < 0)
                {
                    // 第四象限
                    n2DArrayIndexX = nHeightHalf - nIndexZ - 1;
                    n2DArrayIndexZ = nIndexX + nWidthHalf - 1;
                }
                else
                {
                    return -1;
                }
            }
            else if (nIndexX < 0)
            {
                if (nIndexZ > 0)
                {
                    // 第二象限
                    n2DArrayIndexX = nHeightHalf - nIndexZ;
                    n2DArrayIndexZ = nIndexX + nWidthHalf;
                }
                else if (nIndexZ < 0)
                {
                    // 第三象限
                    n2DArrayIndexX = nHeightHalf - nIndexZ - 1;
                    n2DArrayIndexZ = nIndexX + nWidthHalf;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }


            return n2DArrayIndexX * m_nRowNum + n2DArrayIndexZ;
        }

        //////////////////////////////////////////////////////////////////////////
        // Game coordination to one dimestion index
        //////////////////////////////////////////////////////////////////////////
        public void ConvertOneCoord2XZCoord(int nIndex, out int nIndexX, out int nIndexZ)
        {
            // 转到行列坐标
            int n2DArrayIndexX = nIndex / m_nRowNum;
            int n2DArrayIndexZ = nIndex % m_nRowNum;

            // 再转到中心点坐标

            int nHeightHalf = m_nColumnNum / 2;
            int nWidthHalf = m_nRowNum / 2;


            if (n2DArrayIndexX < nHeightHalf)
            {
                if (n2DArrayIndexZ >= nWidthHalf)
                {
                    // 第一象限
                    nIndexX = n2DArrayIndexZ - nWidthHalf + 1;
                    nIndexZ = nHeightHalf - n2DArrayIndexX;
                }
                else
                {
                    // 第二象限
                    nIndexX = n2DArrayIndexZ - nWidthHalf;
                    nIndexZ = nHeightHalf - n2DArrayIndexX;
                }
            }
            else
            {
                if (n2DArrayIndexZ >= nWidthHalf)
                {
                    // 第四象限
                    nIndexX = n2DArrayIndexZ - nWidthHalf + 1;
                    nIndexZ = nHeightHalf - n2DArrayIndexX - 1;
                }
                else
                {
                    // 第三象限
                    nIndexX = n2DArrayIndexZ - nWidthHalf;
                    nIndexZ = nHeightHalf - n2DArrayIndexX - 1;
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////
        // One dimestion index to game coordination
        //////////////////////////////////////////////////////////////////////////



        // get position by pathfind coordination
        public Vector3 GetPositionByIndexCoord(int nIndexX, int nIndexZ)
        {
            if (nIndexX > 0)
            {
                if (nIndexZ > 0)
                {
                    // 第一象限
                    return new Vector3((nIndexX - 0.5f) * m_fUnitSize, 0.0f, (nIndexZ - 0.5f )* m_fUnitSize);
                }
                else if (nIndexZ < 0)
                {
                    // 第四象限
                    return new Vector3((nIndexX - 0.5f) * m_fUnitSize, 0.0f, (nIndexZ + 0.5f )* m_fUnitSize);
                }
            }
            else if (nIndexX < 0)
            {
                if (nIndexZ > 0)
                {
                    // 第二象限
                    return new Vector3((nIndexX + 0.5f) * m_fUnitSize, 0.0f, (nIndexZ - 0.5f )* m_fUnitSize);
                }
                else if (nIndexZ < 0)
                {
                    // 第三象限
                    return new Vector3((nIndexX + 0.5f) * m_fUnitSize, 0.0f, (nIndexZ + 0.5f )* m_fUnitSize);
                }
            }

            return Vector3.zero;
        }

        public bool IsPositionInGrid(Vector3 kTestPos, Vector3 kCenterPos)
        {
            if (Mathf.Abs(kTestPos.x - kCenterPos.x) > 0.4f * m_fUnitSize)
            {
                return false;
            }

            if (Mathf.Abs(kTestPos.z - kCenterPos.z) > 0.4f * m_fUnitSize)
            {
                return false;
            }

            return true;
        }


        // check line segment from source to destion is intersect with collision layer
        public bool CheckPointsVisibility(Vector3 kSource, Vector3 kDestion)
        {
            RaycastHit hitInfo;
            LayerMask mask = 1 << LayerMask.NameToLayer("Wall");

            return !Physics.Linecast(kSource, kDestion, out hitInfo, mask);
        }

        // check position is walkable
        public bool CheckPositionWalkable(Vector3 kCenter)
        {
            RaycastHit hitInfo;
            LayerMask mask = 1 << LayerMask.NameToLayer("Wall");

            
            float m_fHalf = m_fUnitSize * 0.5f;

            return

                


            (!Physics.Linecast(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf), kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf), out hitInfo, mask) &&
                !Physics.Linecast(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf), kCenter + new Vector3(m_fHalf, 0.0f, -m_fHalf), out hitInfo, mask)

                &&

                !Physics.Linecast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf), out hitInfo, mask) &&
                !Physics.Linecast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), kCenter + new Vector3(m_fHalf, 0.0f, -m_fHalf), out hitInfo, mask));
        }


        public bool CheckPositionUnWalkable(Vector3 kCenter)
        {
            RaycastHit hitInfo;
            LayerMask mask = 1 << LayerMask.NameToLayer("Wall");


            float m_fHalf = m_fUnitSize * 0.5f;

            bool ret = 

                

            //(Physics.Linecast(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf), kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf), out hitInfo, mask) ||
            //    Physics.Linecast(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf), kCenter + new Vector3(m_fHalf, 0.0f, -m_fHalf), out hitInfo, mask)

            //    ||

            //    Physics.Linecast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf), out hitInfo, mask) ||
            //    Physics.Linecast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), kCenter + new Vector3(m_fHalf, 0.0f, -m_fHalf), out hitInfo, mask));




             (Physics.Raycast(kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf), new Vector3(0.0f, 0.0f, 1.0f), m_fUnitSize, mask) ||
                Physics.Raycast(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf), new Vector3(1.0f, 0.0f, 0.0f), m_fUnitSize, mask)

                ||

                Physics.Raycast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), new Vector3(-1.0f, 0.0f, 0.0f), m_fUnitSize, mask) ||
                Physics.Raycast(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf), new Vector3(0.0f, 0.0f, -1.0f), m_fUnitSize, mask));


            


            GraphicsUtil.m_kGrid.Add(kCenter + new Vector3(-m_fHalf, 0.0f, -m_fHalf));
            GraphicsUtil.m_kGrid.Add(kCenter + new Vector3(-m_fHalf, 0.0f, m_fHalf));
            GraphicsUtil.m_kGrid.Add(kCenter + new Vector3(m_fHalf, 0.0f, m_fHalf));
            GraphicsUtil.m_kGrid.Add(kCenter + new Vector3(m_fHalf, 0.0f, -m_fHalf));


            return ret;
        }

        public bool IsGridWalkable(int nID)
        {
            return m_arrayMapGridInfo[nID].m_bWalkable;
        }

        public bool IsPositionWalkable(Vector3 kPosition)
        {
            int nIndexX = 0;
            int nIndexZ = 0;

            if (GamePointToIndex(kPosition, out nIndexX, out nIndexZ))
            {
                int nID = ConvertXZCoord2OneCoord(nIndexX, nIndexZ);

                if (nID != -1)
                {
                    return m_arrayMapGridInfo[nID].m_bWalkable;
                }
            }

            return false;
        }

        public bool CheckPointInWall(Vector3 kPoint)
        {
            // 通过检查遍历所有的wall来判断点是否在其中
            for (int i = 0; i < m_kWallObjs.Count; ++i )
            {
                if (CheckPointInCube(kPoint, m_kWallObjs[i].transform))
                {
                    return true;
                }
            }

            return false;
        }

        public void FindWallObj()
        {
            if (m_kWallObjs != null)
            {
                m_kWallObjs.Clear();
            }

            m_kWallObjs = new List<GameObject>();

            GameObject[] kObjs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (GameObject g in kObjs)
            {
                if (g.name == "wall" && (g.GetComponent<BoxCollider>() || g.GetComponent<CapsuleCollider>()))
                {
                    m_kWallObjs.Add(g);

                    Debug.Log("名称 " + g.name);
                }
            }

            Debug.Log("墙壁数 " + m_kWallObjs.Count);
        }

        private static bool CheckPointInCube(Vector3 kPoint, Transform kCube)
        {
            float fXLen = kCube.transform.localScale.x;
            float fYLen = kCube.transform.localScale.y;
            float fZLen = kCube.transform.localScale.z;

            float fHalfX = fXLen * 0.5f;
            float fHalfY = fYLen * 0.5f;
            float fHalfZ = fZLen * 0.5f;

            
            Vector3 k0 = new Vector3(fHalfX, fHalfY, fHalfZ);
            Vector3 k1 = new Vector3(-fHalfX, fHalfY, fHalfZ);
            Vector3 k2 = new Vector3(fHalfX, fHalfY, -fHalfZ);
            Vector3 k3 = new Vector3(-fHalfX, fHalfY, -fHalfZ);

            Vector3 k4 = new Vector3(fHalfX, -fHalfY, fHalfZ);
            Vector3 k5 = new Vector3(-fHalfX, -fHalfY, fHalfZ);
            Vector3 k6 = new Vector3(fHalfX, -fHalfY, -fHalfZ);
            Vector3 k7 = new Vector3(-fHalfX, -fHalfY, -fHalfZ);


            // method 1
            //Vector3 k = kCube.transform.rotation.eulerAngles;

            //Debug.Log("欧拉角 " + k);

            //Debug.Log("欧拉角1 " + kCube.transform.rotation);

            //Debug.Log("欧拉角2 " + kCube.transform.localRotation);

            //Quaternion rotL = Quaternion.Euler(kCube.transform.rotation.eulerAngles);

            //Debug.Log("欧拉角3 " + rotL);

            //k0 = rotL * k0;
            //k1 = rotL * k1;
            //k2 = rotL * k2;
            //k3 = rotL * k3;
            //k4 = rotL * k4;
            //k5 = rotL * k5;
            //k6 = rotL * k6;
            //k7 = rotL * k7;

            //k0 += kCube.transform.position;
            //k1 += kCube.transform.position;
            //k2 += kCube.transform.position;
            //k3 += kCube.transform.position;
            //k4 += kCube.transform.position;
            //k5 += kCube.transform.position;
            //k6 += kCube.transform.position;
            //k7 += kCube.transform.position;



            //float fMinX = Mathf.Min(k0.x, k1.x, k2.x, k3.x, k4.x, k5.x, k6.x, k7.x);
            //float fMinY = Mathf.Min(k0.y, k1.y, k2.y, k3.y, k4.y, k5.y, k6.y, k7.y);
            //float fMinZ = Mathf.Min(k0.z, k1.z, k2.z, k3.z, k4.z, k5.z, k6.z, k7.z);

            //float fMaxX = Mathf.Max(k0.x, k1.x, k2.x, k3.x, k4.x, k5.x, k6.x, k7.x);
            //float fMaxY = Mathf.Max(k0.y, k1.y, k2.y, k3.y, k4.y, k5.y, k6.y, k7.y);
            //float fMaxZ = Mathf.Max(k0.z, k1.z, k2.z, k3.z, k4.z, k5.z, k6.z, k7.z);


            //GraphicsUtil.m_kGrid.Add(k0);
            //GraphicsUtil.m_kGrid.Add(k1);
            //GraphicsUtil.m_kGrid.Add(k2);
            //GraphicsUtil.m_kGrid.Add(k3);
            //GraphicsUtil.m_kGrid.Add(k4);
            //GraphicsUtil.m_kGrid.Add(k5);
            //GraphicsUtil.m_kGrid.Add(k6);
            //GraphicsUtil.m_kGrid.Add(k7);




            //Debug.Log("name " + kCube.name + " " + fXLen + " " + fYLen + " " + fZLen);

            //if ((Mathf.Abs(kPoint.x - kCube.transform.position.x) <= fZLen * 0.5f) &&
            //    (Mathf.Abs(kPoint.y - kCube.transform.position.y) <= fYLen * 0.5f) &&
            //    (Mathf.Abs(kPoint.z - kCube.transform.position.z) <= fXLen * 0.5f))
            //{
            //    return true;
            //}
            //else
            //    return false;

        
            // method 2
            float fMinX = Mathf.Min(k0.x, k1.x, k2.x, k3.x, k4.x, k5.x, k6.x, k7.x);
            float fMinY = Mathf.Min(k0.y, k1.y, k2.y, k3.y, k4.y, k5.y, k6.y, k7.y);
            float fMinZ = Mathf.Min(k0.z, k1.z, k2.z, k3.z, k4.z, k5.z, k6.z, k7.z);

            float fMaxX = Mathf.Max(k0.x, k1.x, k2.x, k3.x, k4.x, k5.x, k6.x, k7.x);
            float fMaxY = Mathf.Max(k0.y, k1.y, k2.y, k3.y, k4.y, k5.y, k6.y, k7.y);
            float fMaxZ = Mathf.Max(k0.z, k1.z, k2.z, k3.z, k4.z, k5.z, k6.z, k7.z);

            kPoint -= kCube.transform.position;

            Vector3 kRevertRot = new Vector3(kCube.transform.rotation.eulerAngles.x, -kCube.transform.rotation.eulerAngles.y, kCube.transform.rotation.eulerAngles.z);
            Quaternion kQuRevertRot = Quaternion.Euler(kRevertRot);

            Vector3 kRevertPt = kQuRevertRot * kPoint;
            
            
            if (kRevertPt.x <= fMaxX && kRevertPt.y <= fMaxY && kRevertPt.z <= fMaxZ && kRevertPt.x >= fMinX && kRevertPt.y >= fMinY && kRevertPt.z >= fMinZ)
            {
                return true;
            }

            return false;
        }



        private bool WriteMapGridData(string strFileName)
        {
            string strAsset = "E:/SVN/client/Assets/Resources/";
            strAsset += strFileName;
            strAsset += ".txt";

            BinaryWriter writer = new BinaryWriter(File.Open(strAsset, FileMode.Create));

            // check 
            if (m_nColumnNum % 2 != 0 || m_nRowNum % 2 != 0 || m_nColumnNum == 0 || m_nRowNum == 0)
            {
                return false;
            }

            UInt32 mapID = 0;
            UInt32 mapVer = 1;

            writer.Write(mapID);
            writer.Write(mapVer);

            for (int i = 0; i < m_un32MaxMapName; ++i)
            {
                if (i < m_kGridName.Length)
                {
                    writer.Write(m_kGridName[i]);
                }
                else
                    writer.Write(' ');
            }

            writer.Write(m_nRowNum);
            writer.Write(m_nColumnNum);
            writer.Write(m_nMapWidth);
            writer.Write(m_nMapHeight);
            writer.Write(m_fUnitSize);

            int nTotal = m_nRowNum * m_nColumnNum;

            for (int i = 0; i < nTotal; ++i)
            {
                byte value = (byte)(m_arrayMapGridInfo[i].m_bWalkable ? 1 : 0);

                if (!m_arrayMapGridInfo[i].m_bWalkable)
                {
                    Debug.Log("有阻挡信息");
                }
                writer.Write(value);
            }

            writer.Close();

            Debug.Log("写入成功!");

            return true;
        }
    }
}
