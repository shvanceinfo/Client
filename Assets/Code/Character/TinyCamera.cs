using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TinyCamera : MonoBehaviour
{
    //public static tinyCamera sTinyCamera;
    public Camera activeCamera;
    RenderTexture _rt;

    void Awake()
    {
        //sTinyCamera = this;

        _rt = new RenderTexture(32, 32, 24);
        _rt.Create();

        activeCamera.targetTexture = _rt;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void PreLoadObject(GameObject obj)
    {
        SnapshotObject(obj);
    }

    void SnapshotObject(GameObject obj)
    {
        //move camera into position 
        Vector3 pos = obj.transform.position;
        pos += new Vector3(0f, .5f, 0f);
        activeCamera.transform.position = pos;
        activeCamera.transform.LookAt(obj.transform.position);

        activeCamera.Render();

        RenderTexture.active = _rt;
        activeCamera.targetTexture = null;
        RenderTexture.active = null;
    }
}