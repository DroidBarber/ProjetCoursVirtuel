using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScaleSalle : MonoBehaviourPunCallbacks
{
    public Log_UI logObj;
    public GameObject s309;
    public GameObject s3092; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.Button.Two))
        { // Si la touche menu est enfoncée
            s309.GetComponent<Transform>().localScale = new Vector3(s309.GetComponent<Transform>().localScale.x + 0.02f, s309.GetComponent<Transform>().localScale.y + 0.02f, s309.GetComponent<Transform>().localScale.z + 0.02f);
            s3092.GetComponent<Transform>().localScale = new Vector3(s3092.GetComponent<Transform>().localScale.x + 0.02f, s3092.GetComponent<Transform>().localScale.y + 0.02f, s3092.GetComponent<Transform>().localScale.z + 0.02f);
            logObj.AjoutLog(s309.GetComponent<Transform>().localScale.ToString());
            logObj.AjoutLog(s3092.GetComponent<Transform>().localScale.ToString());
        }
    }
}
