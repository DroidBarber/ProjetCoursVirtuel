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
    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            var log_ui = GameObject.Find("Log_UI").GetComponent<Log_UI>();
            log_ui.ForceClear();
            if (OVRInput.GetUp(OVRInput.RawButton.RThumbstick) || Input.GetKeyUp(KeyCode.K))
            {
                log_ui.AjoutLog("Trigger R OK");
                PhotonView.Get(placesController).RPC("reserverPlace", RpcTarget.MasterClient, indexPlace,
                    PhotonNetwork.NetworkingClient.UserId);
                indexPlace = (indexPlace+1) % (placesController.nbRangeesGauche * placesController.nbChaisesGauche +
                                placesController.nbRangeesDroite * placesController.nbChaisesDroite);
                isAssis = true;
                log_ui.AjoutLog("Position reelle : "+transform.position.ToString());
            }
            else if (OVRInput.GetUp(OVRInput.RawButton.LThumbstick) || Input.GetKeyUp(KeyCode.L))
            {
                
                log_ui.AjoutLog("Trigger R OK");
                PhotonView.Get(placesController).RPC("libererPlaceRPC", RpcTarget.All, PhotonNetwork.NetworkingClient.UserId);
                indexPlace = 0; 
                isAssis = false;
                transform.position = new Vector3(0, 0, 0);
            }
        }
    }
}
