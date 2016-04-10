using UnityEngine;
using System.Collections;

public class WaitDeal : MonoBehaviour {

    public float dealtime;
    void Awake()
    {
        Destroy(gameObject, dealtime);
    }
}
