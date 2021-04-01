using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StyloWriteTableau : MonoBehaviour
{
    public Color c;
    public float distanceDEcriture = 1.0f;
    public Log_UI log_ui;
    // Start is called before the first frame update
    void Awake()
    {
        log_ui= GameObject.Find("Log_UI").GetComponent<Log_UI>();
        if (!log_ui)
        {
            Debug.LogError("Log_UI non affecté dans StyloWriteTableau");
        }
    }

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
*/            if (Physics.Raycast(ray, out hit, distanceDEcriture, layer_mask))
            {
                //log_ui.AjoutLog("Collide Ray cast", 15);
                if (hit.collider.gameObject.tag == "Tableau")
                {
                    //log_ui.AjoutLog("raycast avec Tableau" + hit.point + " color" +c.ToString(), 15);
                    hit.collider.gameObject.GetComponent<TableauController>().Write(hit.point, c);
                }
            }
        }
    }
}
