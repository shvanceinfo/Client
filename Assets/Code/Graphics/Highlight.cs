using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Highlight : MonoBehaviour 
{

    public class highlightData
    {
        public float m_fTime;
        public GameObject m_kObj;
    }

	public Material highlightMaterial;
	bool highlightObjectShow = true;
    public List<highlightData> highlightObjects = new List<highlightData>();

    private Mesh m_kBakedMesh;

    public float m_fHightlightTime = 0.3f;



    void Awake()
    {
        m_kBakedMesh = new Mesh();
    }

	// Use this for initialization
	void Start () 
    {
	    //InvokeRepeating("Timer", 1, 0.1f);
	}

    void Timer()
    {
        //if (highlightObjectShow) highlightObjectShow = false;
        //else highlightObjectShow = true;
    }


    public void SetHightLightGameObj(GameObject obj)
    {
        highlightData kObj = new highlightData();
        kObj.m_fTime = 0.0f;
        kObj.m_kObj = obj;

        for (int i = 0; i < highlightObjects.Count; ++i )
        {
            if (obj == highlightObjects[i].m_kObj)
            {
                return;
            }
        }

        highlightObjects.Add(kObj);
    }

	void OnPostRender()
    {
        //if (!highlightObjectShow || highlightObjects.Count == 0) return;

        

        for (int i = 0; i < highlightObjects.Count; ++i )
        {
            highlightObjects[i].m_fTime += Time.deltaTime;

            if (highlightObjects[i].m_kObj != null)
            {
                highlightMaterial.SetPass(0);

                if (highlightObjects[i].m_fTime < m_fHightlightTime)
                {

                    Component[] meshFilters = highlightObjects[i].m_kObj.GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
                    foreach (SkinnedMeshRenderer m in meshFilters)
                    {
                        m.BakeMesh(m_kBakedMesh);
                        Graphics.DrawMeshNow(m_kBakedMesh, m.transform.position, m.transform.rotation);
                    }
                }
                else
                {
                    highlightObjects.RemoveAt(i);
                }

                
            }
            


            //if (highlightObjects[i] != null)
            //{
            //    Component[] meshFilters = highlightObjects[i].GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
            //    foreach (SkinnedMeshRenderer m in meshFilters)
            //    {
            //        m.BakeMesh(m_kBakedMesh);
            //        Graphics.DrawMeshNow(m_kBakedMesh, m.transform.position, m.transform.rotation);
            //    }
            //}
        }
     }
}
