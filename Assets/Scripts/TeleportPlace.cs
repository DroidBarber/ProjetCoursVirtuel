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
            if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.5f || Input.GetKeyUp(KeyCode.K))
            {
                PhotonView.Get(placesController).RPC("reserverPlace", RpcTarget.MasterClient, indexPlace,
                    PhotonNetwork.NetworkingClient.UserId);
                indexPlace = (indexPlace+1) % (placesController.nbRangeesGauche * placesController.nbChaisesGauche +
                                placesController.nbRangeesDroite * placesController.nbChaisesDroite);
                isAssis = true;
            }
            else if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.5f || Input.GetKeyUp(KeyCode.L))
            {
                PhotonView.Get(placesController).RPC("libererPlaceRPC", RpcTarget.All, PhotonNetwork.NetworkingClient.UserId);
                indexPlace = 0; 
                isAssis = false;
                transform.position = new Vector3(0, 0, 0);
            }
        }
    }
}
