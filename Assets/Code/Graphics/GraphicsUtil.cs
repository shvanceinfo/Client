using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GraphicsUtil : MonoBehaviour 
{
    static public Material lineMaterial;

	// skill draw lines
    static public Vector3 m_vecLineStart;
    static public Vector3 m_vecLineEnd;
    static public Vector3 m_vecOriginal;

	// A* algorithm
    static public List<Vector3> m_kPoint = new List<Vector3>();
    static public List<Vector3> m_LinePoint = new List<Vector3>();
    static public List<Vector3> m_kGrid = new List<Vector3>();
    static public bool m_bShowGraphics = false;

	// for UI stuck render texture
	static public RenderTexture m_kPlayerCamRenderTexture = null;


	// one screen edge line
	static public List<Vector3> m_kScreenEdge = new List<Vector3>();


	// 
	static public bool m_bNeedRenderTex = false;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () 
    {
	    
	}

    static public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void OnPostRender()
    {
		DrawSegement();
		DrawAStarAlgorithmLine();
		DrawScreenEdge();
	}

    static public void ResetData()
    {
        m_vecLineEnd = Vector3.zero;
        m_vecLineStart = Vector3.zero;
    }

	static public void DrawSegement()
	{
		if (m_vecLineStart != Vector3.zero || m_vecLineEnd != Vector3.zero)
		{
			float radius = (m_vecLineStart - m_vecOriginal).magnitude;
			
			CreateLineMaterial();
			// set the current material
			lineMaterial.SetPass(0);
			
			GL.Begin(GL.LINES);
			GL.Color(Color.green);
			GL.Vertex3(m_vecOriginal.x,
			           m_vecOriginal.y,
			           m_vecOriginal.z);
			
			GL.Vertex3(m_vecLineStart.x,
			           m_vecLineStart.y,
			           m_vecLineStart.z);
			
			GL.End();
			
			for (float factor = 0.01f; factor < 1.0f;)
			{
				Vector3 vecSample = m_vecLineStart * (1f - factor) + m_vecLineEnd * factor;
				
				Vector3 sampleDir = vecSample - m_vecOriginal;
				sampleDir.Normalize();
				vecSample = m_vecOriginal + sampleDir * radius;
				
				GL.Begin(GL.LINES);
				GL.Color(Color.red);
				GL.Vertex3(m_vecOriginal.x,
				           m_vecOriginal.y,
				           m_vecOriginal.z);
				
				GL.Vertex3(vecSample.x,
				           vecSample.y,
				           vecSample.z);
				
				GL.End();
				
				
				factor += 0.01f;
			}
			
			GL.Begin(GL.LINES);
			GL.Color(Color.green);
			GL.Vertex3(m_vecOriginal.x,
			           m_vecOriginal.y,
			           m_vecOriginal.z);
			
			GL.Vertex3(m_vecLineEnd.x,
			           m_vecLineEnd.y,
			           m_vecLineEnd.z);
			
			GL.End();
			
		}
	}
	
	static public void DrawAStarAlgorithmLine()
	{
		if (!m_bShowGraphics)
		{
			return;
		}
		
		if (m_LinePoint.Count != 0)
		{
			CreateLineMaterial();
			// set the current material
			lineMaterial.SetPass(0);
			
			GL.Begin(GL.LINES);
			GL.Color(Color.blue);
			
			if (m_LinePoint.Count == 0)
            {
                return;
            }
            else if (m_LinePoint.Count == 1)
            {
                GL.Vertex3(m_LinePoint[0].x, m_LinePoint[0].y, m_LinePoint[0].z);
                GL.Vertex3(m_LinePoint[0].x, m_LinePoint[0].y, m_LinePoint[0].z);
            }
            else
            {
                for (int i = 0; i < m_LinePoint.Count; ++i)
                {
                    if (i != m_LinePoint.Count - 1)
                    {
                        GL.Vertex3(m_LinePoint[i].x, m_LinePoint[i].y, m_LinePoint[i].z);
                        GL.Vertex3(m_LinePoint[i + 1].x, m_LinePoint[i + 1].y, m_LinePoint[i + 1].z);
                    }
                }
            }

            GL.End();
        }

        if (m_kPoint.Count != 0)
        {
            CreateLineMaterial();
            // set the current material
            lineMaterial.SetPass(0);

            for (int i = 0; i < m_kPoint.Count; ++i)
            {
                GL.Begin(GL.LINES);
                GL.Color(Color.green);
                GL.Vertex3(m_kPoint[i].x + 0.1f, m_kPoint[i].y + 0.1f, m_kPoint[i].z);
                GL.Vertex3(m_kPoint[i].x - 0.1f, m_kPoint[i].y + 0.1f, m_kPoint[i].z);
                GL.Vertex3(m_kPoint[i].x, m_kPoint[i].y + 0.1f, m_kPoint[i].z + 0.1f);
                GL.Vertex3(m_kPoint[i].x, m_kPoint[i].y + 0.1f, m_kPoint[i].z - 0.1f);
                GL.End();
            }
        }



        if (m_kGrid.Count != 0)
        {
            CreateLineMaterial();
            // set the current material
            lineMaterial.SetPass(0);

            for (int i = 0; i < m_kGrid.Count; ++i)
            {
                GL.Begin(GL.LINES);
                GL.Color(Color.red);
                GL.Vertex3(m_kGrid[i].x + 0.1f, m_kGrid[i].y + 0.1f, m_kGrid[i].z);
                GL.Vertex3(m_kGrid[i].x - 0.1f, m_kGrid[i].y + 0.1f, m_kGrid[i].z);
                GL.Color(Color.red);
                GL.Vertex3(m_kGrid[i].x, m_kGrid[i].y + 0.1f, m_kGrid[i].z + 0.1f);
                GL.Vertex3(m_kGrid[i].x, m_kGrid[i].y + 0.1f, m_kGrid[i].z - 0.1f);
                GL.End();
            }

        }
    }

	static public void DrawScreenEdge()
	{
		if (m_kScreenEdge.Count != 0)
		{
			CreateLineMaterial();
			// set the current material
			lineMaterial.SetPass(0);
			
			GL.Begin(GL.LINES);
			GL.Color(Color.blue);

			for (int i = 0; i < m_kScreenEdge.Count; ++i)
			{
				if (i != m_kScreenEdge.Count - 1)
				{
					GL.Vertex3(m_kScreenEdge[i].x, m_kScreenEdge[i].y, m_kScreenEdge[i].z);
					GL.Vertex3(m_kScreenEdge[i + 1].x, m_kScreenEdge[i + 1].y, m_kScreenEdge[i + 1].z);
				}
			}
			
			GL.Vertex3(m_kScreenEdge[m_kScreenEdge.Count-1].x, m_kScreenEdge[m_kScreenEdge.Count-1].y, m_kScreenEdge[m_kScreenEdge.Count-1].z);
			GL.Vertex3(m_kScreenEdge[0].x, m_kScreenEdge[0].y, m_kScreenEdge[0].z);
			
			GL.End();
		}
	}
	
	// draw highlight object
	static public void HideGameObj(Character character)
	{
		Component[] meshFilters = character.gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
		foreach (SkinnedMeshRenderer m in meshFilters)
        {
            m.enabled = false;
        }

		Component[] mesh = character.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
        foreach (MeshRenderer m in mesh)
        {
            m.enabled = false;
        }

		Component[] particleSystem = character.gameObject.GetComponentsInChildren<ParticleSystem>(true); // typeof or MeshFilter
		foreach (ParticleSystem particle in particleSystem)
		{
			particle.enableEmission = false;
		}

        Weapon wp = character.GetComponentInChildren<Weapon>();

        if (wp != null)
        {
			Component[] wpMesh = wp.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
            foreach (MeshRenderer m in wpMesh)
            {
                m.enabled = false;
            }
        }

		if (character.m_kPet != null)
		{
			Component[] petMesh = character.m_kPet.GetComponentsInChildren<SkinnedMeshRenderer>(true); // typeof or MeshFilter
			foreach (SkinnedMeshRenderer m in petMesh)
			{
				m.enabled = false;
			}

			Component[] PetparticleSystem = character.m_kPet.gameObject.GetComponentsInChildren<ParticleSystem>(true); // typeof or MeshFilter
			foreach (ParticleSystem particle in PetparticleSystem)
			{
				particle.enableEmission = false;
			}
		}

		// hide UI
		character.GetComponent<HUD>().HideHUD();
    }

    static public void ShowGameObj(Character character)
    {
        Component[] meshFilters = character.gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer)); // typeof or MeshFilter
        foreach (SkinnedMeshRenderer m in meshFilters)
        {
            m.enabled = true;
        }

        Component[] mesh = character.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
        foreach (MeshRenderer m in mesh)
        {
			m.enabled = true;
		}

		Component[] particleSystem = character.gameObject.GetComponentsInChildren<ParticleSystem>(true); // typeof or MeshFilter
		foreach (ParticleSystem particle in particleSystem)
		{
			particle.enableEmission = true;
		}
		
        Weapon wp = character.GetComponentInChildren<Weapon>();

        if (wp != null)
        {
            Component[] wpMesh = wp.gameObject.GetComponentsInChildren(typeof(MeshRenderer)); // typeof or MeshFilter
            foreach (MeshRenderer m in wpMesh)
            {
				m.enabled = false;
			}
		}

		if (character.m_kPet != null)
		{
			Component[] petMesh = character.m_kPet.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true); // typeof or MeshFilter
			foreach (SkinnedMeshRenderer m in petMesh)
			{
				m.enabled = true;
			}

			Component[] PetparticleSystem = character.m_kPet.gameObject.GetComponentsInChildren<ParticleSystem>(true); // typeof or MeshFilter
			foreach (ParticleSystem particle in PetparticleSystem)
			{
				particle.enableEmission = true;
			}
		}

		// show UI
		character.GetComponent<HUD>().ShowHUD();
    }


	// draw rendertexture
	static public RenderTexture CreatePlayerCamRT(Camera kCamera)
	{
//		if (m_kPlayerCamRenderTexture != null) 
//		{
//			Destroy(m_kPlayerCamRenderTexture);
//			m_kPlayerCamRenderTexture = null;
//		}
//
//		m_bNeedRenderTex = true;

		if (kCamera == null)
			return null;

		if (m_kPlayerCamRenderTexture != null) 
		{
			Destroy(m_kPlayerCamRenderTexture);
			m_kPlayerCamRenderTexture = null;
		}

		m_kPlayerCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);

		kCamera.targetTexture = m_kPlayerCamRenderTexture;

		kCamera.Render();

		kCamera.targetTexture = null;

		return m_kPlayerCamRenderTexture;
	}

	static public void CalculateScreenCorner()
	{


	}

	static public bool IsCharacterInsideCamera(Character kCharacter, Camera kCam)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(kCam);

		Bounds bounds = new Bounds();
		bounds.center = new Vector3(kCharacter.transform.position.x - kCam.transform.position.x,
		                            kCharacter.transform.position.y - kCam.transform.position.y,
		                            kCharacter.transform.position.z - kCam.transform.position.z);
		bounds.extents = kCharacter.gameObject.collider.bounds.extents;
		bounds.size = kCharacter.gameObject.collider.bounds.size;

		bool bret = GeometryUtility.TestPlanesAABB(planes, bounds);
		return bret;
	}

	static public bool IsPointInsideCamera(Vector3 kPoint, Camera kCam)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(kCam);

		bool bRet = true;

		for (int i = 0; i < planes.Length; ++i) 
		{
			bRet &= planes[i].GetSide(kPoint);
		}

		return bRet;
	}
}

