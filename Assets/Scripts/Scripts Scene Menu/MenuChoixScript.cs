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

    }

    void FixedUpdate()
    {

    }
}
