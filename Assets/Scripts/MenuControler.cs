using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;
    public GameObject EventSystem;
    public GameObject CameraRig;
    public GameObject Curseur;
 
    void Start()
    {
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
        

