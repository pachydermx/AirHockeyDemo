using System;
using UnityEngine;
using System.Collections;

public class SprayController : MonoBehaviour {
    private float s_time;
    private float temp_time;

    public int direction;
    public int flag;

    private GameObject manager;
    private ColliderGimmick collider_gimmick;

    public int id;

    public GameObject particle_system;
    public float particle_angle;

    private float particle_duration;

	// Use this for initialization
	void Start () {
        s_time = 0;
        direction = 0;
        flag = 0;
        manager = GameObject.Find("Manager");
        //manager.SendMessage("controlSpray", direction);
	    particle_duration = particle_system.GetComponent<ParticleSystem>().duration;

	    collider_gimmick = this.gameObject.GetComponent<ColliderGimmick>();
	}

    void create_particle()
    {
        // create subobject
        GameObject particle_object = (GameObject)Instantiate(particle_system, this.transform.position, Quaternion.Euler(0, 0, particle_angle));
        // configure
        particle_object.transform.parent = this.transform;
        particle_object.transform.localScale = new Vector3(1, 1, 1);
        ParticleSystem ps = particle_object.GetComponent<ParticleSystem>();
        try
        {
            ps.startColor = collider_gimmick.current_collider.GetComponent<Ball>().GetColor();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        // play
        ps.Play();
        // assign to destory the object
        StartCoroutine(destory_particle(particle_object));
    }

    private IEnumerator destory_particle(GameObject obj)
    {
        yield return new WaitForSeconds(particle_duration);
        Destroy(obj);
    }
	
	// Update is called once per frame
	void Update () {
        s_time += Time.deltaTime;

        if (flag == 1){
            if(s_time >= 3.0f)
            {
                s_time = temp_time;
                flag = 0;
            }           
        }
        else
        {
            //s_time += Time.deltaTime;
            if (s_time >= 5.0f)
            {
                s_time = 0;
                direction++;
                direction %= 2;
                //Debug.Log("time;"+s_time+", dir;"+direction);
                //manager.GetComponent<TCPCommunicator2>().controlSpray(direction, id); // yama 0328 
                //manager.SendMessage("controlSpray", direction);
                //manager.SendMessage("controlSpray", direction);
            }
        }
 /*
        if (s_time >= 5.0f)
        {
            s_time = 0;
            direction++;
            direction %= 2;
            //Debug.Log("time;"+s_time+", dir;"+direction);
            manager.SendMessage("controlSpray", direction);
        }
        //Debug.Log("time;" + s_time + ", dir;" + direction);
        */
    }

    void stopFlag(int stop)
    {
        temp_time = s_time;
        s_time = 0;
        Debug.Log("Stop");
        flag = stop;
        //manager.SendMessage("controlSpray", 2);
        //manager.GetComponent<TCPCommunicator2>().controlSpray(2, id); // yama 0328
        //manager.SendMessage("controlSpray", 2);
        //Invoke("Update", 1);
    }


}