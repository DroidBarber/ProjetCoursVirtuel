using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Network : MonoBehaviourPunCallbacks
{
    public Log_UI logObj;
    public List<GameObject> playerPrefabList;
    public GameObject playerOVR;
    private string avatarName;
    // Start is called before the first frame update
    void Awake()
    {
        if (!logObj)
            Debug.LogError("logObj non assigné");
        foreach (GameObject go in playerPrefabList)
        {
            if (!go)
                Debug.LogError("un playerPrefab dans playerPrefabList non assigné");
        }
        if (!playerOVR)
            Debug.LogError("playerOVR non assigné");

        GameObject g = GameObject.Find("RoomNameToJoin");
        avatarName = g.GetComponent<RoomNameToJoin>().avatarName;
    }

    // Update is called once per frame
    void Update()
    {
        //Pour afficher l'état de la connection au fur et à mesure
        //logObj.AjoutLog(PhotonNetwork.NetworkingClient.State.ToString(),1);


        if (PhotonNetwork.NetworkingClient.InRoom)
        {

            // juste pour le test, et le force 
            /*logObj.ForceClear();*/
            /*logObj.AjoutLog("BTN A: " + OVRInput.Get(OVRInput.RawButton.A), 1);
            logObj.AjoutLog("BTN B: " + OVRInput.Get(OVRInput.RawButton.B), 1);
            logObj.AjoutLog("BTN X: " + OVRInput.Get(OVRInput.RawButton.X), 1);
            logObj.AjoutLog("BTN Y: " + OVRInput.Get(OVRInput.RawButton.Y), 1);*/
            

        }
        
        
    }
    public override void OnJoinedRoom()
    {
        logObj.AjoutLog("Connecté à la room: " + PhotonNetwork.CurrentRoom.Name, 50);
        GameObject p = new GameObject();
        foreach (GameObject avatar in playerPrefabList)
        {
            if (avatar.name.Equals(avatarName))
            {
                p = PhotonNetwork.Instantiate(avatar.name, avatar.transform.position, Quaternion.identity);
                break;
            }
        }
        if (!p.name.Equals(avatarName) && playerPrefabList.Count >=1)
        {
            Destroy(p);
            p = PhotonNetwork.Instantiate(playerPrefabList[0].name, playerPrefabList[0].transform.position, Quaternion.identity);
        }

        p.GetComponent<SyncPosPlayer>().setup(playerOVR);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        logObj.AjoutLog("Déconnecté car: " + cause, 10);
    }
}
