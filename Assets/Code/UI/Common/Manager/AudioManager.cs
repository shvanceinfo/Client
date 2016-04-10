using UnityEngine;
using System.Collections;

namespace manager
{
    public class AudioManager
    {
        static AudioManager _instance;
        private bool _audioActive;
        private bool _musicActive;
        private GameObject _musicObj;
        private AudioManager()
        {
            _audioActive = true;
            _musicActive = true;
        }
        public void PlayAudio(GameObject obj, AudioClip audio)
        {
            if (AudioActive)
            {
                if (obj != null)
                {
                    if (obj.audio && audio)
                    {
                        obj.audio.PlayOneShot(audio);
                    }
                }
            }
        }
        public void PlayMusic(GameObject obj, AudioClip audio)
        {
            if (MusicActive)
            {
                if (obj != null)
                {
                    if (obj.audio && audio)
                    {
                        _musicObj = obj;
                        obj.audio.loop = true;
                        obj.audio.clip = audio;
                        obj.audio.Play();
                    }
                }
            }
        }

        public static AudioManager Instance
        {
            get {
                if (_instance==null)
                {
                    _instance = new AudioManager();
                }
                return AudioManager._instance; 
            }
        }

        public bool AudioActive
        {
            get { return _audioActive; }
            set { _audioActive = value; }
        }
        public bool MusicActive
        {
            get { return _musicActive; }
            set {
                if (_musicObj!=null)
                {
                    _musicObj.SetActive(value);
                }
                _musicActive = value;
            }
        }
    }
}
