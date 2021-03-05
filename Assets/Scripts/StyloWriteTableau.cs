using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StyloWriteTableau : MonoBehaviour
{
    public Color c;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.right * 0.2f);

        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.right);
        int layer_mask = LayerMask.GetMask("Tableau");
        if (this.gameObject.GetComponent<StyloController>().isGrab() && 
            this.gameObject.GetComponent<StyloController>().get_id_player_owner() == PhotonNetwork.LocalPlayer.UserId)
        {
            if (Physics.Raycast(ray, out hit, 0.2f, layer_mask))
            {
                if (hit.collider.gameObject.tag == "Tableau")
                {
                    hit.collider.gameObject.GetComponent<TableauController>().Write(hit.point, c);
                }
            }
        }
    }
}
