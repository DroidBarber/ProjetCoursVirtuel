using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ConnectJoinRoom : MonoBehaviourPunCallbacks
{
    [Tooltip("Si on affiche les Debug.Log() ou non")]
    public bool isShowDebugLogInUnity = true; // Si on affiche les Debug.Log()

    public bool isAutoJoinOrCreateRoom = true;

    /// <summary>Used as PhotonNetwork.GameVersion.</summary>
    public byte Version = 1;

    /// <summary>Max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.</summary>
    [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
    public byte MaxPlayers = 20;

    public int playerTTL = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");
        }
        

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
    }


    public override void OnConnectedToMaster()
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" +
                PhotonNetwork.CloudRegion + "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        }
        if (isAutoJoinOrCreateRoom)
        {
            AutoJoinOrCreateRoom();
        }
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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" +
                PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        }

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;

        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }
    }

    public override void OnJoinedRoom()
    {
        if (isShowDebugLogInUnity)
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        }
    }

    public void AutoJoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = this.MaxPlayers };
        if (playerTTL >= 0)
            roomOptions.PlayerTtl = playerTTL;
        TypedLobby typedLobby = new TypedLobby("3iL",  LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom("RoomAutoJoin3", roomOptions, typedLobby);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        Debug.LogError(roomList.Count);

   

        
    }

}
