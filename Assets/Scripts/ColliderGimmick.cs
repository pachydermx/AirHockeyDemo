// yama 0318 GimmickTrigger 

using UnityEngine;
using System.Collections;

public class ColliderGimmick : MonoBehaviour {
    public GameObject Gimmick;
    private GameObject manager;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("on");
        manager.SendMessage("sendPosition", Gimmick.name);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("On");
        manager.SendMessage("sendPosition", Gimmick.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == "Pack")
        //{
            //Debug.Log("Trigger"+ other.name);
            
            manager.SendMessage("sendPosition", Gimmick.name);
        //}
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger");
        manager.SendMessage("sendPosition", Gimmick.name);
    }
    
}
