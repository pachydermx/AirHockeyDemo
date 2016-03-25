using UnityEngine;
using System.Collections;

public class ItemBoX : MonoBehaviour {
    public const float angle = 150;

    public GameObject Pack; // yama 0323
    public GameObject canvas;
    public GameObject p_another; // yama 0324 clone

    private Vector2 p_scale;
    private Rigidbody2D p_rb;

    //private int range;
    
    private int g_flag = 0;

    private float time;
    private float temp_time;
    private int t_flag = 0;
    public int clone_flag = 0;
    public int bomb_flag = 0;

    Ball ball;
    //Canvas script;

	// Use this for initialization
	void Start () {
        /*
        Pack = GameObject.Find("Ball_0");
        canvas = GameObject.Find("Canvas");
        p_scale = Pack.GetComponent<SpriteRenderer>().bounds.size;
        p_rb = Pack.GetComponent<Rigidbody2D>();
        */
        time = 0;
        bomb_flag = 0;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time >= 5.0)
        {
            time = 0;
            g_flag++;
            g_flag %= 3;
        }

        if(t_flag == 1 && clone_flag == 0)
        {
            float distance = Vector3.Distance(Pack.transform.position, this.gameObject.transform.position);
            if(distance <= 0.55) // yama 0323 微調整必要
            {
                float s_x = p_rb.velocity.x;
                float s_y = p_rb.velocity.y;

                temp_time = 0;
                t_flag = 0;
                clone_flag = 1;

                //Debug.Log("copy OK");

                /*
                p_rb.velocity = new Vector2(s_x * Mathf.Cos(angle) + s_y * Mathf.Sin(angle), s_x * (-Mathf.Sin(angle)) + s_y * Mathf.Cos(angle));
                GameObject Pack_b = (GameObject)Instantiate(p_another, Vector3.zero, Quaternion.identity);
                Pack_b.transform.position = Pack.transform.position;
                Rigidbody2D b_rb = Pack_b.GetComponent<Rigidbody2D>();
                b_rb.velocity = new Vector2(s_x * Mathf.Cos(-angle) + s_y * Mathf.Sin(-angle), s_x * (-Mathf.Sin(-angle)) + s_y * Mathf.Cos(-angle));
                ball = Pack_b.GetComponent<Ball>();
                ball.canvas = GameObject.Find("Canvas");
                ball.manager = GameObject.Find("Manager");
                */
                canvas.SendMessage("AddNewBall");
                //script = GetComponent<Canvas>();
                //ball.canvas.SendMessage("DrawRound");
                //ball.c_script = GameObject.Find("Canvas").GetComponent<Canvas>();
                //ball.canvas.SendMessage("")
                //Debug.Log("c_script.range:" + ball.c_script.default_range);
                
                temp_time = 0;
                t_flag = 0;
                clone_flag = 1;
            }
        }

        if(clone_flag == 1)
        {
            temp_time += Time.deltaTime;
            if(temp_time >= 3.0f)
            {
                clone_flag = 0;
            }
        }

        else if(bomb_flag==1)
        {
            canvas.SendMessage("DoBig", 95);
            Debug.Log("bomb: " + bomb_flag);
            bomb_flag = 2;
            Debug.Log("bomb: " + bomb_flag);

            if (Input.GetKeyDown(KeyCode.B))
            {
                canvas.SendMessage("DoExplode");
                Debug.Log("explosion");
            }

            bomb_flag = 0;
        }

        else
        {

        //else if(t_flag == 2)
        //{
            //bomb_flag = 1;

            if (bomb_flag == 1)
            {
            //canvas.SendMessage("DoExplode");
            //Pack.SendMessage("setFlag", bomb_flag);
                //Debug.Log("bomb_flag:"+bomb_flag);
                
                //t_flag = 0;
            }
        }
	}

    void ItemUse()
    {
        if (g_flag == 0) // yama 0323 巨大化
        {
            canvas.SendMessage("DoBig", 95);
            //Pack.transform.localScale = new Vector3(1.5f * p_scale.x, 1.5f * p_scale.y, 1);
        }
        else if (g_flag == 1) // yama 0323 分身
        {
            t_flag = 1;
        }else if(g_flag == 2) // yama 0325 爆発
        {
            bomb_flag = 1;
        }
        /*else { 
            if (g_flag == 1) // yama 0323 分身
        }


        //GameObject Pack_b = (GameObject)Instantiate(p_another, Vector3.zero, Quaternion.identity);
        //p_rb.velocity = new Vector2(s_x * Mathf.Cos(angle) + s_y * Mathf.Sin(angle), s_x * (-Mathf.Sin(angle)) +s_y * Mathf.Cos(angle)); 

        //t_flag = 1;//分身フラグ
        //}

        bomb_flag = 1;
        */
    }

    void flag_reset()
    {
        clone_flag = 0;
        Debug.Log("flag = " + clone_flag);
        //t_flag = 1;
        //}

        bomb_flag = 1;
    }
}
