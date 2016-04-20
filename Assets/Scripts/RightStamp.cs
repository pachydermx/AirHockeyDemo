using UnityEngine;
using System.Collections;

public class RightStamp : MonoBehaviour {

	private Animator rightstamp;

	private bool Rightdown = false;
	private bool RightUp = false;

	public GameObject r_win;
	public GameObject r_lose;

	private GameObject RightStp;

	private int score = 0;

	// Use this for initialization
	void Start () {

		rightstamp = GetComponent<Animator> ();

		GetComponent<Animator> ().SetBool ("Rightdown", false);
		GetComponent<Animator> ().SetBool ("Rightup", false);

		r_win = GameObject.Find("R_WIN");
		r_lose = GameObject.Find("R_LOSE");

		RightStp = GameObject.Find ("RightStamp");

		r_win.SetActive (false);
		r_lose.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}

	void StampDown(int judge)
	{
		GetComponent<Animator> ().SetBool ("Rightdown", true);


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
			r_lose.SetActive (true);
		}

		else if (score == 2)
		{
			r_win.SetActive (true);
		}
			
		GetComponent<Animator> ().SetBool ("Rightup", true);
	}

	void ResetStamp()
	{
		r_win.SetActive (false);
		r_lose.SetActive (false);

		GetComponent<Animator> ().SetBool ("Rightdown", false);
		GetComponent<Animator> ().SetBool ("Rightup", false);

	}
}
