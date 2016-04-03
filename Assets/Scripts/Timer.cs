using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    private int stage_duration;
    private GameObject[] text_object;
    private TextMesh[] text_mesh;
    private Animator[] text_animation;

    AudioSource audiosource;
    public AudioClip audioclip;

	// Use this for initialization
	void Start () {
        // init
        // get all child
        text_object = new GameObject[transform.childCount];
        text_mesh = new TextMesh[transform.childCount];
        text_animation = new Animator[transform.childCount];

        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.clip = audioclip;

        for(int i = 0; i < transform.childCount; i++)
        {
            text_object[i] = transform.GetChild(i).gameObject;
            text_mesh[i] = text_object[i].GetComponent<TextMesh>();
            text_animation[i] = text_object[i].GetComponent<Animator>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    void SetStageDuration(int duration)
    {
        stage_duration = duration;
    }

    public void ShowText(string to_display, bool with_animation, float count)
    {
        for(int i = 0; i < text_object.Length; i++)
        {
            text_mesh[i].text = to_display;
            if (with_animation)
            {
                text_object[i].SetActive(false);
                text_object[i].SetActive(true);

                if(count > 0)
                {
                    audiosource.Play();
                }

                else if(count == 0)
                {
                    Debug.Log("Finish count: " + count);
                }
            }
        }
    }
}
