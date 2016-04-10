using UnityEngine;
using manager;

namespace AISystem
{
    public class AIPathFinding : MonoBehaviour
    {
        // 是否到达下一个路点
        public bool m_bArrived = true;

        // 寻路单位
        private Character m_kRunner = null;

        // 寻路的目标点 动态更新
        public Vector3 m_kTargetPositon;

        // 保存的上次的目标点 用来跟当前目标点做比较
        public Vector3 m_kLastTarget;

        // 基准点
        private Vector3 m_kDummyPos;

        public void Awake()
        {
            m_kRunner = GetComponent<Character>();

            StopPathFinding();
        }

        public void Start()
        {

        }

        public void Update()
        {
            if (m_bArrived)
            {
                if (m_kTargetPositon != m_kDummyPos)
                {
                    if (m_kLastTarget != m_kTargetPositon)
                    {
                        StartPathFinding(m_kTargetPositon);
                        m_kLastTarget = m_kTargetPositon;
                        m_bArrived = false;
                    }
                    else
                    {
                        int nCount = AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count;

                        if (nCount != 0)
                        {
                            AStarAlgorithm.GetInstance().m_kFoundPointContainer.RemoveAt(nCount - 1);
                            AStarAlgorithm.GetInstance().m_kFoundPath.RemoveAt(nCount - 1);

                            if (AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count == 0)
                            {
                                SearchProcess();
                            }
                            else
                            {
                                int nNewCount = AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count;

                                Vector3 kCurrentTarget = AStarAlgorithm.GetInstance().m_kFoundPointContainer[nNewCount - 1];


                                // 判断当前要走的点是否在阻挡中 如果是需要重新计算
                                //if (!m_kRunner.canMoveTo(kCurrentTarget))
                                //{
                                //    AStarAlgorithm.GetInstance().m_arrayMapGridInfo[AStarAlgorithm.GetInstance().m_kFoundPath[nNewCount - 1]].m_bWalkable = false;
                                //    m_kRunner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                                //    return;
                                //}

                                m_kRunner.PursueTo(kCurrentTarget);

                                m_bArrived = false;
                            }
                        }
                        else
                        {
                            SearchProcess();
                        }
                    }
                }
            }
        }

        // 重置寻路点
        public void StopPathFinding()
        {
            m_bArrived = true;

            m_kTargetPositon = new Vector3(Constant.PLAYER_INIT_POSITION_X, Constant.PLAYER_INIT_POSITION_Y, Constant.PLAYER_INIT_POSITION_Z);
            m_kLastTarget = new Vector3(Constant.PLAYER_INIT_POSITION_X, Constant.PLAYER_INIT_POSITION_Y, Constant.PLAYER_INIT_POSITION_Z);
            m_kDummyPos = new Vector3(Constant.PLAYER_INIT_POSITION_X, Constant.PLAYER_INIT_POSITION_Y, Constant.PLAYER_INIT_POSITION_Z);
        }

        public void StartPathFinding(Vector3 kDestPoint)
        {
            // 1. first check if line segment from source to destion intersect with layer "wall"
            if (AStarAlgorithm.GetInstance().CheckPointsVisibility(m_kRunner.getPosition(), kDestPoint) && !Global.inCityMap())
            {
                m_kRunner.moveTo(kDestPoint);
                return;
            }

            // 2. use A star algorithm
            GraphicsUtil.m_LinePoint.Clear();
            int nStartIndexX = 0;
            int nStartIndexZ = 0;

            int nDestIndexX = 0;
            int nDestIndexZ = 0;

            if (AStarAlgorithm.GetInstance().GamePointToIndex(m_kRunner.getPosition(), out nStartIndexX, out nStartIndexZ))
            {
                if (AStarAlgorithm.GetInstance().GamePointToIndex(kDestPoint, out nDestIndexX, out nDestIndexZ))
                {
                    AStarAlgorithm.EFindPathStatus eFindPathRet = AISystem.AStarAlgorithm.GetInstance().FindPath(nStartIndexX, nStartIndexZ, nDestIndexX, nDestIndexZ);

                    if (AStarAlgorithm.EFindPathStatus.FPS_FOUND == eFindPathRet ||
                        AStarAlgorithm.EFindPathStatus.FPS_FOUND_INIT == eFindPathRet) 
                    {
                        if (AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count != 0)
                        {
                            int nCount = AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count;
                            Vector3 kCurrentTarget = AStarAlgorithm.GetInstance().m_kFoundPointContainer[nCount - 1];

                            // 判断当前要走的点是否在阻挡中 如果是需要重新计算
                            //if (!m_kRunner.canMoveTo(kCurrentTarget))
                            //{
                            //    AStarAlgorithm.GetInstance().m_arrayMapGridInfo[AStarAlgorithm.GetInstance().m_kFoundPath[nCount - 1]].m_bWalkable = false;
                            //    m_kRunner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
                            //    return;
                            //}

                            m_kRunner.PursueTo(kCurrentTarget);
                            m_bArrived = false;

                            //if (m_kRunner.getType() == CharacterType.CT_PLAYER)
                            {
                                for (int i = 0; i < AStarAlgorithm.GetInstance().m_kFoundPointContainer.Count; ++i )
                                {
                                    GraphicsUtil.m_LinePoint.Add(AStarAlgorithm.GetInstance().m_kFoundPointContainer[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("寻路没找到 " + eFindPathRet);
                    }
                }
            }
            else
            {
                Debug.Log("寻路位置错误: 当前错误位置为" + m_kRunner.getPosition());
            }
        }

        public void SearchProcess()
        {
            if (TaskManager.Instance.isTaskFollow)
            {
                if (m_kRunner.GetAI().m_kPursueState.m_bGotoGate)
                {
                    TaskManager.Instance.isTaskFollow = false;
                    if (Global.transportId != 0 && !UIManager.Instance.isWindowOpen(UiNameConst.ui_raid))
                    {
                        RaidManager.Instance.initRaid();
                    }
                }
                else
                {
                    if (TaskManager.Instance.FolllowPath.size == 0) //已经找到了NPC
                    {
                        TaskManager.Instance.isTaskFollow = false;
                        if (m_kRunner.GetAI().m_kPursueState.m_kPursurNPC == null)
                        {
                            m_kRunner.setFaceDir(NPCManager.Instance.mainTaskNPC.FaceDir);
                            NPCManager.Instance.mainTaskNPC.Action.showTaskDialog(true);
                        }
                        else
                        {
                            m_kRunner.setFaceDir(m_kRunner.GetAI().m_kPursueState.m_kPursurNPC.FaceDir);
                            m_kRunner.GetAI().m_kPursueState.m_kPursurNPC.Action.showTaskDialog(true);
                        }
                    }
                }
            }
            

            // 没有寻路点 切到待机
            m_kRunner.GetAI().SendStateMessage(CharacterAI.CHARACTER_STATE.CS_IDLE);
        }
        
    }
}