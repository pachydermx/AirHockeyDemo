using UnityEngine;
using System.Collections;

public class Accelerator : MonoBehaviour {
	public GameObject manager;
	private float move_speed = 2.0f;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {

		// control
		if (Input.GetKey (KeyCode.W)){
			transform.Translate(Vector3.up * move_speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)){
			transform.Translate(Vector3.up * -move_speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)){
			transform.Translate(Vector3.right * move_speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)){
			transform.Translate(Vector3.right * -move_speed * Time.deltaTime);
		}
	
		if (Input.GetKey (KeyCode.Q)){
			transform.Rotate(Vector3.forward * move_speed * 30 * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.E)){
			transform.Rotate(Vector3.forward * -move_speed * 30 * Time.deltaTime);
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		Vector2 direction = new Vector2(-transform.rotation.z, transform.rotation.w);
		Debug.Log(direction);
		manager.SendMessage("Accelerate", direction);
	}
}
