using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

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

    void Awake()
    {
        if (!PhotonNetwork.InRoom)
        {
            GameObject g = GameObject.Find("RoomNameToJoin");
            string roomName = g.GetComponent<RoomNameToJoin>().roomName;
            AutoJoinOrCreateRoom(roomName);
        }
    }

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
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }



}
