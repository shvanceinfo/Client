using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    float destroyTime =20.0f;

    void Start () 
    {
        Destroy (gameObject, destroyTime);
    }
}
