using UnityEngine;
using System.Collections;

public class Accelerator : MonoBehaviour {
	public GameObject manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D collider) {
		Vector2 direction = new Vector2(-transform.rotation.z, transform.rotation.w);
		Debug.Log(direction);
		manager.SendMessage("Accelerate", direction);
	}
}
