using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Script qui gère la scène de connexion. Il gère les différents canvas, et permet de créer ou rejoindre une salle .
/// </summary>

public class ListeBoutonController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject buttonTemplate;

    public byte Version = 1;

    private List<string> listeNomRoom = new List<string>();
    private List<GameObject> listeButtonRoom = new List<GameObject>();

    private int avatarIndex = 0;
    private bool isSalleTP = false;

    // Variables des objets de la scènes
   
    public GameObject scrollList;
    public GameObject buttonCreate;
    public GameObject buttonCreation;
    public GameObject buttonAvatar;
    public GameObject canvasAvatar;
    public GameObject canvasCreationSalle;


    // Fonction appelé au lancement de la scène
    void Start()
    {

        buttonAvatar.GetComponentInChildren<Text>().text = "Choix Avatar (Capsule)";
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    // Fonction qui met à jour la liste des listes existantes
    private void RefreshRoomListUI()
    {

        // destruction des boutons et raz de la liste
        foreach (GameObject btn in listeButtonRoom)
        {
            Destroy(btn);
        }
        listeButtonRoom.Clear();

        // création des boutons pour chaque salle
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


    // Fonction appelée lors du click sur un bouton pour rejoindre une room.
    public void ButtonClicked(string nameRoom) 
    {
        //Debug.LogError("Click sur " + myTextString);
        GameObject g = new GameObject("RoomNameToJoin");
        g.AddComponent<RoomNameToJoin>();
        g.GetComponent<RoomNameToJoin>().roomName = nameRoom;
        g.GetComponent<RoomNameToJoin>().avatarIndex = avatarIndex;

        PhotonNetwork.JoinRoom(nameRoom);


        // il faut que cette scène soit dans les scène du build setting dans file de unity
    }

    // Création d'une room 
    public void CreateRandomRoom()
    {
        //Vérification de la connexion a PUN et au lobby
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            // Définitoon du nom de la salle (Room n° + nb aléatoire)
            string roomName = "Room " + Random.Range(1000, 10000);
            while (listeNomRoom.Contains(roomName))
            {
                roomName = "Room " + Random.Range(1000, 10000);
            }
            // Création de la room PUN et chargement de la scène choisie
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


    // Fonctions canvas Choix Salle

    // Fonctions liés aux boutons de choix d'avatar -> une fonction pour chaque bouton, peut être améliorée
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

    // Fonctions canvas Création Salle
    // Fonctions liés aux boutons de choix de salle pour la création -> une fonction pour chaque bouton, peut être améliorée
    public void choix309()
    {
        isSalleTP = false;
        buttonCreation.GetComponentInChildren<Text>().text = "Créer une Salle (309)";


    }

    public void choixTP()
    {
        isSalleTP = true;
        buttonCreation.GetComponentInChildren<Text>().text = "Créer une Salle (TP)";

    }


    // Chargement du canvas de création de salle
    public void panelCreationSalle()
    {
        if (isSalleTP)
            buttonCreation.GetComponentInChildren<Text>().text = "Créer une Salle (TP)";
        else
            buttonCreation.GetComponentInChildren<Text>().text = "Créer une Salle (309)";

        // Désactivation des éléments du canvas de la liste des salles
        scrollList.SetActive(false);
        buttonCreate.SetActive(false);
        buttonAvatar.SetActive(false);

        // Activation du canvas de création de salle
        canvasCreationSalle.SetActive(true);

       

    }

    // Chargement du canvas de choix d'avatar
    public void panelchoixAvatar()
    {
        // Activation du canvas dechoix d'avatar
        canvasAvatar.SetActive(true);

        // Désactivation des éléments du canvas de la liste des salles
        scrollList.SetActive(false);
        buttonCreate.SetActive(false);
        buttonAvatar.SetActive(false);
    }
    // Retour à la liste des salles
    public void retour()
    {
        string nomAvatar;

        // Pour indiquer quel avatar est choisi
        switch (avatarIndex)
        {
            case 0:
                nomAvatar = "Choix Avatar (Capsule)";
                break;
            case 1:
                nomAvatar = "Choix Avatar (Rick)";
                break;
            case 2:
                nomAvatar = "Choix Avatar (Jessica)";
                break;
            case 3:
                nomAvatar = "Choix Avatar (Billy)";
                break;
            default:
                nomAvatar = "Erreur";
                break;
        }
        buttonAvatar.GetComponentInChildren<Text>().text = nomAvatar;
        // désactivation des canvas 
        canvasAvatar.SetActive(false);
        canvasCreationSalle.SetActive(false);

        // activation des éléments de la liste des salles
        scrollList.SetActive(true);
        buttonCreate.SetActive(true);
        buttonAvatar.SetActive(true);

    }

   


    // Focntion qui met à jour la liste des salles lorqu'un changement (création/destruction)
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
