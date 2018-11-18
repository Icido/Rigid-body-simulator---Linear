using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Movement : MonoBehaviour
{

    public float gravity = 9.81f;
    public float staticFriction;
    public float kineticFriction;
    public float mass;
    public Vector3 velocity;
    public Vector3 currentPosition;
    public Vector3 appliedForce;
    public Vector3 resultantForce;
    public float reactionForce;
    public bool isMoving = false;
    public bool isStatic = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float weight = mass * gravity;
        reactionForce = 0f;
        resultantForce = new Vector3(0,0,0);

        if (GetComponent<Spring_launcher>())
        {
            if (GetComponent<Spring_launcher>().spring_force != 0)
            {
                float angle = GetComponent<Spring_launcher>().launch_angle * Mathf.Deg2Rad;
                float force = GetComponent<Spring_launcher>().spring_force;

                resultantForce += new Vector3(force * Mathf.Cos(angle), force * Mathf.Sin(angle), 0);

                this.GetComponent<Spring_launcher>().spring_force = 0;
            }
        }
        

        //Choose which reaction force to use
        if (!isMoving)
        {
            reactionForce = staticFriction * weight;
        }
        else
        {
            reactionForce = kineticFriction * weight;
        }


        if (!isStatic)
        {
            //Query if resultant force is greater than static
            //  Move if true
            if (resultantForce.x > reactionForce || resultantForce.x < -reactionForce)
            {
                isMoving = true;
                float deltaTime = Time.deltaTime;

                //currentPosition = this.transform.position;
                velocity += ((resultantForce / mass) * deltaTime);
                this.transform.position += (velocity * deltaTime);
                currentPosition = this.transform.position;

            }
            else
            {
                isMoving = false;
                float deltaTime = Time.deltaTime;

                //currentPosition = this.transform.position;
                if (velocity.x < -0.3f)
                    velocity.x += ((reactionForce / mass) * deltaTime);
                else if (velocity.x > 0.3f)
                    velocity.x -= ((reactionForce / mass) * deltaTime);
                else if (velocity.x > -0.3f && velocity.x < 0.3f)
                    velocity.x = 0f;

                if (velocity.y > 0f)
                    velocity.y -= gravity;
                //else if velocity down is greater than terminal velocity, keep it at terminal velocity
                //until it collides with the floor

                this.transform.position += (velocity * deltaTime);
                currentPosition = this.transform.position;
                //currentPosition += suvatDisplacement(velocity, , deltaTime);
            }
        }


    }

}