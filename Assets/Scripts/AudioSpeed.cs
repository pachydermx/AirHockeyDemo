using UnityEngine;
using System.Collections;

public class AudioSpeed : MonoBehaviour {

	AudioSource bgm;
	public AudioClip bgmclip;

	private GameObject manager;
	private Main main_s;



	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Manager");
		main_s = manager.GetComponent<Main>();

		bgm = gameObject.GetComponent<AudioSource>();
		bgm.clip = bgmclip;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    bgm.mute = false;

		if (main_s.remaining_time >= 10) {
			bgm.pitch = 1.0f;
		} else if (main_s.remaining_time < 10) {
			bgm.pitch = 1.3f;
		}

	    if (main_s.remaining_time < 0)
	    {
	        bgm.mute = true;
	    }
	
	}
}
