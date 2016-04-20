using UnityEngine;
using System.Collections;

public class NPAnimation : MonoBehaviour
{
    private Animator npa1;
    private Animator npa2;

    private bool NPA1 = false;
    private bool NPA2 = false;

	// Use this for initialization
	void Start ()
	{

	    npa1 = GetComponent<Animator>();
	    npa2 = GetComponent<Animator>();

        GetComponent<Animator>().SetBool("NPA1", false);
        GetComponent<Animator>().SetBool("NPA2", false);

    }

    // Update is called once per frame
    void Update () {
	
	}

    void NewPage()
    {
        GetComponent<Animator>().SetBool("NPA1", true);
        GetComponent<Animator>().SetBool("NPA2", true);
    }


    void ResetNewPage()
    {
        GetComponent<Animator>().SetBool("NPA1", false);
        GetComponent<Animator>().SetBool("NPA2", false);
    }
}
