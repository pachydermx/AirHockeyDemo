﻿using System;
using UnityEngine;
using System.Collections;

public class ItemBoX : MonoBehaviour {
    public const float angle = 150;

    public GameObject Pack; // yama 0323
    public GameObject canvas;

    public Vector3 p_scale;
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

    private int get_scale = 0;

    private GameObject current_gameobject;

	// Use this for initialization
	void Start () {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>(); //0328 tanaka icon

        canvas = GameObject.Find("Canvas");

        time = 0;
        bomb_flag = 0;

        g_flag = 0; // yama 0325 確認用
        SetObject(g_flag);
    }
	
	// Update is called once per frame
	void Update () {

	    if (get_scale == 0)
	    {
	        get_scale++;
            p_scale = new Vector3(Pack.transform.localScale.x, Pack.transform.lossyScale.y, Pack.transform.localScale.z); // yama 0413 元サイズ取得
        }

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

                //Debug.Log("OK");
                canvas.SendMessage("AddNewBall");   
            //}
        }

        if(bomb_flag == 1)
        {
            current_gameobject = this.gameObject.GetComponent<ColliderGimmick>().current_collider;
            //Debug.Log(Pack);
            //Pack.SendMessage("getReadyFlag", current_gameobject);
            //Pack.SendMessage("setFlag", bomb_flag);
            current_gameobject.SendMessage("getReadyFlag", current_gameobject);
            current_gameobject.SendMessage("setFlag", bomb_flag);
            //DefaultBall.SendMessage("changePack");
            bomb_flag = 0;
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
            if (this.gameObject.GetComponent<ColliderGimmick>().current_collider.name.Contains("Ball_0"))
            {
                canvas.SendMessage("DoBig", 95);
                Pack.transform.localScale = new Vector3(1.5f * p_scale.x, 1.5f * p_scale.y, p_scale.z);
                
            }
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
        //Debug.Log("flag = " + clone_flag);
    }

    public void setBallOriginal(GameObject ball)
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

    
    void reverse_size(GameObject pack)
    {
        //pack.transform.localScale = p_scale;
        Debug.Log(pack.transform.localScale.z);
    }
 

    void reset()
    {
        get_scale = 0;
        g_flag = 0;
        t_flag = 0;
        bomb_flag = 0;
        clone_flag = 0;
    }
}
