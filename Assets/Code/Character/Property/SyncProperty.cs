using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// 同步属性 主机发来的数据同步到这里
public class SyncProperty : MonoBehaviour
{

    private Character m_kOwner;

    void Awake()
    {
        m_kOwner = gameObject.GetComponent<Character>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
}
