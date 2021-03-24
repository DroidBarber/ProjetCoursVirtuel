using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoixScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public GameObject menu;
    public GameObject EventSystem;
    public GameObject CameraRig;
    public GameObject Curseur;
    public Log_UI logObj;

    void Start()
    {
        menu.SetActive(true);// on active ou non le menu (le canvas) 
        EventSystem.GetComponent<UnityEngine.EventSystems.StandaloneInputModule>().enabled = true;
        CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled = true;
        Curseur.SetActive(true);
    }
      
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Start))  // Si la touche menu est enfoncée
        {
            menu.SetActive(!menu.activeSelf); // on active ou non le menu (le canvas) 
            EventSystem.GetComponent<UnityEngine.EventSystems.OVRInputModule>().enabled = !EventSystem.GetComponent<UnityEngine.EventSystems.OVRInputModule>().enabled;
            CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled = !CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled;
            Curseur.SetActive(!Curseur.activeSelf);
        }


    }

    void FixedUpdate()
    {

    }
}
