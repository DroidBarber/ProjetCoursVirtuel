using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    public Log_UI logObj;
    public GameObject playerPrefab;
    public GameObject playerOVR;
    // Start is called before the first frame update
    void Awake()
    {
        if (!logObj)
            Debug.LogError("logObj non assigné");
        if (!playerPrefab)
            Debug.LogError("playerPrefab non assigné");
        if (!playerOVR)
            Debug.LogError("playerOVR non assigné");
    }

    // Update is called once per frame
    void Update()
    {
        //Pour afficher l'état de la connection au fur et à mesure
        //logObj.AjoutLog(PhotonNetwork.NetworkingClient.State.ToString(),1);


        if (PhotonNetwork.NetworkingClient.InRoom)
        {

            // juste pour le test, et le force 
            logObj.ForceClear();
            logObj.AjoutLog("BTN A: " + OVRInput.Get(OVRInput.RawButton.A), 1);
            logObj.AjoutLog("BTN B: " + OVRInput.Get(OVRInput.RawButton.B), 1);
            logObj.AjoutLog("BTN X: " + OVRInput.Get(OVRInput.RawButton.X), 1);
            logObj.AjoutLog("BTN Y: " + OVRInput.Get(OVRInput.RawButton.Y), 1);
            OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        }
        
        
    }
    public override void OnJoinedRoom()
    {
        logObj.AjoutLog("Connecté à la room: " + PhotonNetwork.CurrentRoom.Name, 5);

        GameObject p = PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, Quaternion.identity);
        p.GetComponent<SyncPosPlayer>().setup(playerOVR);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        logObj.AjoutLog("Déconnecté car: " + cause, 10);
    }
}
