using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3 : MonoBehaviour {

    public float m00;
    public float m01;
    public float m02;
    public float m10;
    public float m11;
    public float m12;
    public float m20;
    public float m21;
    public float m22;

    // Initializes a 3x3 matrix
    public Matrix3x3(float im00, float im01, float im02,
                float im10, float im11, float im12,
                float im20, float im21, float im22)
    {
        m00 = im00;
        m01 = im01;
        m02 = im02;

        m10 = im10;
        m11 = im11;
        m12 = im12;

        m20 = im20;
        m21 = im21;
        m22 = im22;

    }



    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
