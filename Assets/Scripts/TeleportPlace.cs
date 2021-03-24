using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TeleportPlace : MonoBehaviour
{
    // Start is called before the first frame update
    public PlacesController placesController;
    private int indexPlace = 0;
    void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0.5f || Input.GetKeyUp(KeyCode.K))
            {
                PhotonView.Get(placesController).RPC("reserverPlace", RpcTarget.MasterClient, indexPlace,
                    PhotonNetwork.NetworkingClient.UserId);
                indexPlace++;
            }
        }
    }
}
