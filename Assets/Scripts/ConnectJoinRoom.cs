using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ConnectJoinRoom : MonoBehaviourPunCallbacks
{
    [Tooltip("Si on affiche les Debug.Log() ou non")]
    public bool isShowDebugLogInUnity = true; // Si on affiche les Debug.Log()

    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
    [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
    public byte MaxPlayers = 20;

    public int playerTTL = -1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.Find("RoomNameToJoin");
        string roomName = g.GetComponent<RoomNameToJoin>().roomName;
        AutoJoinOrCreateRoom(roomName);
        Destroy(g);
    }


    public override void OnConnectedToMaster()
    {
        
    }

    /*public override void OnJoinedLobby()
    {
        
        if (isShowDebugLogInUnity)
        {
            Debug.LogError("OnJoinedLobby(). This client is now connected to Relay in region [" +
                PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
        }
        if (isAutoJoinOrCreateRoom)
        {
            AutoJoinOrCreateRoom();
        }
    }*/

    

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }
        //Mettre ici un changement de scène pour la scène de choix de room
    }

    public override void OnJoinedRoom()
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        }
    }

    public void AutoJoinOrCreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }

    

}
