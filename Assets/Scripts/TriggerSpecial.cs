using UnityEngine;
using System.Collections;

public class TriggerSpecial : MonoBehaviour
{
    private int flag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    flag = Random.Range(1, 5); // yama 0419 発動確率
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ball"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude > 30)
            {
                if (flag == 1)
                {
                    collision.gameObject.SendMessage("Set_Flag", 1);
                }
                else if (flag == 2)
                {
                    collision.gameObject.SendMessage("Set_Flag", 2);
                }
                //if (Mathf.Abs(rb.velocity.x) >= 25 && Mathf.Abs(rb.velocity.y) <= 5)
                
                    //collision.gameObject.SendMessage("Set_Flag", 1);
                
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ball"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb.velocity.magnitude > 30)
            {

                if (collision.gameObject.name.Contains("Ball"))
                {
                    if (flag == 1)
                    {
                        collision.gameObject.SendMessage("Set_Flag", 1);
                    }
                    else if (flag == 2)
                    {
                        collision.gameObject.SendMessage("Set_Flag", 2);
                    }
                }

                //if (Mathf.Abs(rb.velocity.x) >= 25 && Mathf.Abs(rb.velocity.y) <= 5)

                //collision.gameObject.SendMessage("Set_Flag", 1);

            }
        }
    }

    void OnCollisionExit2D(Collision collision)  // yama 0419 線からパックが離れたか判定
    {
        if (collision.gameObject.name.Contains("Ball"))
        {
            collision.gameObject.SendMessage("TouchCheck", 1);
        }
    }
}
