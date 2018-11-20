using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_System : MonoBehaviour {

    [SerializeField]
    private bool isColliding;
    [SerializeField]
    private float coefficientOfRestitution;
    [SerializeField]
    private bool hasImpulsed = false;

    enum CollisionType
    {
        Sphere,
        AABB
    }

    [SerializeField]
    private CollisionType colType;

    private float sphereRadius;

    private float minAABBx;
    private float maxAABBx;
    private float minAABBy;
    private float maxAABBy;
    private float minAABBz;
    private float maxAABBz;

    enum SeparatingPlane
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back,
        NULL
    }

    private SeparatingPlane separatingPlane = SeparatingPlane.NULL;


    //This lists through all the found game objects that contains the spherecollision script.
    //This is so that when checking for collisions, it only checks those with collision detection.
    [SerializeField]
    private List<GameObject> collidableObjects = new List<GameObject>();

    //This list is used to store all those objects this object is currently colliding with.
    [SerializeField]
    private List<GameObject> collidingWith = new List<GameObject>();
    
    // Use this for initialization
	void Start () {


        if (colType == CollisionType.Sphere)
        {
            //This is just using the x scale to create a sphere collider for the box.
            sphereRadius = (this.transform.localScale.x / 2f);
        }
        

        //This foreach loop goes through all the found objects and only adds those that aren't "this" object.
        //When it comes to checking all objects, this one will be excluded from the checks.
        foreach (Collision_System obj in (Collision_System[])FindObjectsOfType(typeof(Collision_System)))
        {
            if (obj.Equals(this))
            {
                continue;
            }
            GameObject newGO = obj.gameObject;
            collidableObjects.Add(newGO);
        }


    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {

        collidingWith.Clear();

        if (colType == CollisionType.AABB)
        {
            minAABBx = this.transform.position.x - (this.transform.localScale.x / 2f);
            maxAABBx = this.transform.position.x + (this.transform.localScale.x / 2f);
            minAABBy = this.transform.position.y - (this.transform.localScale.y / 2f);
            maxAABBy = this.transform.position.y + (this.transform.localScale.y / 2f);
            minAABBz = this.transform.position.z - (this.transform.localScale.z / 2f);
            maxAABBz = this.transform.position.z + (this.transform.localScale.z / 2f);
        }

        //Checking through all the collidable objects in the scene (may get expensive, could limit to things within range
        foreach (GameObject collider in collidableObjects)
        {
            Collision_System thisCollider = collider.GetComponent<Collision_System>();

            if (this.colType == CollisionType.Sphere && thisCollider.colType == CollisionType.Sphere)
            {
                float range = Vector3.Distance(collider.transform.position, transform.position);

                if (range < (thisCollider.sphereRadius + sphereRadius))
                {
                    collidingWith.Add(collider);
                }
            }

            if(this.colType == CollisionType.AABB && thisCollider.colType == CollisionType.AABB)
            {
                if( (minAABBx > thisCollider.maxAABBx) || (thisCollider.minAABBx > maxAABBx) ||
                    (minAABBy > thisCollider.maxAABBy) || (thisCollider.minAABBy > maxAABBy) ||
                    (minAABBz > thisCollider.maxAABBz) || (thisCollider.minAABBz > maxAABBz))
                {
                    //Check closest plane and store locally
                    float centresDistance = Vector3.Distance(this.transform.position, thisCollider.transform.position);

                    separatingPlane = SeparatingPlane.NULL;

                    if( Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((thisCollider.minAABBx - maxAABBx)) || 
                        Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((minAABBy - thisCollider.maxAABBy)) ||
                        Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((thisCollider.minAABBy - maxAABBy)) ||
                        Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((minAABBz - thisCollider.maxAABBz)) ||
                        Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((thisCollider.minAABBz - maxAABBz)))
                    {
                        separatingPlane = SeparatingPlane.Right;
                    }
                    else if (Mathf.Abs((thisCollider.minAABBx - maxAABBx)) > Mathf.Abs((minAABBx - thisCollider.maxAABBx)) ||
                        Mathf.Abs((thisCollider.minAABBx - maxAABBx)) > Mathf.Abs((minAABBy - thisCollider.maxAABBy)) ||
                        Mathf.Abs((thisCollider.minAABBx - maxAABBx)) > Mathf.Abs((thisCollider.minAABBy - maxAABBy)) ||
                        Mathf.Abs((thisCollider.minAABBx - maxAABBx)) > Mathf.Abs((minAABBz - thisCollider.maxAABBz)) ||
                        Mathf.Abs((thisCollider.minAABBx - maxAABBx)) > Mathf.Abs((thisCollider.minAABBz - maxAABBz)))
                    {
                        separatingPlane = SeparatingPlane.Left;
                    }
                    else if (Mathf.Abs((minAABBy - thisCollider.maxAABBy)) > Mathf.Abs((minAABBx - thisCollider.maxAABBx)) ||
                        Mathf.Abs((minAABBy - thisCollider.maxAABBy)) > Mathf.Abs((thisCollider.minAABBx - maxAABBx)) ||
                        Mathf.Abs((minAABBy - thisCollider.maxAABBy)) > Mathf.Abs((thisCollider.minAABBy - maxAABBy)) ||
                        Mathf.Abs((minAABBy - thisCollider.maxAABBy)) > Mathf.Abs((minAABBz - thisCollider.maxAABBz)) ||
                        Mathf.Abs((minAABBy - thisCollider.maxAABBy)) > Mathf.Abs((thisCollider.minAABBz - maxAABBz)))
                    {
                        separatingPlane = SeparatingPlane.Bottom;
                    }
                    else if (Mathf.Abs((thisCollider.minAABBy - maxAABBy)) > Mathf.Abs((minAABBx - thisCollider.maxAABBx)) ||
                        Mathf.Abs((thisCollider.minAABBy - maxAABBy)) > Mathf.Abs((thisCollider.minAABBx - maxAABBx)) ||
                        Mathf.Abs((thisCollider.minAABBy - maxAABBy)) > Mathf.Abs((minAABBy - thisCollider.maxAABBy)) ||
                        Mathf.Abs((thisCollider.minAABBy - maxAABBy)) > Mathf.Abs((minAABBz - thisCollider.maxAABBz)) ||
                        Mathf.Abs((thisCollider.minAABBy - maxAABBy)) > Mathf.Abs((thisCollider.minAABBz - maxAABBz)))
                    {
                        separatingPlane = SeparatingPlane.Top;
                    }
                    else if (Mathf.Abs((minAABBz - thisCollider.maxAABBz)) > Mathf.Abs((minAABBx - thisCollider.maxAABBx)) ||
                        Mathf.Abs((minAABBz - thisCollider.maxAABBz)) > Mathf.Abs((thisCollider.minAABBx - maxAABBx)) ||
                        Mathf.Abs((minAABBz - thisCollider.maxAABBz)) > Mathf.Abs((minAABBy - thisCollider.maxAABBy)) ||
                        Mathf.Abs((minAABBz - thisCollider.maxAABBz)) > Mathf.Abs((thisCollider.minAABBy - maxAABBy)) ||
                        Mathf.Abs((minAABBz - thisCollider.maxAABBz)) > Mathf.Abs((thisCollider.minAABBz - maxAABBz)))
                    {
                        separatingPlane = SeparatingPlane.Front;
                    }
                    else
                    {
                        separatingPlane = SeparatingPlane.Back;
                    }


                }

                if ((minAABBx <= thisCollider.maxAABBx) && (thisCollider.minAABBx <= maxAABBx) &&
                    (minAABBy <= thisCollider.maxAABBy) && (thisCollider.minAABBy <= maxAABBy) &&
                    (minAABBz <= thisCollider.maxAABBz) && (thisCollider.minAABBz <= maxAABBz))
                {
                    //Is intersecting
                    collidingWith.Add(collider);
                }



            }

        }

        //If any object collided with this object, it will switch the boolean to true
        if (collidingWith.Count > 0)
            isColliding = true;
        else
            isColliding = false;

        if (isColliding)
        {
            foreach (GameObject collider in collidingWith)
            {
                if (!this.hasImpulsed && !collider.GetComponent<Collision_System>().hasImpulsed)
                {
                    //Find impulse J
                    var imp1 = (-(this.GetComponent<Object_Movement>().velocity.x - collider.GetComponent<Object_Movement>().velocity.x) * (coefficientOfRestitution + 1));

                    var imp2 = ((1 / this.GetComponent<Object_Movement>().mass) + (1 / collider.GetComponent<Object_Movement>().mass));

                    var impulse = imp1 / imp2;

                    //This new velocity

                    this.GetComponent<Object_Movement>().velocity.x = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.x;

                    collider.GetComponent<Object_Movement>().velocity.x = (-impulse / collider.GetComponent<Object_Movement>().mass) + collider.GetComponent<Object_Movement>().velocity.x;


                    hasImpulsed = true;
                    collider.GetComponent<Collision_System>().hasImpulsed = true;
                }
            }
        }
        hasImpulsed = false;


        if(transform.position.y < transform.localScale.y)
        {
            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.y) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.y = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.y;

        }


    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, sphereRadius);
    }
}
