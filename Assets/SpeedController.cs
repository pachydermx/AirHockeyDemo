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
	        rb.velocity = rb.velocity.normalized* (-10.0f); // yama 0413 場外時，逆方向からリスタート
	    }
        
        // yama 0413 コメントアウト解除
	    if (rb.velocity.magnitude > speed_limit) // yama 0413 速度制限
	    {
	        rb.velocity = rb.velocity.normalized*speed_limit;
	    }
        
	}
}
