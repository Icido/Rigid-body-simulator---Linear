﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotational_Movement : MonoBehaviour {

    public float mass;
    public float radius;
    public Vector3 force;

    public Vector3 angularRotation;
    public Vector3 angularVelocity;
    public Vector3 angularMomentum;

    public Matrix4x4 inverseAngularVelocity;

    public Matrix4x4 angularRotationMat;

    public float inertiaTensorEquation;

    public float inverseInertiaTensorEquation;

    public Vector3 torque;
    public float leverArm;
    public Vector3 leverArmVec3;

    [SerializeField]
    private MomentOfInertia InertiaType;

    private Matrix4x4 InertiaTensor;

    enum MomentOfInertia
    {
        SphereSolid
    }

    // Use this for initialization
    void Start() {
        mass = GetComponent<Object_Movement>().mass;
        radius = GetComponent<Collision_System>().sphereRadius;
    }

    // Update is called once per frame
    void Update() {

        //Incoming force and normal at force point

        //For now: Lever arm = radius
        leverArm = radius;

        leverArmVec3 = new Vector3(leverArm, leverArm, leverArm);



        if (Input.GetKey(KeyCode.K))
            force = new Vector3(0f, 10f, 0f);
        else
            force = new Vector3(0f, 0f, 0f);

        torque = Vector3.Cross(force, leverArmVec3);

        float deltaTime = Time.deltaTime;

        angularMomentum += (torque * deltaTime);

        //Should do switch case for each interia tensor, but since we have one, it's simple
        inertiaTensorEquation = ((2 / 5) * mass * radius * radius);

        inverseInertiaTensorEquation = 1 / inertiaTensorEquation;

        InertiaTensor = new Matrix4x4(new Vector4(inverseInertiaTensorEquation, 0f, 0f, 0f),
                                      new Vector4(0f, inverseInertiaTensorEquation, 0f, 0f),
                                      new Vector4(0f, 0f, inverseInertiaTensorEquation, 0f),
                                      new Vector4(0f, 0f, 0f, 1f));



        //Inverse of Inertia Tensor: 1 / inertiaTensorEquation. Then input where inertiaTensorEquation is in regular 3x3 matrix

        angularVelocity = angularMomentum * inertiaTensorEquation;



        inverseAngularVelocity = new Matrix4x4(new Vector4(0f, angularVelocity.z, -angularVelocity.y, 0f),
                                               new Vector4(-angularVelocity.z, 0f, -angularVelocity.x, 0f),
                                               new Vector4(angularVelocity.y, -angularVelocity.x, 0f, 0f),
                                               new Vector4(0f, 0f, 0f, 1f));


        Quaternion quats = transform.rotation;
        Vector3 eulerA = quats.eulerAngles;
        //Debug.Log(eulerA);

        //Take current rotation of object and convert to 3x3
        /*
        angularRotationMat = to4x4Matrix(eulerA);

        Matrix4x4 tempMat = multiplyByFloat(angularRotationMat, deltaTime);

        Matrix4x4 tempMat2 = inverseAngularVelocity * tempMat;

        angularRotationMat = matrixAddition(angularRotationMat, tempMat2);

        transform.Rotate(toEuler(angularRotationMat));
        */

        transform.Rotate(rotationCalculation(eulerA, deltaTime, inverseAngularVelocity));
    }

    public Vector3 rotationCalculation(Vector3 eulerRotations, float dTime, Matrix4x4 iAV)
    {
        Matrix4x4 rotX = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f),
                                       new Vector4(0f, Mathf.Cos(eulerRotations.x), Mathf.Sin(eulerRotations.x), 0f),
                                       new Vector4(0f, -Mathf.Sin(eulerRotations.x), Mathf.Cos(eulerRotations.x), 0f),
                                       new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 newRotX = matrixAddition(rotX, (iAV * multiplyByFloat(rotX, dTime)));

        float newX = Mathf.Acos(newRotX.m11);

        Matrix4x4 rotY = new Matrix4x4(new Vector4(Mathf.Cos(eulerRotations.y), 0f, -Mathf.Sin(eulerRotations.y), 0f),
                                       new Vector4(0f, 1f, 0f, 0f),
                                       new Vector4(Mathf.Sin(eulerRotations.y), 0f, Mathf.Cos(eulerRotations.y), 0f),
                                       new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 newRotY = matrixAddition(rotY, (iAV * multiplyByFloat(rotY, dTime)));

        float newY = Mathf.Acos(newRotX.m00);

        Matrix4x4 rotZ = new Matrix4x4(new Vector4(Mathf.Cos(eulerRotations.z), Mathf.Sin(eulerRotations.z), 0f, 0f),
                                       new Vector4(-Mathf.Sin(eulerRotations.z), Mathf.Cos(eulerRotations.z), 0f, 0f),
                                       new Vector4(0f, 0f, 1f, 0f),
                                       new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 newRotZ = matrixAddition(rotZ, (iAV * multiplyByFloat(rotZ, dTime)));

        float newZ = Mathf.Acos(newRotX.m00);

        return new Vector3(newX, newY, newZ);
    }


    public Matrix4x4 to4x4Matrix(Vector3 eulerAngles)
    {
        float alpha = Mathf.Deg2Rad * eulerAngles.x;
        float beta = Mathf.Deg2Rad * eulerAngles.y;
        float gamma = Mathf.Deg2Rad * eulerAngles.z;


        float cosAlpha = Mathf.Cos(alpha);
        float sinAlpha = Mathf.Sin(alpha);
        float cosBeta = Mathf.Cos(beta);
        float sinBeta = Mathf.Sin(beta);
        float cosGamma = Mathf.Cos(gamma);
        float sinGamma = Mathf.Sin(gamma);

        Vector4 columnA = new Vector4(cosBeta * cosGamma, sinAlpha * sinBeta * cosGamma - cosAlpha * sinGamma, cosAlpha * sinBeta * cosGamma + sinAlpha * sinGamma, 0f);
        Vector4 columnB = new Vector4(cosBeta * sinGamma, sinAlpha * sinBeta * sinGamma + cosAlpha * cosGamma, cosAlpha * sinBeta * sinGamma - sinAlpha * cosGamma, 0f);
        Vector4 columnC = new Vector4(-sinBeta, sinAlpha * cosBeta, cosAlpha * cosBeta, 0f);
        Vector4 columnD = new Vector4(0f, 0f, 0f, 1f);

        return new Matrix4x4(columnA, columnB, columnC, columnD);
    }

    public Matrix4x4 multiplyByFloat(Matrix4x4 mat, float f)
    {

        Vector4 columnA = mat.GetColumn(0) * f;
        Vector4 columnB = mat.GetColumn(1) * f;
        Vector4 columnC = mat.GetColumn(2) * f;
        Vector4 columnD = mat.GetColumn(3) * f;
        
        return new Matrix4x4(columnA, columnB, columnC, columnD);
    }

    public Matrix4x4 matrixAddition(Matrix4x4 mat1, Matrix4x4 mat2)
    {
        return new Matrix4x4(mat1.GetColumn(0) + mat2.GetColumn(0), mat1.GetColumn(1) + mat2.GetColumn(1), mat1.GetColumn(2) + mat2.GetColumn(2), mat1.GetColumn(3) + mat2.GetColumn(3));
    }

    public Vector3 toEuler(Matrix4x4 mat)
    {

        float z;
        if (mat.GetColumn(1).x == 0)
            z = 0;
        else
            z = Mathf.Rad2Deg * Mathf.Atan(mat.GetColumn(1).z / mat.GetColumn(1).x);

        float y;
        if (mat.GetColumn(3).y == 0)
            y = 0;
        else
            y = Mathf.Rad2Deg * Mathf.Atan(mat.GetColumn(0).y / mat.GetColumn(3).y);


        float x = Mathf.Rad2Deg * Mathf.Asin(-mat.GetColumn(1).y);
            

        return new Vector3(x, y, z);
    }


}
