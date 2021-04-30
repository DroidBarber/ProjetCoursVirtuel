using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectJoinLobby : MonoBehaviourPunCallbacks
{
    public byte Version = 1;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "";
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" +
                PhotonNetwork.CloudRegion + "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
        Debug.LogError("In room: " + PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnJoinedLobby()
    {
        // pour tester et faire des salles random dans des .exe
        /*string roomName = "Room " + Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions { MaxPlayers = 8 };
        PhotonNetwork.CreateRoom(roomName, options, null);*/
    }
}
