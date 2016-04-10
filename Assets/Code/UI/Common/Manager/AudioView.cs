using UnityEngine;
using System.Collections;
using manager;
public class AudioView : MonoBehaviour {

    public AudioClip Clip;
    private void Awake()
    {
        AudioManager.Instance.PlayMusic(this.gameObject, Clip);
    }
}
