using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;


public class Object_Selection : MonoBehaviour {

    public bool isSelecting = false;
    private GameObject storedGameObject;
    //private SceneView sceneView;
    public Vector3 hitPoint = new Vector3();

    public Text gameObjectName;
    public Text gameObjectPosition;
    public Text gameObjectVelocity;
    public Text gameObjectRotation;
    public Text gameObjectHasCollided;
    public Text springLauncherAngle;


    private List<GameObject> objectList = new List<GameObject>();



    // Use this for initialization
    void Start () {
        //sceneView = ScriptableObject.CreateInstance<SceneView>();
        gameObjectName.text = "";
        gameObjectPosition.text = "";
        gameObjectVelocity.text = "";
        gameObjectRotation.text = "";
        gameObjectHasCollided.text = "";
        springLauncherAngle.text = "";

        foreach (GameObject go in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if(go.tag == "SelectableObject" && go.activeInHierarchy)
                objectList.Add(go);
        }

        if(objectList.Count > 0)
            storedGameObject = objectList[0];

    }

    // Update is called once per frame
    void Update () {

        //sceneView = SceneView.currentDrawingSceneView;

        if (isSelecting)
        {
            gameObjectName.text = "Current selected object: " + storedGameObject.name;
            gameObjectPosition.text = "Current position: " + storedGameObject.GetComponent<Object_Movement>().currentPosition;
            gameObjectVelocity.text = "Current velocity: " + storedGameObject.GetComponent<Object_Movement>().velocity;
            gameObjectRotation.text = "Current rotation: " + storedGameObject.transform.rotation.eulerAngles;
            gameObjectHasCollided.text = "Has recently collided: " + storedGameObject.GetComponent<Collision_System>().isColliding;

            if (storedGameObject.GetComponent<Spring_launcher>())
                springLauncherAngle.text = "Launch angle: " + storedGameObject.GetComponent<Spring_launcher>().launch_angle;
            else
                springLauncherAngle.text = "Does not have spring launcher attached.";
        }
        else
        {
            gameObjectName.text = "";
            gameObjectPosition.text = "";
            gameObjectVelocity.text = "";
            gameObjectRotation.text = "";
            gameObjectHasCollided.text = "";
            springLauncherAngle.text = "";
        }


        if (Input.GetMouseButtonDown(0))
        {
            var plane = new Plane(Vector3.up, transform.position);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;

            if(plane.Raycast(ray, out distance))
            {
                hitPoint = ray.GetPoint(distance);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.I))
            isSelecting = !isSelecting;

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (storedGameObject == objectList[0])
                storedGameObject = objectList[objectList.Count - 1];
            else
            {
                storedGameObject = objectList[objectList.IndexOf(storedGameObject) - 1];
            }
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (storedGameObject == objectList[objectList.Count - 1])
                storedGameObject = objectList[0];
            else
            {
                storedGameObject = objectList[objectList.IndexOf(storedGameObject) + 1];
            }
        }
    }


    /*
    void OnSceneGUI(SceneView view)
    {
        Event e = new Event();
        e = Event.current;
        if (e.mousePosition != null)
        {
            storedGameObject = HandleUtility.PickGameObject(e.mousePosition, true);

            Debug.Log(storedGameObject);
        }
    }
    */
}
