﻿using UnityEngine;
using System.Collections;

public class ColliderMessenager : MonoBehaviour {
	public int player_id;
	private GameObject manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {
		manager.SendMessage("Player", player_id);
	}
}