using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ListeBoutonController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject buttonTemplate;

    public byte Version = 1;

    private List<string> listeNomRoom = new List<string>();
    private List<GameObject> listeButtonRoom = new List<GameObject>();

    private int avatarIndex = 0;
    private bool isSalleTP = false;

   
    public GameObject scrollList;
    public GameObject buttonCreate;
    public GameObject buttonAvatar;
    public GameObject canvasAvatar;
    public GameObject canvasCreationSalle;



    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            avatarIndex = (avatarIndex + 1) % 4;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            isSalleTP = !isSalleTP;
        }
        
    }


    private void RefreshRoomListUI()
    {
        foreach (GameObject btn in listeButtonRoom)
        {
            Destroy(btn);
        }
        listeButtonRoom.Clear();

        foreach (string nomRoom in listeNomRoom)
        {

            GameObject button = Instantiate(buttonTemplate);
            listeButtonRoom.Add(button);
            button.SetActive(true);

            button.GetComponentInChildren<Text>().text = nomRoom + ""; // test

            button.transform.SetParent(buttonTemplate.transform.parent, false);
            button.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(nomRoom));
        }
    }

    public void ButtonClicked(string nameRoom) 
    {
        //Debug.LogError("Click sur " + myTextString);
        GameObject g = new GameObject("RoomNameToJoin");
        g.AddComponent<RoomNameToJoin>();
        g.GetComponent<RoomNameToJoin>().roomName = nameRoom;
        g.GetComponent<RoomNameToJoin>().avatarIndex = avatarIndex;

        /*if (isSalleTP)
        {
            SceneManager.LoadScene("Salle TP");
        }
        else
        {
            SceneManager.LoadScene("Salle 309");
        }*/
        PhotonNetwork.JoinRoom(nameRoom);


        // il faut que cette scène soit dans les scène du build setting dans file de unity
    }

    public void CreateRandomRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            string roomName = "Room " + Random.Range(1000, 10000);
            while (listeNomRoom.Contains(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }
            GameObject g = new GameObject("RoomNameToJoin");
            g.AddComponent<RoomNameToJoin>();
            g.GetComponent<RoomNameToJoin>().roomName = roomName;

            if (isSalleTP)
            {
                SceneManager.LoadScene("Salle TP");
            }
            else
            {
                SceneManager.LoadScene("Salle 309");
            }
            // il faut que cette scène soit dans les scène du build setting dans file de unity
        }
    }

    public void choix309()
    {
        isSalleTP = false;

    }

    public void choixCapsule()
    {
        avatarIndex = 0;
    }

    public void choixRick()
    {
        avatarIndex = 1;
    }

    public void choixJessica()
    {
        avatarIndex = 2;
    }

    public void choixBilly()
    {
        avatarIndex = 3;
    }

    public void choixTP()
    {
        isSalleTP = true;

    }

    public void panelCreationSalle()
    {
        canvasCreationSalle.SetActive(true);
        scrollList.SetActive(false);
        buttonCreate.SetActive(false);
        buttonAvatar.SetActive(false);

    }

    public void retour()
    {
        canvasAvatar.SetActive(false);
        canvasCreationSalle.SetActive(false);
        scrollList.SetActive(true);
        buttonCreate.SetActive(true);
        buttonAvatar.SetActive(true);

    }

    public void panelchoixAvatar()
    {
        canvasAvatar.SetActive(true);
        scrollList.SetActive(false);
        buttonCreate.SetActive(false);
        buttonAvatar.SetActive(false);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        
        foreach(RoomInfo roomInfo in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!roomInfo.IsOpen || !roomInfo.IsVisible || roomInfo.RemovedFromList)
            {
                if (this.listeNomRoom.Contains(roomInfo.Name))
                {
                    this.listeNomRoom.Remove(roomInfo.Name);
                }

                continue;
            }
            else
            {
                if (!this.listeNomRoom.Contains(roomInfo.Name))
                {
                    this.listeNomRoom.Add(roomInfo.Name);
                }
            }
        }
        RefreshRoomListUI();
    }

}
