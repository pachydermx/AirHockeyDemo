using UnityEngine;
using System.Collections;

public class CloneDelete : MonoBehaviour {
    private GameObject box;
    private GameObject box2;

    // Use this for initialization
    void Start()
    {
        box = GameObject.Find("ItemBox1");
        box2 = GameObject.Find("ItemBox2");
        
       
        StartCoroutine(Delete());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(3.0f);
        
        yield return new WaitForSeconds(8.0f);
        box.SendMessage("flag_reset");
        box2.SendMessage("flag_reset");
        Destroy(this.gameObject);
    }
}
