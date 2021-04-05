using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ListeBoutonController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject buttonTemplate;

    public byte Version = 1;

    private List<string> listeNomRoom = new List<string>();



    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
       

    }

    private void Update()
    {
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }



    void FixedUpdate()
    {
        foreach(string nomRoom in listeNomRoom)
        {

            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<ListeBouton>().SetText(nomRoom);

            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }

    }

    public void ButtonClicked(string myTextString) 
    { 

    }

    /*
     *
     * Partie Multi 
     *
     *
     *
     */

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" +
            PhotonNetwork.CloudRegion + "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
       
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" +
            PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        this.listeNomRoom.Clear();

        foreach(RoomInfo room in roomList)
            this.listeNomRoom.Add(room.Name);
    }


}
