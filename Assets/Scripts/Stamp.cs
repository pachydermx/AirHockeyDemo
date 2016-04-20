using UnityEngine;
using System.Collections;

public class Stamp : MonoBehaviour {

	private Animator leftstamp;
	//private Animator rightstamp;


	private bool Leftdown = false;
	private bool LeftUp = false;
	//private bool Rightdown = false;
	//private bool RightUp = false;

	public GameObject l_win;
	public GameObject l_lose;
	//public GameObject r_win;
	//public GameObject r_lose;

	private GameObject LeftStp;
	//public GameObject RightStp;

	private int score = 0;

	// Use this for initialization
	void Start () {
	
		leftstamp = GetComponent<Animator> ();
		//rightstamp = GetComponent<Animator> ();

		GetComponent<Animator> ().SetBool ("Leftdown", false);
		GetComponent<Animator> ().SetBool ("Leftup", false);
		//GetComponent<Animator> ().SetBool ("Rightdown", false);
		//GetComponent<Animator> ().SetBool ("Rightup", false);

		l_win = GameObject.Find("L_WIN");
		l_lose = GameObject.Find("L_LOSE");
		//r_win = GameObject.Find("R_WIN");
		//r_lose = GameObject.Find("R_LOSE");


	    LeftStp = GameObject.Find ("LeftStamp");
		//RightStp = GameObject.Find ("RightStamp");

		l_win.SetActive (false);
		l_lose.SetActive (false);
		//r_win.SetActive (false);
		//r_lose.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void StampDown(int judge)
	{
		GetComponent<Animator> ().SetBool ("Leftdown", true);
		//GetComponent<Animator> ().SetBool ("Rightdown", true);


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
			//r_lose.SetActive (true);
		}

		else if (score == 2)
		{
			l_lose.SetActive (true);
			//r_win.SetActive (true);
		}


		GetComponent<Animator> ().SetBool ("Leftup", true);
		//GetComponent<Animator> ().SetBool ("Rightup", true);
	}

	void ResetStamp()
	{
		l_win.SetActive (false);
		l_lose.SetActive (false);
		//r_win.SetActive (false);
		//r_lose.SetActive (false);

		GetComponent<Animator> ().SetBool ("Leftdown", false);
		GetComponent<Animator> ().SetBool ("Leftup", false);
		//GetComponent<Animator> ().SetBool ("Rightdown", false);
		//GetComponent<Animator> ().SetBool ("Rightup", false);

	}

}
