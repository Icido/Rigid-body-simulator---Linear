using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring_launcher : MonoBehaviour {

    public float spring_extension; //m
    public float spring_ext_step;
    public float spring_limit; //m
    public float spring_constant; //N/m
    public float spring_force;
    public float launch_angle;
    public Vector3 hit_vector;
    public GameObject object_selector;

    public float last_force_applied;

	// Use this for initialization
	void Start () {
        hit_vector = new Vector3();
        //object_selector = new GameObject();
        //foreach (GameObject go in Object.FindObjectsOfType(typeof(GameObject)))
        //{
        //    if (go.tag == "SelectorObject" && go.GetComponent<Object_Selection>())
        //    {
        //        object_selector = go;
        //        break;
        //    }
        //}
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetMouseButton(0))
        {
            //Add extension
            if (spring_extension <= spring_limit)
            {
                spring_extension = spring_limit;
            }
            else
            {
                spring_extension -= spring_ext_step;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Release spring
            spring_force = (-spring_constant * spring_extension);

            last_force_applied = this.spring_force;

            hit_vector = object_selector.GetComponent<Object_Selection>().hitPoint;

            spring_extension = 0;
        }


        if(Input.GetKey(KeyCode.UpArrow))
        {
            if(launch_angle > 359)
            {
                launch_angle = 0;
            }
            else
            {
                launch_angle++;
            }
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(launch_angle < 0)
            {
                launch_angle = 359;
            }
            else
            {
                launch_angle--;
            }
        }



    }
}
