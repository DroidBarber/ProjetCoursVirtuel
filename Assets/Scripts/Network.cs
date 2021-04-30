using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Ce script régit les premiers instant en multijoueur dans une room, afin entre autres d'instancier chez les
/// autres joueurs l'avatar que nous avons choisis (chez les autres uniquement, car on ne se vois pas nous même)
/// </summary>
public class Network : MonoBehaviourPunCallbacks
{
    public Log_UI logObj;
    public List<GameObject> playerPrefabList;
    public GameObject playerOVR;
    private int avatarIndex;
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

        // Le script RoomNameToJoin contient l'index de l'avatar à utiliser, et ce paramètre provient d'un choix lors du menu
        GameObject g = GameObject.Find("RoomNameToJoin");
        avatarIndex = g.GetComponent<RoomNameToJoin>().avatarIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //Pour afficher l'état de la connection au fur et à mesure
        //logObj.AjoutLog(PhotonNetwork.NetworkingClient.State.ToString(),1);

        
    }

    /// <summary>
    /// Fonction appelé dès lors que l'on rentre dans une room
    /// on s'en sert afin d'instancier chez les autres joueur/client l'avatar qu'on a sélectionné
    /// </summary>
    public override void OnJoinedRoom()
    {
        logObj.AjoutLog("Connecté à la room: " + PhotonNetwork.CurrentRoom.Name, 50);
        GameObject p;
        
        
        if (playerPrefabList.Count >= avatarIndex)
        {
            p = PhotonNetwork.Instantiate(playerPrefabList[avatarIndex].name, playerPrefabList[avatarIndex].transform.position, Quaternion.identity);
        }
        else
        {
            p = PhotonNetwork.Instantiate(playerPrefabList[0].name, playerPrefabList[0].transform.position, Quaternion.identity);
        }

        p.GetComponent<SyncPosPlayer>().setup(playerOVR); // setup de la synchronisation position/rotation joueur et controllers
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        logObj.AjoutLog("Déconnecté car: " + cause, 10);

    }
}
