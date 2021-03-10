using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuControler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;
    public GameObject rightController;
    public Log_UI logObj;
    public GameObject curseur;
    private RaycastHit hit;
    public GameObject bouton;
  

   
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
        
        Debug.Log(hit.point);
        int layerMask = LayerMask.GetMask("LayerMenu");
      //  if (Physics.Raycast(rightController.GetComponent<Transform>().position, rightController.GetComponent<Transform>().forward, out hit, Mathf.Infinity,layerMask))
       // {
            this.curseur.SetActive(true);
        
            //logObj.AjoutLog("HitDistance : " + hit.distance);
            this.curseur.GetComponent<Transform>().SetPositionAndRotation(this.bouton.GetComponent<Transform>().position, curseur.GetComponent<Transform>().rotation);
            
       // }
      //  else
      //  {
          //  this.curseur.SetActive(false);
     //   }
        
    }

    void FixedUpdate()
    {
         
    }
       
}
        

