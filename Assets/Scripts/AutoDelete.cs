using UnityEngine;
using System.Collections;

public class AutoDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Delete());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Delete () {
		yield return new WaitForSeconds(0.6F); 

        Destroy(this.gameObject);
	}
}
