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
        manager.SendMessage("sendPosition", Gimmick.name);

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        manager.SendMessage("sendPosition", Gimmick.name);
    }
}
