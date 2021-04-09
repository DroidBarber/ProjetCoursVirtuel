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

    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            
            if (OVRInput.GetUp(OVRInput.RawButton.RThumbstick) || Input.GetKeyUp(KeyCode.K))
            {
                log_ui.AjoutLog("Trigger R OK");
                PhotonView.Get(placesController).RPC("reserverPlace", RpcTarget.MasterClient, indexPlace,
                    PhotonNetwork.NetworkingClient.UserId);
                indexPlace = (indexPlace+1) % (placesController.nbRangeesGauche * placesController.nbChaisesGauche +
                                placesController.nbRangeesDroite * placesController.nbChaisesDroite);
                isAssis = true;
                duplicationDiapo.gameObject.SetActive(true);
                duplicationDiapo.transform.position = transform.position + placesController.offsetDuplicateDiapo;
                log_ui.AjoutLog("Position reelle : "+transform.position.ToString());
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.LThumbstick) || Input.GetKeyUp(KeyCode.L))
            {
                
                log_ui.AjoutLog("Trigger L OK");
                PhotonView.Get(placesController).RPC("libererPlaceRPC", RpcTarget.All, PhotonNetwork.NetworkingClient.UserId);
                indexPlace = 0; 
                isAssis = false;
                duplicationDiapo.gameObject.SetActive(false);
                log_ui.AjoutLog("Position reelle : "+transform.position.ToString());
            }
        }
    }
}
