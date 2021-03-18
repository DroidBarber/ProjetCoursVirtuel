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

    void Start()
    {
        menu.SetActive(true);// on active ou non le menu (le canvas) 
        EventSystem.GetComponent<UnityEngine.EventSystems.OVRInputModule>().enabled = true;
        CameraRig.GetComponent<UnityEngine.EventSystems.OVRPhysicsRaycaster>().enabled = true;
        Curseur.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}
