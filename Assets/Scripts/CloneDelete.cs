using UnityEngine;
using System.Collections;

public class CloneDelete : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Delete());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(3.0f);
       
        Destroy(this.gameObject);
    }
}
