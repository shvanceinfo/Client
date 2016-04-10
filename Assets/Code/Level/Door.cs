using UnityEngine;
using System.Collections;

public class Door : LevelEventHandler {
	
	bool is_open = false;
	AudioClip clip_open;
	AudioClip clip_close;

	// Use this for initialization
	void Start ()
	{
	    if (BundleMemManager.debugVersion)
	    {
            clip_open = BundleMemManager.Instance.loadResource(PathConst.AUDIO_DOOR_OPEN) as AudioClip;
            clip_close = BundleMemManager.Instance.loadResource(PathConst.AUDIO_DOOR_CLOSE) as AudioClip;
	    }
	    else
	    {
            BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_DOOR_OPEN,
           (obj) => { clip_open = obj as AudioClip; });
            BundleMemManager.Instance.loadPrefabViaWWW<AudioClip>(EBundleType.eBundleMusic, PathConst.AUDIO_DOOR_CLOSE,
                (obj) => { clip_close = obj as AudioClip; });
	    }      
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void openDoor() {
		AnimationState animState = animation["door_anim"];
		animState.speed = 1.0f;
		animation.Play("door_anim");
		is_open = true;
		
		
		if (CharacterPlayer.sPlayerMe.audio) {
			CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip_open);
		}
	}
	
	void closeDoor() {
		AnimationState animState = animation["door_anim"];
		animState.speed = -1.0f;
		animState.time = animState.length;
		animation.Play("door_anim");
		is_open = false;
		
		
		if (CharacterPlayer.sPlayerMe.audio) {
			CharacterPlayer.sPlayerMe.audio.PlayOneShot(clip_close);
		}
	}
	
	public override void onTrigger() {
		
		if (!active)
			return;
		
		if (is_open) {
			closeDoor();
		}
		else {
			openDoor();
		}
	}
}
