using UnityEngine;
using System.Collections;

public class ItemBoX : MonoBehaviour {
    public const float angle = 150;

    public GameObject Pack; // yama 0323
    public GameObject canvas;

    public Vector2 p_scale;
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
        canvas = GameObject.Find("Canvas");

        time = 0;
        bomb_flag = 0;

        g_flag = 0; // yama 0325 確認用
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
            /*
            float distance = Vector3.Distance(Pack.transform.position, this.gameObject.transform.position);
            if(distance <= 1.0)
            { 
            */
                temp_time = 0;
                t_flag = 0;
                clone_flag = 1;

                Debug.Log("OK");
                canvas.SendMessage("AddNewBall");   
            //}
        }

        if(bomb_flag == 1)
        {
            Pack.SendMessage("setFlag", bomb_flag);
        }
	}

    void ItemUse()
    {
       
               
        if (g_flag == 0) // yama 0323 巨大化
        {
            canvas.SendMessage("DoBig", 95);
            Pack.transform.localScale = new Vector3(1.5f * p_scale.x, 1.5f * p_scale.y, 1);
        }
        else if (g_flag == 1) // yama 0323 分身
        {
            if (clone_flag != 1)
            {
                t_flag = 1;
            }
        }else if(g_flag == 2) // yama 0325 爆発
        {
            bomb_flag = 1;
        }
    }

    void flag_reset()
    {
        clone_flag = 0;
        Debug.Log("flag = " + clone_flag);
    }

    void setBallOriginal(GameObject ball)
    {
        Pack = ball;
        p_scale = Pack.transform.localScale;
        p_rb = GetComponent<Rigidbody2D>();
    }
}
