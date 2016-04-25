using UnityEngine;
using System.Collections;

public class ColliderPack : MonoBehaviour {
    public GameObject canvas;
    private GameObject box;
    private int bomb_flag;
    private bool ready = false;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        box = GameObject.Find("ItemBox1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Canvas" && bomb_flag == 1)
        {
            canvas.SendMessage("DoExplode");
            //Debug.Log("bomb");
            box.GetComponent<ItemBoX>().bomb_flag = 0;
            // yama 0425 爆発したらボム状態解除 
            this.gameObject.transform.FindChild("bombpack").gameObject.SetActive(false);
            bomb_flag = 0;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Canvas" && bomb_flag == 1)
        {
            canvas.SendMessage("DoExplode");
        }
    }

    void setFlag(int flag)
    {
        if (ready)
        {
            bomb_flag = flag;
            this.gameObject.transform.FindChild("bombpack").gameObject.SetActive(true);
            ready = false;
        }
    }

    void getReadyFlag(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            ready = true;
        }
    }
}
