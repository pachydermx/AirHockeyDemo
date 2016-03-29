using UnityEngine;
using System.Collections;

public class ItemBoX : MonoBehaviour {
    public const float angle = 150;

    public GameObject Pack; // yama 0323
    public GameObject canvas;

    public GameObject DefaultBall;

    public Vector2 p_scale;
    private Rigidbody2D p_rb;

    //private int range;
    
    private int g_flag = 0;

    private float time;
    private float temp_time;
    private int t_flag = 0;
    public int clone_flag = 0;
    public int bomb_flag = 0;

    public GameObject bomb_obj;
    public GameObject split_obj;
    public GameObject expand_obj;

    Ball ball;
    //Canvas script;

    //Icon 0328 tanaka
    SpriteRenderer MainSpriteRenderer;
    public Sprite BigIcon;
    public Sprite DoubleIcon;
    public Sprite BombIcon;

	// Use this for initialization
	void Start () {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>(); //0328 tanaka icon

        canvas = GameObject.Find("Canvas");
        DefaultBall = GameObject.Find("ball");

        time = 0;
        bomb_flag = 0;

        g_flag = 0; // yama 0325 確認用
        SetObject(g_flag);
    }
	
	// Update is called once per frame
	void Update () {
        
        time += Time.deltaTime;
        if(time >= 5.0)
        {
            time = 0;
            g_flag++;
            g_flag %= 3;
            SetObject(g_flag);
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
            DefaultBall.SendMessage("changePack");
        }

        //0328 tanaka icon
        if(g_flag == 0)
        {
            MainSpriteRenderer.sprite = BigIcon;
        }
        else if(g_flag == 1)
        {
            MainSpriteRenderer.sprite = DoubleIcon;
        }
        else if(g_flag == 2)
        {
            MainSpriteRenderer.sprite = BombIcon;
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

    void SetObject(int g_flag)
    {
        // reset objects
        bomb_obj.SetActive(false);
        split_obj.SetActive(false);
        expand_obj.SetActive(false);

        switch (g_flag)
        {
            case 0:
                expand_obj.SetActive(true);
                break;
            case 1:
                split_obj.SetActive(true);
                break;
            case 2:
                bomb_obj.SetActive(true);
                break;
            default:
                break;
        } 
    }
}
