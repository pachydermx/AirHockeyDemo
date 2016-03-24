using UnityEngine;
using System.Collections;

public class CloneDelete : MonoBehaviour {
    private GameObject box;

    // Use this for initialization
    void Start()
    {
        box = GameObject.Find("ItemBox");
        
       
        StartCoroutine(Delete());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(8.0f);
        box.SendMessage("flag_reset");
        Destroy(this.gameObject);
    }
}
