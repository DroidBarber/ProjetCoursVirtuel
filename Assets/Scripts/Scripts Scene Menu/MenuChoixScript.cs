using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoixScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject menu;
    public GameObject EventSystem;
    public GameObject Camera;
    public GameObject Curseur;
    public Log_UI logObj;

    void Start()
    {
        menu.SetActive(true);// on active ou non le menu (le canvas) 
        EventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>().enabled = true;
        Camera.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled = true;
        Curseur.SetActive(true);
    /*    Camera.transform.position = new Vector3(0, 0, 0);
        menu.transform.position = new Vector3(10, 0, 0);*/
    }

    // Update is called once per frame
    void Update()
    {

        /*if (OVRInput.GetUp(OVRInput.Button.Start))  // Si la touche menu est enfoncée
        {
            menu.SetActive(!menu.activeSelf); // on active ou non le menu (le canvas) 
            EventSystem.GetComponent<UnityEngine.EventSystems.OVRInputModule>().enabled = !EventSystem.GetComponent<UnityEngine.EventSystems.OVRInputModule>().enabled;
            CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled = !CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled;
            Curseur.SetActive(!Curseur.activeSelf);
        }*/

/*
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
           // menu.transform.position = new Vector3(Camera.GetComponent<Transform>().position.x, Camera.GetComponent<Transform>().position.y, Camera.GetComponent<Transform>().position.z);
            logObj.AjoutLog("x = " + menu.transform.position.x + ", y = " + menu.transform.position.y + ", z = " + menu.transform.position.z);
            logObj.AjoutLog("x = " + Camera.GetComponent<Transform>().position.x + ",y =  " + Camera.GetComponent<Transform>().position.y + ",z = " + Camera.GetComponent<Transform>().position.z);
        }*/

        //if (OVRInput.GetUp(OVRInput.Button.Two))
        //    menu.transform.position = new Vector3(menu.transform.position.x + 0.25f, menu.transform.position.y, menu.transform.position.z);



        //if (OVRInput.GetUp(OVRInput.Button.Three))
        //    menu.transform.position = new Vector3(menu.transform.position.x, menu.transform.position.y + 0.25f, menu.transform.position.z);



        //if (OVRInput.GetUp(OVRInput.Button.Four))
        //    menu.transform.position = new Vector3(menu.transform.position.x, menu.transform.position.y, menu.transform.position.z + 0.25f);


    }

    void FixedUpdate()
    {

    }
}
