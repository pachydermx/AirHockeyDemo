using UnityEngine;
using System.Collections;

public class SpecialShoot : MonoBehaviour {
    private Rigidbody2D rb;
    private Rigidbody2D rb_original;
    private Vector2 pos_original;
    private Transform t;
    public GameObject canvas;
    int shoot;
    float angle = 0;
    float x;
    float y;

    int count = 0;
    int zig = 250;
    Vector3 dir;

    int zig_angle = 315;
    int first_flag = 0;

    private int check = 0;

    public AudioClip invocationclip;

    // Use this for initialization
    void Start () {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        t = this.gameObject.transform;
        canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update () {
	    if(shoot == 1)
        {
            CircleShoot();
        }else if(shoot == 2)
        {
            ZigShoot();
        }
	}

    void Set_Flag(int flag)
    {
        if (shoot == 0)
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(invocationclip, 1.0f);

            if (flag == 1)
            {
                shoot = flag;
                rb_original = rb;
                x = rb.velocity.x;
                y = rb.velocity.y;
                Debug.Log("CircleShoot");
                pos_original = new Vector2(t.transform.position.x, t.transform.position.y);
                canvas.SendMessage("ChangeRange", 40);
            }
            else if (flag == 2)
            {
                shoot = flag;
                rb_original = rb;
                x = rb.velocity.x * 0.8f;
                y = rb.velocity.y * 0.8f;
                dir = new Vector3(x*Mathf.Cos(zig_angle) - y*Mathf.Sin(zig_angle),
                    x*Mathf.Sin(zig_angle) + y*Mathf.Cos(zig_angle), 0);
                canvas.SendMessage("ChangeRange", 40);
                Debug.Log("ZigShoot");
            }

            //check = 1;
        }

    }

    void CircleShoot()
    {
        // yama 0419 直線方向のみ
        
        float x = Mathf.Sin(angle) * 1.0f + (rb_original.velocity.x / 35) * angle;
        float y = Mathf.Cos(angle) * 1.0f + (rb_original.velocity.y / 35) * angle;
        transform.position = new Vector3(pos_original.x + x, pos_original.y + y, t.transform.position.z);
        angle += 0.2f;             

        // yama 0419 velocityバージョン
        /*
        if(rb_original.velocity.x >= 0)
        {
            angle = 5.0f;
        }
        else
        {
            angle = -5.0f;
        }
        
        float vx = Mathf.Sin(angle) * 1.0f + rb_original.velocity.x;// + rb_original.velocity.x;
        float vy = Mathf.Cos(angle) * 1.0f + rb_original.velocity.y;// + rb_original.velocity.y;
        */
        /*
        angle = 0.05f;
        float vx = Mathf.Cos(angle) * x - Mathf.Sin(angle) * y + (rb_original.velocity.x / 1000);// + rb_original.velocity.x;
        float vy = Mathf.Sin(angle) * x + Mathf.Cos(angle) * y + (rb_original.velocity.y / 1000);// + rb_original.velocity.y;

        Vector3 direction = new Vector3(x, y, 0);
        rb.velocity = direction;

        x = vx;
        y = vy;
        */
        //angle += 0.001f;
    }

    void ZigShoot()
    {
        if(first_flag == 0)
        {
            if(count >= 750)
            {
                count = 0;
                zig_angle = -zig_angle;
                first_flag = 1;
                dir = new Vector3(x * Mathf.Cos(zig_angle) - y * Mathf.Sin(zig_angle), x * Mathf.Sin(zig_angle) + y * Mathf.Cos(zig_angle), 0);
            }
        }
        if(count >= 1500)
        {
            count = 0; 
            zig_angle = -zig_angle;
            dir = new Vector3(x * Mathf.Cos(zig_angle) - y * Mathf.Sin(zig_angle), x * Mathf.Sin(zig_angle) + y * Mathf.Cos(zig_angle), 0);
        }
        rb.velocity = dir;
        count += zig;
    }

    // yama 0419 パックが線から離れているか判定
    void TouchCheck(int c) 
    {
        if (shoot != 0)
        {
            check = c;
            Debug.Log("Check OK");
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //yama 0419 線に触れた瞬間ているかもしくは線から離れているか判定
        if (!collision.gameObject.name.Contains("StrokeOffset") || check == 1)
        {
            if (shoot != 0)
            {

                angle = 0;
                count = 0;
                check = 0;
                if (shoot == 1)
                {
                    rb.velocity = rb_original.velocity;
                    t.transform.position = this.gameObject.transform.position;
                }
                if (shoot == 2)
                {
                    rb.velocity = rb_original.velocity*1.25f;
                }
                canvas.SendMessage("ChangeRange", 50);
                shoot = 0;
                Debug.Log("release");
            }
        }
        else
        {
            TouchCheck(1);
        } 
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //yama 0419 線に触れた瞬間ているかもしくは線から離れているか判定
        if (!collision.gameObject.name.Contains("StrokeOffset") || check == 1)
        {
            if (shoot != 0)
            {
                
                angle = 0;
                count = 0;
                check = 0;
                if (shoot == 1)
                {
                    rb.velocity = rb_original.velocity;
                    t.transform.position = this.gameObject.transform.position;
                }
                if (shoot == 2)
                {
                    rb.velocity = rb_original.velocity * 1.25f;
                }
                canvas.SendMessage("ChangeRange", 50);
                shoot = 0;
                Debug.Log("release");
            }            
        }
        else
        {
            TouchCheck(1);
        }
    }
    
}
