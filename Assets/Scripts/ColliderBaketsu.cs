using UnityEngine;
using System.Collections;

public class ColliderBaketsu : MonoBehaviour { 
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
        manager.SendMessage("sendB2position");

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        manager.SendMessage("sendB2position");
    }
}
