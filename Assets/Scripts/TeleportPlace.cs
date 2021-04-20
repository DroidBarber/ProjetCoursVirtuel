using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TeleportPlace : MonoBehaviour
{
    // Start is called before the first frame update
    public PlacesController placesController;
    private int indexPlace = 0;
    public bool isAssis = false;
    public Log_UI log_ui;
    public Renderer duplicationDiapo;
    public bool isDuplicationDiapoMove = true;
    public bool dupliActive = true;

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {

            if (OVRInput.GetUp(OVRInput.RawButton.RThumbstick) || Input.GetKeyUp(KeyCode.K))
            {
                PhotonView.Get(placesController).RPC("reserverPlace", RpcTarget.MasterClient, indexPlace,
                    PhotonNetwork.NetworkingClient.UserId);
                indexPlace = (indexPlace + 1) % (placesController.getNbPlace());
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.LThumbstick) || Input.GetKeyUp(KeyCode.L))
            {
                PhotonView.Get(placesController).RPC("libererPlaceRPC", RpcTarget.All, PhotonNetwork.NetworkingClient.UserId);
            }
        }
    }


    public void teleportPlace(Vector3 pos, bool isAssis)
    {
        this.isAssis = isAssis;
        if (isAssis)
        {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0;
            this.transform.position = pos;
            this.gameObject.SetActive(true);
            if (isDuplicationDiapoMove)
                duplicationDiapo.transform.position = transform.position + placesController.offsetDuplicateDiapo;
            duplicationDiapo.gameObject.SetActive(true);
            log_ui.AjoutLog("K: Position reelle : " + transform.position.ToString());
        }
        else
        {
            if (dupliActive)
            {
                indexPlace = 0;
                duplicationDiapo.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                //this.transform.position = new Vector3(0, 1, 0);
                this.transform.position = pos;
                this.gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1;
                this.gameObject.SetActive(true);
                log_ui.AjoutLog("L: Position reelle : " + transform.position.ToString());
            }

        }
    }

    public void setDupliActive(bool val)
    {
        this.dupliActive = val;
    }
}
