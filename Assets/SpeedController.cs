using UnityEngine;
using System.Collections;

public class SpeedController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform t;
    public float speed_limit = 50f;

	// Use this for initialization
	void Start ()
	{
	    rb = this.gameObject.GetComponent<Rigidbody2D>();
	    t = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Mathf.Abs(t.position.x) > 10f || Mathf.Abs(t.position.y) > 10f)
	    {
	      t.position = Vector3.zero;
	        rb.velocity = rb.velocity.normalized*speed_limit;
	    }
        /*
	    if (rb.velocity.magnitude > speed_limit)
	    {
	        //rb.velocity = rb.velocity.normalized*speed_limit;
	    }
        */
	}
}
