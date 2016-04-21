using UnityEngine;
using System.Collections;

public class Stamp : MonoBehaviour {

	private Animator leftstamp;

	private bool Leftdown = false;
	private bool LeftUp = false;

	public GameObject l_win;
	public GameObject l_lose;

	private GameObject LeftStp;

	private int score = 0;

	// Use this for initialization
	void Start () {
	
		leftstamp = GetComponent<Animator> ();

		GetComponent<Animator> ().SetBool ("Leftdown", false);
		GetComponent<Animator> ().SetBool ("Leftup", false);

		l_win = GameObject.Find("L_WIN");
		l_lose = GameObject.Find("L_LOSE");

	    LeftStp = GameObject.Find ("LeftStamp");

		l_win.SetActive (false);
		l_lose.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void StampDown(int judge)
	{
		GetComponent<Animator> ().SetBool ("Leftdown", true);

		if (judge == 1)
		{
			score = 1;
		}

		else if (judge == 2)
		{
			score = 2;
		}

		else
		{
			Debug.Log ("SCORE ERROR");
		}
	}

	void ResultAppear()
	{
		if (score == 1)
		{
			l_win.SetActive (true);
		}

		else if (score == 2)
		{
			l_lose.SetActive (true);

		}


		GetComponent<Animator> ().SetBool ("Leftup", true);

	}

	void ResetStamp()
	{
		l_win.SetActive (false);
		l_lose.SetActive (false);

		GetComponent<Animator> ().SetBool ("Leftdown", false);
		GetComponent<Animator> ().SetBool ("Leftup", false);
	}

}
