using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;
    public GameObject rightController;
    public Log_UI logObj;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Start))  // Si la touche menu est enfoncée
        {
            menu.SetActive(!menu.activeSelf); // on active ou non le menu (le canvas) 
        }
    }

    void FixedUpdate()
    {
         RaycastHit hit;
         //int layerMask = LayerMask.GetMask("UI");
         if(Physics.Raycast(rightController.GetComponent<Transform>().position, rightController.GetComponent<Transform>().forward, out hit, Mathf.Infinity))
        {
            logObj.AjoutLog("HitDistance : " + hit.distance);
        }
    }
       
}
        

