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

    public float last_force_applied;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.Space))
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

        if(Input.GetKeyUp(KeyCode.Space))
        {
            //Release spring
            spring_force = (-spring_constant * spring_extension);

            last_force_applied = this.spring_force;

            spring_extension = 0;
        }


        if(Input.GetKey(KeyCode.W))
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

        if(Input.GetKey(KeyCode.S))
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
