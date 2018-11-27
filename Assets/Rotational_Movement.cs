﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotational_Movement : MonoBehaviour {

    public float mass;
    public float radius;

    public Vector3 angularRotation;
    public Vector3 angularVelocity;
    public Vector3 angularMomentum;
    public Matrix3x3 inverseAngularVelocity;
    public Matrix3x3 angularRotation3x3;

    public float inertiaTensorEquation;
    
    public Vector3 torque;
    public float leverArm;
    public Vector3 leverArmVec3;

    [SerializeField]
    private MomentOfInertia InertiaType;

    private Matrix3x3 InertiaTensor;

    enum MomentOfInertia
    {
        SphereSolid
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Incoming force and normal at force point

        leverArmVec3 = new Vector3(leverArm, leverArm, leverArm);

        //Torque = Vector3.Cross(Force, leverArmVec3);

        float deltaTime = Time.deltaTime;

        angularMomentum += (torque * deltaTime);

        //Should do switch case for each interia tensor, but since we have one, it's simple
        inertiaTensorEquation = ((2 / 5) * mass * radius * radius);

        InertiaTensor = new Matrix3x3(inertiaTensorEquation, 0f, 0f,
                                      0f, inertiaTensorEquation, 0f,
                                      0f, 0f, inertiaTensorEquation);

        angularVelocity = angularMomentum * inertiaTensorEquation;

        inverseAngularVelocity = new Matrix3x3(0f, -angularVelocity.z, angularVelocity.y,
                                               angularVelocity.z, 0f, -angularVelocity.x,
                                               -angularVelocity.y, angularVelocity.x, 0f);

        angularRotation3x3.plusEquals(inverseAngularVelocity.matrixMultiplication(angularRotation3x3).matrixMultiplication(deltaTime));

    }
}
