using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;


public class Object_Selection : MonoBehaviour {

    public Text gameObjectName;
    public bool isSelected = false;
    private GameObject storedGameObject;
    private SceneView sceneView;


    // Use this for initialization
    void Start () {
        sceneView = ScriptableObject.CreateInstance<SceneView>();
        storedGameObject = new GameObject();
        gameObjectName.text = "";

    }

    // Update is called once per frame
    void Update () {

        sceneView = SceneView.currentDrawingSceneView;

        if (isSelected)
        {
            gameObjectName.text = "Current selected object: " + storedGameObject.name;
            Debug.Log("Display!");

        }

        isSelected = false;

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");

            OnSceneGUI(sceneView);

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

        

    }

    

    void OnSceneGUI(SceneView view)
    {
        Event e = new Event();
        e = Event.current;
        Debug.Log(e);
        if (e != null)
        {
            storedGameObject = HandleUtility.PickGameObject(e.mousePosition, true);

            Debug.Log(storedGameObject);
        }
    }
}
