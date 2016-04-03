using UnityEngine;
using System.Collections;

public class BallClliderSound : MonoBehaviour {
    public AudioClip collision_sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(collision_sound, 0.3f);
	}

}
