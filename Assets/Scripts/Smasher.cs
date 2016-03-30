using UnityEngine;
using System.Collections;

public class Smasher : MonoBehaviour {
    private Vector3 last_position;
    private Rigidbody2D rb;

    private int frame_counter = 0;
    private float time_counter = 0.0f;
    private float last_framerate = 0.0f;
    private float refresh_time = 0.3f;

	// Use this for initialization
	void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
        // calculate framerate
        if (time_counter < refresh_time)
        {
            time_counter += Time.deltaTime;
            frame_counter++;
        } else
        {
            last_framerate = (float)frame_counter / time_counter;
            frame_counter = 0;
            time_counter = 0.0f;
        }

        // remember position
        last_position = this.gameObject.transform.position;
	}

    void Move (Vector3 position)
    {
        this.gameObject.transform.position = position;
        if (rb == null)
        {
            rb = this.gameObject.GetComponent<Rigidbody2D>();
        }
        rb.velocity = (position - last_position) * last_framerate / 10;
        //Debug.Log(last_framerate + ", " + rb.velocity.magnitude);
    }
}
