using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_System : MonoBehaviour {

    public bool isColliding;
    
    public float coefficientOfRestitution;
    [SerializeField]
    private bool hasImpulsed = false;

    public enum CollisionType
    {
        NULL,
        Sphere,
        AABB
    }

    [SerializeField]
    public CollisionType colType = CollisionType.NULL;

    public float sphereRadius;

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

    [SerializeField]
    private List<Vector3> collisionNormals = new List<Vector3>();
    
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
        collisionNormals.Clear();


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

            #region Sphere_Collision_Test

            if (this.colType == CollisionType.Sphere && thisCollider.colType == CollisionType.Sphere)
            {
                if (Vector3.Distance(collider.transform.position, transform.position) < (thisCollider.sphereRadius + sphereRadius))
                {
                    Debug.Log("Collision between " + name + " and " + thisCollider.name);

                    collidingWith.Add(collider);

                    collisionNormals.Add(Vector3.Normalize(collider.transform.position - transform.position));

                    if (collider.GetComponent<Object_Movement>().previousPosition != collider.GetComponent<Object_Movement>().currentPosition && 
                        GetComponent<Object_Movement>().previousPosition != GetComponent<Object_Movement>().currentPosition)
                    {
                        Vector3 thisPositionMinus = collider.GetComponent<Object_Movement>().previousPosition - collider.GetComponent<Object_Movement>().currentPosition;
                        Vector3 myPositionMinus = GetComponent<Object_Movement>().previousPosition - GetComponent<Object_Movement>().currentPosition;
                        recursiveStepBack(collider, thisCollider, thisPositionMinus, myPositionMinus);
                    }
                }
            }

            #endregion

            #region AABB_Collision_Tests

            if (this.colType == CollisionType.AABB && thisCollider.colType == CollisionType.AABB)
            {
                if ((minAABBx > thisCollider.maxAABBx) || (thisCollider.minAABBx > maxAABBx) ||
                    (minAABBy > thisCollider.maxAABBy) || (thisCollider.minAABBy > maxAABBy) ||
                    (minAABBz > thisCollider.maxAABBz) || (thisCollider.minAABBz > maxAABBz))
                {
                    //Check closest plane and store locally
                    //float centresDistance = Vector3.Distance(this.transform.position, thisCollider.transform.position);

                    separatingPlane = SeparatingPlane.NULL;

                    if (Mathf.Abs((minAABBx - thisCollider.maxAABBx)) > Mathf.Abs((thisCollider.minAABBx - maxAABBx)) ||
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
                    collisionNormals.Add(Vector3.Normalize(collider.transform.position - transform.position));

                    Debug.Log("Collision between " + name + " and " + thisCollider.name);

                    Vector3 thisPositionMinus = collider.GetComponent<Object_Movement>().previousPosition - collider.GetComponent<Object_Movement>().currentPosition;
                    Vector3 myPositionMinus = GetComponent<Object_Movement>().previousPosition - GetComponent<Object_Movement>().currentPosition;

                    recursiveStepBack(collider, thisCollider, thisPositionMinus, myPositionMinus);
                }
            }

            #endregion

            #region Sphere-AABB_Collision_Tests

            if (this.colType == CollisionType.Sphere && thisCollider.colType == CollisionType.AABB)
            {
                Vector3 normanL = Vector3.Normalize(collider.transform.position - transform.position);
                Vector3 sphereChecker = this.transform.position + (normanL * this.sphereRadius);

                if ((sphereChecker.x <= thisCollider.maxAABBx) && (thisCollider.minAABBx <= sphereChecker.x) &&
                    (sphereChecker.y <= thisCollider.maxAABBy) && (thisCollider.minAABBy <= sphereChecker.y) &&
                    (sphereChecker.z <= thisCollider.maxAABBz) && (thisCollider.minAABBz <= sphereChecker.z))
                {
                    collidingWith.Add(collider);

                    collisionNormals.Add(Vector3.Normalize(collider.transform.position - transform.position));

                    Debug.Log("Collision between " + name + " and " + thisCollider.name);

                    Vector3 thisPositionMinus = collider.GetComponent<Object_Movement>().previousPosition - collider.GetComponent<Object_Movement>().currentPosition;
                    Vector3 myPositionMinus = GetComponent<Object_Movement>().previousPosition - GetComponent<Object_Movement>().currentPosition;

                    recursiveStepBack(collider, thisCollider, thisPositionMinus, myPositionMinus);
                }
            }

            #endregion

            #region AABB-Sphere_Collision_Tests

            if (this.colType == CollisionType.AABB && thisCollider.colType == CollisionType.Sphere)
            {
                Vector3 normanL = Vector3.Normalize(collider.transform.position - transform.position);
                Vector3 sphereChecker = thisCollider.transform.position + (normanL * thisCollider.sphereRadius);

                if ((minAABBx <= sphereChecker.x) && (sphereChecker.x <= maxAABBx) &&
                    (minAABBy <= sphereChecker.y) && (sphereChecker.y <= maxAABBy) &&
                    (minAABBz <= sphereChecker.z) && (sphereChecker.z <= maxAABBz))
                {
                    collidingWith.Add(collider);

                    collisionNormals.Add(Vector3.Normalize(collider.transform.position - transform.position));

                    Debug.Log("Collision between " + name + " and " + thisCollider.name);

                    Vector3 thisPositionMinus = collider.GetComponent<Object_Movement>().previousPosition - collider.GetComponent<Object_Movement>().currentPosition;
                    Vector3 myPositionMinus = GetComponent<Object_Movement>().previousPosition - GetComponent<Object_Movement>().currentPosition;

                    recursiveStepBack(collider, thisCollider, thisPositionMinus, myPositionMinus);
                }
            }

            #endregion


        }

        #region Impulse_Method

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
                    Vector3 norm = collisionNormals[collidingWith.IndexOf(collider)];

                    //Find impulse J
                    float imp1 = Vector3.Dot((-(this.GetComponent<Object_Movement>().velocity - collider.GetComponent<Object_Movement>().velocity) * (coefficientOfRestitution + 1)), norm);

                    float imp2 = ((1 / this.GetComponent<Object_Movement>().mass) + (1 / collider.GetComponent<Object_Movement>().mass));

                    float impulse = imp1 / imp2;

                    //This new velocity

                    this.GetComponent<Object_Movement>().velocity = ((impulse / this.GetComponent<Object_Movement>().mass) * norm) + this.GetComponent<Object_Movement>().velocity;

                    collider.GetComponent<Object_Movement>().velocity = ((-impulse / collider.GetComponent<Object_Movement>().mass) * norm) + collider.GetComponent<Object_Movement>().velocity;


                    hasImpulsed = true;
                    collider.GetComponent<Collision_System>().hasImpulsed = true;



                }
            }
        }

        hasImpulsed = false;

        #endregion

        //Rebounds off of the ground to prevent falling through the "ground"
        #region Ground_Plane_Impulse

        if (transform.position.y < transform.localScale.y)
        {
            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.y) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.y = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.y;

        }

        #endregion

        //These checks are just to keep the objects within the area, as regular bounding boxes surrounding it were breaking the entirety of scene
        #region Containment_Box_Impulses

        if (transform.position.x < (transform.localScale.x - 50))
        {
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);

            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.x) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.x = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.x;
        }

        if (transform.position.x > (50 - transform.localScale.x))
        {
            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);

            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.x) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.x = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.x;
        }

        if (transform.position.z < (transform.localScale.z - 25))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f);

            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.z) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.z = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.z;
        }

        if (transform.position.z > (25 - transform.localScale.z))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);

            var imp1 = (-(this.GetComponent<Object_Movement>().velocity.z) * (coefficientOfRestitution + 1));
            var imp2 = (1 / this.GetComponent<Object_Movement>().mass);
            var impulse = imp1 / imp2;

            this.GetComponent<Object_Movement>().velocity.z = (impulse / this.GetComponent<Object_Movement>().mass) + this.GetComponent<Object_Movement>().velocity.z;
        }

        #endregion


    }

    public void recursiveStepBack(GameObject objectCollidingWith, Collision_System objectCollision, Vector3 collidingPositionMinus, Vector3 myPositionMinus)
    {
        objectCollidingWith.transform.Translate(collidingPositionMinus);
        this.transform.Translate(myPositionMinus);

        if (objectCollidingWith.GetComponent<Collision_System>().colType == CollisionType.Sphere && colType == CollisionType.Sphere)
        {
            if (Vector3.Distance(objectCollidingWith.transform.position, transform.position) < (objectCollision.sphereRadius + sphereRadius))
                recursiveStepBack(objectCollidingWith, objectCollision, collidingPositionMinus, myPositionMinus);
            else
                return;
        }

        //Boxes aren't always cubes

        if (objectCollidingWith.GetComponent<Collision_System>().colType == CollisionType.AABB && colType == CollisionType.AABB)
        {
            //if (Vector3.Distance(objectCollidingWith.transform.position, transform.position) < (objectCollision.sphereRadius + sphereRadius))
            //    recursiveStepBack(objectCollidingWith, objectCollision, collidingPositionMinus, myPositionMinus);
            //else
                return;
        }

        if (objectCollidingWith.GetComponent<Collision_System>().colType == CollisionType.Sphere && colType == CollisionType.AABB)
        {
            //if (Vector3.Distance(objectCollidingWith.transform.position, transform.position) < (objectCollision.sphereRadius + sphereRadius))
            //    recursiveStepBack(objectCollidingWith, objectCollision, collidingPositionMinus, myPositionMinus);
            //else
                return;
        }

        if (objectCollidingWith.GetComponent<Collision_System>().colType == CollisionType.AABB && colType == CollisionType.Sphere)
        {
            //if (Vector3.Distance(objectCollidingWith.transform.position, transform.position) < (objectCollision.sphereRadius + sphereRadius))
            //    recursiveStepBack(objectCollidingWith, objectCollision, collidingPositionMinus, myPositionMinus);
            //else
                return;
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.transform.position, sphereRadius);
    }
}
