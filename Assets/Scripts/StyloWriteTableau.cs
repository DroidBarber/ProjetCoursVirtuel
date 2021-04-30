using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Ce script permet de gérer l'interraction entre un stylo et un tableau, s'il sont proche d'une certaine distance, et que le stylo est orienté vers le tableau, 
/// cela afin de tracer sur la tableau
/// </summary>

public class StyloWriteTableau : MonoBehaviour
{
    public Color c;
    public float distanceDEcriture = 1.0f; // distance entre le stylo et la tableau, afin que cela trace sur le tableau
    public Log_UI log_ui;

    private GameObject previousHitTableau;
    // Start is called before the first frame update
    void Awake()
    {
        log_ui = GameObject.Find("Log_UI").GetComponent<Log_UI>();
        if (!log_ui)
        {
            Debug.LogError("Log_UI non affecté dans StyloWriteTableau");
        }
    }

    /// <summary>
    /// Par un raycast, on vérifie que l'on est proche et bien orienté, et si c'est le cas, alors on dit au tableau où il doit mettre un point de tel couleur
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.right * distanceDEcriture);

        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.right);
        int layer_mask = LayerMask.GetMask("Tableau");
        if (this.gameObject.GetComponent<StyloController>().isGrab() &&
            this.gameObject.GetComponent<StyloController>().get_id_player_owner() == PhotonNetwork.LocalPlayer.UserId)
        {
            /*            log_ui.ForceClear();
                        log_ui.AjoutLog("Grab dans Write", 15);
            */
            if (Physics.Raycast(ray, out hit, distanceDEcriture, layer_mask)) // si il y a une collision avec le raycast
            {
                //log_ui.AjoutLog("Collide Ray cast", 15);
                if (hit.collider.gameObject.tag == "Tableau")
                {
                    //log_ui.AjoutLog("raycast avec Tableau" + hit.point + " color" +c.ToString(), 15);
                    hit.collider.gameObject.GetComponent<TableauController>().Write(hit.point, c);
                    previousHitTableau = hit.collider.gameObject;
                }
            }
            else
            {
                if (previousHitTableau)
                {

                    previousHitTableau.GetComponent<TableauController>().WriteEnd();
                    previousHitTableau = null;
                }
            }
        }

    }
}
