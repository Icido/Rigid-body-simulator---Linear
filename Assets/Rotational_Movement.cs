using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotational_Movement : MonoBehaviour {

    public float mass;
    public float radius;
    public Vector3 force;

    public Vector3 angularRotation;
    public Vector3 angularVelocity;
    public Vector3 angularMomentum;

    public Vector3 inverseAngularVelocityRowA;
    public Vector3 inverseAngularVelocityRowB;
    public Vector3 inverseAngularVelocityRowC;

    public Vector3 angularRotation3x3RowA;
    public Vector3 angularRotation3x3RowB;
    public Vector3 angularRotation3x3RowC;

    public float inertiaTensorEquation;
    
    public Vector3 torque;
    public float leverArm;
    public Vector3 leverArmVec3;

    [SerializeField]
    private MomentOfInertia InertiaType;

    private Vector3 InertiaTensorRowA;
    private Vector3 InertiaTensorRowB;
    private Vector3 InertiaTensorRowC;

    enum MomentOfInertia
    {
        SphereSolid
    }

	// Use this for initialization
	void Start () {
        mass = GetComponent<Object_Movement>().mass;
        radius = GetComponent<Collision_System>().sphereRadius;
	}
	
	// Update is called once per frame
	void Update () {

        //Incoming force and normal at force point

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

        InertiaTensorRowA = new Vector3(inertiaTensorEquation, 0f, 0f);
        InertiaTensorRowB = new Vector3(0f, inertiaTensorEquation, 0f);
        InertiaTensorRowC = new Vector3(0f, 0f, inertiaTensorEquation);

        angularVelocity = angularMomentum * inertiaTensorEquation;


        Quaternion quats = transform.rotation;
        Debug.Log(quats);
        


        inverseAngularVelocityRowA = new Vector3(0f, -angularVelocity.z, angularVelocity.y);
        inverseAngularVelocityRowB = new Vector3(angularVelocity.z, 0f, -angularVelocity.x);
        inverseAngularVelocityRowC = new Vector3(-angularVelocity.y, angularVelocity.x, 0f);

        //This is an abomination
        Vector3 tempMat3x3R1 = angularRotation3x3RowA * deltaTime;
        Vector3 tempMat3x3R2 = angularRotation3x3RowB * deltaTime;
        Vector3 tempMat3x3R3 = angularRotation3x3RowC * deltaTime;

        Vector3 tempMat3x3num2R1 = rowMult(inverseAngularVelocityRowA, tempMat3x3R1, tempMat3x3R2, tempMat3x3R3);
        Vector3 tempMat3x3num2R2 = rowMult(inverseAngularVelocityRowA, tempMat3x3R1, tempMat3x3R2, tempMat3x3R3);
        Vector3 tempMat3x3num2R3 = rowMult(inverseAngularVelocityRowA, tempMat3x3R1, tempMat3x3R2, tempMat3x3R3);

        angularRotation3x3RowA += tempMat3x3num2R1;
        angularRotation3x3RowB += tempMat3x3num2R2;
        angularRotation3x3RowC += tempMat3x3num2R3;

        //Debug.Log(angularRotation3x3RowA);
        //Debug.Log(angularRotation3x3RowB);
        //Debug.Log(angularRotation3x3RowC);

    }

    public Vector3 rowMult(Vector3 initRow, Vector3 otherRowA, Vector3 otherRowB, Vector3 otherRowC)
    {
        return new Vector3((initRow.x * otherRowA.x) + (initRow.y * otherRowB.x) + (initRow.z * otherRowC.x),
                           (initRow.x * otherRowA.y) + (initRow.y * otherRowB.y) + (initRow.z * otherRowC.y),
                           (initRow.x * otherRowA.z) + (initRow.y * otherRowB.z) + (initRow.z * otherRowC.z));
    }


}
