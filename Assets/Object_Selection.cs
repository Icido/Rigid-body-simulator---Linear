using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;


public class Object_Selection : MonoBehaviour {

    public bool isSelecting = false;
    private GameObject storedGameObject;
    private SceneView sceneView;

    public Text gameObjectName;
    public Text gameObjectPosition;
    public Text gameObjectVelocity;
    public Text gameObjectRotation;


    private List<GameObject> objectList = new List<GameObject>();



    // Use this for initialization
    void Start () {
        sceneView = ScriptableObject.CreateInstance<SceneView>();
        gameObjectName.text = "";
        gameObjectPosition.text = "";
        gameObjectVelocity.text = "";
        gameObjectRotation.text = "";
        
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

        sceneView = SceneView.currentDrawingSceneView;

        if (isSelecting)
        {
            gameObjectName.text = "Current selected object: " + storedGameObject.name;
            gameObjectPosition.text = "Current position: " + storedGameObject.GetComponent<Object_Movement>().currentPosition;
            gameObjectVelocity.text = "Current velocity: " + storedGameObject.GetComponent<Object_Movement>().velocity;
            gameObjectRotation.text = "Current rotation: " + storedGameObject.transform.rotation.eulerAngles;
            Debug.Log("Display!");

        }
        else
        {
            gameObjectName.text = "";
            gameObjectPosition.text = "";
            gameObjectVelocity.text = "";
            gameObjectRotation.text = "";
            Debug.Log("No display!");
        }


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");













            //OnSceneGUI(sceneView);

            /*
            RaycastHit hitInformation = new RaycastHit();
            Debug.Log(hitInformation);

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInformation);

            //Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition));
            Debug.Log(hitInformation);
            if (hit)
            {
                if(hitInformation.transform.gameObject.tag == "PhysicsObject")
                {
                    isSelected = true;
                    Debug.Log("Hit!");
                    storedGameObject = hitInformation.transform.gameObject;
                }
            }
            else
            {
                Debug.Log("And then I missed...");
            }
            */

            


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
