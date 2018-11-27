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

    public Matrix3x3 matrixMultiplication(Matrix3x3 m2)
    {
        float n00 = ((this.m00 * m2.m00) + (this.m01 * m2.m10) + (this.m02 * m2.m20));
        float n01 = ((this.m00 * m2.m01) + (this.m01 * m2.m11) + (this.m02 * m2.m21));
        float n02 = ((this.m00 * m2.m02) + (this.m01 * m2.m12) + (this.m02 * m2.m22));

        float n10 = ((this.m10 * m2.m00) + (this.m11 * m2.m10) + (this.m12 * m2.m20));
        float n11 = ((this.m10 * m2.m01) + (this.m11 * m2.m11) + (this.m12 * m2.m21));
        float n12 = ((this.m10 * m2.m02) + (this.m11 * m2.m12) + (this.m12 * m2.m22));

        float n20 = ((this.m20 * m2.m00) + (this.m21 * m2.m10) + (this.m22 * m2.m20));
        float n21 = ((this.m20 * m2.m01) + (this.m21 * m2.m11) + (this.m22 * m2.m21));
        float n22 = ((this.m20 * m2.m02) + (this.m21 * m2.m12) + (this.m22 * m2.m22));


        return new Matrix3x3(n00, n01, n02, 
                             n10, n11, n12, 
                             n20, n21, n22);
    }

    public Matrix3x3 matrixMultiplication(float m2)
    {
        float n00 = this.m00 * m2;
        float n01 = this.m01 * m2;
        float n02 = this.m02 * m2;

        float n10 = this.m10 * m2;
        float n11 = this.m11 * m2;
        float n12 = this.m12 * m2;

        float n20 = this.m20 * m2;
        float n21 = this.m21 * m2;
        float n22 = this.m22 * m2;

        return new Matrix3x3(n00, n01, n02,
                             n10, n11, n12,
                             n20, n21, n22);
    }

    public Matrix3x3 plusEquals(Matrix3x3 m2)
    {

        return new Matrix3x3(this.m00 + m2.m00, this.m01 + m2.m01, this.m02 + m2.m02,
                            this.m10 + m2.m10, this.m11 + m2.m11, this.m12 + m2.m12,
                            this.m20 + m2.m20, this.m21 + m2.m21, this.m22 + m2.m22);
    }


}
